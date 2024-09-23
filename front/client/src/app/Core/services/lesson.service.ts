import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { ApiResponse, ApiResponseWithData } from 'src/app/Models/ApiResponse';
import { LessonAddOrUpdateRequest } from 'src/app/Models/Course/Lesson/LessonAddOrUpdateRequest';
import { LessonResponse } from 'src/app/Models/Course/Lesson/LessonResponse';
import { LessonStepAddOrUpdateRequest } from 'src/app/Models/Course/Lesson/LessonStepAddOrUpdateRequest';
import { LessonTitleResponse } from 'src/app/Models/Course/Lesson/LessonTitleResponse';
import { LessonUpdateResponse } from 'src/app/Models/Course/Lesson/LessonUpdateResponse';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LessonService {
  baseProfileUrl: string = environment.baseApiUrl + "/lesson";
  $lessonAddedOrUpdated: BehaviorSubject<LessonTitleResponse> = new BehaviorSubject<LessonTitleResponse>(null);

  constructor(private readonly httpClient: HttpClient) { }

  getLessonById(id: string): Observable<ApiResponseWithData<LessonResponse>> {
    return this.httpClient.get<ApiResponseWithData<LessonResponse>>(this.baseProfileUrl + "/" + id);
  }

  getLessonForUpdateById(id: string): Observable<ApiResponseWithData<LessonUpdateResponse>> {
    return this.httpClient.get<ApiResponseWithData<LessonUpdateResponse>>(this.baseProfileUrl + "/" + id);
  }

  getLessonTitlesForParagraph(paragraphId: number): Observable<ApiResponseWithData<LessonTitleResponse[]>> {
    return this.httpClient.get<ApiResponseWithData<LessonTitleResponse[]>>(this.baseProfileUrl + "/titles/" + paragraphId);
  }

  createOrUpdateLesson(lessonAddOrUpdateRequest: LessonAddOrUpdateRequest): Observable<ApiResponseWithData<LessonUpdateResponse>> {
    return this.httpClient.post<ApiResponseWithData<LessonUpdateResponse>>(this.baseProfileUrl, lessonAddOrUpdateRequest).pipe(
      tap(response => {
        const lessonTitleResponse: LessonTitleResponse = {
          id: response.data.id,
          title: response.data.title
        }
        this.$lessonAddedOrUpdated.next(lessonTitleResponse);
      })
    );
  }

  saveDraft(lessonAddOrUpdateRequest: LessonAddOrUpdateRequest): Observable<ApiResponseWithData<LessonUpdateResponse>> {
    return this.httpClient.post<ApiResponseWithData<LessonUpdateResponse>>(this.baseProfileUrl + "/draft", lessonAddOrUpdateRequest);
  }

  deleteLesson(id: string): Observable<ApiResponse> {
    return this.httpClient.delete<ApiResponse>(this.baseProfileUrl + "/" + id);
  }
}
