import { Component, EventEmitter, Input, Output } from '@angular/core';
import { LessonResponse } from 'src/app/Models/Course/Lesson/LessonResponse';

@Component({
  selector: 'app-create-lesson',
  templateUrl: './create-lesson.component.html',
  styleUrls: ['./create-lesson.component.scss']
})
export class CreateLessonComponent {
  @Input() lessonId: string;
  @Input({ required: true }) paragraphId: number;
  @Output() onUpdate: EventEmitter<LessonResponse> = new EventEmitter<LessonResponse>(null);

  lessonResponse: LessonResponse;

}
