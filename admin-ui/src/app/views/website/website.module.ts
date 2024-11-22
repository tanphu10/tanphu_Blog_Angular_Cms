import { NgModule } from '@angular/core';
import {
  CommonModule,
  HashLocationStrategy,
  LocationStrategy,
} from '@angular/common';
import { Title } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { NgScrollbarModule } from 'ngx-scrollbar';
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
  AdminApiPostCategoryApiClient,
  AdminApiUserApiClient,
  AdminApiWebsitePostApiClient,
} from '../../api/admin-api.service.generated';
import { AuthGuard } from '../../shared/auth.guard';
import { AlertService } from '../../shared/services/alert.service';
import { TokenStorageService } from '../../shared/services/token-storage.service';
import { UploadService } from '../../shared/services/upload.service';
import { UtilityService } from '../../shared/services/utility.service';
import { WebsiteRoutingModule } from './website-routing.module';
import { GlobalHttpInterceptorService } from '../../shared/interceptors/error-hadnler.interceptor';
import { TokenInterceptor } from '../../shared/interceptors/token.interceptor';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { SignalRService } from '../../shared/services/signalr-service';
import { WebsiteComponent } from './website.component';
import { WebSiteLayoutComponent } from './website-containers/website-layout';
import { ToastModule } from 'primeng/toast';
import { BlockUIModule } from 'primeng/blockui';
import { LandingComponent } from './website-landingpages/website-landing.component';

@NgModule({
  declarations: [WebsiteComponent],
  imports: [
    CommonModule,
    WebsiteRoutingModule,
    WebSiteLayoutComponent,
    LandingComponent,
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
    BadgeModule,
    ToastModule,
    BlockUIModule,
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
    UploadService,
    SignalRService,
    AdminApiWebsitePostApiClient ,
    AdminApiPostCategoryApiClient
  ],
  bootstrap: [WebsiteComponent],
})
export class WebsiteModule {}
