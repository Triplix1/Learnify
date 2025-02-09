import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { take, takeUntil } from 'rxjs';
import { CourseService } from 'src/app/Core/services/course.service';
import { PaymentService } from 'src/app/Core/services/payment.service';
import { BaseComponent } from 'src/app/Models/BaseComponent';
import { CourseMainInfo } from 'src/app/Models/Course/CourseMainInfo';
import { PaymentCreateRequest } from 'src/app/Models/Payment/PaymentCreateRequest';

@Component({
  selector: 'app-main-course-page',
  templateUrl: './main-course-page.component.html',
  styleUrls: ['./main-course-page.component.scss']
})
export class MainCoursePageComponent extends BaseComponent implements OnInit {
  @Input({ required: true }) courseId: number;

  courseMainInfo: CourseMainInfo;

  constructor(private readonly courseService: CourseService, private readonly paymentService: PaymentService, private readonly router: Router) {
    super();
  }

  ngOnInit(): void {
    this.courseService.getMainInfo(this.courseId).pipe(take(1), takeUntil(this.destroySubject)).subscribe(response => {
      this.courseMainInfo = response.data;
    })
  }

  completePayment() {
    this.paymentService.requestMemberSession(this.courseId);
  }

  goToTheCourse() {
    this.router.navigate([`/course/study/${this.courseId}`]);
  }
}