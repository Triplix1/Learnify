import { Injectable } from '@angular/core';
import { ManagersListParams as ModeratorsListParams } from 'src/app/Models/Params/ManagersListParams';
import { environment } from 'src/environments/environment';
import { PagedParamsService } from './paged-params.service';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ProfileResponse } from 'src/app/Models/Profile/ProfileResponse';
import { OrderParamsService } from './order-params.service';
import { RegisterModeratorRequest } from 'src/app/Models/Auth/RegisterModeratorRequest';

@Injectable({
  providedIn: 'root'
})
export class ModeratorsService {
  baseApiUrl = environment.baseApiUrl + '/moderators'

  constructor(private readonly httpClient: HttpClient,
    private readonly paginationService: PagedParamsService,
    private readonly orderByService: OrderParamsService) { }

  getList(managersListParams: ModeratorsListParams) {
    let params = this.getManagersHeaders(managersListParams);
    return this.paginationService.getPaginatedResult<ProfileResponse>(this.baseApiUrl, params, this.httpClient);
  }

  private getManagersHeaders(userParams: ModeratorsListParams) {
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
