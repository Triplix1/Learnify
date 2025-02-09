import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse, ApiResponseWithData } from 'src/app/Models/ApiResponse';
import { AnswersValidateRequest } from 'src/app/Models/Course/Lesson/QuizQuestion/Anwers/AnswersValidateRequest';
import { UserLessonQuizAnswerResponse } from 'src/app/Models/Course/Lesson/QuizQuestion/Anwers/UserLessonQuizAnswerResponse';
import { UserQuizAnswerResponse } from 'src/app/Models/Course/Lesson/QuizQuestion/Anwers/UserQuizAnswerResponse';
import { QuizQuestionAddOrUpdateRequest } from 'src/app/Models/Course/Lesson/QuizQuestion/QuizQuestionAddOrUpdateRequest';
import { QuizQuestionUpdateResponse } from 'src/app/Models/Course/Lesson/QuizQuestion/QuizQuestionUpdateResponse';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class QuizService {
  baseQuizUrl: string = environment.baseApiUrl + '/quiz';

  constructor(private readonly httpClient: HttpClient) { }

  addOrUpdate(quizQuestionAddOrUpdateRequest: QuizQuestionAddOrUpdateRequest): Observable<ApiResponseWithData<QuizQuestionUpdateResponse>> {
    return this.httpClient.post<ApiResponseWithData<QuizQuestionUpdateResponse>>(this.baseQuizUrl, quizQuestionAddOrUpdateRequest);
  }

  delete(quizId: string, lessonId: string): Observable<ApiResponse> {
    return this.httpClient.delete<ApiResponse>(this.baseQuizUrl + "/" + lessonId + "/" + quizId);
  }

  check(answersValidateRequest: AnswersValidateRequest): Observable<ApiResponseWithData<UserQuizAnswerResponse[]>> {
    return this.httpClient.post<ApiResponseWithData<UserQuizAnswerResponse[]>>(this.baseQuizUrl + "/check", answersValidateRequest);
  }
}
