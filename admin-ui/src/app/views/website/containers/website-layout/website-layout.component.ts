import { Component, CUSTOM_ELEMENTS_SCHEMA, OnInit } from '@angular/core';
import { WebsiteHeaderComponent } from './website-header/website-header.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-website-layout',
  templateUrl: './website-layout.component.html',
  styleUrls: ['./website-layout.component.scss'],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  standalone: true,
  imports: [CommonModule, WebsiteHeaderComponent],
})
export class WebSiteLayoutComponent {
  constructor() {
    console.log('check layout hiển thị lại');
  }
}
