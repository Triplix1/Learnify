import { Injectable } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ManagersListParams } from 'src/app/Models/Params/ManagersListParams';
import { OrderParamsDeepLinkingService } from './order-params-deep-linking.service';
import { PagedParamsDeepLinkingService } from './paged-params-deep-linking.service';
import { AdminsListParams } from 'src/app/Models/Params/AdminsListParams';

@Injectable({
  providedIn: 'root'
})
export class AdminsDeepLinkingService {
  private paginationDeepLinkingService = new PagedParamsDeepLinkingService(this.route, this.router);
  private orderDeepLinkingService = new OrderParamsDeepLinkingService(this.route, this.router);

  constructor(private route: ActivatedRoute, private router: Router) { }

  setManagersListParams(managerParams: AdminsListParams) {
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {
        ...this.paginationDeepLinkingService.getPaginationQueryParams(managerParams),
        ...this.orderDeepLinkingService.getOrderByQueryParams(managerParams)
      },
      queryParamsHandling: 'merge',
      skipLocationChange: false
    });
  }

  getUserParams(): AdminsListParams {
    let managerParams: AdminsListParams;

    const paginatedParams = this.paginationDeepLinkingService.getPaginatedParams();
    const orderByParams = this.orderDeepLinkingService.getOrderByParams();

    if (paginatedParams) {
      managerParams = {
        ...paginatedParams,
        ...orderByParams
      };
    }
    else {
      managerParams = {
        pageNumber: 1,
        pageSize: 10,
        asc: null,
        orderBy: null
      };
    }

    return managerParams;
  }

}
