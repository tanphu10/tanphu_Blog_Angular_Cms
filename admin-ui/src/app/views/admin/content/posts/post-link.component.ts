// pdf-link.component.ts
import { Component, Input } from '@angular/core';
import {environment} from '../../../../../environments/environment'
@Component({
  selector: 'app-pdf-link',
  template: `
    <a *ngIf="pdfPath" [href]="getPdfUrl()" target="_blank">link</a>

  `
})
export class PostLinkComponent {
  @Input() pdfPath: string; // Nhận đường dẫn PDF từ component cha

  // Phương thức để kết hợp đường dẫn
  getPdfUrl(): string {
    return `${environment.API_URL}/${this.pdfPath}`;
  }
}
