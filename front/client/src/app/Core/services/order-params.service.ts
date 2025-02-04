import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { OrderByParams } from 'src/app/Models/Params/OrderByParams';

@Injectable({
  providedIn: 'root'
})
export class OrderParamsService {

  constructor() { }

  includeOrderHeaders(orderByParams: OrderByParams, params: HttpParams) {
    params = params.append('pageNumber', orderByParams.orderBy);
    params = params.append('pageSize', orderByParams.asc);

    return params;
  }

}
