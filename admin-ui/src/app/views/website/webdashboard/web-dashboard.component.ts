import { Component, OnInit } from '@angular/core';
import { UntypedFormControl, UntypedFormGroup } from '@angular/forms';
import {
  WebDashboardChartsData,
  IChartProps,
} from './web-dashboard-charts-data';
import { Subject, takeUntil } from 'rxjs';
import {
  AdminApiUserApiClient,
  UserDto,
  UserDtoPageResult,
} from '../../../api/admin-api.service.generated';
import { environment } from '../../../../environments/environment';

@Component({
  templateUrl: 'web-dashboard.component.html',
  styleUrls: ['web-dashboard.component.scss'],
})
export class WebDashboardComponent implements OnInit {
  private ngUnsubscribe = new Subject<void>();
  //Paging variables
  public pageIndex: number = 1;
  public pageSize: number = 10;
  public totalCount: number;
  // public thumbnailImages: string[] = []; // Mảng chứa URL hình ảnh đại diện cho từng UserDto
  public environment = environment;

  //Business variables
  public items: UserDto[] = [];
  public selectedItems: UserDto[] = [];
  public keyword: string = '';
  constructor(
    private chartsData: WebDashboardChartsData,
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
          this.items = response.results ?? [];
          this.totalCount = response.rowCount ?? 0;
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
