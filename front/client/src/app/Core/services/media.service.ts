import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, of, switchMap } from 'rxjs';
import { PrivateFileBlobCreateRequest } from 'src/app/Models/File/PrivateFileBlobCreateRequest';
import { environment } from 'src/environments/environment';
import { objectToFormData } from '../helpers/formDataHelper';
import { AuthService } from './auth.service';
import { ApiResponseWithData } from 'src/app/Models/ApiResponse';
import { UrlResponse } from 'src/app/Models/File/UrlResponse';

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

  getFile(fileId: number): Observable<Blob> {
    const token = localStorage.getItem('authToken');  // Retrieve stored token

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });


    return this.authService.tokenData$.pipe(
      switchMap(tokenData => {
        let fileUrl = this.baseUrl + "/" + fileId;

        if (tokenData)
          fileUrl += "?access_token=" + tokenData.token;

        return this.httpClient.get(fileUrl,
          {
            responseType: 'blob'  // Get response as Blob to handle both text & binary
          })
      })
    )
  }

  // getFileUrl(fileId: number): Observable<string> {
  //   return this.authService.tokenData$.pipe(
  //     switchMap(tokenData => {
  //       let fileUrl = this.baseUrl + "/" + fileId;

  //       const headers = new HttpHeaders({
  //         Authorization: `Bearer ${tokenData.token}` // Отримання токена
  //       });

  //       return this.httpClient.get(fileUrl, {
  //         headers: headers,
  //         responseType: 'blob'
  //       }).pipe(map(blob => {
  //         return URL.createObjectURL(blob);
  //       }));
  //     })
  //   )
  // }

  getVideoUrl(fileId: number): Observable<ApiResponseWithData<UrlResponse>> {
    return this.httpClient.get<ApiResponseWithData<UrlResponse>>(this.baseUrl + "/video/" + fileId);
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
