import { Injectable } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { OrderByParams } from 'src/app/Models/Params/OrderByParams';

@Injectable({
  providedIn: 'root'
})
export class OrderParamsDeepLinkingService {

  constructor(private route: ActivatedRoute, private router: Router) { }

  setOrderParams(orderByParams: OrderByParams): void {
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: this.getOrderByQueryParams(orderByParams),
      queryParamsHandling: 'merge',
      skipLocationChange: false
    });
  }

  getOrderByQueryParams(orderByParams: OrderByParams): Params {
    return {
      pageSize: orderByParams.orderBy,
      pageNumber: !!orderByParams.asc
    };
  }

  getOrderByParams(): OrderByParams {
    const orderBy: string = this.route.snapshot.queryParamMap.get('orderBy');

    const ascStr = this.route.snapshot.queryParamMap.get('asc');
    const asc: boolean = ascStr ? !!ascStr : null;

    if (!orderBy || !asc) {
      return null;
    }

    return { orderBy, asc };
  }
}
