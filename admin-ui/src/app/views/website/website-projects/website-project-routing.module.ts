import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../../../shared/auth.guard';
import { ProjectComponent } from './website-project.component';
// import { PostComponent } from './post.component';
const routes: Routes = [
  {
    path: '',
    redirectTo: 'projects',
    pathMatch: 'full',
  },
  {
    path: 'projects/:slug',
    component: ProjectComponent,
    data: {
      title: 'Bài viết',
      requiredPolicy: 'Permissions.Projects.View',
    },
    canActivate: [AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ProjectRoutingModule {}
