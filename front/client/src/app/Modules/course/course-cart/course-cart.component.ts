import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Router } from '@angular/router';
import { CourseTitleResponse } from 'src/app/Models/Course/CourseTitleResponse';

@Component({
  selector: 'app-course-cart',
  templateUrl: './course-cart.component.html',
  styleUrls: ['./course-cart.component.scss']
})
export class CourseCartComponent {
  @Input({ required: true }) courseTitleResponse: CourseTitleResponse;
  @Output() onClick: EventEmitter<null> = new EventEmitter();
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
    this.onClick.emit();
  }
}
