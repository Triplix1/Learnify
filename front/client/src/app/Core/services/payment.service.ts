import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { take } from 'rxjs';
import { ApiResponseWithData } from 'src/app/Models/ApiResponse';
import { PaymentCreateRequest } from 'src/app/Models/Payment/PaymentCreateRequest';
import { Session } from 'src/app/Models/Session/Session';
import { environment } from 'src/environments/environment';


declare const Stripe: any;

@Injectable({
  providedIn: 'root'
})

export class PaymentService {
  baseUrl = environment.baseApiUrl + "/checkout"

  constructor(private http: HttpClient) { }

  requestMemberSession(courseId: number): void {
    const paymentCreateRequest: PaymentCreateRequest = {
      courseId: courseId,
      cancelUrl: environment.paymentCancelUrl,
      successUrl: environment.paymentSuccessUrl,
    }

    this.http
      .post<ApiResponseWithData<Session>>(this.baseUrl, paymentCreateRequest).pipe(take(1))
      .subscribe((session) => {
        this.redirectToCheckout(session.data);
      });
  }

  private redirectToCheckout(session: Session) {
    const stripe = Stripe(session.publicKey);

    stripe.redirectToCheckout({
      sessionId: session.sessionId,
    });
  }

}
