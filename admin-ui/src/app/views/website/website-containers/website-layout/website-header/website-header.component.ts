import { CommonModule } from '@angular/common';
import {
  Component,
  CUSTOM_ELEMENTS_SCHEMA,
  HostListener,
  Input,
  NgZone,
  OnInit,
} from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import {
  DropdownModule,
  HeaderComponent,
  HeaderModule,
  NavbarComponent,
  AvatarModule,
  BadgeModule,
  BreadcrumbModule,
  ButtonGroupModule,
  ButtonModule,
  FormModule,
  GridModule,
  ListGroupModule,
  NavModule,
  SharedModule,
  TabsModule,
  UtilitiesModule,
  CollapseModule,
  NavbarModule,
} from '@coreui/angular';
import { IconModule, IconSetService } from '@coreui/icons-angular';
import { Subject, takeUntil } from 'rxjs';
import {
  AnnouncementViewModel,
  AdminApiAnnouncementApiClient,
  AnnouncementViewModelPageResult,
  AdminApiPostCategoryApiClient,
  PostCategoryDto,
  AdminApiProjectApiClient,
  ProjectDto,
} from 'src/app/api/admin-api.service.generated';
import { iconSubset } from 'src/app/icons/icon-subset';
import { UrlConstants } from 'src/app/shared/constants/url.constants';
import { UserModel } from 'src/app/shared/models/user.model';
import { SignalRService } from 'src/app/shared/services/signalr-service';
import { TokenStorageService } from 'src/app/shared/services/token-storage.service';
import { environment } from 'src/environments/environment';
import { TabService } from 'src/app/shared/services/tab.service';
import { FormsModule } from '@angular/forms';
@Component({
  selector: 'website-header',
  templateUrl: './website-header.component.html',
  standalone: true,
  styleUrls: ['./website-header.component.scss'],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  imports: [
    CommonModule,
    RouterModule,
    HeaderModule,
    DropdownModule,
    NavbarComponent,
    AvatarModule,
    BadgeModule,
    BreadcrumbModule,
    ButtonGroupModule,
    ButtonModule,
    FormModule,
    GridModule,
    ListGroupModule,
    NavModule,
    SharedModule,
    TabsModule,
    UtilitiesModule,
    CollapseModule,
    NavbarModule,
    IconModule,
    FormsModule,
  ],
  providers: [IconSetService],
})
export class WebsiteHeaderComponent extends HeaderComponent implements OnInit {
  activeIndex: number = 0; // Chỉ số tab mặc định

  private unsubscribe$ = new Subject<void>(); // This will be used to trigger the unsubscription
  isLoggedIn: boolean = false;
  public postCategories: any[] = [];
  public projectCategories: any[] = [];

  public announcements: AnnouncementViewModel[];
  public canSendMessage: Boolean;

  //Paging variables
  public pageIndex: number = 1;
  public pageSize: number = 10;
  public totalCount: number;
  keyword: string = ''; // Biến liên kết với input

  userImage: string | null = null;
  public environment = environment;

  // currentTheme: string = 'light'; // Mặc định là theme sáng
  isScrolled = false;

  @HostListener('window:scroll', [])
  onWindowScroll() {
    this.isScrolled = window.scrollY > 0;
  }

  // Your existing constructor and methods...

