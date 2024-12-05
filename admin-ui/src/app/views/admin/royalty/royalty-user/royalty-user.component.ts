import { Component, OnDestroy, OnInit } from '@angular/core';
import { ConfirmationService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { Subject, takeUntil } from 'rxjs';
import { RoyaltyReportByUserDto, AdminApiRoyaltyApiClient } from 'src/app/api/admin-api.service.generated';
import { MessageConstants } from 'src/app/shared/constants/message.constant';
import { AlertService } from 'src/app/shared/services/alert.service';
// import { AdminApiRoyaltyApiClient, RoyaltyReportByUserDto } from '../../../api/admin-api.service.generated';
// import { AlertService } from '../../../shared/services/alert.service';
// import { MessageConstants } from '../../../shared/constants/message.constant';

@Component({
  selector: 'app-royalty-user',
  templateUrl: './royalty-user.component.html',

})
export class RoyaltyUserComponent implements OnInit, OnDestroy {

  //System variables
  private ngUnsubscribe = new Subject<void>();
  public blockedPanel: boolean = false;
  public items: RoyaltyReportByUserDto[] = [];
  public userName: string = '';
  public fromMonth: number = 1;
  public fromYear: number = new Date().getFullYear();
  public toMonth: number = 12;
  public toYear: number = new Date().getFullYear();
  constructor(
    private RoyaltyApiClient: AdminApiRoyaltyApiClient,
    public dialogService: DialogService,
    private alertService: AlertService,
    private confirmationService: ConfirmationService) { }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.toggleBlockUI(true);
    // console.log("userName royaltyNAme=>>>",this.u);
    this.RoyaltyApiClient.getRoyaltyReportByUser(this.userName, this.fromMonth, this.fromYear, this.toMonth, this.toYear)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: RoyaltyReportByUserDto[]) => {
          // console.log("checkdata royalty trả về",response)
          this.items = response;
          this.toggleBlockUI(false);
        },
        error: () => {
          this.toggleBlockUI(false);

        }
      });
  }
  payForUser(userId: string) {
    // console.log("check",userId)
    this.confirmationService.confirm({
      message: "Bạn có chắc muốn thanh toán?",
      accept: () => {
        this.payConfirm(userId)
      }
    });
  }

  payConfirm(userId: string) {
    // console.log("confirm id",userId);
    this.toggleBlockUI(true);
    this.RoyaltyApiClient.payRoyalty(userId)
      .subscribe({
        next: () => {
          // console.log("oke")
          this.alertService.showSuccess(MessageConstants.UPDATED_OK_MSG);
          this.loadData();
          this.toggleBlockUI(false);
        },
        error: (error) => {
          // console.log("check lỗi pay for user",error);
          this.toggleBlockUI(false);
        }
      });
  }
  private toggleBlockUI(enabled: boolean) {
    if (enabled == true) {
      this.blockedPanel = true;
    }
    else {
      setTimeout(() => {
        this.blockedPanel = false;
      }, 1000);
    }

  }

}