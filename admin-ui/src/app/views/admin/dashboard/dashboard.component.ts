import { Component, OnInit } from '@angular/core';
import { UntypedFormControl, UntypedFormGroup } from '@angular/forms';
import { DashboardChartsData, IChartProps } from './dashboard-charts-data';
import { Subject, takeUntil } from 'rxjs';
import {
  AdminApiUserApiClient,
  UserDto,
  UserDtoPageResult,
} from 'src/app/api/admin-api.service.generated';
import { environment } from 'src/environments/environment';


@Component({
  templateUrl: 'dashboard.component.html',
  styleUrls: ['dashboard.component.scss'],
})
export class DashboardComponent implements OnInit {
  private ngUnsubscribe = new Subject<void>();
  //Paging variables
  public pageIndex: number = 1;
  public pageSize: number = 10;
  public totalCount: number;
  public environment=environment;

  //Business variables
  public items: UserDto[];
  public selectedItems: UserDto[] = [];
  public keyword: string = '';
  constructor(
    private chartsData: DashboardChartsData,
    private userService: AdminApiUserApiClient
  ) {}

  public mainChart: IChartProps = {};
  public chart: Array<IChartProps> = [];
  public trafficRadioGroup = new UntypedFormGroup({
    trafficRadio: new UntypedFormControl('Month'),
  });

  ngOnInit(): void {
    this.initCharts();
    this.loadData();
    // console.log(environment.API_URL);
  }

  initCharts(): void {
    this.mainChart = this.chartsData.mainChart;
  }
  loadData(selectionId = null) {
    this.toggleBlockUI(true);
    this.userService
      .getAllUserPaging(this.keyword, this.pageIndex, this.pageSize)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: UserDtoPageResult) => {
          // console.log('res', response);
          this.items = response.results;

          // Duyệt qua từng item trong `items` để gán URL hình ảnh đại diện
          // this.thumbnailImages = this.items.map(
          //   (item) => environment.API_URL +"/"+ item.avatar
          // );

          this.totalCount = response.rowCount;
          if (selectionId != null && this.items.length > 0) {
            this.selectedItems = this.items.filter((x) => x.id == selectionId);
          }

          this.toggleBlockUI(false);
        },
        error: () => {
          this.toggleBlockUI(false);
        },
      });
  }

  private toggleBlockUI(enabled: boolean) {
    if (enabled == true) {
      // this.blockedPanel = true;
    } else {
      setTimeout(() => {
        // this.blockedPanel = false;
      }, 1000);
    }
  }
  setTrafficPeriod(value: string): void {
    this.trafficRadioGroup.setValue({ trafficRadio: value });
    this.chartsData.initMainChart(value);
    this.initCharts();
  }
}
