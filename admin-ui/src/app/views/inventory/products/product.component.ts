import { Component, OnDestroy, OnInit } from '@angular/core';
import { ConfirmationService } from 'primeng/api';
import { DialogService, DynamicDialogComponent } from 'primeng/dynamicdialog';
import { Subject, takeUntil } from 'rxjs';
import { MessageConstants } from '../../../../app/shared/constants/message.constant';
import { ProductDetailComponent } from './product-detail.component';
import { AlertService } from '../../../../app/shared/services/alert.service';
import {
  AdminApiProductApiClient,
  AdminApiProductCategoryApiClient,
  ProductCategoryDto,
  ProductDto,
  ProductInListDto,
  ProductInListDtoPageResult,
} from '../../../../app/api/admin-api.service.generated';
@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
})
export class ProductComponent implements OnInit, OnDestroy {
  //System variables
  private ngUnsubscribe = new Subject<void>();
  public blockedPanel: boolean = false;

  //Paging variables
  public pageIndex: number = 1;
  public pageSize: number = 10;
  public totalCount: number;

  //Business variables
  public items: ProductInListDto[];
  public selectedItems: ProductInListDto[] = [];
  public keyword: string = '';

  public categoryId?: string = null;
  public productCategories: any[] = [];

  constructor(
    private productCategoryApiClient: AdminApiProductCategoryApiClient,
    private productApiClient: AdminApiProductApiClient,
    public dialogService: DialogService,
    private alertService: AlertService,
    private confirmationService: ConfirmationService
  ) {}

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  ngOnInit() {
    this.loadPostCategories();
    this.loadData();
  }

  loadData(selectionId = null) {
    this.toggleBlockUI(true);
    this.productApiClient
      .getProductPaging(
        this.keyword,
        // this.categoryId,
        this.pageIndex,
        this.pageSize
      )
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: ProductInListDtoPageResult) => {
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

  loadPostCategories() {
    this.productCategoryApiClient
      .getProductCategories()
      .subscribe((response: ProductCategoryDto[]) => {
        response.forEach((element) => {
          this.productCategories.push({
            value: element.id,
            label: element.name,
          });
        });
      });
  }

  showAddModal() {
    const ref = this.dialogService.open(ProductDetailComponent, {
      header: 'Thêm mới Sản Phẩm',
      width: '70%',
    });
    const dialogRef = this.dialogService.dialogComponentRefMap.get(ref);
    const dynamicComponent = dialogRef?.instance as DynamicDialogComponent;
    const ariaLabelledBy = dynamicComponent.getAriaLabelledBy();
    dynamicComponent.getAriaLabelledBy = () => ariaLabelledBy;
    ref.onClose.subscribe((data: ProductCategoryDto) => {
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
    const ref = this.dialogService.open(ProductDetailComponent, {
      data: {
        id: id,
      },
      header: 'Cập nhật Sản Phẩm',
      width: '70%',
    });
    const dialogRef = this.dialogService.dialogComponentRefMap.get(ref);
    const dynamicComponent = dialogRef?.instance as DynamicDialogComponent;
    const ariaLabelledBy = dynamicComponent.getAriaLabelledBy();
    dynamicComponent.getAriaLabelledBy = () => ariaLabelledBy;
    ref.onClose.subscribe((data: ProductDto) => {
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
    this.productApiClient.deleteProduct(ids).subscribe({
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
  //   this.productApiClient.approveProduct(id).subscribe({
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
  //   const ref = this.dialogService.open(ProductActivityLogsComponent, {
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
