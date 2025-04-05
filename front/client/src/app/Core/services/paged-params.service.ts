import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { map } from 'rxjs';
import { PagedList } from 'src/app/Models/PagedList';
import { ApiResponseWithData } from 'src/app/Models/ApiResponse';
import { PagedListParams } from 'src/app/Models/Params/PagedListParams';

@Injectable({
  providedIn: 'root'
})
export class PagedParamsService {

  constructor() { }

  getPaginatedResult<T>(url: string, params: HttpParams, http: HttpClient) {
    return http.get<ApiResponseWithData<PagedList<T>>>(url, { observe: 'response', params }).pipe(
      map(response => {
        if (response.body.data) {
          return response.body.data;
        }
        return null;
      })
    );
  }

  includePaginationHeaders(paginatedParams: PagedListParams, params: HttpParams) {
    params = params.append('pageNumber', paginatedParams.pageNumber);
    params = params.append('pageSize', paginatedParams.pageSize);

    return params;
  }
}
