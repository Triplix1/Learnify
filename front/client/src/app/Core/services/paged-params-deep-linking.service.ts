import { Injectable } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { PagedListParams } from 'src/app/Models/Params/PagedListParams';

@Injectable({
  providedIn: 'root'
})
export class PagedParamsDeepLinkingService {

  constructor(private route: ActivatedRoute, private router: Router) { }

  setPaginatedParams(paginatedParams: PagedListParams): void {
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: this.getPaginationQueryParams(paginatedParams),
      queryParamsHandling: 'merge',
      skipLocationChange: false
    });
  }

  getPaginationQueryParams(paginatedParams: PagedListParams): Params {
    return {
      pageSize: paginatedParams.pageSize as number,
      pageNumber: paginatedParams.pageNumber as number
    };
  }

  getPaginatedParams(): PagedListParams | null {
    const pageNumberStr = this.route.snapshot.queryParamMap.get('pageNumber');
    const pageSizeStr = this.route.snapshot.queryParamMap.get('pageSize');

    const pageNumber: number = pageNumberStr ? +pageNumberStr : null;
    const pageSize: number = pageSizeStr ? +pageSizeStr : null;

    if (!pageNumber || !pageSize) {
      return null;
    }

    return { pageSize, pageNumber };
  }

}
