import { Component, CUSTOM_ELEMENTS_SCHEMA, OnInit } from '@angular/core';
import { NgFor } from '@angular/common';
import { RouterLink } from '@angular/router';
import {
  CarouselCaptionComponent,
  CarouselComponent,
  CarouselControlComponent,
  CarouselIndicatorsComponent,
  CarouselInnerComponent,
  CarouselItemComponent,
} from '@coreui/angular';
import { TabViewModule } from 'primeng/tabview';
import { CommonModule } from '@angular/common';
import { WebsiteFooterComponent } from '../website-containers/website-layout/website-footer/website-footer.component';
import { NewsEventComponent } from './website-news/website-news-event.component';
@Component({
  selector: 'website-app-landing',
  templateUrl: 'website-landing.component.html',
  styleUrls: ['./website-landing.component.scss'],
  // template: `<div>landing Page</div>`,
  standalone: true,
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  imports: [
    CarouselComponent,
    CarouselIndicatorsComponent,
    CarouselInnerComponent,
    NgFor,
    CarouselItemComponent,
    CarouselCaptionComponent,
    CarouselControlComponent,
    RouterLink,
    TabViewModule,
    CommonModule,
    WebsiteFooterComponent,
    NewsEventComponent,
  ],
})
export class LandingComponent implements OnInit {
  slides: any[] = new Array(3).fill({
    id: -1,
    src: '',
    title: '',
    subtitle: '',
  });

  ngOnInit(): void {
    this.slides[0] = {
      id: 0,
      src: './assets/img/controls/hinhnen_4.jpg',
      title: 'Our goals. Our mission',
      subtitle: 'How we help other companies to grow',
    };
    this.slides[1] = {
      id: 1,
      src: './assets/img/carousel/xd6.jpg',
      title: 'Our goals. Our mission',
      subtitle: 'How we help other companies to grow',
    };
    this.slides[2] = {
      id: 2,
      src: './assets/img/controls/hinhnen_3.jpg',
      title: 'Our goals. Our mission',
      subtitle: 'How we help other companies to grow',
    };
    this.slides[3] = {
      id: 3,
      src: './assets/img/carousel/xd2.jpg',
      title: 'Our goals. Our mission',
      subtitle: 'How we help other companies to grow',
    };
    this.slides[4] = {
      id: 4,
      src: './assets/img/carousel/xd3.jpg',
      title: 'Our goals. Our mission',
      subtitle: 'How we help other companies to grow',
    };
    this.slides[5] = {
      id: 5,
      src: './assets/img/carousel/xd4.jpg',
      title: 'Our goals. Our mission',
      subtitle: 'How we help other companies to grow',
    };
    this.slides[6] = {
      id: 6,
      src: './assets/img/carousel/xd6.jpg',
      title: 'Our goals. Our mission',
      subtitle: 'How we help other companies to grow',
    };
  }
}
