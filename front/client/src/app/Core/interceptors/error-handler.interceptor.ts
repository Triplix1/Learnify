import { Injectable } from '@angular/core';
import {
  HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse,
  HttpResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { NavigationExtras, Router } from '@angular/router';
import { ApiResponse } from 'src/app/Models/ApiResponse';

@Injectable()
export class ErrorHandlerInterceptor implements HttpInterceptor {

  constructor(private toastr: ToastrService, private router: Router) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    return next.handle(request).pipe(
      tap((event: HttpEvent<any>) => {
        if (event instanceof HttpResponse) {
          const apiResponse: ApiResponse<any> = event.body;
          if (apiResponse && apiResponse.error) {
            this.toastr.error(apiResponse.error);
          }
        }
      }),
      catchError(error => {
        this.toastr.error("Something went wrong");
        throw error;
      })
    )
  }
}
