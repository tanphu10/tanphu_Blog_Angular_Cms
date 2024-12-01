import { Component, OnDestroy, OnInit } from '@angular/core';
import { ConfirmationService } from 'primeng/api';
import { DialogService, DynamicDialogComponent } from 'primeng/dynamicdialog';
import { Subject, takeUntil } from 'rxjs';
import { MessageConstants } from 'src/app/shared/constants/message.constant';
import { PostDetailComponent } from './post-detail.component';
import { AlertService } from 'src/app/shared/services/alert.service';
import {
  AdminApiPostApiClient,
  AdminApiPostCategoryApiClient,
  AdminApiProjectApiClient,
  PostCategoryDto,
  PostDto,
  PostInListDto,
  PostInListDtoPageResult,
  ProjectInListDto,
} from 'src/app/api/admin-api.service.generated';
import { PostSeriesComponent } from './post-series.component';
import { PostReturnReasonComponent } from './post-return-reason.component';
import { PostActivityLogsComponent } from './post-activity-logs.component';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.scss'],
})
export class PostComponent implements OnInit, OnDestroy {
  //System variables
  private ngUnsubscribe = new Subject<void>();
  public blockedPanel: boolean = false;

  //Paging variables
  public pageIndex: number = 1;
  public pageSize: number = 10;
  public totalCount: number;

  //Business variables
  public items: PostInListDto[];
  public selectedItems: PostInListDto[] = [];
  public keyword: string = '';

  public categoryId?: string = null;
  public postCategories: any[] = [];
  public projectCategory: any[] = [];
  public projectId?: string = null;

  constructor(
    private postCategoryApiClient: AdminApiPostCategoryApiClient,
    private postApiClient: AdminApiPostApiClient,
    public dialogService: DialogService,
    private alertService: AlertService,
    private confirmationService: ConfirmationService,
    private projectApiClient: AdminApiProjectApiClient
  ) {}

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  ngOnInit() {
    this.loadPostCategories();
    this.loadProjects();
    this.loadData();

  }