  ngOnInit(): void {
    this.subscribeToEvents();
    this.loadAnnouncements();
    // this.getAvatar();
    this.tabService.userImage$.subscribe((imageUrl) => {
      this.userImage = imageUrl; // Cập nhật hình ảnh từ service
    });
    // console.log(this.userImage);
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
  constructor(
    private tokenService: TokenStorageService,
    private router: Router,
    private _signalRService: SignalRService,
    private _ngZone: NgZone,
    private announcementService: AdminApiAnnouncementApiClient,
    private iconSetService: IconSetService,
    private postCategoryApiClient: AdminApiPostCategoryApiClient,
    private projectCategoryApiClient: AdminApiProjectApiClient,
    private tabService: TabService
  ) {
    super();
    this.canSendMessage = _signalRService.connectionExists;
    iconSetService.icons = { ...iconSubset };
    this.isLoggedIn = this.checkLoginStatus();
    this.loadPostCategories();
    this.loadProjectCategories();
  }

  calculateTimeDifference(dateCreated: string): string {
    const createdDate = new Date(dateCreated);
    const now = new Date();
    const differenceInMs = now.getTime() - createdDate.getTime();
    const differenceInMinutes = Math.floor(differenceInMs / (1000 * 60));
    const differenceInHours = Math.floor(differenceInMinutes / 60);
    const differenceInDays = Math.floor(differenceInHours / 24);
    const differenceInWeeks = Math.floor(differenceInDays / 7);
    const differenceInYears = Math.floor(differenceInDays / 365);
    if (differenceInYears > 0) {
      return `${differenceInYears} y`;
    } else if (differenceInWeeks > 0) {
      return `${differenceInWeeks} w`;
    } else if (differenceInDays > 0) {
      return `${differenceInDays} d`;
    } else if (differenceInHours > 0) {
      return `${differenceInHours} h`;
    } else if (differenceInMinutes > 0) {
      return `${differenceInMinutes} m`;
    } else {
      return `m`;
    }
  }
  navigateToDetail(name: string, slugOrKeyword?: string) {
    // const index =0;
    if (slugOrKeyword) {
      const index = this.postCategories.findIndex(
        (cat) => cat?.slug === slugOrKeyword
      );
      if (index !== -1) {
        this.activeIndex = index;
        this.tabService.setActiveIndex(index); // Cập nhật activeIndex trong service
        this.router.navigate([`/${name}`, slugOrKeyword]);
      } else {
        // console.log("check searh",slugOrKeyword);
        this.router.navigate([`/${name}`, slugOrKeyword]);
      }
    } else {
      // console.log('profile');
      this.router.navigate([`/${name}`]);
    }
  }
  loadPostCategories() {
    this.postCategoryApiClient
      .getPostCategories()
      .subscribe((response: PostCategoryDto[]) => {
        response.forEach((element) => {
          this.postCategories.push({
            value: element.id,
            label: element.name,
            slug: element.slug,
          });
        });
      });
  }
  loadProjectCategories() {
    this.projectCategoryApiClient
      .getAllProjects()
      .subscribe((response: ProjectDto[]) => {
        response.forEach((element) => {
          this.projectCategories.push({
            value: element.id,
            label: element.name,
            slug: element.slug,
          });
        });
      });
  }
  navigateToAdmin(): void {
    const user = this.tokenService.getUser();
    var permissions = JSON.parse(user.permissions);

    if (permissions.includes('Permissions.Admin.View')) {
      this.router.navigate(['/admin/dashboard']);
    } else {
      this.router.navigate(['/admin/auth/login']);
    }
  }

  checkLoginStatus(): boolean {
    // Logic kiểm tra trạng thái đăng nhập
    // Ví dụ: dựa trên token trong localStorage
    return !!localStorage.getItem('auth-user');
  }
  logOut() {
    this.tokenService.signOut();
    this.router.navigate([UrlConstants.HOME_WEB]);
    this.isLoggedIn = this.checkLoginStatus();
  }
  private subscribeToEvents(): void {
    this.announcements = [];

    // If connection exists, it can call the method
    this._signalRService.connectionEstablished
      .pipe(takeUntil(this.unsubscribe$)) // Automatically unsubscribes when the component is destroyed
      .subscribe(() => {
        // console.log('_signalRService');
        this.canSendMessage = true;
      });

    // Service method to call when the response is received from the server event
    this._signalRService.announcementReceived
      // .pipe(takeUntil(this.unsubscribe$))
      .subscribe((response: AnnouncementViewModel) => {
        this._ngZone.run(() => {
          this.announcements.push(response);
        });
      });
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
          this.announcements = response.results;
          this.totalCount = response.rowCount;
        },
        error: (e) => {},
      });
  }
}
