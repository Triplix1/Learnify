import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-payment-success',
  templateUrl: './payment-success.component.html',
  styleUrls: ['./payment-success.component.scss']
})
export class PaymentSuccessComponent {
  @Input({ required: true }) courseId: number;

  constructor(private readonly router: Router) { }

  navigateToCourse() {
    this.router.navigate([`/course/main-info/${this.courseId}`])
  }
}
