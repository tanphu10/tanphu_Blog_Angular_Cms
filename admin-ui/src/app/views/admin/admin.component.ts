import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { IconSetService } from '@coreui/icons-angular';
import { Title } from '@angular/platform-browser';
import { iconSubset } from 'src/app/icons/icon-subset';

@Component({
  selector: 'admin-root',
  template: ` <p-toast position="top-center"></p-toast>
    <p-confirmDialog
      header="Xác nhận"
      acceptLabel="Có"
      rejectLabel="Không"
      icon="pi pi-exclamation-triangle"
    ></p-confirmDialog>
    <router-outlet></router-outlet>`,
})
export class AdminComponent implements OnInit {
  title = 'core cms blog admin ui';

  constructor(private router: Router, private iconSetService: IconSetService) {
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
