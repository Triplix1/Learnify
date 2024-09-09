import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse, ApiResponseWithData } from 'src/app/Models/ApiResponse';
import { CourseCreateRequest } from 'src/app/Models/Course/CourseCreateRequest';
import { CourseResponse } from 'src/app/Models/Course/CourseResponse';
import { CourseTitleResponse } from 'src/app/Models/Course/CourseTitleResponse';
import { CourseUpdateRequest } from 'src/app/Models/Course/CourseUpdateRequest';
import { PagedList } from 'src/app/Models/PagedList';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CourseService {
  baseCourseUrl: string = environment.baseApiUrl + "/course";

  constructor(private readonly httpClient: HttpClient) { }

  getCourseTitles(): Observable<ApiResponseWithData<PagedList<CourseTitleResponse>>> {
    return this.httpClient.get<ApiResponseWithData<PagedList<CourseTitleResponse>>>(this.baseCourseUrl);
  }

  getForUpdate(id: number): Observable<ApiResponseWithData<CourseResponse>> {
    return this.httpClient.get<ApiResponseWithData<CourseResponse>>(this.baseCourseUrl + "/" + id);
  }

  createCourse(courseCreateRequest: CourseCreateRequest): Observable<ApiResponseWithData<CourseResponse>> {
    return this.httpClient.post<ApiResponseWithData<CourseResponse>>(this.baseCourseUrl, courseCreateRequest);
  }

  publishCourse(id: number, publish: boolean): Observable<ApiResponseWithData<CourseResponse>> {
    return this.httpClient.post<ApiResponseWithData<CourseResponse>>(this.baseCourseUrl + "/" + id, { publish });
  }

  updateCourse(courseUpdateRequest: CourseUpdateRequest): Observable<ApiResponseWithData<CourseResponse>> {
    return this.httpClient.put<ApiResponseWithData<CourseResponse>>(this.baseCourseUrl, courseUpdateRequest);
  }

  deleteCourse(id: number): Observable<ApiResponse> {
    return this.httpClient.delete<ApiResponse>(this.baseCourseUrl + "/" + id);
  }
}
