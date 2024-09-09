import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { CourseTitleResponse } from 'src/app/Models/Course/CourseTitleResponse';

@Component({
  selector: 'app-course-cart',
  templateUrl: './course-cart.component.html',
  styleUrls: ['./course-cart.component.scss']
})
export class CourseCartComponent {
  @Input() courseTitleResponse: CourseTitleResponse;
  isHovered = false;

  constructor(private router: Router) { }

  // Method to handle mouse enter (hover)
  onMouseEnter() {
    this.isHovered = true;
  }

  // Method to handle mouse leave
  onMouseLeave() {
    this.isHovered = false;
  }

  navigateToCourse() {
    this.router.navigate([`/course/managing/${this.courseTitleResponse.id}`]);
  }
}
