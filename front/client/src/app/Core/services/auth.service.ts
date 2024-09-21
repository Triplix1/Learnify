import { Injectable, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, catchError, Observable, of, tap } from 'rxjs';

import { ApiResponseWithData } from 'src/app/Models/ApiResponse';
import { AuthResponse } from 'src/app/Models/Auth/AuthReponse';
import { environment } from 'src/environments/environment';
import { GoogleCodeRequest } from 'src/app/Models/Auth/GoogleCodeRequest';
import { ReqisterRequest } from 'src/app/Models/Auth/RegisterRequest';
import { LoginRequest } from 'src/app/Models/Auth/LoginRequest';
import { UserFromToken } from 'src/app/Models/UserFromToken';
import { GoogleAuthRequest } from 'src/app/Models/Auth/GoogleAuthRequest';
import { Role } from 'src/app/Models/enums/Role';

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

  constructor(private readonly httpClient: HttpClient) {
    this.updateTokenData()
  }

  public loginGoogleCodeRequest(googleCodeRequest: GoogleCodeRequest) {
    // const params = new HttpParams();

    // params.append("client_id", googleCodeRequest.clientId);
    // params.append("redirect_uri", googleCodeRequest.redirectUrl);
    // params.append("scope", googleCodeRequest.scope);
    // params.append("code_challange", googleCodeRequest.codeChallange);
    // params.append("code_challange_method", googleCodeRequest.codeChalangeMethod);
    // params.append("state", googleCodeRequest.state);
    // params.append("response_type", "code");

    return this.httpClient.post<ApiResponseWithData<AuthResponse>>(this.GoogleAuthUrl, googleCodeRequest);
  }

  public loginGoogleExchangeCode(googleAuthRequest: GoogleAuthRequest): Observable<ApiResponseWithData<AuthResponse>> {
    return this.httpClient.post<ApiResponseWithData<AuthResponse>>(this.BaseApiUrl + "/auth/external/google", googleAuthRequest).pipe(
      tap((response: ApiResponseWithData<AuthResponse>) => {
        this.handleTokenUpdate(response.data);
      })
    );
  }

  public register(registerRequest: ReqisterRequest): Observable<ApiResponseWithData<AuthResponse>> {
    return this.httpClient.post<ApiResponseWithData<AuthResponse>>(this.BaseApiUrl + "/auth/register", registerRequest).pipe(
      tap((response: ApiResponseWithData<AuthResponse>) => {
        this.handleTokenUpdate(response.data);
      })
    );
  }

  public login(loginRequest: LoginRequest): Observable<ApiResponseWithData<AuthResponse>> {
    return this.httpClient.post<ApiResponseWithData<AuthResponse>>(this.BaseApiUrl + "/auth/login", loginRequest).pipe(
      tap((response: ApiResponseWithData<AuthResponse>) => {
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

  getExpiration(): Date | null {
    return this.tokenData.value?.expires;
  }

  refreshToken(): Observable<ApiResponseWithData<AuthResponse>> {
    if (!this.refreshTokenInProgress) {
      this.refreshTokenInProgress = true;

      return this.httpClient.post<ApiResponseWithData<AuthResponse>>(this.BaseApiUrl + '/auth/refresh', {
        jwt: this.getAccessToken(),
        refreshToken: this.getRefreshToken()
      }).pipe(
        catchError(err => {
          this.refreshTokenInProgress = false;
          this.tokenData.next(null);
          this.userData.next(null);
          return of(null);
        }),
        tap((response: ApiResponseWithData<AuthResponse>) => {
          this.refreshTokenInProgress = false;
          if (response !== null) {
            this.handleTokenUpdate(response.data);
          }
        })
      );
    } else {
      return of({ data: { token: this.getAccessToken(), refreshToken: this.getRefreshToken(), expires: this.getExpiration() }, isSuccess: true } as ApiResponseWithData<AuthResponse>);
    }
  }

  logout(): void {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('expires');
    this.tokenData.next(null);
    this.userData.next(null);
  }

  private handleTokenUpdate(authResponse: AuthResponse): void {
    if (authResponse === null || authResponse === undefined) {
      this.logout();
      console.log('log out');
      return;
    }

    localStorage.setItem('accessToken', authResponse.token);
    localStorage.setItem('refreshToken', authResponse.refreshToken);
    localStorage.setItem('expires', authResponse.expires + '');
    this.tokenData.next(authResponse);
    this.userData.next(this.getUserTokenDataFromToken(authResponse.token));
  }

  private getUserTokenDataFromToken(accessToken: string): UserFromToken {
    const decodedToken = this.getDecodedToken(accessToken);

    return {
      id: decodedToken.id,
      email: decodedToken.email,
      username: decodedToken.username,
      name: decodedToken.name,
      surname: decodedToken.surname,
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
    const expires = new Date(localStorage.getItem('expires'));
    console.log("Hello from auth");

    if (access != null || refresh != null) {
      this.tokenData.next({ refreshToken: refresh, token: access, expires: expires });
      this.userData.next(this.getUserTokenDataFromToken(access));
    }
  }
}