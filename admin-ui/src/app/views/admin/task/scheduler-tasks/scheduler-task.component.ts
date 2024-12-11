import {
  AfterViewInit,
  Component,
  ElementRef,
  OnDestroy,
  OnInit,
  ViewChild,
  ViewEncapsulation,
} from '@angular/core';
import { EventService } from '../../../../shared/services/event.service';
import { scheduler } from 'dhtmlx-scheduler';
import { AdminApiTaskApiClient } from 'src/app/api/admin-api.service.generated';

@Component({
  encapsulation: ViewEncapsulation.None,
  selector: 'scheduler',
  providers: [EventService],
  styleUrls: ['../../admin.component.scss', './scheduler-task.component.scss'],
  templateUrl: './scheduler-task.component.html',
})
export class SchedulerTaskComponent
  implements OnInit, AfterViewInit, OnDestroy
{
  ngOnInit(): void {
    // Lấy dữ liệu từ API
    this.taskApiClient.getAllTasks().subscribe((tasks) => {
      const events = tasks.map((task) => ({
        id: task.id,
        text: task.name,
        start_date: task.dateCreated, // Thời gian bắt đầu từ API
        end_date: task.complete, // Thời gian kết thúc từ API
      }));

      console.log('event', events);
      // Thêm sự kiện vào lịch
      scheduler.parse(events);
    });
  }
  @ViewChild('scheduler_here', { static: true })
  schedulerContainer!: ElementRef;

  constructor(private taskApiClient: AdminApiTaskApiClient) {}

  ngOnDestroy(): void {
    scheduler.clearAll();
  }
  ngAfterViewInit(): void {
    // Initialize Scheduler
    scheduler.init(this.schedulerContainer.nativeElement, new Date(), 'week');
  }
}
