import { EventEmitter, Injectable } from '@angular/core';
import { HubConnectionBuilder, HubConnection } from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { TokenStorageService } from 'src/app/shared/services/token-storage.service'; // Import the token service
import {
  AdminApiTaskApiClient,
  TaskInListDtoPageResult,
} from 'src/app/api/admin-api.service.generated';
import { Subject, takeUntil } from 'rxjs';

@Injectable()
export class SignalRService {
  private connection: HubConnection;
  public announcementReceived: EventEmitter<any>;
  public taskNotificationReceived: EventEmitter<any>;
  public connectionEstablished: EventEmitter<Boolean>;
  public connectionExists: Boolean;
  private unsubscribe = new Subject<void>(); // This will be used to trigger the unsubscription

  constructor(
    private tokenService: TokenStorageService,
    private taskApiService: AdminApiTaskApiClient
  ) {
    // Inject token service
    this.connectionEstablished = new EventEmitter<Boolean>();
    this.announcementReceived = new EventEmitter<any>();
    this.taskNotificationReceived = new EventEmitter<any>();

    this.connectionExists = false;

    const token = this.tokenService.getToken();
    if (!token) {
      console.error('No token found!');
      return;
    }
    this.connection = new HubConnectionBuilder()
      .withUrl(`${environment.API_URL}/notificationsHub`, {
        accessTokenFactory: () => token,
      })
      .build();

    this.registerOnServerEvents();
    this.startConnection();
  }

  private startConnection(): void {
    if (!this.connection) {
      // console.error('Connection object is not initialized');
      return;
    }

    this.connection
      .start()
      .then(async () => {
        console.log('SignalR connected');
        this.connection
          .invoke('JoinGroup', 'SystemNotifications')
          .then(() => console.log('Joined group: SystemNotifications'))
          .catch((err) => console.error('Error joining group:', err));

         //Tasks
        try {
          const userTaskResult  = await this.getUserTaskIds(); 
          const userTaskIds = userTaskResult.results.map(task => task.id); 
          await Promise.all(
            userTaskIds.map(async (taskId) => {
              try {
                await this.connection.invoke('JoinGroup', taskId.toString());
                console.log(`Joined task group: ${taskId}`);
              } catch (err) {
                console.error(`Error joining task group ${taskId}:`, err);
              }
            })
          );
        } catch (error) {
          console.error('Error fetching user task IDs:', error);
        }


        this.connectionExists = true;
        this.connectionEstablished.emit(true);
      })
      .catch((error) => {
        console.error('Connection failed:', error);
        this.connectionExists = false;
        this.connectionEstablished.emit(false);
      });
  }
  async getUserTaskIds(): Promise<TaskInListDtoPageResult> {
    // Lấy userId từ token hoặc dịch vụ người dùng
    const userId = this.tokenService.getUser().id;
  
    try {
      const response = await this.taskApiService
        .getUserTaskPaging('', null, null, userId, 1, 100)
        .pipe(takeUntil(this.unsubscribe)) 
        .toPromise(); 
  
      return response;  
    } catch (error) {
      console.error("Error fetching user tasks:", error);
      throw error; 
    }
  }
  
  private registerOnServerEvents(): void {
    if (this.connection) {
      this.connection.on('ReceiveSystemNotification', (announcement: any) => {
        // console.log('Received announcement:', announcement);
        this.announcementReceived.emit(announcement);
      });

      // Lắng nghe sự kiện thông báo nhiệm vụ
      this.connection.on('ReceiveTaskNotification', (message: any) => {
        console.log('Task notification received:', message);
        this.taskNotificationReceived.emit(message);
      });
    } else {
      console.error('Connection object is not initialized');
    }
  }

  public stopConnection(): void {
    if (this.connection) {
      this.connection
        .stop()
        .then(() => {
          // console.log('SignalR connection stopped');
          this.connectionExists = false;
        })
        .catch((error) => {
          console.log('Error stopping connection', error);
        });
    }
  }
}
