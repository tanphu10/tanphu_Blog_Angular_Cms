import { Component, OnDestroy, OnInit } from '@angular/core';
import { ConfirmationService } from 'primeng/api';
import { DialogService, DynamicDialogComponent } from 'primeng/dynamicdialog';
import { Subject, takeUntil } from 'rxjs';
import * as messageConstant from 'src/app/shared/constants/message.constant';
import { AlertService } from 'src/app/shared/services/alert.service';
import * as adminApiServiceGenerated from 'src/app/api/admin-api.service.generated';
import { InventoryEntryDto, InventoryInListDto, InventoryInListDtoPageResult, ProjectInListDto } from 'src/app/api/admin-api.service.generated';
import { InventoryDetailComponent } from './inventory-detail.component';
import { MessageConstants } from 'src/app/shared/constants/message.constant';


@Component({
  selector: 'app-inventory',
  templateUrl: './inventory.component.html',
})
export class InventoryComponent implements OnInit, OnDestroy {
  //System variables
  private ngUnsubscribe = new Subject<void>();
  public blockedPanel: boolean = false;

  //Paging variables
  public pageIndex: number = 1;
  public pageSize: number = 10;
  public totalCount: number;
  public stockQuantity: number;


  //Business variables
  public items: InventoryEntryDto[];
  public selectedItems: InventoryInListDto[] = [];
  public keyword: string = '';

  public projectId?: string = null;
  public projectCategory: any[] = [];

  constructor(
    private inventoryApiClient: adminApiServiceGenerated.AdminApiInventoryApiClient,
    private projectApiClient: adminApiServiceGenerated.AdminApiProjectApiClient,
    public dialogService: DialogService,
    private alertService: AlertService,
    private confirmationService: ConfirmationService
  ) {}

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  ngOnInit() {
    this.loadProjects();
    this.loadData();
  }

