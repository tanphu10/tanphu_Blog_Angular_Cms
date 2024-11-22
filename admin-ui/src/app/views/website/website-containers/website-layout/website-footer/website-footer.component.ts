import { Component } from '@angular/core';
// import { FooterComponent } from '@coreui/angular';
import { FooterComponent, FooterModule } from '@coreui/angular';
@Component({
  selector: 'website-footer',
  templateUrl: './website-footer.component.html',
  styleUrls: ['./website-footer.component.scss'],
  standalone: true,
  imports: [FooterModule],
})
export class WebsiteFooterComponent extends FooterComponent {
  constructor() {
    super();
  }
}
