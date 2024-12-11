import { Component, OnDestroy, OnInit } from '@angular/core';
import { ConfirmationService } from 'primeng/api';
import { DialogService, DynamicDialogComponent } from 'primeng/dynamicdialog';
import { Subject, takeUntil } from 'rxjs';
import { MessageConstants } from '../../../../shared/constants/message.constant';
import { TaskDetailComponent } from './task-detail.component';
import { AlertService } from '../../../../shared/services/alert.service';
import {
  AdminApiProjectApiClient,
  AdminApiTaskApiClient,
  AdminApiUserApiClient,
  AssignToUserRequest,
  ProjectDto,
  ProjectInListDto,
  TaskDto,
  TaskInListDto,
  TaskInListDtoPageResult,
  UserPagingDto,
  UserPagingDtoPageResult,
} from '../../../../api/admin-api.service.generated';
import { environment } from 'src/environments/environment';
// Import các module của PrimeNG

@Component({
  selector: 'app-task',
  templateUrl: './task.component.html',
  styleUrls: ['../../admin.component.scss', './task.component.scss'],
})
export class TaskComponent implements OnInit, OnDestroy {
  //System variables
  private ngUnsubscribe = new Subject<void>();
  public blockedPanel: boolean = false;
  // public form: FormGroup;

  //Paging variables
  public pageIndex: number = 1;
  public pageSize: number = 10;
  public totalCount: number;

  //Business variables
  public items: TaskInListDto[];
  public selectedItems: TaskInListDto[] = [];
  public keyword: string = '';

  public categoryId?: string = null;
  public productCategories: any[] = [];

  public projectId?: string = null;
  public projectCategory: any[] = [];
  public environment = environment;
  slug: string | null = null;

