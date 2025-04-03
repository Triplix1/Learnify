import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponseWithData } from 'src/app/Models/ApiResponse';
import { ProfileResponse } from 'src/app/Models/Profile/ProfileResponse';
import { ProfileUpdateRequest } from 'src/app/Models/Profile/ProfileUpdateRequest';
import { environment } from 'src/environments/environment';
import { objectToFormData } from '../helpers/formDataHelper';
import { UpdateUserRoleRequest } from 'src/app/Models/Profile/UpdateUserRoleRequest';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {
  baseProfileUrl: string = environment.baseApiUrl + "/profile";

  constructor(private readonly httpClient: HttpClient) { }

  getById(id: number): Observable<ApiResponseWithData<ProfileResponse>> {
    return this.httpClient.get<ApiResponseWithData<ProfileResponse>>(this.baseProfileUrl + "/" + id);
  }

  update(profile: ProfileUpdateRequest): Observable<ApiResponseWithData<ProfileResponse>> {
    const formData = objectToFormData(profile);
    let params = new HttpParams();

    const options = {
      params: params,
      reportProgress: true,
    };
    return this.httpClient.put<ApiResponseWithData<ProfileResponse>>(this.baseProfileUrl + "/update", formData, options);

  }

  updateUserRole(userRoleUpdateRequest: UpdateUserRoleRequest): Observable<ApiResponseWithData<ProfileResponse>> {
    return this.httpClient.put<ApiResponseWithData<ProfileResponse>>(this.baseProfileUrl + "/update-role", userRoleUpdateRequest);
  }
}
