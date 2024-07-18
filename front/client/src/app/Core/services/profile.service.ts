import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse } from 'src/app/Models/ApiResponse';
import { ProfileResponse } from 'src/app/Models/ProfileResponse';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {
  baseProfileUrl: string = environment.baseApiUrl + "/profile";

  constructor(private readonly httpClient: HttpClient) { }

  getById(id: string): Observable<ApiResponse<ProfileResponse>> {
    return this.httpClient.get<ApiResponse<ProfileResponse>>(this.baseProfileUrl + "/" + id);
  }
}
