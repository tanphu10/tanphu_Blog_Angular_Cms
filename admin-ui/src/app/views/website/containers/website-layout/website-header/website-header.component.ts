import { Component, NgZone } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { TokenStorageService } from 'src/app/shared/services/token-storage.service';
import { Router } from '@angular/router';
import * as adminApiServiceGenerated from 'src/app/api/admin-api.service.generated';
import { UrlConstants } from 'src/app/shared/constants/url.constants';
import { SignalRService } from 'src/app/shared/services/signalr-service';
import { Subject, takeUntil } from 'rxjs';
import { UserModel } from 'src/app/shared/models/user.model';
import { environment } from 'src/environments/environment';
import {
  AvatarModule,
  BadgeModule,
  BreadcrumbModule,
  ButtonGroupModule,
  ButtonModule,
  CardModule,
  ContainerComponent,
  DropdownModule,
  FooterModule,
  FormModule,
  GridModule,
  HeaderModule,
  ListGroupModule,
  NavbarModule,
  NavModule,
  ProgressModule,
  SharedModule,
  SidebarModule,
  TabsModule,
  UtilitiesModule,
} from '@coreui/angular';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { IconModule } from '@coreui/icons-angular';
import { NgScrollbarModule } from 'ngx-scrollbar';
@Component({
  selector: 'website-header',
  templateUrl: './website-header.component.html',
  standalone: true,
  imports: [
    // Các module khác
    NavbarModule, // Đảm bảo đã import module này
    ContainerComponent,
    CommonModule,
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
    BadgeModule,
  ],
})
export class WebsiteHeaderComponent {}
