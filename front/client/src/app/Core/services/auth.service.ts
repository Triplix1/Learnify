import { Injectable } from '@angular/core';
import { UserManager, UserManagerSettings, User } from 'oidc-client';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, catchError } from 'rxjs';

import { BaseService } from "./base.service";
import { ConfigService } from 'src/app/Core/services/config.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService extends BaseService {

  // Observable navItem source
  private _authNavStatusSource = new BehaviorSubject<boolean>(false);
  // Observable navItem stream
  authNavStatus$ = this._authNavStatusSource.asObservable();

  private manager = new UserManager(getClientSettings());
  private user: User | null = null;

  constructor(private http: HttpClient, private configService: ConfigService) {
    super();

    this.manager.getUser().then(user => {
      this.user = user;
      this._authNavStatusSource.next(this.isAuthenticated());
    });
  }

  login() {
    return this.manager.signinRedirect();
  }

  async completeAuthentication() {
    this.user = await this.manager.signinRedirectCallback();
    this._authNavStatusSource.next(this.isAuthenticated());
  }

  register(userRegistration: any) {
    return this.http.post(this.configService.authApiURI + '/account', userRegistration).pipe(catchError(this.handleError));
  }

  isAuthenticated(): boolean {
    return this.user != null && !this.user.expired;
  }

  get authorizationHeaderValue(): string {
    if (this.user)
      return `${this.user.token_type} ${this.user.access_token}`;
    return '';
  }

  get name() {
    return this.user != null ? this.user.profile.name : '';
  }

  public logout = () => {
    this.manager.signoutRedirect();
  }

  public finishLogout = () => {
    this.user = null;
    this._authNavStatusSource.next(false);
    return this.manager.signoutRedirectCallback();
  }
}

export function getClientSettings(): UserManagerSettings {
  return {
    authority: 'http://localhost:5000',
    client_id: 'angular_spa',
    redirect_uri: 'http://localhost:4200/auth-callback',
    response_type: "id_token token",
    scope: "openid profile email api.read",
    post_logout_redirect_uri: `http://localhost:4200/signout-callback`,
  };
}