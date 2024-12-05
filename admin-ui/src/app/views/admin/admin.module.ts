import { NgModule } from '@angular/core';
import {
  CommonModule,
  HashLocationStrategy,
  LocationStrategy,
} from '@angular/common';
import { Title } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgScrollbarModule } from 'ngx-scrollbar';
import {
  DefaultFooterComponent,
  DefaultHeaderComponent,
  DefaultLayoutComponent,
} from './containers';
import {
  AvatarModule,
  BadgeModule,
  BreadcrumbModule,
  ButtonGroupModule,
  ButtonModule,
  CardModule,
  DropdownModule,
  FooterModule,
  FormModule,
  GridModule,
  HeaderModule,
  ListGroupModule,
  NavModule,
  ProgressModule,
  SharedModule,
  SidebarModule,
  TabsModule,
  UtilitiesModule,
} from '@coreui/angular';
import { IconModule, IconSetService } from '@coreui/icons-angular';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { MessageService, ConfirmationService } from 'primeng/api';
import { DynamicDialogModule, DialogService } from 'primeng/dynamicdialog';
import { environment } from '../../../environments/environment';
import {
  ADMIN_API_BASE_URL,
  AdminApiAuthApiClient,
  AdminApiUserApiClient,
  AdminApiTestApiClient,
  AdminApiTokenApiClient,
  AdminApiRoleApiClient,
  AdminApiPostCategoryApiClient,
  AdminApiSeriesApiClient,
  AdminApiPostApiClient,
  AdminApiRoyaltyApiClient,
  AdminApiProductCategoryApiClient,
  AdminApiProductApiClient,
  AdminApiInventoryApiClient,
  AdminApiProjectApiClient,
  AdminApiAnnouncementApiClient,
} from '../../api/admin-api.service.generated';
import { AuthGuard } from '../../shared/auth.guard';
import { AlertService } from '../../shared/services/alert.service';
import { TokenStorageService } from '../../shared/services/token-storage.service';
import { UploadService } from '../../shared/services/upload.service';
import { UtilityService } from '../../shared/services/utility.service';
import { AdminRoutingModule } from './admin-routing.module';
import { GlobalHttpInterceptorService } from '../../shared/interceptors/error-hadnler.interceptor';
import { TokenInterceptor } from '../../shared/interceptors/token.interceptor';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { AdminComponent } from './admin.component';
import { SignalRService } from '../../shared/services/signalr-service';
import { ToastModule } from 'primeng/toast';
import { NavbarModule } from '@coreui/angular';
import { CalendarModule } from 'primeng/calendar';
// import { CustomSidebarNavComponent } from './containers/default-layout/custom-sidebar/custom-sidebar-nav.component';
const APP_CONTAINERS = [
  DefaultFooterComponent,
  DefaultLayoutComponent,
  DefaultHeaderComponent,
];

@NgModule({
  declarations: [AdminComponent, ...APP_CONTAINERS],
  imports: [
    CommonModule,
    AdminRoutingModule,
    AvatarModule,
    BreadcrumbModule,
    FooterModule,
    DropdownModule,
    GridModule,
    HeaderModule,
    IconModule,
    NavModule,
    ButtonModule,
    FormModule,
    UtilitiesModule,
    ButtonGroupModule,
    ReactiveFormsModule,
    SidebarModule,
    SharedModule,
    TabsModule,
    ProgressModule,
    BadgeModule,
    ListGroupModule,
    CardModule,
    NgScrollbarModule,
    HttpClientModule,
    ConfirmDialogModule,
    DynamicDialogModule,
    ToastModule,
    NavbarModule,
  
  ],
  providers: [
    { provide: ADMIN_API_BASE_URL, useValue: environment.API_URL },
    {
      provide: LocationStrategy,
      useClass: HashLocationStrategy,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: GlobalHttpInterceptorService,
      multi: true,
    },
    IconSetService,
    Title,
    MessageService,
    AlertService,
    TokenStorageService,
    AuthGuard,
    UtilityService,
    DialogService,
    ConfirmationService,
    AdminApiAuthApiClient,
    AdminApiUserApiClient,
    AdminApiTestApiClient,
    AdminApiTokenApiClient,
    AdminApiRoleApiClient,
    AdminApiPostCategoryApiClient,
    AdminApiSeriesApiClient,
    AdminApiPostApiClient,
    AdminApiRoyaltyApiClient,
    AdminApiProductCategoryApiClient,
    AdminApiProductApiClient,
    AdminApiInventoryApiClient,
    AdminApiProjectApiClient,
    AdminApiAnnouncementApiClient,
    UploadService,
    SignalRService,
  ],
  bootstrap: [AdminComponent],
})
export class AdminModule {}
