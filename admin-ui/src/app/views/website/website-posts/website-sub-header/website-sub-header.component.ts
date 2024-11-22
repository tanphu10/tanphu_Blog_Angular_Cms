import { Component, OnInit } from '@angular/core';
import { CarouselModule } from 'primeng/carousel';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';

@Component({
  selector: 'website-sub-header-post',
  templateUrl: './website-sub-header.component.html',
  styleUrls: ['./website-sub-header.component.scss'],
  standalone: true,
  imports: [CarouselModule, ButtonModule, TagModule],
  providers: [],
})
export class WebsiteSubHeaderPost {
  constructor() {}
}
