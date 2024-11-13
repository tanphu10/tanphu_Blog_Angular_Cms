import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import * as authGuard from 'src/app/shared/auth.guard';
import { InventoryComponent } from './inventories/inventory.component';
import { ProductComponent } from './products/product.component';
import { ProductCategoryComponent } from './product-categories/product-category.component';

// import {SeriesComponent} from './series/series.component';
const routes: Routes = [
  {
    path: '',
    redirectTo: 'inventories',
    pathMatch: 'full',
  },
  {
    path: 'inventories',
    component: InventoryComponent,
    data: {
      title: 'Tồn Kho',
      requiredPolicy: 'Permissions.Inventories.View',
    },
    canActivate: [authGuard.AuthGuard],
  },
  {
    path: 'products',
    component: ProductComponent,
    data: {
      title: 'Sản Phẩm',
      requiredPolicy: 'Permissions.Products.View',
    },
    canActivate: [authGuard.AuthGuard],
  },
  {
    path: 'product-categories',
    component: ProductCategoryComponent,
    data: {
      title: 'Danh Mục Sản Phẩm',
      requiredPolicy: 'Permissions.ProductCategories.View',
    },
    canActivate: [authGuard.AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class InventoryRoutingModule{}
