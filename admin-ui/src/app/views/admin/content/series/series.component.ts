import { Component, OnDestroy, OnInit } from '@angular/core';
import { ConfirmationService } from 'primeng/api';
import { DialogService, DynamicDialogComponent } from 'primeng/dynamicdialog';
import { Subject, takeUntil } from 'rxjs';
import { SeriesDetailComponent } from './series-detail.component';
import { AlertService } from '../../../../shared/services/alert.service';
import { SeriesPostsComponent } from './series-post.component';
import { MessageConstants } from '../../../../shared/constants/message.constant';
import {
  AdminApiProjectApiClient,
  AdminApiSeriesApiClient,
  PostInListDtoPageResult,
  ProjectInListDto,
  SeriesDto,
  SeriesInListDto,
} from '../../../../api/admin-api.service.generated';

@Component({
  selector: 'app-series',
  templateUrl: './series.component.html',
  styleUrls: ['../content.component.scss'],
})
export class SeriesComponent implements OnInit, OnDestroy {
  //System variables
  private ngUnsubscribe = new Subject<void>();
  public blockedPanel: boolean = false;

  //Paging variables
  public pageIndex: number = 1;
  public pageSize: number = 10;
  public totalCount: number;

  //Business variables
  public items: SeriesInListDto[];
  public selectedItems: SeriesInListDto[] = [];
  public keyword: string = '';
  public projectCategory: any[] = [];
  public projectId?: string = null;
  constructor(
    private seriesApiClient: AdminApiSeriesApiClient,
    public dialogService: DialogService,
    private notificationService: AlertService,
    private confirmationService: ConfirmationService,
    private projectApiClient: AdminApiProjectApiClient
  ) {}

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  ngOnInit() {
    this.loadData();
    this.loadProjects();
  }

  loadData(selectionId = null) {
    this.toggleBlockUI(true);

    this.seriesApiClient
      .getSeriesPaging(
        this.keyword,
        this.projectId,
        this.pageIndex,
        this.pageSize
      )
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: PostInListDtoPageResult) => {
          console.log('PostInListDtoPageResult', response);

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
          // console.log('elementProject', element);
          this.projectCategory.push({
            value: element.id,
            label: element.name,
          });
        });
      });
  }
  showAddModal() {
    const ref = this.dialogService.open(SeriesDetailComponent, {
      header: 'Thêm mới series bài viết',
      width: '70%',
    });
    const dialogRef = this.dialogService.dialogComponentRefMap.get(ref);
    const dynamicComponent = dialogRef?.instance as DynamicDialogComponent;
    const ariaLabelledBy = dynamicComponent.getAriaLabelledBy();
    dynamicComponent.getAriaLabelledBy = () => ariaLabelledBy;
    ref.onClose.subscribe((data: SeriesDto) => {
      if (data) {
        this.notificationService.showSuccess(MessageConstants.CREATED_OK_MSG);
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
      this.notificationService.showError(
        MessageConstants.NOT_CHOOSE_ANY_RECORD
      );
      return;
    }
    var id = this.selectedItems[0].id;
    const ref = this.dialogService.open(SeriesDetailComponent, {
      data: {
        id: id,
      },
      header: 'Cập nhật series bài viết',
      width: '70%',
    });
    const dialogRef = this.dialogService.dialogComponentRefMap.get(ref);
    const dynamicComponent = dialogRef?.instance as DynamicDialogComponent;
    const ariaLabelledBy = dynamicComponent.getAriaLabelledBy();
    dynamicComponent.getAriaLabelledBy = () => ariaLabelledBy;
    ref.onClose.subscribe((data: SeriesDto) => {
      if (data) {
        this.notificationService.showSuccess(MessageConstants.UPDATED_OK_MSG);
        this.selectedItems = [];
        this.loadData(data.id);
      }
    });
  }

  showPosts() {
    if (this.selectedItems.length == 0) {
      this.notificationService.showError(
        MessageConstants.NOT_CHOOSE_ANY_RECORD
      );
      return;
    }
    var id = this.selectedItems[0].id;
    const ref = this.dialogService.open(SeriesPostsComponent, {
      data: {
        id: id,
      },
      header: 'Quản lý danh sách bài viết',
      width: '70%',
    });
    const dialogRef = this.dialogService.dialogComponentRefMap.get(ref);
    const dynamicComponent = dialogRef?.instance as DynamicDialogComponent;
    const ariaLabelledBy = dynamicComponent.getAriaLabelledBy();
    dynamicComponent.getAriaLabelledBy = () => ariaLabelledBy;
    ref.onClose.subscribe((data: SeriesDto) => {
      if (data) {
        this.notificationService.showSuccess(MessageConstants.UPDATED_OK_MSG);
        this.selectedItems = [];
        this.loadData(data.id);
      }
    });
  }

  deleteItems() {
    if (this.selectedItems.length == 0) {
      this.notificationService.showError(
        MessageConstants.NOT_CHOOSE_ANY_RECORD
      );
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

    this.seriesApiClient.deleteSeries(ids).subscribe({
      next: () => {
        this.notificationService.showSuccess(MessageConstants.DELETED_OK_MSG);
        this.loadData();
        this.selectedItems = [];
        this.toggleBlockUI(false);
      },
      error: () => {
        this.toggleBlockUI(false);
      },
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
