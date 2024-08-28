import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from 'src/app/Models/ApiResponse';
import { ProfileResponse } from 'src/app/Models/Profile/ProfileResponse';
import { ProfileUpdateRequest } from 'src/app/Models/Profile/ProfileUpdateRequest';
import { environment } from 'src/environments/environment.development';
import { objectToFormData } from '../helpers/formDataHelper';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {
  baseProfileUrl: string = environment.baseApiUrl + "/profile";

  constructor(private readonly httpClient: HttpClient) { }

  getById(id: number): Observable<ApiResponse<ProfileResponse>> {
    return this.httpClient.get<ApiResponse<ProfileResponse>>(this.baseProfileUrl + "/" + id);
  }

  update(profile: ProfileUpdateRequest): Observable<ApiResponse<ProfileResponse>> {
    const formData = objectToFormData(profile);
    let params = new HttpParams();

    const options = {
      params: params,
      reportProgress: true,
    };
    return this.httpClient.put<ApiResponse<ProfileResponse>>(this.baseProfileUrl + "/update", formData, options);

  }
}
