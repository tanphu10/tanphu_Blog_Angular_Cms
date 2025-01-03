import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { IconModule } from '@coreui/icons-angular';
import { ChartjsModule } from '@coreui/angular-chartjs';
import { InventoryRoutingModule } from './inventory-routing.module';
import { BlockUIModule } from 'primeng/blockui';
import { PaginatorModule } from 'primeng/paginator';
import { PanelModule } from 'primeng/panel';
import { CheckboxModule } from 'primeng/checkbox';
import { BadgeModule } from 'primeng/badge';
import { TableModule } from 'primeng/table';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { KeyFilterModule } from 'primeng/keyfilter';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { InputNumberModule } from 'primeng/inputnumber';
import { ImageModule } from 'primeng/image';
import { DynamicDialogModule } from 'primeng/dynamicdialog';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { EditorModule } from 'primeng/editor';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { AppLinkComponent } from './products/app-link.component';
import { InventoryComponent } from './inventories/inventory.component';
import {ProductComponent} from './products/product.component';
import {ProductCategoryComponent} from './product-categories/product-category.component';
import { ProductCategoryDetailComponent } from './product-categories/product-category-detail.component';
import { ProductDetailComponent } from './products/product-detail.component';
import { InventoryDetailComponent } from './inventories/inventory-detail.component';
import { CmsSharedModule } from '../../../shared/modules/cms-shared.module';
import { CardModule } from '@coreui/angular';
import { DividerModule } from 'primeng/divider';
import { TabViewModule } from 'primeng/tabview';
import { TagModule } from 'primeng/tag';
import { WebsiteFooterComponent } from '../../website/website-containers/website-layout/website-footer/website-footer.component';
import { WebsiteSubHeader } from '../../website/website-containers/website-layout/website-sub-header/website-sub-header.component';
import { CalendarModule } from 'primeng/calendar';
import { FormsModule } from '@angular/forms';



@NgModule({
  imports: [
    InventoryRoutingModule,
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
    DividerModule,
    CardModule,
    ButtonModule,
    TabViewModule,
    CommonModule,
    WebsiteSubHeader,
    BlockUIModule,
    PaginatorModule,
    PanelModule,
    WebsiteFooterComponent,
    TagModule,
    DropdownModule,
    CalendarModule,
    FormsModule,
  ],
  declarations: [
    InventoryComponent,
    ProductComponent,
    ProductCategoryComponent,
    ProductCategoryDetailComponent,
    ProductDetailComponent,
    AppLinkComponent,
    InventoryDetailComponent    
  ],
})
export class InventoryModule {}
