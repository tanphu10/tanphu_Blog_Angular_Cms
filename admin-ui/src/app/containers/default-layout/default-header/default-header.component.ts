import { Component, Input, NgZone } from '@angular/core';
import { Router } from '@angular/router';
import { HeaderComponent } from '@coreui/angular';
import { Subject, takeUntil } from 'rxjs';
import {
  AdminApiAnnouncementApiClient,
  AnnouncementViewModel,
  AnnouncementViewModelPageResult,
} from 'src/app/api/admin-api.service.generated';
import { UrlConstants } from 'src/app/shared/constants/url.constants';
import { SignalRService } from 'src/app/shared/services/signalr-service';
import { TokenStorageService } from 'src/app/shared/services/token-storage.service';

@Component({
  selector: 'app-default-header',
  templateUrl: './default-header.component.html',
})
export class DefaultHeaderComponent extends HeaderComponent {
  @Input() sidebarId: string = 'sidebar';

  private unsubscribe$ = new Subject<void>(); // This will be used to trigger the unsubscription

  // Your existing constructor and methods...

  ngOnInit(): void {
    this.subscribeToEvents();
    this.loadAnnouncements();
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

  // --theme
  readonly #colorModeService = inject(ColorModeService);
  readonly colorMode = this.#colorModeService.colorMode;

  readonly colorModes = [
    { name: 'light', text: 'Light', icon: 'cilSun' },
    { name: 'dark', text: 'Dark', icon: 'cilMoon' },
    { name: 'auto', text: 'Auto', icon: 'cilContrast' },
  ];

  readonly icons = computed(() => {
    const currentMode = this.colorMode();
    return (
      this.colorModes.find((mode) => mode.name === currentMode)?.icon ??
      'cilSun'
    );
  });

  constructor(
    private tokenService: TokenStorageService,
    private router: Router,
    private _signalRService: SignalRService,
    private _ngZone: NgZone,
    private announcementService: AdminApiAnnouncementApiClient
  ) {
    super();
    this.canSendMessage = _signalRService.connectionExists;
  }
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
function inject(ColorModeService: any) {
  throw new Error('Function not implemented.');
}

function computed(arg0: () => string) {
  throw new Error('Function not implemented.');
}
