import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponseWithData } from 'src/app/Models/ApiResponse';
import { PrivateFileBlobCreateRequest } from 'src/app/Models/File/PrivateFileBlobCreateRequest';
import { PrivateFileDataResponse } from 'src/app/Models/File/PrivateFileDataResponse';
import { environment } from 'src/environments/environment';
import { objectToFormData } from '../helpers/formDataHelper';

@Injectable({
  providedIn: 'root'
})
export class MediaService {
  baseUrl: string = environment.baseApiUrl + "/media";

  constructor(private readonly httpClient: HttpClient) { }

  create(fileCreateRequest: PrivateFileBlobCreateRequest): Observable<ApiResponseWithData<PrivateFileDataResponse>> {
    const formData = objectToFormData(fileCreateRequest);
    let params = new HttpParams();

    const options = {
      params: params,
      reportProgress: true,
    };

    return this.httpClient.post<ApiResponseWithData<PrivateFileDataResponse>>(this.baseUrl, formData, options);
  }
}
