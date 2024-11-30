import { Component, CUSTOM_ELEMENTS_SCHEMA, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, Subscription, takeUntil } from 'rxjs';
import {
  AdminApiInventoryCategoryApiClient,
  AdminApiProjectApiClient,
  AdminApiWebsiteInventoryApiClient,
  InventoryCategoryDto,
  InventoryInListDto,
  InventoryInListDtoPageResult,
  ProjectInListDto,
} from '../../../../app/api/admin-api.service.generated';
import { DividerModule } from 'primeng/divider';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { TabViewModule } from 'primeng/tabview';
import { CommonModule } from '@angular/common';
import { environment } from '../../../../environments/environment';
import { TabService } from '../../../../app/shared/services/tab.service';
import { WebsiteSubHeader } from '../website-containers/website-layout/website-sub-header/website-sub-header.component';
import { BlockUIModule } from 'primeng/blockui';
import { PaginatorModule } from 'primeng/paginator';
import { PanelModule } from 'primeng/panel';
import { WebsiteFooterComponent } from '../website-containers/website-layout/website-footer/website-footer.component';
import { TagModule } from 'primeng/tag';
import { DropdownModule } from 'primeng/dropdown';
@Component({
  selector: 'website-inventory',
  templateUrl: 'website-inventory.component.html',
  styleUrls: ['./website-inventory.component.scss'],
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
    TagModule,
    DropdownModule,
  ],
})
export class InventoryComponent implements OnInit {
  activeIndex: number = 0;
  private activeIndexSubscription: Subscription;
  private ngUnsubscribe = new Subject<void>();

  public blockedPanel: boolean = false;
  public invtCategories: any[] = [];
  public invtItems: InventoryInListDto[] | [];
  public categorySlug?: string | undefined;
  public projectId: string;
  public projectCategories: any[] = [];

  //Paging variables
  public pageIndex: number = 1;
  public pageSize: number = 10;
  public totalCount: number | 0;
  public keyword: string = '';

  public environment = environment;

  constructor(
    private route: ActivatedRoute,
    private invtCategoryApiClient: AdminApiInventoryCategoryApiClient,
    private invtApiClient: AdminApiWebsiteInventoryApiClient,
    private projectApiclient: AdminApiProjectApiClient,
    private router: Router,
    private tabService: TabService
  ) {}
  ngOnInit(): void {
    this.activeIndexSubscription = this.tabService.activeIndex$.subscribe(
      (index) => {
        this.activeIndex = index;
        this.loadData(this.invtCategories[index]?.slug);
      }
    );
    this.route.paramMap.subscribe((params) => {
      // this.categorySlug = params.get('slug');
      // this.loadData(this.categorySlug);
      // this.setActiveTabFromSlug(this.categorySlug);
    });
    this.loadInvtCategories(); // Giả sử đây là async method

    this.loadProjects();
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
    if (this.invtCategories.length > 0) {
      const index = this.invtCategories.findIndex((cat) => cat?.slug === slug);
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
    const slug = this.invtCategories[this.activeIndex]?.slug;
    if (slug) {
      this.router.navigate(['/inventories', slug]);
      this.categorySlug = slug;
      this.loadData();
    }
  }
  loadInvtCategories() {
    this.invtCategoryApiClient
      .getInventoryCategories()
      .subscribe((response: InventoryCategoryDto[]) => {
        response.forEach((element) => {
          this.invtCategories.push({
            value: element.id,
            label: element.name,
            slug: element.slug,
          });
        });
      });
  }
  loadProjects() {
    this.projectApiclient
      .getAllProjects()
      .subscribe((response: ProjectInListDto[]) => {
        response.forEach((element) => {
          this.projectCategories.push({
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
    console.log("key =>>>",this.keyword);
    this.toggleBlockUI(true);
    this.invtApiClient
      .getIntentoryWebsitePaging(
        this.keyword,
        this.categorySlug,
        this.projectId,
        this.pageIndex,
        this.pageSize
      )
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: InventoryInListDtoPageResult) => {
          // console.log('check invtItems', response);
          this.invtItems = response.results;
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
