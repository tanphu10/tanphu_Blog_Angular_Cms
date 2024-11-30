import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../../../shared/auth.guard';
import { InventoryComponent } from './website-inventory.component';
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
    canActivate: [AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class InventoryRoutingModule {}
