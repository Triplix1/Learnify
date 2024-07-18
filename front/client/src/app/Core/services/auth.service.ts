import { Injectable, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of, tap } from 'rxjs';

import { GoogleAuthRequest } from 'src/app/Models/GoogleAuthRequest';
import { ApiResponse } from 'src/app/Models/ApiResponse';
import { AuthResponse } from 'src/app/Models/AuthReponse';
import { environment } from 'src/environments/environment';
import { GoogleCodeRequest } from 'src/app/Models/GoogleCodeRequest';
import { ReqisterRequest } from 'src/app/Models/RegisterRequest';
import { LoginRequest } from 'src/app/Models/LoginRequest';
import { UserFromToken } from 'src/app/Models/UserFromToken';
import { Role } from 'src/app/Models/Role';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly BaseApiUrl: string = environment.baseApiUrl;
  private readonly GoogleAuthUrl: string = environment.googleAuth;
  private refreshTokenInProgress = false;
  private tokenData = new BehaviorSubject<AuthResponse>(null);
  private userData = new BehaviorSubject<UserFromToken>(null);
  tokenData$ = this.tokenData.asObservable();
  userData$ = this.userData.asObservable();

  constructor(private readonly httpClient: HttpClient) { }

  public loginGoogleCodeRequest(googleCodeRequest: GoogleCodeRequest) {
    // const params = new HttpParams();

    // params.append("client_id", googleCodeRequest.clientId);
    // params.append("redirect_uri", googleCodeRequest.redirectUrl);
    // params.append("scope", googleCodeRequest.scope);
    // params.append("code_challange", googleCodeRequest.codeChallange);
    // params.append("code_challange_method", googleCodeRequest.codeChalangeMethod);
    // params.append("state", googleCodeRequest.state);
    // params.append("response_type", "code");

    return this.httpClient.post<ApiResponse<AuthResponse>>(this.GoogleAuthUrl, googleCodeRequest);
  }

  public loginGoogleExchangeCode(googleAuthRequest: GoogleAuthRequest): Observable<ApiResponse<AuthResponse>> {
    return this.httpClient.post<ApiResponse<AuthResponse>>(this.BaseApiUrl + "/identity/external/google", googleAuthRequest).pipe(
      tap((response: ApiResponse<AuthResponse>) => {
        this.handleTokenUpdate(response.data);
      })
    );
  }

  public register(registerRequest: ReqisterRequest): Observable<ApiResponse<AuthResponse>> {
    return this.httpClient.post<ApiResponse<AuthResponse>>(this.BaseApiUrl + "/identity/register", registerRequest).pipe(
      tap((response: ApiResponse<AuthResponse>) => {
        this.handleTokenUpdate(response.data);
      })
    );
  }

  public login(loginRequest: LoginRequest): Observable<ApiResponse<AuthResponse>> {
    return this.httpClient.post<ApiResponse<AuthResponse>>(this.BaseApiUrl + "/identity/login", loginRequest).pipe(
      tap((response: ApiResponse<AuthResponse>) => {
        this.handleTokenUpdate(response.data);
      })
    );
  }

  getAccessToken(): string | null {
    return this.tokenData.value?.token;
  }

  getRefreshToken(): string | null {
    return this.tokenData.value?.refreshToken;
  }

  refreshToken(): Observable<ApiResponse<AuthResponse>> {
    if (!this.refreshTokenInProgress) {
      this.refreshTokenInProgress = true;
      localStorage.removeItem('refreshToken');

      return this.httpClient.post<ApiResponse<AuthResponse>>(this.BaseApiUrl + '/refresh', {
        jwt: this.getAccessToken(),
        refreshToken: this.getRefreshToken()
      }).pipe(
        tap((response: ApiResponse<AuthResponse>) => {
          this.refreshTokenInProgress = false;
          this.handleTokenUpdate(response.data);
        })
      );
    } else {
      return of({ data: { token: this.getAccessToken(), refreshToken: this.getRefreshToken() }, error: {} } as ApiResponse<AuthResponse>);
    }
  }

  logout(): void {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    this.tokenData.next(null);
    this.userData.next(null);
  }

  private handleTokenUpdate(authResponse: AuthResponse): void {
    localStorage.setItem('accessToken', authResponse.token);
    localStorage.setItem('refreshToken', authResponse.refreshToken);
    this.tokenData.next(authResponse);
    this.userData.next(this.getUserTokenDataFromToken(authResponse.token));
  }

  private getUserTokenDataFromToken(accessToken: string): UserFromToken {
    const decodedToken = this.getDecodedToken(accessToken);

    return {
      email: decodedToken.email,
      username: decodedToken.username,
      imageUrl: decodedToken.imageUrl,
      role: decodedToken["role"] as Role
    };
  }

  private getDecodedToken(token: string) {
    return JSON.parse(atob(token.split('.')[1]));
  }

  updateTokenData() {
    const access = localStorage.getItem('accessToken');
    const refresh = localStorage.getItem('refreshToken');
    console.log("Hello from auth");

    if (access != null || refresh != null) {
      this.tokenData.next({ refreshToken: refresh, token: access });
    }
  }
}