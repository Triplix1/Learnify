import { Injectable } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { CourseParams } from 'src/app/Models/Params/Course/CourseParams';
import { OrderParamsDeepLinkingService } from './order-params-deep-linking.service';
import { PagedParamsDeepLinkingService } from './paged-params-deep-linking.service';

@Injectable({
  providedIn: 'root'
})
export class CourseParamsDeepLinkingService {
  private orderParamsDeepLinking: OrderParamsDeepLinkingService = new OrderParamsDeepLinkingService(this.route, this.router);
  private pagedParamsDeepLinkingService: PagedParamsDeepLinkingService = new PagedParamsDeepLinkingService(this.route, this.router);

  constructor(private route: ActivatedRoute, private router: Router) { }

  setCourseParams(courseParams: CourseParams): void {
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: this.getCourseQueryParams(courseParams),
      queryParamsHandling: 'merge',
      skipLocationChange: false
    });
  }

  getCourseQueryParams(courseParams: CourseParams): Params {
    return {
      ...this.pagedParamsDeepLinkingService.getPaginationQueryParams(courseParams.pagedListParams),
      ...this.orderParamsDeepLinking.getOrderByQueryParams(courseParams.orderByParams),
      search: courseParams.search,
      authorId: courseParams.authorId,
    };
  }

  getCourseParams(): CourseParams {
    const authorIdStr = this.route.snapshot.queryParamMap.get('authorId');
    const authorId: number = authorIdStr ? +authorIdStr : null;

    const search: string = this.route.snapshot.queryParamMap.get('search');

    let pagedListParams = this.pagedParamsDeepLinkingService.getPaginatedParams();

    if (!pagedListParams)
      pagedListParams = { pageNumber: 1, pageSize: 10 };

    const orderByParams = this.orderParamsDeepLinking.getOrderByParams();

    return { pagedListParams, orderByParams, search, authorId };
  }
}
