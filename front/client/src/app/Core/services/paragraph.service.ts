import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse, ApiResponseWithData } from 'src/app/Models/ApiResponse';
import { ParagraphCreateRequest } from 'src/app/Models/Course/Paragraph/ParagraphCreateRequest';
import { ParagraphResponse } from 'src/app/Models/Course/Paragraph/ParagraphResponse';
import { ParagraphUpdateRequest } from 'src/app/Models/Course/Paragraph/ParagraphUpdateRequest';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ParagraphService {
  baseParagraphUrl: string = environment.baseApiUrl + "/paragraph";

  constructor(private readonly httpClient: HttpClient) { }

  createParagraph(paragraphCreateRequest: ParagraphCreateRequest): Observable<ApiResponseWithData<ParagraphResponse>> {
    return this.httpClient.post<ApiResponseWithData<ParagraphResponse>>(this.baseParagraphUrl, paragraphCreateRequest);
  }

  updateParagraph(ParagraphUpdateRequest: ParagraphUpdateRequest): Observable<ApiResponseWithData<ParagraphResponse>> {
    return this.httpClient.put<ApiResponseWithData<ParagraphResponse>>(this.baseParagraphUrl, ParagraphUpdateRequest);
  }

  deleteParagraph(id: number): Observable<ApiResponse> {
    return this.httpClient.delete<ApiResponse>(this.baseParagraphUrl + "/" + id);
  }
}
