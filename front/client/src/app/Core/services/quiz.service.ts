import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse, ApiResponseWithData } from 'src/app/Models/ApiResponse';
import { QuizQuestionAddOrUpdateRequest } from 'src/app/Models/Course/Lesson/QuizQuestion/QuizQuestionAddOrUpdateRequest';
import { QuizQuestionUpdateResponse } from 'src/app/Models/Course/Lesson/QuizQuestion/QuizQuestionUpdateResponse';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class QuizService {
  baseQuizUrl: string = environment.baseApiUrl + '/quiz';

  constructor(private readonly httpClient: HttpClient) { }

  public addOrUpdate(quizQuestionAddOrUpdateRequest: QuizQuestionAddOrUpdateRequest): Observable<ApiResponseWithData<QuizQuestionUpdateResponse>> {
    return this.httpClient.post<ApiResponseWithData<QuizQuestionUpdateResponse>>(this.baseQuizUrl, quizQuestionAddOrUpdateRequest);
  }

  public delete(quizId: string, lessonId: string): Observable<ApiResponse> {
    return this.httpClient.delete<ApiResponse>(this.baseQuizUrl + "/" + lessonId + "/" + quizId);
  }
}
