import { Component, OnInit, EventEmitter, OnDestroy } from '@angular/core';
import {
  Validators,
  FormControl,
  FormGroup,
  FormBuilder,
} from '@angular/forms';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { forkJoin, Subject, takeUntil } from 'rxjs';
import { UtilityService } from '../../../shared/services/utility.service';
import * as adminApiServiceGenerated from '../../../api/admin-api.service.generated';
import { UploadService } from '../../../shared/services/upload.service';
import { environment } from '../../../../environments/environment';
import {
  AdminApiInventoryApiClient,
  AdminApiProjectApiClient,
  InventoryEntryDto,
  InventoryInListDto,
  ProjectDto,
  ProjectInListDto,
} from '../../../api/admin-api.service.generated';
interface AutoCompleteCompleteEvent {
  originalEvent: Event;
  query: string;
}
@Component({
  templateUrl: 'inventory-detail.component.html',
})
export class InventoryDetailComponent implements OnInit, OnDestroy {
  private ngUnsubscribe = new Subject<void>();

  // Default
  public blockedPanelDetail: boolean = false;
  public form: FormGroup;
  public title: string;
  public btnDisabled = false;
  public saveBtnName: string;
  public projectCategories: any[] = [];
  // public contentTypes: any[] = [];
  // public series: any[] = [];

  selectedEntity = {} as InventoryEntryDto;
  public thumbnailImages: string[] = []; // Array to hold image previews
  public catalogPdf;

  tags: string[] | undefined;
  filteredTags: string[] | undefined;
  postTags: string[];
  formSavedEventEmitter: EventEmitter<any> = new EventEmitter();
  constructor(
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig,
    private utilService: UtilityService,
    private fb: FormBuilder,
    private inventoryApiClient: AdminApiInventoryApiClient,
    private projectApiClient: AdminApiProjectApiClient,
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
    console.log("sss")
    //Init form
    this.buildForm();
    //Load data to form
    var projects = this.projectApiClient.getAllProjects();
    console.log("projects",projects)

    // var tags = this.productApiClient.getAllTags();
    this.toggleBlockUI(true);
    forkJoin({
      projects,
      // tags,
    })
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (repsonse: any) => {
          //Push categories to dropdown list
          // this.tags = repsonse.tags as string[];

          var projects = repsonse.project as ProjectInListDto[];
          projects.forEach((element) => {
            this.projectCategories.push({
              value: element.id,
              label: element.name,
            });
          });
          if (this.utilService.isEmpty(this.config.data?.id) == false) {
            // this.postApiClient
            //   .getPostTags(this.config.data?.id)
            //   .subscribe((res) => {
            //     this.postTags = res;
            //   });
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
    console.log('check id', id);
    this.inventoryApiClient
      .getInventoryById(id)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: InventoryEntryDto) => {
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
    if (fileInput?.files && fileInput.files.length) {
      this.uploadService.uploadImage('products', fileInput.files).subscribe({
        next: (response: any[]) => {
          // Assuming `response` is an array of paths for each uploaded image
          const paths = response.map((file) => file.path);

          // Set the form control with the array of paths
          this.form.controls['thumbnail'].setValue(paths);

          // Update `thumbnailImages` to display each image URL
          this.thumbnailImages = paths.map(
            (path) => `${environment.API_URL}${path}`
          );
        },
        error: (err: any) => {
          console.error(err);
        },
      });
    }
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
  onFilePdfChange(event) {
    if (event.target.files && event.target.files.length) {
      this.uploadService.uploadPdf('inventories', event.target.files).subscribe({
        next: (response: any) => {
          this.form.controls['pdf'].setValue(response.path);
          this.catalogPdf = environment.API_URL + response.path;
        },
        error: (err: any) => {
          console.log(err);
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
      console.log('check data->>>', this.form.value);
      this.inventoryApiClient
        .purchaseOrder(this.form.value)
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
      this.inventoryApiClient
        .saleItem(this.config.data?.id, this.form.value)
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
    // console.log('post cate>>>', this.productCategories);
    this.form = this.fb.group({
      // id: new FormControl(
      //   this.selectedEntity.name || null,
      //   Validators.compose([
      //     Validators.required,
      //     Validators.maxLength(255),
      //     Validators.minLength(3),
      //   ])
      // ),
      // documentType: new FormControl(
      //   this.selectedEntity.documentType || null,
      //   Validators.required
      // ),
      documentNo: new FormControl(
        this.selectedEntity.documentNo || null,
        Validators.required
      ),
      itemNo: new FormControl(
        this.selectedEntity.itemNo || null,
        Validators.required
      ),
      quantity: new FormControl(this.selectedEntity.quantity || 0),
      filePdf: new FormControl(this.selectedEntity.filePdf || null),
      thumbnail: new FormControl(this.selectedEntity.thumbnail || []),
      projectId: new FormControl(this.selectedEntity.projectId ),
      notice: new FormControl(this.selectedEntity.notice || null),
    });
    if (this.selectedEntity.thumbnail) {
      this.thumbnailImages = this.selectedEntity.thumbnail.map(
        (imgPath: string) => `${environment.API_URL}${imgPath}`
      );
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
