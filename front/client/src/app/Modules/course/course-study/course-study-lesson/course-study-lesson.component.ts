import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-course-study-lesson',
  templateUrl: './course-study-lesson.component.html',
  styleUrls: ['./course-study-lesson.component.scss']
})
export class CourseStudyLessonComponent {
  @Input({ required: true }) lessonId: string;
}
