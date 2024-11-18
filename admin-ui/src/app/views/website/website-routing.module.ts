import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { WebSiteLayoutComponent } from './containers/website-layout/website-layout.component';

const websiteRoutes: Routes = [
  {
    path: 'user',
    loadChildren: () => import('./auth-web/auth.module').then((m) => m.AuthModule),
  },
  {
    path: '',
    component: WebSiteLayoutComponent,
    data: {
      title: 'Home',
    },
    children: [
      {
        path: 'dashboard',
        loadChildren: () =>
          import('./webdashboard/web-dashboard.module').then(
            (m) => m.WebDashboardModule
          ),
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(websiteRoutes)],
  exports: [RouterModule],
})
export class WebsiteRoutingModule {}
