import {
  Component,
  OnInit,
  EventEmitter,
  OnDestroy,
  CUSTOM_ELEMENTS_SCHEMA,
} from '@angular/core';
import {
  Validators,
  FormControl,
  FormGroup,
  FormBuilder,
  ReactiveFormsModule,
} from '@angular/forms';
import { forkJoin, Subject, takeUntil } from 'rxjs';
import { CommonModule, formatDate } from '@angular/common';
import {
  AdminApiRoleApiClient,
  AdminApiUserApiClient,
  RoleDto,
  UserDto,
} from '../../../../app/api/admin-api.service.generated';
import { UploadService } from '../../../../app/shared/services/upload.service';
import { environment } from '../../../../environments/environment';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { ImageModule } from 'primeng/image';
import { BlockUIModule } from 'primeng/blockui';
import { CheckboxModule } from 'primeng/checkbox';
import { InputNumberModule } from 'primeng/inputnumber';
import { CmsSharedModule } from 'src/app/shared/modules/cms-shared.module';
import { KeyFilterModule } from 'primeng/keyfilter';
import { PanelModule } from 'primeng/panel';
import { WebsiteSubHeader } from '../website-containers/website-layout/website-sub-header/website-sub-header.component';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { TokenStorageService } from 'src/app/shared/services/token-storage.service';
import { WebsiteFooterComponent } from '../website-containers/website-layout/website-footer/website-footer.component';
import { TabService } from 'src/app/shared/services/tab.service';
@Component({
  templateUrl: 'website-system.component.html',
  styleUrls: ['./website-system.component.scss'],
  standalone: true,
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  imports: [
    ProgressSpinnerModule,
    ImageModule,
    BlockUIModule,
    CheckboxModule,
    InputNumberModule,
    CmsSharedModule,
    KeyFilterModule,
    PanelModule,
    ReactiveFormsModule,
    WebsiteSubHeader,
    CardModule,
    ButtonModule,
    CommonModule,
    WebsiteFooterComponent,
  ],
})
export class WebsiteSystemComponent implements OnInit, OnDestroy {
  private ngUnsubscribe = new Subject<void>();

  // Default
  public blockedPanelDetail: boolean = false;
  public form: FormGroup;
  public title: string;
  public btnDisabled = false;
  public saveBtnName: string;
  public roles: any[] = [];
  public selectedEntity = {} as UserDto;
  public avatarImage;

  formSavedEventEmitter: EventEmitter<any> = new EventEmitter();

