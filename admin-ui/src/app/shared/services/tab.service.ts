import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { TokenStorageService } from './token-storage.service';
@Injectable({
  providedIn: 'root',
})
export class TabService {
  private activeIndexSource = new BehaviorSubject<number>(0);
  activeIndex$ = this.activeIndexSource.asObservable();

  private userImageSource = new BehaviorSubject<string | null>(null);
  userImage$ = this.userImageSource.asObservable();

  constructor(private tokenSerive: TokenStorageService) {
    // const savedUser = localStorage.getItem('auth-token');
    const userImageStorage = this.tokenSerive.getUser().avatar;
    if (userImageStorage) {
      // console.log('kiểm tra khi load data', userImageStorage);

      this.setUserImage(userImageStorage);
    }
  }
  setActiveIndex(index: number): void {
    // console.log("servicetab",index)
    this.activeIndexSource.next(index);
  }
  setUserImage(imageUrl: string): void {
    this.userImageSource.next(imageUrl);
  }
}
