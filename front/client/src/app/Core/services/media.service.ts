import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { ApiResponseWithData } from 'src/app/Models/ApiResponse';
import { PrivateFileBlobCreateRequest } from 'src/app/Models/File/PrivateFileBlobCreateRequest';
import { PrivateFileDataResponse } from 'src/app/Models/File/PrivateFileDataResponse';
import { environment } from 'src/environments/environment';
import { objectToFormData } from '../helpers/formDataHelper';
import { AuthService } from './auth.service';
import { MediaType } from 'src/app/Models/enums/MediaType';

@Injectable({
  providedIn: 'root'
})
export class MediaService {
  baseUrl: string = environment.baseApiUrl + "/media";

  constructor(private readonly httpClient: HttpClient, private readonly authService: AuthService) { }

  getFileUrl(fileId: number): Observable<string> {
    return this.authService.tokenData$.pipe(
      map(tokenData => {
        let fileUrl = this.baseUrl + "/" + fileId;

        if (tokenData)
          fileUrl += "?access_token=" + tokenData.token;

        return fileUrl
      })
    )
  }

  create(fileCreateRequest: PrivateFileBlobCreateRequest): Observable<Object> {
    const formData = objectToFormData(fileCreateRequest);

    return this.httpClient.post(this.baseUrl, formData, {
      params: new HttpParams(),
      reportProgress: true,  // Enable progress reporting
      observe: 'events',      // Observe multiple events including upload progress
      headers: new HttpHeaders({
        'enctype': 'multipart/form-data'
      }),
    });
  }
}
