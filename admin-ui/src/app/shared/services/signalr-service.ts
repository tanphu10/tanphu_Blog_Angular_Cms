import { EventEmitter, Injectable } from '@angular/core';
import { HubConnectionBuilder, HubConnection } from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { TokenStorageService } from 'src/app/shared/services/token-storage.service'; // Import the token service

@Injectable()
export class SignalRService {
  private connection: HubConnection;
  public announcementReceived: EventEmitter<any>;
  public connectionEstablished: EventEmitter<Boolean>;
  public connectionExists: Boolean;

  constructor(private tokenService: TokenStorageService) {
    // Inject token service
    this.connectionEstablished = new EventEmitter<Boolean>();
    this.announcementReceived = new EventEmitter<any>();
    this.connectionExists = false;

    // Retrieve the token from local storage or wherever you store it
    const token = this.tokenService.getToken(); // Adjust based on your implementation
    if (!token) {
      console.error('No token found!');
      return;
    }
    // Khởi tạo kết nối SignalR với token trong header
    this.connection = new HubConnectionBuilder()
      .withUrl(`${environment.API_URL}/notificationsHub`, {
        accessTokenFactory: () => token, // Gửi token vào header
      })
      // .withAutomaticReconnect() // Tự động kết nối lại khi bị mất kết nối
      .build();

    // Register events and start connection
    this.registerOnServerEvents();
    this.startConnection();
  }

  // Bắt đầu kết nối SignalR
  private startConnection(): void {
    if (this.connection) {
      this.connection
        .start()
        .then(() => {
          // console.log('SignalR connected');
          this.connectionExists = true;
          this.connectionEstablished.emit(true);
          // this.connectionExists = true;
          // Lắng nghe sự kiện từ server để nhận thông tin connectionId và userName
          // this.connection.on(
          //   'ReceiveConnectionInfo',
          //   (connectionId: string, userName: string) => {
          //     console.log('Connection ID: ', connectionId);
          //     console.log('User Name: ', userName);
          //   }
          // );
        })
        .catch((error) => {
          console.error('Connection failed:', error);
          this.connectionExists = false;
          this.connectionEstablished.emit(false);
        });

      // // Quản lý kết nối lại
      // this.connection.onclose((error) => {
      //   console.error('Connection closed:', error);
      //   this.connectionExists = false;
      // });

      // // Quản lý khi kết nối lại thành công
      // this.connection.onreconnected(() => {
      //   console.log('SignalR reconnected');
      //   this.connectionExists = true;
      // });

      // // Quản lý khi kết nối lần đầu
      // this.connection.onreconnecting((error) => {
      //   console.error('SignalR reconnecting', error);
      //   this.connectionExists = false;
      // });
    }
  }

  // private startConnection(): void {
  //   if (this.connection) {
  //     this.connection
  //       .start()
  //       .then((data: any) => {
  //         console.log('Now connected:', data); // Log the entire data object to check its structure
  //         if (data && data.transport) {
  //           console.log(
  //             'Connected transport:',
  //             data.transport?.name,
  //             ', connection ID:',
  //             data.id
  //           );
  //         } else {
  //           console.log('Transport is undefined, connection ID:', data?.id);
  //         }
  //         this.connectionEstablished.emit(true);
  //         this.connectionExists = true;
  //       })
  //       .catch((error) => {
  //         console.log('Could not connect', error);
  //         this.connectionEstablished.emit(false);
  //       });
  //   } else {
  //     console.log('Connection object is undefined');
  //   }
  // }

  private registerOnServerEvents(): void {
    if (this.connection) {
      this.connection.on('addAnnouncement', (announcement: any) => {
        // console.log('Received announcement:', announcement);
        this.announcementReceived.emit(announcement);
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
