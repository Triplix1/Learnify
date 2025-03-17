import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponse, ApiResponseWithData } from 'src/app/Models/ApiResponse';
import { SessionIdResponse } from 'src/app/Models/Meeting/SessionIdResponse';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MeetingService {
  meetingSessionUrl = environment.baseApiUrl + '/MeetingSession';

  constructor(private readonly httpClient: HttpClient) { }

  createSession(courseId: number): Observable<ApiResponseWithData<SessionIdResponse>> {
    return this.httpClient.post<ApiResponseWithData<SessionIdResponse>>(this.meetingSessionUrl + "/" + courseId, {});
  }

  getSessionForCourse(courseId: number): Observable<ApiResponseWithData<SessionIdResponse>> {
    return this.httpClient.get<ApiResponseWithData<SessionIdResponse>>(this.meetingSessionUrl + "/" + courseId, {});
  }

  stopSession(session: string): Observable<ApiResponse> {
    return this.httpClient.post<ApiResponse>(this.meetingSessionUrl + "/stop/" + session, {});
  }
}
