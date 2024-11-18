import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module'; 
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { MessageService } from 'primeng/api';
import { AdminModule } from './admin/admin.module';
import { WebsiteModule } from './website/website.module';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule, // Bắt buộc để sử dụng các module UI với hiệu ứng
    ToastModule, // Sử dụng để hiển thị thông báo (như p-toast)
    ConfirmDialogModule, // Sử dụng cho hộp thoại xác nhận
    WebsiteModule,
    AdminModule, // Import module con vào đây
  ],
  providers: [MessageService],
  bootstrap: [AppComponent], // Thành phần khởi động ứng dụng
})
export class AppModule {}
