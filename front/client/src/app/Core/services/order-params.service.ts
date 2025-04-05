import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { OrderByParams } from 'src/app/Models/Params/OrderByParams';

@Injectable({
  providedIn: 'root'
})
export class OrderParamsService {

  constructor() { }

  includeOrderHeaders(orderByParams: OrderByParams, params: HttpParams) {
    if (orderByParams.orderBy) {
      params = params.append('orderBy', orderByParams.orderBy);
      params = params.append('asc', orderByParams.asc);
    }

    return params;
  }

}
