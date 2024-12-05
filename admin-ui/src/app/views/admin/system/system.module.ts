
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { SystemRoutingModule } from './system-routing.module';
import { UserComponent } from './users/user.component';
import { RoleComponent } from './roles/role.component';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { BlockUIModule } from 'primeng/blockui';
import { PaginatorModule } from 'primeng/paginator';
import { PanelModule } from 'primeng/panel';
import { CheckboxModule } from 'primeng/checkbox';
import { SharedModule } from 'primeng/api';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { RoleDetailComponent } from './roles/role-detail.component';
import { KeyFilterModule } from 'primeng/keyfilter';
import { PermissionGrantComponent } from './roles/permission-grant.component';
import { ChangeEmailComponent } from './users/change-email.component';
import { RoleAssignComponent } from './users/role-assign.component';
import { SetPasswordComponent } from './users/set-password.component';
import { UserDetailComponent } from './users/user-detail.component';
import { BadgeModule } from 'primeng/badge';
import { PickListModule } from 'primeng/picklist';
import { ImageModule } from 'primeng/image';
import { AnnouncementComponent } from './announcements/announcement.component';
import { AnnouncementDetailComponent } from './announcements/announcement-detail.component';
import { CmsSharedModule } from 'src/app/shared/modules/cms-shared.module';
import { MultiSelectModule } from 'primeng/multiselect';

@NgModule({
  imports: [
    SystemRoutingModule,
    CommonModule,
    ReactiveFormsModule,
    TableModule,
    ProgressSpinnerModule,
    BlockUIModule,
    PaginatorModule,
    PanelModule,
    CheckboxModule,
    ButtonModule,
    InputTextModule,
    KeyFilterModule,
    SharedModule,
    CmsSharedModule,
    BadgeModule,
    PickListModule,
    ImageModule,
     MultiSelectModule
  ],
  declarations: [
    UserComponent,
    RoleComponent,
    RoleDetailComponent,
    PermissionGrantComponent,
    ChangeEmailComponent,
    RoleAssignComponent,
    SetPasswordComponent,
    UserDetailComponent,
    AnnouncementComponent,
    AnnouncementDetailComponent,
  ],
})
export class SystemModule {}
