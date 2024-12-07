import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { IconModule } from '@coreui/icons-angular';
import { ChartjsComponent, ChartjsModule } from '@coreui/angular-chartjs';
import { BlockUIModule } from 'primeng/blockui';
import { PaginatorModule } from 'primeng/paginator';
import { PanelModule } from 'primeng/panel';
import { CheckboxModule } from 'primeng/checkbox';
import { BadgeModule } from 'primeng/badge';
import { TableModule } from 'primeng/table';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { KeyFilterModule } from 'primeng/keyfilter';
// import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { InputNumberModule } from 'primeng/inputnumber';
import { ImageModule } from 'primeng/image';
import { DynamicDialogModule } from 'primeng/dynamicdialog';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { EditorModule } from 'primeng/editor';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { AppLinkComponent } from './tasks/app-link.component';
import { CmsSharedModule } from '../../../../app/shared/modules/cms-shared.module';
import { TaskRoutingModule } from './task-routing.module';
import { TaskComponent } from './tasks/task.component';
import { TaskDetailComponent } from './tasks/task-detail.component';
import { CalendarModule } from 'primeng/calendar';
import { FormsModule } from '@angular/forms';
import { TagModule } from 'primeng/tag';
import { AvatarModule } from 'primeng/avatar';
import { ButtonModule } from 'primeng/button';
import { ToastModule } from 'primeng/toast';
import { ConfirmPopupModule } from 'primeng/confirmpopup';
import { MultiSelectModule } from 'primeng/multiselect';
import { DashboardTaskComponent } from '../task/dashboard-tasks/dashboard-task.component';
import { DashboardTaskDetailComponent } from '../task/dashboard-tasks/dashboard-task-detail.component';
import { ChartModule } from 'primeng/chart';

@NgModule({
  imports: [
    TaskRoutingModule,
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
    CmsSharedModule,
    ButtonModule,
    InputTextModule,
    InputTextareaModule,
    DropdownModule,
    EditorModule,
    InputNumberModule,
    ImageModule,
    AutoCompleteModule,
    DynamicDialogModule,
    CalendarModule,
    FormsModule,
    TagModule,
    AvatarModule,
    ToastModule,
    ConfirmPopupModule,
    MultiSelectModule,
    ChartjsComponent,
  ],
  declarations: [
    TaskComponent,
    TaskDetailComponent,
    AppLinkComponent,
    DashboardTaskDetailComponent,
    DashboardTaskComponent,
  ],
})
export class TaskModule {}
