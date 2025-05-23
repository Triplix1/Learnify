import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponseWithData } from 'src/app/Models/ApiResponse';
import { AnswerAddOrUpdateRequest } from 'src/app/Models/Course/Lesson/QuizQuestion/Anwers/AnswerAddOrUpdateRequest';
import { AnswersUpdatedResponse } from 'src/app/Models/Course/Lesson/QuizQuestion/Anwers/AnswersUpdatedResponse';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AnswersService {
  baseAnswersUrl: string = environment.baseApiUrl + '/answers';

  constructor(private readonly httpClient: HttpClient) { }

  updateAnwers(answersAddOrUpdateRequest: AnswerAddOrUpdateRequest): Observable<ApiResponseWithData<AnswersUpdatedResponse>> {
    return this.httpClient.post<ApiResponseWithData<AnswersUpdatedResponse>>(this.baseAnswersUrl, answersAddOrUpdateRequest);
  }
}
