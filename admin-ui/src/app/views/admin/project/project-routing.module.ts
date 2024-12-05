import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import * as authGuard from '../../../shared/auth.guard';
import { ProjectComponent } from './projects/project.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'project',
    pathMatch: 'full',
  },
  {
    path: '',
    component: ProjectComponent,
    data: {
      title: 'Dự Án',
      requiredPolicy: 'Permissions.Projects.View',
    },
    canActivate: [authGuard.AuthGuard],
  },
  // {
  //   path: 'products',
  //   component: ProductComponent,
  //   data: {
  //     title: 'Sản Phẩm',
  //     requiredPolicy: 'Permissions.Products.View',
  //   },
  //   canActivate: [authGuard.AuthGuard],
  // },
  // {
  //   path: 'product-categories',
  //   component: ProductCategoryComponent,
  //   data: {
  //     title: 'Danh Mục Sản Phẩm',
  //     requiredPolicy: 'Permissions.ProductCategories.View',
  //   },
  //   canActivate: [authGuard.AuthGuard],
  // },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ProjectRoutingModule{}
