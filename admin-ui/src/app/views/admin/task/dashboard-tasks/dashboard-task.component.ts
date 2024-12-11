import { Component, OnDestroy, OnInit } from '@angular/core';
import { ConfirmationService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { Subject, takeUntil } from 'rxjs';
// import { TaskDetailComponent } from './task-detail.component';
import { AlertService } from '../../../../shared/services/alert.service';
import {
  AdminApiProjectApiClient,
  AdminApiTaskApiClient,
  AdminApiUserApiClient,
  ProjectInListDto,
  TaskInListDto,
  TaskInListDtoPageResult,
} from '../../../../api/admin-api.service.generated';
import { Chart, ChartOptions } from 'chart.js';
// import { environment } from 'src/environments/environment';

@Component({
  selector: 'dashboard-task',
  templateUrl: './dashboard-task.component.html',
  styleUrls: ['../../admin.component.scss', './dashboard-task.component.scss'],
})
export class DashboardTaskComponent implements OnInit, OnDestroy {
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

  public projectId?: string = null;
  public projectCategory: any[] = [];
  slug: string | null = null;
  public projectItem: any[] = [];
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
    public dialogService: DialogService
  ) {}

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  dataValues = [0, 0];
  // Hàm tạo màu ngẫu nhiên
  getRandomColor() {
    return `#${Math.floor(Math.random() * 16777215).toString(16)}`;
  }
  backgroundColors = this.dataValues.map(() => this.getRandomColor());
  // Tạo cấu trúc dataDepartment
  dataDepartment = {
    labels: this.projectItem,
    datasets: [
      {
        backgroundColor: this.backgroundColors,
        data: this.dataValues,
      },
    ],
  };

  // Tạo mảng màu động
  dataPriority = {
    labels: ['Low', 'Medium', 'High', 'Very High'],
    datasets: [
      {
        label: 'Priority',
        color: 'red',
        backgroundColor: [' #d4edda', '#cce5ff', '#feddc7', '#b32b23'],
        data: [0, 0, 0, 0], // Giá trị mặc định
        width: '10px',
      },
    ],
  };

  dataPolarStatus = {
    labels: ['Pending', 'InProgress', 'OnHold', 'Completed', 'Cancelled'],
    datasets: [
      {
        data: [0, 0, 0, 0, 0], // Giá trị mặc định
        backgroundColor: [
          '#e6670d',
          '#004085',
          '#475569',
          '#155724',
          '#b32b23',
        ],
      },
    ],
  };

  ngOnInit() {
    this.loadProjects();
    this.loadData();
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
          console.log('check TaskInListDtoPageResult', response);
          this.items = response.results;
          // Nhóm dữ liệu theo `projectSlug` và đếm số lượng mục trong mỗi dự án
          const groupedProjects = this.items.reduce((acc, item) => {
            if (!acc[item.projectSlug]) {
              acc[item.projectSlug] = [];
            }
            acc[item.projectSlug].push(item);
            return acc;
          }, {});
          // Lấy danh sách dự án (loại bỏ trùng lặp)
          this.projectItem = Object.keys(groupedProjects);
          this.dataValues = this.projectItem.map(
            (project) => groupedProjects[project].length
          );

          // Cấu hình dataDepartment
          this.dataDepartment = {
            labels: this.projectItem,
            datasets: [
              {
                backgroundColor: this.backgroundColors,
                data: this.dataValues,
              },
            ],
          };

          this.updateChartData();
          this.totalCount = response.rowCount;
          this.toggleBlockUI(false);
          this.calculateTaskSummary();
        },
        error: () => {
          this.toggleBlockUI(false);
        },
      });
  }

  updateChartData() {
    const priorityData = [0, 0, 0, 0]; // Lưu trữ số lượng cho mỗi mức độ ưu tiên (Low, Medium, High, Very High)
    const statusData = [0, 0, 0, 0, 0];
    this.items.forEach((t: TaskInListDto) => {
      priorityData[t.priority]++;
      statusData[t.status]++;
    });

    // Cập nhật dữ liệu cho biểu đồ ưu tiên
    this.dataPriority = {
      labels: ['Low', 'Medium', 'High', 'Very High'],
      datasets: [
        {
          label: 'Priority',
          color: 'red',
          backgroundColor: ['#d4edda', '#cce5ff', '#feddc7', '#b32b23'],
          data: priorityData,
          width: '10px',
        },
      ],
    }; // Cập nhật dữ liệu cho biểu đồ trạng thái
    console.log('priorityData', priorityData);
    this.dataPolarStatus = {
      labels: ['Pending', 'InProgress', 'OnHold', 'Completed', 'Cancelled'],
      datasets: [
        {
          data: statusData,
          backgroundColor: [
            '#e6670d',
            '#004085',
            '#475569',
            '#155724',
            '#b32b23',
          ],
        },
      ],
    };
  }
  calculateTaskSummary() {
    if (!this.items || !Array.isArray(this.items)) {
      console.log('items is not initialized or not an array', this.items);
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

  pageChanged(event: any): void {
    this.pageIndex = event.page;
    this.pageIndex = event.page + 1;
    this.pageSize = event.rows;
    this.loadData();
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
