import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { WebSiteLayoutComponent } from './website-containers/website-layout/website-layout.component';
import { DashboardComponent } from '../admin/dashboard/dashboard.component';
import { LandingComponent } from './website-landingpages/website-landing.component';
// import { AuthGuard } from 'src/app/shared/auth.guard';
const websiteRoutes: Routes = [
  {
    path: 'user',
    loadChildren: () =>
      import('./website-auth/auth.module').then((m) => m.AuthModule),
  },
  {
    path: '',
    component: WebSiteLayoutComponent,
    data: {
      title: 'Home',
    },
    children: [
      {
        path: 'board',
        component: DashboardComponent,
      },
      {
        path: '',
        component: LandingComponent,
      },
      {
        path: '',
        loadChildren: () =>
          import('./website-search/website-search.module').then(
            (m) => m.WebsiteSearchModule
          ),
      },
      {
        path: '',
        loadChildren: () =>
          import('./website-posts/website-post.module').then(
            (m) => m.PostModule
          ),
      },
      {
        path: '',
        loadChildren: () =>
          import('./website-projects/website-project.module').then(
            (m) => m.ProjectModule
          ),
      },
      {
        path: '',
        loadChildren: () =>
          import('./website-system/website-system.module').then(
            (m) => m.WebsiteSystemModule
          ),
      },
      {
        path: '',
        loadChildren: () =>
          import('./website-inventory/website-inventory.module').then(
            (m) => m.InventoryModule
          ),
      },
      { path: '', redirectTo: 'user/login', pathMatch: 'full' }, // Đường dẫn mặc định
      { path: '**', redirectTo: 'user/login', pathMatch: 'full' }, // Đường dẫn mặc định
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(websiteRoutes)],
  exports: [RouterModule],
})
export class WebsiteRoutingModule {}
