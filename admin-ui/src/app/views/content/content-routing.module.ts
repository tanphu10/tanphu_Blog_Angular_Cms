import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PostComponent } from './posts/post.component';
import { AuthGuard } from 'src/app/shared/auth.guard';
import { UserComponent } from '../system/users/user.component';
const routes: Routes = [
  {
    path: '',
    redirectTo: 'posts',
    pathMatch: 'full',
    // canActivate: [AuthGuard],
  },
  {
    path: 'posts',
    component: PostComponent,
    data: {
      title: 'Bài viết',
      requiredPolicy: 'Permissions.Posts.View',
    },
    canActivate: [AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class contentRoutingModule {}
