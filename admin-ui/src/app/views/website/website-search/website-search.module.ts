import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { BlockUIModule } from 'primeng/blockui';
import { PaginatorModule } from 'primeng/paginator';
import { PanelModule } from 'primeng/panel';
import { CheckboxModule } from 'primeng/checkbox';
import { SharedModule } from 'primeng/api';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { KeyFilterModule } from 'primeng/keyfilter';
import { BadgeModule } from 'primeng/badge';
import { PickListModule } from 'primeng/picklist';
import { ImageModule } from 'primeng/image';
import { CmsSharedModule } from '../../../shared/modules/cms-shared.module';
import { WebsiteSearchRoutingModule } from './website-search-routing.module';

@NgModule({
  imports: [
    WebsiteSearchRoutingModule,
    CommonModule,
    ReactiveFormsModule,
    TableModule,
    ProgressSpinnerModule,
    BlockUIModule,
    PaginatorModule,
    PanelModule,
    CheckboxModule,
    ButtonModule,
    InputTextModule,
    KeyFilterModule,
    SharedModule,
    CmsSharedModule,
    BadgeModule,
    PickListModule,
    ImageModule,
    CmsSharedModule,
  ],
  declarations: [],
})
export class WebsiteSearchModule {}
