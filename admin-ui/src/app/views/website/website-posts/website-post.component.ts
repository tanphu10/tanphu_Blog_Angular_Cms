import { Component, CUSTOM_ELEMENTS_SCHEMA, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, Subscription, takeUntil } from 'rxjs';
import {
  AdminApiPostCategoryApiClient,
  AdminApiWebsitePostApiClient,
  PostCategoryDto,
  PostInListDto,
  PostInListDtoPageResult,
} from 'src/app/api/admin-api.service.generated';
import { DividerModule } from 'primeng/divider';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { TabViewModule } from 'primeng/tabview';
import { CommonModule } from '@angular/common';
import { environment } from 'src/environments/environment';
import { TabService } from 'src/app/shared/services/tab.service';
import { WebsiteSubHeader } from '../website-containers/website-layout/website-sub-header/website-sub-header.component';
import { BlockUIModule } from 'primeng/blockui';
import { PaginatorModule } from 'primeng/paginator';
import { PanelModule } from 'primeng/panel';
import { WebsiteFooterComponent } from '../website-containers/website-layout/website-footer/website-footer.component';
import { BadgeModule } from 'primeng/badge';
import { TagModule } from 'primeng/tag';
@Component({
  selector: 'app-post',
  templateUrl: 'website-post.component.html',
  styleUrls: ['./website-post.component.scss'],
  standalone: true,
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  imports: [
    DividerModule,
    CardModule,
    ButtonModule,
    TabViewModule,
    CommonModule,
    WebsiteSubHeader,
    BlockUIModule,
    PaginatorModule,
    PanelModule,
    WebsiteFooterComponent,
    // BadgeModule,
    TagModule
  ],
})
export class PostComponent implements OnInit {
  activeIndex: number = 0;
  private activeIndexSubscription: Subscription;
  private ngUnsubscribe = new Subject<void>();

  public blockedPanel: boolean = false;
  public postCategories: any[] = [];
  public postItems: PostInListDto[];
  public categorySlug?: string = null;
  public projectSlug?: string = null;

  //Paging variables
  public pageIndex: number = 1;
  public pageSize: number = 10;
  public totalCount: number;
  public keyword: string = '';

  public environment = environment;

  constructor(
    private route: ActivatedRoute,
    private postCategoryApiClient: AdminApiPostCategoryApiClient,
    private postApiClient: AdminApiWebsitePostApiClient,
    private router: Router,
    private tabService: TabService
  ) {}
  ngOnInit(): void {
    this.activeIndexSubscription = this.tabService.activeIndex$.subscribe(
      (index) => {
        this.activeIndex = index;
        this.loadData(this.postCategories[index]?.slug);
      }
    );
    this.route.paramMap.subscribe((params) => {
      this.categorySlug = params.get('slug');
      this.loadData(this.categorySlug);
      this.setActiveTabFromSlug(this.categorySlug);
    });
    this.loadPostCategories(); // Giả sử đây là async method
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
    if (this.activeIndexSubscription) {
      this.activeIndexSubscription.unsubscribe();
    }
  }

  // Cập nhật activeIndex từ slug trong URL
  setActiveTabFromSlug(slug: string): void {
    if (this.postCategories.length > 0) {
      const index = this.postCategories.findIndex((cat) => cat?.slug === slug);
      if (index !== -1) {
        this.activeIndex = index;
        this.tabService.setActiveIndex(index);
      } else {
        this.activeIndex = 0;
        this.tabService.setActiveIndex(0);
      }
    }
  }
  onTabChange(event: any): void {
    // Khi tab được thay đổi, cập nhật URL
    const slug = this.postCategories[this.activeIndex]?.slug;
    if (slug) {
      this.router.navigate(['/posts', slug]);
      this.categorySlug = slug;
      this.loadData();
    }
  }
  loadPostCategories() {
    this.postCategoryApiClient

      .getPostCategories()
      .subscribe((response: PostCategoryDto[]) => {
        response.forEach((element) => {
          this.postCategories.push({
            value: element.id,
            label: element.name,
            slug: element.slug,
          });
        });
      });
  }

  pageChanged(event: any): void {
    this.pageIndex = event.page;
    this.pageIndex = event.page + 1;
    this.pageSize = event.rows;
    this.loadData();
  }
  loadData(categorySlug = null) {
    // console.log("key =>>>",this.keyword);
    this.toggleBlockUI(true);
    this.postApiClient
      .getPostWebsitePaging(
        this.keyword,
        this.categorySlug,
        this.projectSlug,
        this.pageIndex,
        this.pageSize
      )
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: PostInListDtoPageResult) => {
          // console.log('check postItems', response.results);
          this.postItems = response.results;
          this.totalCount = response.rowCount;
          this.toggleBlockUI(false);
        },
        error: () => {
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
