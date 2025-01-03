import { Component, OnDestroy } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import {
  AdminApiAuthApiClient,
  AuthenticatedResult,
  LoginRequest,
} from 'src/app/api/admin-api.service.generated';
import { AlertService } from 'src/app/shared/services/alert.service';
import { UrlConstants } from 'src/app/shared/constants/url.constants';
import { TokenStorageService } from 'src/app/shared/services/token-storage.service';
import { Subject, takeUntil } from 'rxjs';
import { TabService } from 'src/app/shared/services/tab.service';
@Component({
  selector: 'app-web-login',
  templateUrl: './login-web.component.html',
  styleUrls: ['./login-web.component.scss'],
})
export class LoginWebComponent implements OnDestroy {
  loginForm: FormGroup;
  private ngUnsubscribe = new Subject<void>();
  loading = false;

  constructor(
    private fb: FormBuilder,
    private authApiClient: AdminApiAuthApiClient,
    private alertService: AlertService,
    private router: Router,
    private tokenSerivce: TokenStorageService,
    private tabService: TabService
  ) {
    this.loginForm = this.fb.group({
      userName: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required),
    });
  }
  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  login() {
    this.loading = true;
    var request: LoginRequest = new LoginRequest({
      userName: this.loginForm.controls['userName'].value,
      password: this.loginForm.controls['password'].value,
    });

    this.authApiClient
      .login(request)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (res: AuthenticatedResult) => {
          // console.log("check trả về res",res)
          //Save token and refresh token to localstorage
          this.tokenSerivce.saveToken(res.token);
          this.tokenSerivce.saveRefreshToken(res.refreshToken);
          this.tokenSerivce.saveUser(res);

          this.tabService.setUserImage(this.tokenSerivce.getUser()?.avatar);
          //Redirect to dashboard
          this.router.navigate([UrlConstants.HOME_WEB]);
          // console.log('check web', res);
        },
        error: (error: any) => {
          this.alertService.showError('Đăng nhập không đúng.');
          this.loading = false;
        },
      });
  }
}