  constructor(
    private roleService: AdminApiRoleApiClient,
    private userService: AdminApiUserApiClient,
    private fb: FormBuilder,
    private uploadService: UploadService,
    private token: TokenStorageService,
    private tabService: TabService
  ) {}
  ngOnDestroy(): void {
    // if (this.ref) {
    //   this.ref.close();
    // }
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
  // Validate
  noSpecial: RegExp = /^[^<>*!_~]+$/;
  validationMessages = {
    fullName: [{ type: 'required', message: 'Bạn phải nhập tên' }],
    email: [{ type: 'required', message: 'Bạn phải nhập email' }],
    userName: [{ type: 'required', message: 'Bạn phải nhập tài khoản' }],
    password: [
      { type: 'required', message: 'Bạn phải nhập mật khẩu' },
      {
        type: 'pattern',
        message:
          'Mật khẩu ít nhất 8 ký tự, ít nhất 1 số, 1 ký tự đặc biệt, và một chữ hoa',
      },
    ],
    royaltyAmountPerPost: [
      { type: 'required', message: 'Bạn phải nhập nhuận bút' },
    ],
    phoneNumber: [{ type: 'required', message: 'Bạn phải nhập số điện thoại' }],
  };

  ngOnInit() {
    //Init form
    this.buildForm();
    var roles = this.roleService.getAllRole();
    this.toggleBlockUI(true);
    forkJoin({
      roles,
    })
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (repsonse: any) => {
          //Push categories to dropdown list
          var roles = repsonse.roles as RoleDto[];
          roles.forEach((element) => {
            this.roles.push({
              value: element.id,
              label: element.name,
            });
          });

          // if (this.utilService.isEmpty(this.config.data?.id) == false) {
          this.loadFormDetails(this.token.getUser().id);
          // } else {
          this.setMode('update');
          this.toggleBlockUI(false);
          // }
        },
        error: () => {
          this.toggleBlockUI(false);
        },
      });
  }
  // loadInformation(){
  //   this.userService.getUserById
  // }
  loadFormDetails(id: string) {
    this.userService
      .getUserById(id)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: UserDto) => {
          this.selectedEntity = response;
          this.buildForm();
          this.setMode('update');

          this.toggleBlockUI(false);
        },
        error: () => {
          this.toggleBlockUI(false);
        },
      });
  }

  onFileChange(event) {
    if (event.target.files && event.target.files.length) {
      this.uploadService.uploadImage('avatars', event.target.files).subscribe({
        next: (response: any) => {
          // console.log(response)
          this.form.controls['avatar'].setValue(response.path);
          this.tabService.setUserImage(response.path); // Gửi hình ảnh lên service

          this.avatarImage = environment.API_URL + response.path;
        },
        error: () => {
          // console.log(err);
        },
      });
    }
  }
  saveChange() {
    this.toggleBlockUI(true);

    this.saveData();
  }

  private saveData() {
    console.log('check data', this.form.value);
    this.toggleBlockUI(true);
    this.userService
      .updateUser(this.token.getUser().id, this.form.value)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: () => {
          console.log('update tc');

          // console.log("update user")
          this.toggleBlockUI(false);
          // this.ref.close(this.form.value);
        },
        error: () => {
          console.log('update thất bại');

          this.toggleBlockUI(false);
        },
      });
    // }
  }
  private toggleBlockUI(enabled: boolean) {
    if (enabled == true) {
      this.btnDisabled = true;
      this.blockedPanelDetail = true;
    } else {
      setTimeout(() => {
        this.btnDisabled = false;
        this.blockedPanelDetail = false;
      }, 1000);
    }
  }

  setMode(mode: string) {
    if (mode == 'update') {
      this.form.controls['userName'].clearValidators();
      this.form.controls['userName'].disable();
      this.form.controls['email'].clearValidators();
      this.form.controls['email'].disable();
      this.form.controls['password'].clearValidators();
      this.form.controls['password'].disable();
    } else if (mode == 'create') {
      this.form.controls['userName'].addValidators(Validators.required);
      this.form.controls['userName'].enable();
      this.form.controls['email'].addValidators(Validators.required);
      this.form.controls['email'].enable();
      this.form.controls['password'].addValidators(Validators.required);
      this.form.controls['password'].enable();
    }
  }
  buildForm() {
    this.form = this.fb.group({
      firstName: new FormControl(
        this.selectedEntity.firstName || null,
        Validators.required
      ),
      lastName: new FormControl(
        this.selectedEntity.lastName || null,
        Validators.required
      ),
      userName: new FormControl(
        this.selectedEntity.userName || null,
        Validators.required
      ),
      email: new FormControl(
        this.selectedEntity.email || null,
        Validators.required
      ),
      phoneNumber: new FormControl(
        this.selectedEntity.phoneNumber || null,
        Validators.required
      ),
      password: new FormControl(
        null,
        Validators.compose([
          Validators.required,
          Validators.pattern(
            '^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[$@$!%*?&])[A-Za-zd$@$!%*?&].{8,}$'
          ),
        ])
      ),
      dob: new FormControl(
        this.selectedEntity.dob
          ? formatDate(this.selectedEntity.dob, 'yyyy-MM-dd', 'en')
          : null
      ),
      // avatarFile: new FormControl(null),
      avatar: new FormControl(this.selectedEntity.avatar || null),
      isActive: new FormControl(this.selectedEntity.isActive || true),
      royaltyAmountPerPost: new FormControl(
        this.selectedEntity.royaltyAmountPerPost || 0,
        Validators.required
      ),
    });
    if (this.selectedEntity.avatar) {
      this.avatarImage = environment.API_URL + this.selectedEntity.avatar;
    }
  }
}
