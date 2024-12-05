import {
  Component,
  OnInit,
  OnDestroy,
  CUSTOM_ELEMENTS_SCHEMA,
} from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { ImageModule } from 'primeng/image';
import { BlockUIModule } from 'primeng/blockui';
import { CheckboxModule } from 'primeng/checkbox';
import { InputNumberModule } from 'primeng/inputnumber';
import { KeyFilterModule } from 'primeng/keyfilter';
import { PanelModule } from 'primeng/panel';
import { WebsiteSubHeader } from '../website-containers/website-layout/website-sub-header/website-sub-header.component';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { WebsiteFooterComponent } from '../website-containers/website-layout/website-footer/website-footer.component';
import { ActivatedRoute } from '@angular/router';
import { PaginatorModule } from 'primeng/paginator';
import { Subject, takeUntil } from 'rxjs';
import { FieldsetModule } from 'primeng/fieldset';

import {
  AdminApiPostApiClient,
  PostInListDto,
  PostInListDtoPageResult,
} from 'src/app/api/admin-api.service.generated';
import { environment } from 'src/environments/environment';
@Component({
  templateUrl: 'website-search.component.html',
  styleUrls: ['./website-search.component.scss'],
  standalone: true,
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  imports: [
    ProgressSpinnerModule,
    ImageModule,
    BlockUIModule,
    CheckboxModule,
    InputNumberModule,
    KeyFilterModule,
    PanelModule,
    ReactiveFormsModule,
    WebsiteSubHeader,
    CardModule,
    ButtonModule,
    CommonModule,
    WebsiteFooterComponent,
    BlockUIModule,
    PaginatorModule,
    FieldsetModule,
  ],
})
export class WebsiteSearchComponent implements OnInit, OnDestroy {
  //Paging variables
  public pageIndex: number = 1;
  public pageSize: number = 10;
  public totalCount: number;
  keyword: string | null = null;
  public categorySlug?: string = null;
  public projectSlug?: string = null;
  public postItems: PostInListDto[];

  private ngUnsubscribe = new Subject<void>();
  public blockedPanel: boolean = false;
  public environment = environment;

  constructor(
    private route: ActivatedRoute,
    private postApiClient: AdminApiPostApiClient
  ) {}

  ngOnInit(): void {
    // this.routeParam.queryParamMap.subscribe((p) => {
    //   this.keyword = p.get('q');
    // });
    this.route.paramMap.subscribe((params) => {
      this.keyword = params.get('search');
      console.log('key', this.keyword);
      this.loadData(this.keyword);
    });
  }
  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
  pageChanged(event: any): void {
    this.pageIndex = event.page;
    this.pageIndex = event.page + 1;
    this.pageSize = event.rows;
    this.loadData();
  }
  loadData(categorySlug = null) {
    console.log('key =>>>', this.keyword);
    console.log('categorySlug =>>>', this.categorySlug);
    this.toggleBlockUI(true);
    this.postApiClient
      .getPostsPaging(
        this.keyword,
        this.categorySlug,
        this.projectSlug,
        this.pageIndex,
        this.pageSize
      )
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: PostInListDtoPageResult) => {
          console.log('check postItems', response.results);
          this.postItems = response.results;
          this.totalCount = response.rowCount;
          this.toggleBlockUI(false);
        },
        error: (e) => {
          console.log('error', e);

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
