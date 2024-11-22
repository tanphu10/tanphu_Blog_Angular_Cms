import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
@Injectable({
  providedIn: 'root',
})
export class TabService {
  private activeIndexSource = new BehaviorSubject<number>(0);
  activeIndex$ = this.activeIndexSource.asObservable();
  constructor() {}
  setActiveIndex(index: number): void {
    // console.log("servicetab",index)
    this.activeIndexSource.next(index);
  }
}
