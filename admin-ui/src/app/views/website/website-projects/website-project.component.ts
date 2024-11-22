import { Component, CUSTOM_ELEMENTS_SCHEMA, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'website-app-project',
  templateUrl: 'website-project.component.html',
  styleUrls: ['./website-project.component.scss'],
  standalone: true,
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  imports: [],
})
export class ProjectComponent implements OnInit {
  constructor(public router: Router) {}
  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }
}
