import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { WebsiteSearchComponent } from './website-search.component';
// import {UserComponent} from './users/user.component'
import { AuthGuard } from '../../../shared/auth.guard';
// import {RoleComponent} from './roles/role.component';
// import {AnnouncementComponent} from './announcements/announcement.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'searchs',
    pathMatch: 'full',
  },
  {
    path: 'searchs/:search',
    component: WebsiteSearchComponent,
    data: {
      title: 'tìm kiếm',
      requiredPolicy: 'Permissions.Users.View',
    },
    canActivate: [AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class WebsiteSearchRoutingModule {}
