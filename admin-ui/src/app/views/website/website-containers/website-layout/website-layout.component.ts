import {
  Component,
  CUSTOM_ELEMENTS_SCHEMA,
  HostListener,
  OnInit,
} from '@angular/core';
import { WebsiteHeaderComponent } from './website-header/website-header.component';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ProgressSpinnerModule } from 'primeng/progressspinner';

@Component({
  selector: 'app-website-layout',
  templateUrl: './website-layout.component.html',
  styleUrls: ['./website-layout.component.scss'],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  standalone: true,
  imports: [
    CommonModule,
    WebsiteHeaderComponent,
    RouterModule,
    ProgressSpinnerModule,
  ],
})
export class WebSiteLayoutComponent {
  isSmallScreen: boolean = false;
  @HostListener('window:resize', [])
  onResize() {
    this.checkScreenSize();
  }
  checkScreenSize() {
    this.isSmallScreen = window.innerWidth <= 200;
  }
  constructor() {
    // console.log('check layout hiển thị lại');
    this.checkScreenSize();
  }
}
