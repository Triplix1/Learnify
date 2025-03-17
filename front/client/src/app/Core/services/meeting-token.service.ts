import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiResponseWithData } from 'src/app/Models/ApiResponse';
import { MeetingTokenResponse } from 'src/app/Models/Meeting/MeetingTokenResponse';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MeetingTokenService {
  meetingSessionUrl = environment.baseApiUrl + '/MeetingToken';

  constructor(private readonly httpClient: HttpClient) { }

  generateToken(sessionId: string): Observable<ApiResponseWithData<MeetingTokenResponse>> {
    return this.httpClient.post<ApiResponseWithData<MeetingTokenResponse>>(this.meetingSessionUrl + "/" + sessionId, {});
  }
}