  loadData(selectionId = null) {
    this.toggleBlockUI(true);
    this.postApiClient
      .getPostsPaging(
        this.keyword,
        this.categoryId,
        this.projectId,
        this.pageIndex,
        this.pageSize
      )
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: PostInListDtoPageResult) => {
          // console.log("check postinlistDto",response)
          this.items = response.results;
          this.totalCount = response.rowCount;
          this.toggleBlockUI(false);
        },
        error: () => {
          this.toggleBlockUI(false);
        },
      });
  }

  loadProjects() {
    this.projectApiClient
      .getAllProjects()
      .subscribe((response: ProjectInListDto[]) => {
        response.forEach((element) => {
          console.log("elementProject",element)
          this.projectCategory.push({
            value: element.id,
            label: element.name,
          });
        });
      });
  }

  loadPostCategories() {
    this.postCategoryApiClient
      .getPostCategories()
      .subscribe((response: PostCategoryDto[]) => {
        response.forEach((element) => {
          this.postCategories.push({
            value: element.id,
            label: element.name,
          });
        });
      });
  }

  showAddModal() {
    const ref = this.dialogService.open(PostDetailComponent, {
      header: 'Thêm mới bài viết',
      width: '70%',
    });
    const dialogRef = this.dialogService.dialogComponentRefMap.get(ref);
    const dynamicComponent = dialogRef?.instance as DynamicDialogComponent;
    const ariaLabelledBy = dynamicComponent.getAriaLabelledBy();
    dynamicComponent.getAriaLabelledBy = () => ariaLabelledBy;
    ref.onClose.subscribe((data: PostCategoryDto) => {
      if (data) {
        this.alertService.showSuccess(MessageConstants.CREATED_OK_MSG);
        this.selectedItems = [];
        this.loadData();
      }
    });
  }

  pageChanged(event: any): void {
    this.pageIndex = event.page;
    this.pageIndex = event.page + 1;
    this.pageSize = event.rows;
    this.loadData();
  }

  showEditModal() {
    if (this.selectedItems.length == 0) {
      this.alertService.showError(MessageConstants.NOT_CHOOSE_ANY_RECORD);
      return;
    }
    var id = this.selectedItems[0].id;
    const ref = this.dialogService.open(PostDetailComponent, {
      data: {
        id: id,
      },
      header: 'Cập nhật bài viết',
      width: '70%',
    });
    const dialogRef = this.dialogService.dialogComponentRefMap.get(ref);
    const dynamicComponent = dialogRef?.instance as DynamicDialogComponent;
    const ariaLabelledBy = dynamicComponent.getAriaLabelledBy();
    dynamicComponent.getAriaLabelledBy = () => ariaLabelledBy;
    ref.onClose.subscribe((data: PostDto) => {
      if (data) {
        this.alertService.showSuccess(MessageConstants.UPDATED_OK_MSG);
        this.selectedItems = [];
        this.loadData(data.id);
      }
    });
  }

  deleteItems() {
    if (this.selectedItems.length == 0) {
      this.alertService.showError(MessageConstants.NOT_CHOOSE_ANY_RECORD);
      return;
    }
    var ids = [];
    this.selectedItems.forEach((element) => {
      ids.push(element.id);
    });
    this.confirmationService.confirm({
      message: MessageConstants.CONFIRM_DELETE_MSG,
      accept: () => {
        this.deleteItemsConfirm(ids);
      },
    });
  }

  deleteItemsConfirm(ids: any[]) {
    this.toggleBlockUI(true);
    this.postApiClient.deletePosts(ids).subscribe({
      next: () => {
        this.alertService.showSuccess(MessageConstants.DELETED_OK_MSG);
        this.loadData();
        this.selectedItems = [];
        this.toggleBlockUI(false);
      },
      error: () => {
        this.toggleBlockUI(false);
      },
    });
  }
  addToSeries(id: string) {
    const ref = this.dialogService.open(PostSeriesComponent, {
      data: {
        id: id,
      },
      header: 'Thêm vào loạt bài',
      width: '70%',
    });
    const dialogRef = this.dialogService.dialogComponentRefMap.get(ref);
    const dynamicComponent = dialogRef?.instance as DynamicDialogComponent;
    const ariaLabelledBy = dynamicComponent.getAriaLabelledBy();
    dynamicComponent.getAriaLabelledBy = () => ariaLabelledBy;
    ref.onClose.subscribe((data: PostDto) => {
      if (data) {
        this.alertService.showSuccess(MessageConstants.UPDATED_OK_MSG);
        this.selectedItems = [];
        this.loadData(data.id);
      }
    });
  }
  approve(id: string) {
    this.toggleBlockUI(true);
    this.postApiClient.approvePost(id).subscribe({
      next: () => {
        this.alertService.showSuccess(MessageConstants.UPDATED_OK_MSG);
        this.loadData();
        this.selectedItems = [];
        this.toggleBlockUI(false);
      },
      error: () => {
        this.toggleBlockUI(false);
      },
    });
  }

  sendToApprove(id: string) {
    this.toggleBlockUI(true);
    this.postApiClient.sendToApprove(id).subscribe({
      next: () => {
        // console.log("check send approve",id)
        this.alertService.showSuccess(MessageConstants.UPDATED_OK_MSG);
        this.loadData();
        this.selectedItems = [];
        this.toggleBlockUI(false);
      },
      error: () => {
        this.toggleBlockUI(false);
      },
    });
  }

  reject(id: string) {
    const ref = this.dialogService.open(PostReturnReasonComponent, {
      data: {
        id: id,
      },
      header: 'Trả lại bài',
      width: '70%',
    });
    const dialogRef = this.dialogService.dialogComponentRefMap.get(ref);
    const dynamicComponent = dialogRef?.instance as DynamicDialogComponent;
    const ariaLabelledBy = dynamicComponent.getAriaLabelledBy();
    dynamicComponent.getAriaLabelledBy = () => ariaLabelledBy;
    ref.onClose.subscribe((data: PostDto) => {
      if (data) {
        this.alertService.showSuccess(MessageConstants.UPDATED_OK_MSG);
        this.selectedItems = [];
        this.loadData(data.id);
      }
    });
  }

  showLogs(id: string) {
    const ref = this.dialogService.open(PostActivityLogsComponent, {
      data: {
        id: id,
      },
      header: 'Xem lịch sử',
      width: '70%',
    });
    const dialogRef = this.dialogService.dialogComponentRefMap.get(ref);
    const dynamicComponent = dialogRef?.instance as DynamicDialogComponent;
    const ariaLabelledBy = dynamicComponent.getAriaLabelledBy();
    dynamicComponent.getAriaLabelledBy = () => ariaLabelledBy;
    ref.onClose.subscribe((data: PostDto) => {
      if (data) {
        this.alertService.showSuccess(MessageConstants.UPDATED_OK_MSG);
        this.selectedItems = [];
        this.loadData(data.id);
      }
    });
  }
  private toggleBlockUI(enabled: boolean) {
    if (enabled == true) {
      this.blockedPanel = true;
    } else {
      setTimeout(() => {
        this.blockedPanel = false;
      }, 1000);
    }
  }
}
