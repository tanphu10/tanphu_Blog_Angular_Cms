import { Component, OnDestroy, OnInit } from '@angular/core';
import { ConfirmationService } from 'primeng/api';
import { DialogService, DynamicDialogComponent } from 'primeng/dynamicdialog';
import { Subject, takeUntil } from 'rxjs';
import * as messageConstant from 'src/app/shared/constants/message.constant';
import { AlertService } from 'src/app/shared/services/alert.service';
import * as adminApiServiceGenerated from 'src/app/api/admin-api.service.generated';
import {
  AdminApiInventoryCategoryApiClient,
  AdminApiProjectApiClient,
  InventoryCategoryDto,
  InventoryEntryDto,
  InventoryInListDto,
  InventoryInListDtoPageResult,
  ProjectInListDto,
} from 'src/app/api/admin-api.service.generated';
import { InventoryDetailComponent } from './inventory-detail.component';
import { MessageConstants } from 'src/app/shared/constants/message.constant';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-inventory',
  templateUrl: './inventory.component.html',
  styleUrls: ['./inventory.component.scss', '../../admin.component.scss'],
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

  public invtCategories: any[] = [];
  public categorySlug?: string | undefined;
  public invtItems: InventoryInListDto[] | [];

  //Business variables
  public items: InventoryInListDto[];
  public selectedItems: InventoryInListDto[] = [];
  public keyword: string = '';

  public projectId?: string = null;
  public projectCategory: any[] = [];
  activeIndex: number = 3;
  public environment = environment;
  slug: string | null = null;

  fromDate: Date | undefined;
  toDate: Date | undefined;
  public categoryId?: string = null;

  constructor(
    private inventoryApiClient: adminApiServiceGenerated.AdminApiInventoryApiClient,
    private invtCategoryApiClient: AdminApiInventoryCategoryApiClient,
    private projectApiClient: AdminApiProjectApiClient,
    public dialogService: DialogService,
    private alertService: AlertService,
    private confirmationService: ConfirmationService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  ngOnInit() {
    this.route.paramMap.subscribe((params) => {
      this.slug = params.get('slug');
      console.log('Slug:', this.slug);
      // Xử lý logic tùy theo giá trị của slug
    });
    this.loadProjects();
    this.loadData();
    this.loadInvtCategories();
  }
  loadInvtCategories() {
    this.invtCategoryApiClient
      .getInventoryCategories()
      .subscribe((response: InventoryCategoryDto[]) => {
        console.log('int', response);
        response.forEach((element) => {
          this.invtCategories.push({
            value: element.id,
            label: element.name,
            slug: element.slug,
          });
        });
      });
  }
  onTabChange(event: any): void {
    // Khi tab được thay đổi, cập nhật URL
    console.log('activeIndex', this.activeIndex);

    // -- NHƯ VẬY TRÊN BE PHẢI  LÀM RIÊNG RA THÊM 1.API/ NHẬP KHO 2.XUẤT KHO 3. KIỂM KÊ KHO 4. CẤP VẬT TƯ
    console.log('invtCategories', this.invtCategories[this.activeIndex]);

    const slug = this.invtCategories[this.activeIndex]?.slug;
    if (slug) {
      this.router.navigate(['/stock', slug]);
      this.categorySlug = slug;
      this.loadData();
    }
  }
  loadData(projectId = null) {
    this.toggleBlockUI(true);
    this.inventoryApiClient
      .getInventoryPaging(
        this.keyword,
        this.fromDate,
        this.toDate,
        this.projectId,
        this.categorySlug,
        this.pageIndex,
        this.pageSize
      )
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: InventoryInListDtoPageResult) => {
          console.log('InventoryInListDtoPageResult ', response);
          this.items = response.results;
          this.totalCount = response.rowCount;
          this.stockQuantity = response.additionalData;
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
          console.log('project', element);
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

  deleteItems() {
    if (this.selectedItems.length == 0) {
      this.alertService.showError(
        messageConstant.MessageConstants.NOT_CHOOSE_ANY_RECORD
      );
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
        this.alertService.showSuccess(
          messageConstant.MessageConstants.DELETED_OK_MSG
        );
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
