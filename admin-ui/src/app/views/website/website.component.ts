import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { IconSetService } from '@coreui/icons-angular';
import { Title } from '@angular/platform-browser';
import { iconSubset } from 'src/app/icons/icon-subset';

@Component({
  selector: 'website-root',
  template: `<p-toast position="top-center"></p-toast>
    <router-outlet></router-outlet>
    <p-confirmDialog
      header="Xác nhận"
      acceptLabel="Có"
      rejectLabel="Không"
      icon="pi pi-exclamation-triangle"
    ></p-confirmDialog> `,
})
export class WebsiteComponent implements OnInit {
  title = 'core cms blog admin ui';
  constructor(
    private router: Router,
    private titleService: Title,
    private iconSetService: IconSetService
  ) {
    titleService.setTitle(this.title);
    iconSetService.icons = { ...iconSubset };
  }
  ngOnInit(): void {
    this.router.events.subscribe((evt) => {
      if (!(evt instanceof NavigationEnd)) {
        return;
      }
    });
  }
}
