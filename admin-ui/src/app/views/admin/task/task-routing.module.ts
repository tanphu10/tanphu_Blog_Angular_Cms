import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TaskComponent } from './tasks/task.component';
import { DashboardTaskComponent } from './dashboard-tasks/dashboard-task.component';
import { AuthGuard } from '../../../shared/auth.guard';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'task-dashboard',
    pathMatch: 'full',
  },
  {
    path: 'management',
    component: TaskComponent,
    data: {
      title: 'Tasks',
      requiredPolicy: 'Permissions.Projects.View',
    },
    canActivate: [AuthGuard],
  },
  {
    path: 'task-dashboard',
    component: DashboardTaskComponent,
    data: {
      title: 'Dashboard-task',
      // requiredPolicy: 'Permissions.Projects.View',
    },
    // canActivate: [AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TaskRoutingModule {}
