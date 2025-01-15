import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponseWithData } from 'src/app/Models/ApiResponse';
import { AnswerAddOrUpdateRequest } from 'src/app/Models/Course/Lesson/QuizQuestion/Anwers/AnswerAddOrUpdateRequest';
import { AnswersUpdateResponse } from 'src/app/Models/Course/Lesson/QuizQuestion/Anwers/AnswersUpdateResponse';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AnswersService {
  baseAnswersUrl: string = environment.baseApiUrl + '/answers';

  constructor(private readonly httpClient: HttpClient) { }

  updateAnwers(answersAddOrUpdateRequest: AnswerAddOrUpdateRequest): Observable<ApiResponseWithData<AnswersUpdateResponse>> {
    return this.httpClient.post<ApiResponseWithData<AnswersUpdateResponse>>(this.baseAnswersUrl, answersAddOrUpdateRequest);
  }
}