  userOptions: UserPagingDto[] = [];
  selectUsers: UserPagingDto[] = [];
  arrayUser?: AssignToUserRequest | undefined;
  fromDate: Date | undefined;
  toDate: Date | undefined;
  totalTasks: number = 0; // Tổng số task
  incompleteTasks: number = 0; // Số task chưa hoàn thành
  overdueTasks: number = 0; // Số task quá hạn
  today: string = new Intl.DateTimeFormat('vi-VN', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
  }).format(new Date());
  constructor(
    private projectApiClient: AdminApiProjectApiClient,
    private taskApiClient: AdminApiTaskApiClient,
    private userApiClient: AdminApiUserApiClient,
    public dialogService: DialogService,
    private alertService: AlertService,
    private confirmationService: ConfirmationService // private fb: FormBuilder
  ) {}
  toggleDescription(row: any) {
    row.isDescriptionExpanded = !row.isDescriptionExpanded; // Đảo ngược trạng thái hiển thị mô tả
  }

  async toggleEditingUser(row: any) {
    await this.loadUser();
    // this.buildForm();

    // console.log('check User', row.listAssignedTo);
    // console.log('check ', this.userOptions);
    // console.log('check selectUsers before update', this.selectUsers);

    this.selectUsers = this.userOptions.filter((user) =>
      row.listAssignedTo?.some((assign) => assign.userId == user.id)
    );
    // console.log('check selectUsers after update', this.selectUsers);

    if (!this.arrayUser) {
      this.arrayUser = new AssignToUserRequest(); // Khởi tạo arrayUser với kiểu AssignToUserRequest
    }
    this.arrayUser.assignedToUser = this.selectUsers.map((x) => x.id);

    row.isEditingUser = true;
  }

  onSelectUsersChange() {
    // Kiểm tra giá trị của selectUsers sau khi thay đổi
    // console.log('selectUsers has changed:', this.selectUsers);
    this.arrayUser.assignedToUser = this.selectUsers.map((x) => x.id);
    // console.log('Assigned Users after change:', this.arrayUser.assignedToUser);
  }

  saveAssignedUsers(row: any) {
    console.log('saveAssignedUsers', row.id);
    console.log('arrayUser', this.arrayUser);
    this.taskApiClient
      .assignToUser(row.id, this.arrayUser)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: () => {
          this.loadData();
          console.log('update');
          this.toggleBlockUI(false);
        },
        error: () => {
          this.toggleBlockUI(false);
        },
      });
    row.isEditingUser = false;
  }

  cancelEditingUser(row: any) {
    row.isEditingUser = false;
  }
  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  ngOnInit() {
    this.loadProjects();
    this.loadData();
  }

  async loadUser() {
    try {
      const response = await this.userApiClient
        .getAllUserPaging(
          this.keyword,
          this.projectId,
          this.pageIndex,
          this.pageSize
        )
        .toPromise();
      this.userOptions = response.results;
      this.toggleBlockUI(false);
    } catch (error) {
      this.toggleBlockUI(false);
      // console.error('Error loading users:', error);
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
  assignToUser() {
    // console.log('updateUser');
  }

  loadData(selectionId = null) {
    this.toggleBlockUI(true);
    this.taskApiClient
      .getTaskPaging(
        this.keyword,
        this.fromDate,
        this.toDate,
        this.projectId,
        this.pageIndex,
        this.pageSize
      )
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: TaskInListDtoPageResult) => {
          // console.log('check TaskInListDtoPageResult', response);
          this.items = response.results;
          this.totalCount = response.rowCount;
          this.toggleBlockUI(false);
          this.calculateTaskSummary();
        },
        error: () => {
          this.toggleBlockUI(false);
        },
      });
  }
  calculateTaskSummary() {
    if (!this.items || !Array.isArray(this.items)) {
      // console.log('items is not initialized or not an array', this.items);
      return;
    }

    this.totalTasks = this.items.filter(
      (x) => x.status !== 3 && x.status !== 4
    ).length;

    this.incompleteTasks = this.items.filter(
      (task) => task.status !== 3 && task.status !== 4
    ).length;

    this.overdueTasks = this.items.filter(
      (task) =>
        new Date(task.dueDate) < new Date() &&
        task.status !== 3 &&
        task.status !== 4
    ).length;
  }

  showAddModal() {
    const ref = this.dialogService.open(TaskDetailComponent, {
      header: 'Thêm mới Task',
      width: '70%',
    });
    const dialogRef = this.dialogService.dialogComponentRefMap.get(ref);
    const dynamicComponent = dialogRef?.instance as DynamicDialogComponent;
    const ariaLabelledBy = dynamicComponent.getAriaLabelledBy();
    dynamicComponent.getAriaLabelledBy = () => ariaLabelledBy;
    ref.onClose.subscribe((data: TaskInListDto) => {
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
    // console.log("check item",this.selectedItems[0]);
    var id = this.selectedItems[0].id;
    const ref = this.dialogService.open(TaskDetailComponent, {
      data: {
        id: id,
      },
      header: 'Cập nhật Task',
      width: '70%',
    });
    const dialogRef = this.dialogService.dialogComponentRefMap.get(ref);
    const dynamicComponent = dialogRef?.instance as DynamicDialogComponent;
    const ariaLabelledBy = dynamicComponent.getAriaLabelledBy();
    dynamicComponent.getAriaLabelledBy = () => ariaLabelledBy;
    ref.onClose.subscribe((data: TaskDto) => {
      // console.log('update Success Task', data);
      if (data) {
        this.alertService.showSuccess(MessageConstants.UPDATED_OK_MSG);
        this.selectedItems = [];
        this.loadData();
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
    this.taskApiClient.deleteTask(ids).subscribe({
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
  // buildForm() {
  //   // console.log('project>>>');
  //   this.form = this.fb.group({
  //     userName: new FormControl(
  //       this.selectUsers.userName || null,
  //       Validators.compose([
  //         Validators.required,
  //         Validators.maxLength(255),
  //         Validators.minLength(3),
  //       ])
  //     ),
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
