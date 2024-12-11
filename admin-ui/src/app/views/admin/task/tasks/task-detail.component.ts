import { Component, OnInit, EventEmitter, OnDestroy } from '@angular/core';
import {
  Validators,
  FormControl,
  FormGroup,
  FormBuilder,
} from '@angular/forms';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { forkJoin, Subject, takeUntil } from 'rxjs';
import { UtilityService } from '../../../../shared/services/utility.service';
import {
  AdminApiProjectApiClient,
  AdminApiTaskApiClient,
  ProjectInListDto,
  TaskDto,
} from '../../../../api/admin-api.service.generated';
import { UploadService } from '../../../../shared/services/upload.service';
import { environment } from '../../../../../environments/environment';
import { ConfirmationService, MessageService } from 'primeng/api';
import { TokenStorageService } from 'src/app/shared/services/token-storage.service';
interface AutoCompleteCompleteEvent {
  originalEvent: Event;
  query: string;
}

@Component({
  templateUrl: 'task-detail.component.html',
  providers: [ConfirmationService, MessageService],
})
export class TaskDetailComponent implements OnInit, OnDestroy {
  private ngUnsubscribe = new Subject<void>();

  // Default
  public blockedPanelDetail: boolean = false;
  public form: FormGroup;
  public title: string;
  public btnDisabled = false;
  public saveBtnName: string;
  // public productCategories: any[] = [];

  selectedEntity = {} as TaskDto;
  public thumbnailImage: string; // Array to hold image previews
  // public catalogPdf;
  fromDate: Date | null = null;
  toDate: Date | null = null;
  public projectCategory: any[] = [];
  public priorityCategory: { label: string; value: number }[] = [
    { label: 'Low', value: 0 },
    { label: 'Medium', value: 1 },
    { label: 'High', value: 2 },
    { label: 'Very High', value: 3 },
  ];
  public statusCategory: { label: string; value: number }[] = [
    { label: 'Pending', value: 0 },
    { label: 'In Progress', value: 1 },
    { label: 'On Hold', value: 2 },
    { label: 'Completed', value: 3 },
    { label: 'Cancelled', value: 4 },
  ];
  public projectSlug?: string = null;