  loadData(projectId = null) {
    this.toggleBlockUI(true);
    this.inventoryApiClient
      .getInventoryPaging(
        this.keyword,
        this.projectId,
        this.pageIndex,
        this.pageSize
      )
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: InventoryInListDtoPageResult) => {
          // console.log("check ",response)
          this.items = response.results;
          this.totalCount = response.rowCount;
          this.stockQuantity=response.additionalData;
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
          this.projectCategory.push({
            value: element.id,
            label: element.name,
          });
        });
      });
  }

  showAddModal() {
    const ref = this.dialogService.open(InventoryDetailComponent, {
      header: 'Mua Sản Phẩm',
      width: '70%',
    });
    const dialogRef = this.dialogService.dialogComponentRefMap.get(ref);
    const dynamicComponent = dialogRef?.instance as DynamicDialogComponent;
    const ariaLabelledBy = dynamicComponent.getAriaLabelledBy();
    dynamicComponent.getAriaLabelledBy = () => ariaLabelledBy;
    ref.onClose.subscribe((data: InventoryEntryDto) => {
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

  // showEditModal() {
  //   if (this.selectedItems.length == 0) {
  //     this.alertService.showError(MessageConstants.NOT_CHOOSE_ANY_RECORD);
  //     return;
  //   }
  //   var id = this.selectedItems[0].id;
  //   const ref = this.dialogService.open(PostDetailComponent, {
  //     data: {
  //       id: id,
  //     },
  //     header: 'Cập nhật bài viết',
  //     width: '70%',
  //   });
  //   const dialogRef = this.dialogService.dialogComponentRefMap.get(ref);
  //   const dynamicComponent = dialogRef?.instance as DynamicDialogComponent;
  //   const ariaLabelledBy = dynamicComponent.getAriaLabelledBy();
  //   dynamicComponent.getAriaLabelledBy = () => ariaLabelledBy;
  //   ref.onClose.subscribe((data: PostDto) => {
  //     if (data) {
  //       this.alertService.showSuccess(MessageConstants.UPDATED_OK_MSG);
  //       this.selectedItems = [];
  //       this.loadData(data.id);
  //     }
  //   });
  // }

  deleteItems() {
    if (this.selectedItems.length == 0) {
      this.alertService.showError(messageConstant.MessageConstants.NOT_CHOOSE_ANY_RECORD);
      return;
    }
    var ids = [];
    this.selectedItems.forEach((element) => {
      ids.push(element.id);
    });
    this.confirmationService.confirm({
      message: messageConstant.MessageConstants.CONFIRM_DELETE_MSG,
      accept: () => {
        this.deleteItemsConfirm(ids);
      },
    });
  }

  deleteItemsConfirm(ids: any[]) {
    this.toggleBlockUI(true);
    this.inventoryApiClient.deleteById(ids).subscribe({
      next: () => {
        this.alertService.showSuccess(messageConstant.MessageConstants.DELETED_OK_MSG);
        this.loadData();
        this.selectedItems = [];
        this.toggleBlockUI(false);
      },
      error: () => {
        this.toggleBlockUI(false);
      },
    });
  }
  // addToSeries(id: string) {
  //   const ref = this.dialogService.open(PostSeriesComponent, {
  //     data: {
  //       id: id,
  //     },
  //     header: 'Thêm vào loạt bài',
  //     width: '70%',
  //   });
  //   const dialogRef = this.dialogService.dialogComponentRefMap.get(ref);
  //   const dynamicComponent = dialogRef?.instance as DynamicDialogComponent;
  //   const ariaLabelledBy = dynamicComponent.getAriaLabelledBy();
  //   dynamicComponent.getAriaLabelledBy = () => ariaLabelledBy;
  //   ref.onClose.subscribe((data: PostDto) => {
  //     if (data) {
  //       this.alertService.showSuccess(MessageConstants.UPDATED_OK_MSG);
  //       this.selectedItems = [];
  //       this.loadData(data.id);
  //     }
  //   });
  // }
  // approve(id: string) {
  //   this.toggleBlockUI(true);
  //   this.postApiClient.approvePost(id).subscribe({
  //     next: () => {
  //       this.alertService.showSuccess(MessageConstants.UPDATED_OK_MSG);
  //       this.loadData();
  //       this.selectedItems = [];
  //       this.toggleBlockUI(false);
  //     },
  //     error: () => {
  //       this.toggleBlockUI(false);
  //     },
  //   });
  // }

  // sendToApprove(id: string) {
  //   this.toggleBlockUI(true);
  //   this.postApiClient.sendToApprove(id).subscribe({
  //     next: () => {
  //       // console.log("check send approve",id)
  //       this.alertService.showSuccess(MessageConstants.UPDATED_OK_MSG);
  //       this.loadData();
  //       this.selectedItems = [];
  //       this.toggleBlockUI(false);
  //     },
  //     error: () => {
  //       this.toggleBlockUI(false);
  //     },
  //   });
  // }

  // reject(id: string) {
  //   const ref = this.dialogService.open(PostReturnReasonComponent, {
  //     data: {
  //       id: id,
  //     },
  //     header: 'Trả lại bài',
  //     width: '70%',
  //   });
  //   const dialogRef = this.dialogService.dialogComponentRefMap.get(ref);
  //   const dynamicComponent = dialogRef?.instance as DynamicDialogComponent;
  //   const ariaLabelledBy = dynamicComponent.getAriaLabelledBy();
  //   dynamicComponent.getAriaLabelledBy = () => ariaLabelledBy;
  //   ref.onClose.subscribe((data: PostDto) => {
  //     if (data) {
  //       this.alertService.showSuccess(MessageConstants.UPDATED_OK_MSG);
  //       this.selectedItems = [];
  //       this.loadData(data.id);
  //     }
  //   });
  // }

  // showLogs(id: string) {
  //   const ref = this.dialogService.open(PostActivityLogsComponent, {
  //     data: {
  //       id: id,
  //     },
  //     header: 'Xem lịch sử',
  //     width: '70%',
  //   });
  //   const dialogRef = this.dialogService.dialogComponentRefMap.get(ref);
  //   const dynamicComponent = dialogRef?.instance as DynamicDialogComponent;
  //   const ariaLabelledBy = dynamicComponent.getAriaLabelledBy();
  //   dynamicComponent.getAriaLabelledBy = () => ariaLabelledBy;
  //   ref.onClose.subscribe((data: PostDto) => {
  //     if (data) {
  //       this.alertService.showSuccess(MessageConstants.UPDATED_OK_MSG);
  //       this.selectedItems = [];
  //       this.loadData(data.id);
  //     }
  //   });
  // }
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