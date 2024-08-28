import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, catchError, map, of, switchMap, take, throwError } from 'rxjs';
import { ApiResponse } from 'src/app/Models/ApiResponse';
import { AuthResponse } from 'src/app/Models/Auth/AuthReponse';
import { AuthService } from '../services/auth.service';

@Injectable()
export class RefreshInterceptor implements HttpInterceptor {

  constructor(private authService: AuthService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    return this.authService.tokenData$.pipe(
      take(1),
      switchMap(tokenData => {
        if (tokenData === null || tokenData === undefined) {
          return of(null)
        }
        let current = new Date(Date.now());
        let isless = tokenData.expires <= new Date(Date.now())
        if (new Date(tokenData.expires) <= new Date(Date.now())) {
          return this.authService.refreshToken().pipe(map(result => result.data));
        }

        return of(tokenData);
      }),
      switchMap((tokenData => {

        let clonedReq = req;

        if (tokenData !== null && tokenData !== undefined) {
          clonedReq = req.clone({
            headers: req.headers.set('Authorization', `Bearer ${tokenData.token}`)
          });
        }

        return next.handle(clonedReq);
      }))
    );

    // return next.handle(clonedReq).pipe(
    //   catchError((error: HttpErrorResponse) => {
    //     if (error.status === 401 && accessToken !== null && accessToken !== undefined) {
    //       return this.authService.refreshToken().pipe(
    //         switchMap((newToken: ApiResponse<AuthResponse>) => {
    //           const retryReq = req.clone({
    //             headers: req.headers.set('Authorization', `Bearer ${newToken.data.token}`)
    //           });
    //           return next.handle(retryReq);
    //         }),
    //         catchError((err) => {
    //           this.authService.logout();
    //           return throwError(err);
    //         })
    //       );
    //     } else {
    //       return throwError(error);
    //     }
    //   })
    // );

  }
}
