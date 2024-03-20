import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { IconModule } from '@coreui/icons-angular';
import { ChartjsModule } from '@coreui/angular-chartjs';
import { ContentRoutingModule } from './content-routing.module';
import { PostCategoryComponent } from './post-categories/post-category.component';
import { BlockUIModule } from 'primeng/blockui';
import { PostCategoryDetailComponent } from './post-categories/post-category-detail.component';
import { PaginatorModule } from 'primeng/paginator';
import { PanelModule } from 'primeng/panel';
import { CheckboxModule } from 'primeng/checkbox';
import { BadgeModule } from 'primeng/badge';
import { TableModule } from 'primeng/table';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { KeyFilterModule } from 'primeng/keyfilter';
import { TeduSharedModule } from 'src/app/shared/modules/tedu-shared.module';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
@NgModule({
  imports: [
    ContentRoutingModule,
    IconModule,
    CommonModule,
    ReactiveFormsModule,
    ChartjsModule,
    BlockUIModule,
    PaginatorModule,
    PanelModule,
    CheckboxModule,
    TableModule,
    ProgressSpinnerModule,
    BadgeModule,
    KeyFilterModule,
    TeduSharedModule,
    ButtonModule,
    InputTextModule,
  ],
  declarations: [PostCategoryComponent, PostCategoryDetailComponent],
})
export class ContentModule {}
