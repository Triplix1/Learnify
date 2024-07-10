import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, catchError, switchMap, throwError } from 'rxjs';
import { ApiResponse } from 'src/app/Models/ApiResponse';
import { AuthResponse } from 'src/app/Models/AuthReponse';
import { AuthService } from '../services/auth.service';

@Injectable()
export class RefreshInterceptor implements HttpInterceptor {

  constructor(private authService: AuthService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const accessToken = this.authService.getAccessToken();

    const clonedReq = req.clone({
      headers: req.headers.set('Authorization', `Bearer ${accessToken}`)
    });

    return next.handle(clonedReq).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          return this.authService.refreshToken().pipe(
            switchMap((newToken: ApiResponse<AuthResponse>) => {
              const retryReq = req.clone({
                headers: req.headers.set('Authorization', `Bearer ${newToken.data.token}`)
              });
              return next.handle(retryReq);
            }),
            catchError((err) => {
              this.authService.logout();
              return throwError(err);
            })
          );
        } else {
          return throwError(error);
        }
      })
    );
  }
}
