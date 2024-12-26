import { Component, Input } from '@angular/core';
import { QuizQuestionResponse } from 'src/app/Models/Course/Lesson/QuizQuestion/QuizQuestionResponse';

@Component({
  selector: 'app-create-single-quiz',
  templateUrl: './create-single-quiz.component.html',
  styleUrls: ['./create-single-quiz.component.scss']
})
export class CreateSingleQuizComponent {
  @Input({ required: true }) lessonId: string;
  @Input({ required: true }) quiz: QuizQuestionResponse;


}
