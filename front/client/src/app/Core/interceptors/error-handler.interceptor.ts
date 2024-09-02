import { Injectable } from '@angular/core';
import {
  HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse,
  HttpResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { NavigationExtras, Router } from '@angular/router';
import { ApiResponseWithData } from 'src/app/Models/ApiResponse';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable()
export class ErrorHandlerInterceptor implements HttpInterceptor {

  constructor(private toastr: ToastrService, private router: Router, private readonly spinner: NgxSpinnerService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    return next.handle(request).pipe(
      tap((event: HttpEvent<any>) => {
        if (event instanceof HttpResponse) {
          const apiResponse: ApiResponseWithData<any> = event.body;
          if (apiResponse && !apiResponse.isSuccess) {
            if (apiResponse.errorMessage)
              this.toastr.error(apiResponse.errorMessage);

            this.spinner.hide();
          }
        }
      }),
      catchError(error => {
        this.toastr.error("Something went wrong");
        this.spinner.hide();
        throw error;
      })
    )
  }
}
