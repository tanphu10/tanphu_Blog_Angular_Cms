import {
  Component,
  OnInit,
  EventEmitter,
  OnDestroy,
} from '@angular/core';
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
  ProjectDto,
} from '../../../../api/admin-api.service.generated';
import { UploadService } from '../../../../shared/services/upload.service';
import { environment } from '../../../../../environments/environment';
interface AutoCompleteCompleteEvent {
  originalEvent: Event;
  query: string;
}
@Component({
  templateUrl: 'project-detail.component.html',
})
export class ProjectDetailComponent implements OnInit, OnDestroy {
  private ngUnsubscribe = new Subject<void>();

  // Default
  public blockedPanelDetail: boolean = false;
  public form: FormGroup;
  public title: string;
  public btnDisabled = false;
  public saveBtnName: string;
  // public productCategories: any[] = [];


  selectedEntity = {} as ProjectDto;
  public thumbnailImage: string ; // Array to hold image previews
  // public catalogPdf;

  tags: string[] | undefined;
  filteredTags: string[] | undefined;
  postTags: string[];
  formSavedEventEmitter: EventEmitter<any> = new EventEmitter();
  constructor(
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig,
    private utilService: UtilityService,
    private fb: FormBuilder,
    private projectApiClient: AdminApiProjectApiClient,
    // private productCategoryApiClient: AdminApiProductCategoryApiClient,
    private uploadService: UploadService
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
    this.toggleBlockUI(true);
    if (this.utilService.isEmpty(this.config.data?.id) == false) {
      this.loadFormDetails(this.config.data?.id);
    } else {
      this.toggleBlockUI(false);
    }
  }
  loadFormDetails(id: string) {
    // console.log("check id",id)
    this.projectApiClient
      .getProjectById(id)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: ProjectDto) => {
          // console.log(response);
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

    // if (event.target.files && event.target.files.length) {
    //   this.uploadService.uploadImage('products', event.target.files).subscribe({
    //             next: (response: any) => {
    //       this.form.controls['thumbnail'].setValue(response.path);
    //       this.thumbnailImage = environment.API_URL + response.path;
    //     },
    //     error: (err: any) => {
    //       console.log(err);
    //     },
    //   });
    // }
  }
  // onFilePdfChange(event) {
  //   if (event.target.files && event.target.files.length) {
  //     this.uploadService.uploadPdf('products', event.target.files).subscribe({
  //       next: (response: any) => {
  //         this.form.controls['catalogPdf'].setValue(response.path);
  //         this.catalogPdf = environment.API_URL + response.path;
  //       },
  //       error: (err: any) => {
  //         console.log(err);
  //       },
  //     });
  //   }
  // }
  saveChange() {
    this.toggleBlockUI(true);
    this.saveData();
  }

  private saveData() {
    this.toggleBlockUI(true);
    if (this.utilService.isEmpty(this.config.data?.id)) {
      // console.log('check data->>>', this.form.value);
      this.projectApiClient
        .createProject(this.form.value)
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
      this.projectApiClient
        .updateProject(this.config.data?.id, this.form.value)
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe({
          next: () => {
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
      // proCategoryId: new FormControl(
      //   this.selectedEntity.proCategoryId || null,
      //   Validators.required
      // ),
      description: new FormControl(
        this.selectedEntity.description || null,
        Validators.required
      ),
      seoDescription: new FormControl(this.selectedEntity.seoDescription || null),
      thumbnail: new FormControl(this.selectedEntity.thumbnail || null),
      content: new FormControl(this.selectedEntity.content || null),
      sortOrder: new FormControl(this.selectedEntity.sortOrder || 0),
      isActive: new FormControl(this.selectedEntity.isActive || true),
    });
    if (this.selectedEntity.thumbnail) {
      this.thumbnailImage = environment.API_URL + this.selectedEntity.thumbnail;
    }
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
