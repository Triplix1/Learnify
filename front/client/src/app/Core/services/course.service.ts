import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse, ApiResponseWithData } from 'src/app/Models/ApiResponse';
import { CourseCreateRequest } from 'src/app/Models/Course/CourseCreateRequest';
import { CourseResponse } from 'src/app/Models/Course/CourseResponse';
import { CourseTitleResponse } from 'src/app/Models/Course/CourseTitleResponse';
import { CourseUpdateRequest } from 'src/app/Models/Course/CourseUpdateRequest';
import { PagedList } from 'src/app/Models/PagedList';
import { environment } from 'src/environments/environment';
import { objectToFormData } from '../helpers/formDataHelper';
import { PrivateFileBlobCreateRequest } from 'src/app/Models/File/PrivateFileBlobCreateRequest';
import { CourseParams } from 'src/app/Models/Params/Course/CourseParams';
import { objectToQueryParams } from '../helpers/queryParamsHelper';
import { PagedParamsService } from './paged-params.service';
import { OrderParamsService } from './order-params.service';
import { CourseMainInfo } from 'src/app/Models/Course/CourseMainInfo';
import { CourseStudyResponse } from 'src/app/Models/Course/CourseStudyResponse';
import { PublishCourseRequest } from 'src/app/Models/Course/PublishCourseRequest';

@Injectable({
  providedIn: 'root'
})
export class CourseService {
  baseCourseUrl: string = environment.baseApiUrl + "/course";

  constructor(private readonly httpClient: HttpClient, private readonly pagedParamsService: PagedParamsService, private readonly orderParamsService: OrderParamsService) { }

  getCourseTitles(courseParams: CourseParams): Observable<ApiResponseWithData<PagedList<CourseTitleResponse>>> {
    // const params = this.getHttpParamsFromCourseParams(courseParams);
    const params = objectToQueryParams(courseParams);

    return this.httpClient.get<ApiResponseWithData<PagedList<CourseTitleResponse>>>(this.baseCourseUrl, { params: params });
  }

  getMyCourseTitles(courseParams: CourseParams): Observable<ApiResponseWithData<PagedList<CourseTitleResponse>>> {
    // const params = this.getHttpParamsFromCourseParams(courseParams);
    const params = objectToQueryParams(courseParams);

    return this.httpClient.get<ApiResponseWithData<PagedList<CourseTitleResponse>>>(this.baseCourseUrl + '/my-courses', { params: params });
  }

  getMySubscribedCourseTitles(courseParams: CourseParams): Observable<ApiResponseWithData<PagedList<CourseTitleResponse>>> {
    // const params = this.getHttpParamsFromCourseParams(courseParams);
    const params = objectToQueryParams(courseParams);

    return this.httpClient.get<ApiResponseWithData<PagedList<CourseTitleResponse>>>(this.baseCourseUrl + '/my-subscribed-courses', { params: params });
  }


  getForUpdate(id: number): Observable<ApiResponseWithData<CourseResponse>> {
    return this.httpClient.get<ApiResponseWithData<CourseResponse>>(this.baseCourseUrl + "/" + id);
  }

  getStudyResponse(id: number): Observable<ApiResponseWithData<CourseStudyResponse>> {
    return this.httpClient.get<ApiResponseWithData<CourseStudyResponse>>(this.baseCourseUrl + '/study/' + id);
  }

  getMainInfo(id: number): Observable<ApiResponseWithData<CourseMainInfo>> {
    return this.httpClient.get<ApiResponseWithData<CourseMainInfo>>(this.baseCourseUrl + "/main-info/" + id);
  }

  createCourse(courseCreateRequest: CourseCreateRequest): Observable<ApiResponseWithData<CourseResponse>> {
    return this.httpClient.post<ApiResponseWithData<CourseResponse>>(this.baseCourseUrl, courseCreateRequest);
  }

  updatePhoto(fileCreateRequest: PrivateFileBlobCreateRequest): Observable<Object> {
    const formData = objectToFormData(fileCreateRequest);

    return this.httpClient.post(this.baseCourseUrl + "/photo", formData, {
      params: new HttpParams(),
      reportProgress: true,  // Enable progress reporting
      observe: 'events',      // Observe multiple events including upload progress
      headers: new HttpHeaders({
        'enctype': 'multipart/form-data'
      }),
    });
  }

  updateVideo(fileCreateRequest: PrivateFileBlobCreateRequest): Observable<Object> {
    const formData = objectToFormData(fileCreateRequest);

    return this.httpClient.post(this.baseCourseUrl + "/video", formData, {
      params: new HttpParams(),
      reportProgress: true,  // Enable progress reporting
      observe: 'events',      // Observe multiple events including upload progress
      headers: new HttpHeaders({
        'enctype': 'multipart/form-data'
      }),
    });
  }


  publishCourse(id: number, publish: PublishCourseRequest): Observable<ApiResponseWithData<CourseResponse>> {
    return this.httpClient.post<ApiResponseWithData<CourseResponse>>(this.baseCourseUrl + "/" + id, publish);
  }

  updateCourse(courseUpdateRequest: CourseUpdateRequest): Observable<ApiResponseWithData<CourseResponse>> {
    return this.httpClient.put<ApiResponseWithData<CourseResponse>>(this.baseCourseUrl, courseUpdateRequest);
  }

  deleteCourse(id: number): Observable<ApiResponse> {
    return this.httpClient.delete<ApiResponse>(this.baseCourseUrl + "/" + id);
  }

  private getHttpParamsFromCourseParams(courseParams: CourseParams): HttpParams {
    let params = new HttpParams();

    if (courseParams.pagedListParams)
      params = this.pagedParamsService.includePaginationHeaders(courseParams.pagedListParams, params);

    if (courseParams.orderByParams)
      params = this.orderParamsService.includeOrderHeaders(courseParams.orderByParams, params);

    if (courseParams.search)
      params = params.append('search', courseParams.search);

    return params
  }

}
