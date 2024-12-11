import { Component, Input, NgZone } from '@angular/core';
import { Router } from '@angular/router';
import { HeaderComponent } from '@coreui/angular';
import { AvatarModule } from 'primeng/avatar';
import { Subject, takeUntil } from 'rxjs';
import {
  AdminApiAnnouncementApiClient,
  AdminApiTaskApiClient,
  AnnouncementViewModel,
  AnnouncementViewModelPageResult,
  TaskNotificationViewModel,
  TaskNotificationViewModelPageResult,
} from 'src/app/api/admin-api.service.generated';
import { UrlConstants } from 'src/app/shared/constants/url.constants';
import { UserModel } from 'src/app/shared/models/user.model';
import { SignalRService } from 'src/app/shared/services/signalr-service';
import { TokenStorageService } from 'src/app/shared/services/token-storage.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-default-header',
  templateUrl: './default-header.component.html',
  styleUrls: ['./default-header.component.scss'],
})
export class DefaultHeaderComponent extends HeaderComponent {
  @Input() sidebarId: string = 'sidebar';

  private unsubscribe$ = new Subject<void>(); // This will be used to trigger the unsubscription

  // Your existing constructor and methods...

  public announcements: AnnouncementViewModel[];
  public taskNotifications: TaskNotificationViewModel[];
  public canSendMessage: Boolean;

  //Paging variables
  public pageIndex: number = 1;
  public pageSize: number = 10;
  public totalCount: number;

  public user: UserModel;
  public environment = environment;

  constructor(
    private tokenService: TokenStorageService,
    private router: Router,
    private _signalRService: SignalRService,
    private _ngZone: NgZone,
    private announcementService: AdminApiAnnouncementApiClient,
    private taskService: AdminApiTaskApiClient
  ) {
    super();
    this.canSendMessage = _signalRService.connectionExists;
  }
  ngOnInit(): void {
    this.subscribeToEvents();
    this.loadAnnouncements();
    this.loadTaskNotifications();
    this.getAvatar();
  }

  ngOnDestroy(): void {
    // Emit value to unsubscribe all subscriptions
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
    this._signalRService.stopConnection();
  }

  logout() {
    this.tokenService.signOut();
    this.router.navigate([UrlConstants.LOGIN]);
  }

  calculateTimeDifference(date: string): string {
    const currentDate = new Date();
    const dateCreated = new Date(date);

    if (isNaN(dateCreated.getTime())) {
      console.error('Invalid date format');
      return ''; // Trả về chuỗi rỗng nếu date không hợp lệ
    }

    const diffInMilliseconds = currentDate.getTime() - dateCreated.getTime();
    const diffInMinutes = Math.floor(diffInMilliseconds / (1000 * 60));

    if (diffInMinutes < 60) {
      return `${diffInMinutes} phút trước`;
    }

    const diffInHours = Math.floor(diffInMinutes / 60);
    if (diffInHours < 24) {
      return `${diffInHours} giờ trước`;
    }

    const diffInDays = Math.floor(diffInHours / 24);
    return `${diffInDays} ngày trước`;
  }

  private subscribeToEvents(): void {
    this.announcements = [];
    this.taskNotifications = [];
    // If connection exists, it can call the method
    this._signalRService.connectionEstablished
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(() => {
        // console.log('_signalRService111');
        this.canSendMessage = true;
        this.getUserAnnoucements();
        this.getTaskNotifications();
      });

    this._signalRService.announcementReceived
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((response: AnnouncementViewModel) => {
        this._ngZone.run(() => {
          console.log('check before', response);
          this.announcements.push(response);
          console.log('check announcements', this.announcements);
        });
      });
    this._signalRService.taskNotificationReceived
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((response: TaskNotificationViewModel) => {
        this._ngZone.run(() => {
          console.log('check before', response);
          this.taskNotifications.push(response);
          console.log(
            'check after TaskNotificationViewModel',
            this.taskNotifications
          );
        });
      });
  }

  getTaskNotifications(): void {
    const userId = this.tokenService.getUser().id;
    this.announcementService
      .userAnnoucement(userId)
      .subscribe((data: TaskNotificationViewModelPageResult) => {
        this._ngZone.run(() => {
          // console.log('check taskNotifications', data.results);
          if (Array.isArray(data.results)) {
            this.taskNotifications = [
              ...this.taskNotifications,
              ...data.results,
            ];
          } else {
            console.error('Dữ liệu nhận được task no');
          }
        });
      });
  }
  getUserAnnoucements(): void {
    const userId = this.tokenService.getUser().id;
    this.announcementService
      .userAnnoucement(userId)
      .subscribe((data: AnnouncementViewModelPageResult) => {
        this._ngZone.run(() => {
          // console.log('check AnnouncementViewModel', data.results);
          if (Array.isArray(data.results)) {
            this.announcements = [...this.announcements, ...data.results];
          } else {
            console.error('Dữ liệu nhận notification no');
          }
        });
      });
  }
  getAvatar() {
    this.user = this.tokenService.getUser();
  }

  markAsRead(id: number) {
    this.announcementService.markAsRead(id).subscribe({
      next: () => {
        this.loadAnnouncements();
      },
      error: (err) => {
        // console.log(err);
      },
    });
  }

  loadAnnouncements() {
    this.announcementService
      .getTopMyAnnouncement(this.pageIndex, this.pageSize)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe({
        next: (response: AnnouncementViewModelPageResult) => {
          // console.log('loadAnnouncements', response.results);
          this.announcements = response.results;
          this.totalCount = response.rowCount;
        },
        error: (e) => {},
      });
  }


  get unreadAnnouncementsCount(): number {
    return this.announcements
      ? this.announcements.filter((a) => !a.hasRead).length
      : 0;
  }

  get hasUnreadAnnouncements(): boolean {
    return this.unreadAnnouncementsCount > 0;
  }






  markTaskAsRead(id: string) {
    this.taskService.markAsTaskRead(id).subscribe({
      next: () => {
        this.loadTaskNotifications();
        // console.log('mark đã đọc task');
      },
      error: (err) => {
        // console.log(err);
      },
    });
  }

  loadTaskNotifications() {
    this.taskService
      .getTopMyTaskNotifications(this.pageIndex, this.pageSize)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe({
        next: (response: TaskNotificationViewModelPageResult) => {
          console.log("task",response.results)
          this.taskNotifications = response.results;
          this.totalCount = response.rowCount;
        },
        error: (e) => {},
      });
  }

  get unreadTasksCount(): number {
    return this.taskNotifications
      ? this.taskNotifications.filter((a) => !a.hasRead).length
      : 0;
  }

  get hasUnreadTasks(): boolean {
    return this.unreadAnnouncementsCount > 0;
  }

  
}
