import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { take, takeUntil } from 'rxjs';
import { LessonService } from 'src/app/Core/services/lesson.service';
import { BaseComponent } from 'src/app/Models/BaseComponent';
import { LessonResponse } from 'src/app/Models/Course/Lesson/LessonResponse';

@Component({
  selector: 'app-course-study-lesson',
  templateUrl: './course-study-lesson.component.html',
  styleUrls: ['./course-study-lesson.component.scss']
})
export class CourseStudyLessonComponent extends BaseComponent implements OnChanges {
  @Input({ required: true }) lessonId: string;
  lesson: LessonResponse;

  constructor(private readonly lessonService: LessonService, private readonly spinnerService: NgxSpinnerService) {
    super();
  }

  ngOnChanges(changes: SimpleChanges): void {
    const currentLessonId = changes['lessonId'];
    if (currentLessonId.currentValue) {
      this.spinnerService.show("lessonLoading");
      this.lessonService.getLessonById(currentLessonId.currentValue).pipe(takeUntil(this.destroySubject), take(1)).subscribe(
        response => {
          this.spinnerService.hide("lessonLoading");
          this.lesson = response.data;
        }
      )
    }
  }
}
