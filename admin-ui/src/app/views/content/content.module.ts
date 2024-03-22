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
import { DropdownModule } from 'primeng/dropdown';
import { InputNumberModule } from 'primeng/inputnumber';
import { ImageModule } from 'primeng/image';
import { DynamicDialogModule } from 'primeng/dynamicdialog';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { EditorModule } from 'primeng/editor';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { PostDetailComponent } from './posts/post-detail.component';
import { PostSeriesComponent } from './posts/post-series.component';
import { PostReturnReasonComponent } from './posts/post-return-reason.component';
import { PostActivityLogsComponent } from './posts/post-activity-logs.component';
import { PostComponent } from './posts/post.component';
import { SeriesComponent } from './series/series.component';
import { SeriesDetailComponent } from './series/series-detail.component';
import { SeriesPostsComponent } from './series/series-post.component';

@NgModule({
  imports: [
    ContentRoutingModule,
    IconModule,
    CommonModule,
    ReactiveFormsModule,
    ChartjsModule,
    ProgressSpinnerModule,
    PanelModule,
    BlockUIModule,
    PaginatorModule,
    BadgeModule,
    CheckboxModule,
    TableModule,
    KeyFilterModule,
    TeduSharedModule,
    ButtonModule,
    InputTextModule,
    InputTextareaModule,
    DropdownModule,
    EditorModule,
    InputNumberModule,
    ImageModule,
    AutoCompleteModule,
    DynamicDialogModule
  ],
  declarations: [
    PostComponent,
    PostDetailComponent,
    PostCategoryComponent,
    PostCategoryDetailComponent,
    SeriesComponent,
    SeriesDetailComponent,
    SeriesPostsComponent,
    PostSeriesComponent,
    PostReturnReasonComponent,
    PostSeriesComponent,
    PostActivityLogsComponent,
  ],
})
export class ContentModule {}