  tags: string[] | undefined;
  filteredTags: string[] | undefined;
  postTags: string[];
  formSavedEventEmitter: EventEmitter<any> = new EventEmitter();
  constructor(
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig,
    private utilService: UtilityService,
    private fb: FormBuilder,
    private taskApiClient: AdminApiTaskApiClient,
    private projectApiClient: AdminApiProjectApiClient,
    private uploadService: UploadService,
    private tokenService: TokenStorageService
  ) {}
  ngOnDestroy(): void {
    if (this.ref) {
      this.ref.close();
    }
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  public generateSlug() {
    var slug = this.utilService.makeSeoTitle(this.form.get('name').value);
    this.form.controls['slug'].setValue(slug);
  }
  // Validate
  noSpecial: RegExp = /^[^<>*!_~]+$/;
  validationMessages = {
    name: [
      { type: 'required', message: 'Bạn phải nhập tên' },
      { type: 'minlength', message: 'Bạn phải nhập ít nhất 3 kí tự' },
      { type: 'maxlength', message: 'Bạn không được nhập quá 255 kí tự' },
    ],
    slug: [{ type: 'required', message: 'Bạn phải URL duy nhất' }],
    description: [{ type: 'required', message: 'Bạn phải nhập mô tả ngắn' }],
  };

  ngOnInit() {
    //Init form
    this.buildForm();
    // this.toggleBlockUI(true);
    // if (this.utilService.isEmpty(this.config.data?.id) == false) {
    //   this.loadFormDetails(this.config.data?.id);
    // } else {
    //   this.toggleBlockUI(false);
    // }
    var projects = this.projectApiClient.getAllProjects();
    // console.log("projects",projects)

    // var tags = this.productApiClient.getAllTags();
    this.toggleBlockUI(true);
    forkJoin({
      projects,
      // tags,
    })
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (repsonse: any) => {
          var projects = repsonse.projects as ProjectInListDto[];
          projects.forEach((element) => {
            this.projectCategory.push({
              value: element.slug,
              label: element.name,
            });
          });
          if (this.utilService.isEmpty(this.config.data?.id) == false) {
            this.loadFormDetails(this.config.data?.id);
          } else {
            this.toggleBlockUI(false);
          }
        },
        error: () => {
          this.toggleBlockUI(false);
        },
      });
  }

  loadFormDetails(id: string) {
    // console.log("check id",id)
    this.taskApiClient
      .getTaskProject(id)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: TaskDto) => {
          console.log(response);
          this.selectedEntity = response;
          this.buildForm();
          this.toggleBlockUI(false);
        },
        error: () => {
          this.toggleBlockUI(false);
        },
      });
  }

  onFileChange(event) {
    const fileInput = event.target;
    if (event.target.files && event.target.files.length) {
      this.uploadService.uploadImage('projects', event.target.files).subscribe({
        next: (response: any) => {
          this.form.controls['thumbnail'].setValue(response.path);
          this.thumbnailImage = environment.API_URL + response.path;
        },
        error: (err: any) => {
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
    this.toggleBlockUI(true);
    if (this.utilService.isEmpty(this.config.data?.id)) {
      // console.log('check data->>>', this.form.value);
      this.taskApiClient
        .createTasks(this.form.value)
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe({
          next: () => {
            this.ref.close(this.form.value);
            this.toggleBlockUI(false);
          },
          error: () => {
            this.toggleBlockUI(false);
          },
        });
    } else {
      this.taskApiClient
        .updateTasks(this.config.data?.id, this.form.value)
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe({
          next: () => {
            console.log('update');
            this.toggleBlockUI(false);
            this.ref.close(this.form.value);
          },
          error: () => {
            this.toggleBlockUI(false);
          },
        });
    }
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

  buildForm() {
    const userId = this.tokenService.getUser().id;
    // Lấy userId từ token

    // console.log('project>>>');
    this.form = this.fb.group({
      name: new FormControl(
        this.selectedEntity.name || null,
        Validators.compose([
          Validators.required,
          Validators.maxLength(255),
          Validators.minLength(3),
        ])
      ),
      slug: new FormControl(
        this.selectedEntity.slug || null,
        Validators.required
      ),
      userId: new FormControl(
        userId, // Gán userId lấy từ token
        Validators.required
      ),

      description: new FormControl(
        this.selectedEntity.description || null,
        Validators.required
      ),
      priority: new FormControl(
        this.selectedEntity.priority || null,
        Validators.required
      ),
      status: new FormControl(
        this.selectedEntity.status || null,
        Validators.required
      ),
      dueDate: new FormControl(
        this.selectedEntity.dueDate || null,
        Validators.required
      ),
      startDate: new FormControl(
        this.selectedEntity.startDate || null,
        Validators.required
      ),
      projectSlug: new FormControl(
        this.selectedEntity.projectSlug || null,
        Validators.required
      ),
      timeTrackingSpent: new FormControl(
        this.selectedEntity.timeTrackingSpent || 0,
        Validators.required
      ),
      originalEstimate: new FormControl(
        this.selectedEntity.originalEstimate || 0,
        Validators.required
      ),
    });
  }
  filterTag(event: AutoCompleteCompleteEvent) {
    let filtered: string[] = [];
    let query = event.query;
    // console.log('query tag', query);
    for (let i = 0; i < (this.tags as any[]).length; i++) {
      let tag = (this.tags as string[])[i];
      if (tag.toLowerCase().indexOf(query.toLowerCase()) == 0) {
        filtered.push(tag);
      }
    }
    if (filtered.length == 0) {
      filtered.push(query);
    }

    this.filteredTags = filtered;
  }
}
