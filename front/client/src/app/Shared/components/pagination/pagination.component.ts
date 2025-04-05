import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PagedParamsDeepLinkingService } from 'src/app/Core/services/paged-params-deep-linking.service';
import { BaseComponent } from 'src/app/Models/BaseComponent';
import { DropdownItem } from 'src/app/Models/DropdownItem';
import { PagedListParams } from 'src/app/Models/Params/PagedListParams';


@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.scss']
})
export class PaginationComponent extends BaseComponent implements OnInit {
  @Input({ required: true }) itemsPerPage: number[] = [10];
  @Output() changePage: EventEmitter<PagedListParams> = new EventEmitter<PagedListParams>();
  @Input({ required: true }) totalItems: number = 0;

  pageSizes: DropdownItem[] = [];
  paginatedParams: PagedListParams = { pageSize: this.itemsPerPage[0], pageNumber: 1 };
  pagedParamsDeepLinkingService: PagedParamsDeepLinkingService;

  get totalPages(): number {
    return Math.ceil(this.totalItems / this.paginatedParams.pageSize);
    //return 10;
  }

  constructor(private readonly route: ActivatedRoute, private readonly router: Router) {
    super();

    this.pagedParamsDeepLinkingService = new PagedParamsDeepLinkingService(route, router);
  }

  ngOnInit(): void {
    const paginatedParams = this.pagedParamsDeepLinkingService.getPaginatedParams();
    if (paginatedParams)
      this.paginatedParams = paginatedParams;

    for (let size of this.itemsPerPage) {
      this.pageSizes.push({ itemText: size + '', action: () => this.onPageSizeChange(size) })
    }
  }

  onPageChange(page: number) {
    this.paginatedParams.pageNumber = +page;
    this.pagedParamsDeepLinkingService.setPaginatedParams(this.paginatedParams);
    this.changePage.emit(this.paginatedParams)
  }

  onPageSizeChange(pageSize: number) {
    this.paginatedParams.pageNumber = 1;
    this.paginatedParams.pageSize = +pageSize;
    this.pagedParamsDeepLinkingService.setPaginatedParams(this.paginatedParams);
    this.changePage.emit(this.paginatedParams);
  }
}
