import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { WebDashboardComponent } from './web-dashboard.component';
import { AuthGuard } from '../../../shared/auth.guard';

const routes: Routes = [
  {
    path: '',
    component: WebDashboardComponent,
    data: {
      title: 'Trang Chủ',
      requiredPolicy: 'Permissions.Posts.View',
    },
    canActivate:[AuthGuard]
  },
 ];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class WebDashboardRoutingModule {
}
