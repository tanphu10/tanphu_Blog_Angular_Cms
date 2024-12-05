import { Component, Input, OnInit } from '@angular/core';
import { CarouselModule } from 'primeng/carousel';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';

@Component({
  selector: 'website-sub-header',
  templateUrl: './website-sub-header.component.html',
  styleUrls: ['./website-sub-header.component.scss'],
  standalone: true,
  imports: [CarouselModule, ButtonModule, TagModule],
  providers: [],
})
export class WebsiteSubHeader {
  @Input() headerText: string = ''; // Biến nhận giá trị từ component cha

  constructor() {}
}
