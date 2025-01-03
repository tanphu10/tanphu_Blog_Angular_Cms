import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {UserComponent} from './users/user.component'
import { AuthGuard } from 'src/app/shared/auth.guard';
import {RoleComponent} from './roles/role.component';
import {AnnouncementComponent} from './announcements/announcement.component';


const routes: Routes = [
 
  {
    path:'',
    redirectTo:'users',
    pathMatch:'full',
   },
   {
    path: 'users',
    component:UserComponent,
    data: {
      title: 'users',
      requiredPolicy:'Permissions.Users.View',
    },
    canActivate:[AuthGuard],
  },
  {
    path: 'roles',
    component:RoleComponent,
    data: {
      title: 'roles',
      requiredPolicy:'Permissions.Roles.View',
    },
    canActivate:[AuthGuard],
  },
  {
    path: 'announcements',
    component:AnnouncementComponent,
    data: {
      title: 'announcements',
      requiredPolicy:'Permissions.Announcements.View',
    },
    canActivate:[AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SystemRoutingModule {}
