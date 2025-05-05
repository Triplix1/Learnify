import { Component, EventEmitter, Input, Output } from '@angular/core';
import { take } from 'rxjs';
import { LessonService } from 'src/app/Core/services/lesson.service';
import { LessonDeletedEvent } from 'src/app/Models/Course/Lesson/LessonDeletedEvent';
import { LessonTitleResponse } from 'src/app/Models/Course/Lesson/LessonTitleResponse';

@Component({
  selector: 'app-lesson-title',
  templateUrl: './lesson-title.component.html',
  styleUrls: ['./lesson-title.component.scss']
})
export class LessonTitleComponent {
  @Input({ required: true }) lessonTitle: LessonTitleResponse;
  @Output() delete: EventEmitter<LessonDeletedEvent> = new EventEmitter<LessonDeletedEvent>();
  @Output() edit: EventEmitter<string> = new EventEmitter<string>();

  constructor(private readonly lessonService: LessonService) { }

  onDelete() {
    this.lessonService.deleteLesson(this.lessonTitle.id).pipe(take(1)).subscribe(response => {
      this.delete.emit({ lessonId: this.lessonTitle.id, lessonDeletedResponse: response.data });
    });
  }

  invokeEdit() {
    this.edit.emit(this.lessonTitle.id);
  }
}
