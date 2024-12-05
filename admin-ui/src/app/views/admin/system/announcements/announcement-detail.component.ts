import { Component, OnInit, EventEmitter, OnDestroy } from '@angular/core';
import {
    Validators,
    FormControl,
    FormGroup,
    FormBuilder,
} from '@angular/forms';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Subject, takeUntil } from 'rxjs';
import { AdminApiAnnouncementApiClient, AdminApiProjectApiClient, AnnouncementViewModel, CreateAnnouncementRequest, ProjectInListDto } from 'src/app/api/admin-api.service.generated';
import { UtilityService } from 'src/app/shared/services/utility.service';


@Component({
    templateUrl: 'announcement-detail.component.html',
})
export class AnnouncementDetailComponent implements OnInit, OnDestroy {
    private ngUnsubscribe = new Subject<void>();

    // Default
    public blockedPanelDetail: boolean = false;
    public form: FormGroup;
    public title: string;
    public btnDisabled = false;
    public saveBtnName: string;
    public closeBtnName: string;
    selectedEntity = {} as CreateAnnouncementRequest;

    formSavedEventEmitter: EventEmitter<any> = new EventEmitter();


    public projectId?: string = null;
    public projectCategory: any[] = [];
    constructor(
        private projectApiClient: AdminApiProjectApiClient,
        public ref: DynamicDialogRef,
        public config: DynamicDialogConfig,
        private announcementService: AdminApiAnnouncementApiClient,
        private utilService: UtilityService,
        private fb: FormBuilder
    ) { }

    ngOnDestroy(): void {
        if (this.ref) {
            this.ref.close();
        }
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();
    }

    ngOnInit() {
        this.loadProjects();
        this.buildForm();
        if (this.utilService.isEmpty(this.config.data?.id) == false) {
            this.loadDetail(this.config.data.id);
            this.saveBtnName = 'Cập nhật';
            this.closeBtnName = 'Hủy';
        } else {
            this.saveBtnName = 'Thêm';
            this.closeBtnName = 'Đóng';
        }
    }

    loadProjects() {
        this.projectApiClient
          .getAllProjects()
          .subscribe((response: ProjectInListDto[]) => {
            response.forEach((element) => {
              this.projectCategory.push({
                value: element.id,
                label: element.name,
              });
            });
          });
      }
    // Validate
    noSpecial: RegExp = /^[^<>*!_~]+$/;
    validationMessages = {
        name: [
            { type: 'required', message: 'Bạn phải nhập tên quyền' },
            { type: 'minlength', message: 'Bạn phải nhập ít nhất 3 kí tự' },
            { type: 'maxlength', message: 'Bạn không được nhập quá 255 kí tự' },
        ],
        displayName: [{ type: 'required', message: 'Bạn phải tên hiển thị' }],
    };

    loadDetail(id: any) {
        // console.log("id detail",id)
        this.toggleBlockUI(true);
        this.announcementService
            .deleteAnnouncementById(id)
            .pipe(takeUntil(this.ngUnsubscribe))
            .subscribe({
                next: (response: any) => {
                    // console.log("response",response)
                    this.selectedEntity = response;
                    this.buildForm();
                    this.toggleBlockUI(false);
                },
                error: (error:any) => {
                    // console.log("check lỗi get id",error)
                    this.toggleBlockUI(false);
                },
            });
    }
    saveChange() {
        this.toggleBlockUI(true);
        this.saveData();
    }

    private saveData() {
        if (this.utilService.isEmpty(this.config.data?.id)) {
            this.announcementService
                .createNotification(this.form.value)
                .pipe(takeUntil(this.ngUnsubscribe))
                .subscribe(() => {
                    this.ref.close(this.form.value);
                    this.toggleBlockUI(false);
                });
        } 
    }

    buildForm() {
        this.form = this.fb.group({
            title: new FormControl(
                this.selectedEntity.title || null,
                Validators.compose([
                    Validators.required,
                    Validators.maxLength(255),
                    Validators.minLength(3),
                ])
            ),
            content: new FormControl(
                this.selectedEntity.content || null,
                Validators.required
            ),
            userId: new FormControl(
                this.selectedEntity.userId || null,
            ),
            status: new FormControl(
                this.selectedEntity.status || true,
            ),
            projectId: new FormControl(
                this.selectedEntity.projectId || null,
                Validators.required
              ),
            // dateCreated: new FormControl(
            //     this.selectedEntity.dateCreated || new Date(),
            // ),
        });
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
}