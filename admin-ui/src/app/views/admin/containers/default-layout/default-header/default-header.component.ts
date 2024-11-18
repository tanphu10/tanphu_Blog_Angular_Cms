import {
  Component,
  CUSTOM_ELEMENTS_SCHEMA,
  Input,
  NgZone,
} from '@angular/core';
import { Router } from '@angular/router';
import { HeaderComponent, NavComponent } from '@coreui/angular';
import { Subject, takeUntil } from 'rxjs';
import {
  AdminApiAnnouncementApiClient,
  AnnouncementViewModel,
  AnnouncementViewModelPageResult,
} from 'src/app/api/admin-api.service.generated';
import { UrlConstants } from 'src/app/shared/constants/url.constants';
import { UserModel } from 'src/app/shared/models/user.model';
import { SignalRService } from 'src/app/shared/services/signalr-service';
import { TokenStorageService } from 'src/app/shared/services/token-storage.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-default-header',
  // templateUrl: './default-header.component.html',
  templateUrl: './header.component.html',
})
export class DefaultHeaderComponent extends NavComponent {
  @Input() sidebarId: string = 'sidebar';

  private unsubscribe$ = new Subject<void>(); // This will be used to trigger the unsubscription

  // Your existing constructor and methods...

  ngOnInit(): void {
    this.subscribeToEvents();
    this.loadAnnouncements();
    this.getAvatar();
  }

  ngOnDestroy(): void {
    // Emit value to unsubscribe all subscriptions
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
  public announcements: AnnouncementViewModel[];
  public canSendMessage: Boolean;

  //Paging variables
  public pageIndex: number = 1;
  public pageSize: number = 10;
  public totalCount: number;

  public user: UserModel;
  public environment = environment;

  currentTheme: string = 'light'; // Mặc định là theme sáng

  constructor(
    private tokenService: TokenStorageService,
    private router: Router,
    private _signalRService: SignalRService,
    private _ngZone: NgZone,
    private announcementService: AdminApiAnnouncementApiClient
  ) {
    super();
    this.canSendMessage = _signalRService.connectionExists;
    // Kiểm tra theme đã lưu trong localStorage
    // const savedTheme = localStorage.getItem('theme');
    // if (savedTheme) {
    //   this.currentTheme = savedTheme;
    //   this.setTheme(savedTheme);
    // } else {
    //   this.setTheme(this.currentTheme);
    // }
  }
  // toggleTheme() {
  //   this.currentTheme = this.currentTheme === 'light' ? 'dark' : 'light';
  //   this.setTheme(this.currentTheme);
  //   localStorage.setItem('theme', this.currentTheme); // Lưu trạng thái theme vào localStorage
  // }
  // setTheme(theme: string) {
  //   const themeLink = document.getElementById('theme-link') as HTMLLinkElement;

  //   if (theme === 'dark') {
  //     themeLink.href = 'assets/styles/dark-theme.scss';
  //   } else {
  //     themeLink.href = 'assets/styles/light-theme.scss';
  //   }
  // }
  logout() {
    this.tokenService.signOut();
    this.router.navigate([UrlConstants.LOGIN]);
  }
  private subscribeToEvents(): void {
    this.announcements = [];

    // If connection exists, it can call the method
    this._signalRService.connectionEstablished
      .pipe(takeUntil(this.unsubscribe$)) // Automatically unsubscribes when the component is destroyed
      .subscribe(() => {
        console.log('_signalRService');
        this.canSendMessage = true;
      });

    // Service method to call when the response is received from the server event
    this._signalRService.announcementReceived
      // .pipe(takeUntil(this.unsubscribe$))
      .subscribe((response: AnnouncementViewModel) => {
        this._ngZone.run(() => {
          console.log('check data', this.announcements);
          // Format date and push the announcement
          // announcement.dateCreated = moment(announcement.dateCreated).fromNow(); // Sử dụng moment để chuyển đổi ngày
          // this.announcements.push(announcement);
          this.announcements.push(response);
          console.log('check res  ', response);
        });
      });
  }
  getAvatar() {
    this.user = this.tokenService.getUser();
    console.log(this.environment);
    console.log(this.user);
  }
  markAsRead(id: number) {
    this.announcementService.markAsRead(id).subscribe({
      next: () => {
        this.loadAnnouncements();
      },
      error: (err) => {
        console.log(err);
      },
    });
  }

  loadAnnouncements() {
    this.announcementService
      .getTopMyAnnouncement(this.pageIndex, this.pageSize)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe({
        next: (response: AnnouncementViewModelPageResult) => {
          this.announcements = response.results;
          this.totalCount = response.rowCount;
          // this.toggleBlockUI(false);
          // console.log('check listUnRead', response);
        },
        error: (e) => {
          console.log(e);
          // this.toggleBlockUI(false);
        },
      });
  }
}
