import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RegisterModeratorRequest } from 'src/app/Models/Auth/RegisterModeratorRequest';
import { ProfileResponse } from 'src/app/Models/Profile/ProfileResponse';
import { environment } from 'src/environments/environment';
import { OrderParamsService } from './order-params.service';
import { PagedParamsService } from './paged-params.service';
import { AdminsListParams } from 'src/app/Models/Params/AdminsListParams';

@Injectable({
  providedIn: 'root'
})
export class AdminsService {
  baseApiUrl = environment.baseApiUrl + '/admins'

  constructor(private readonly httpClient: HttpClient,
    private readonly paginationService: PagedParamsService,
    private readonly orderByService: OrderParamsService) { }

  getList(managersListParams: AdminsListParams) {
    let params = this.getManagersHeaders(managersListParams);
    return this.paginationService.getPaginatedResult<ProfileResponse>(this.baseApiUrl, params, this.httpClient);
  }

  private getManagersHeaders(userParams: AdminsListParams) {
    let params = new HttpParams();

    params = this.paginationService.includePaginationHeaders(userParams, params);
    params = this.orderByService.includeOrderHeaders(userParams, params);

    return params;
  }

  create(moderatorCreateRequest: RegisterModeratorRequest) {
    return this.httpClient.post(this.baseApiUrl, moderatorCreateRequest);
  }

  delete(id: number) {
    return this.httpClient.delete(this.baseApiUrl + "/" + id);
  }

}
