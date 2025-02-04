import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-payment-cancelled',
  templateUrl: './payment-cancelled.component.html',
  styleUrls: ['./payment-cancelled.component.scss']
})
export class PaymentCancelledComponent {
  @Input({ required: true }) courseId: number;

  constructor(private readonly router: Router) { }

  navigateToCourse() {
    this.router.navigate([`/course/main-info/${this.courseId}`])
  }
}
