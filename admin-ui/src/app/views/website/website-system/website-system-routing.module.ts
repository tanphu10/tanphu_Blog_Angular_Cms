import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { WebsiteSystemComponent } from './website-system.component';
// import {UserComponent} from './users/user.component'
import { AuthGuard } from '../../../../app/shared/auth.guard';
// import {RoleComponent} from './roles/role.component';
// import {AnnouncementComponent} from './announcements/announcement.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: '',
    pathMatch: 'full',
  },
  {
    path: 'profiles',
    component: WebsiteSystemComponent,
    data: {
      title: 'hồ sơ cá nhân',
      requiredPolicy: 'Permissions.Users.View',
    },
    canActivate: [AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class WebsiteSystemRoutingModule {}
