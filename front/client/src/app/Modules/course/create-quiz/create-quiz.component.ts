import { Component, Input } from '@angular/core';
import { QuizQuestionResponse } from 'src/app/Models/Course/Lesson/QuizQuestion/QuizQuestionResponse';
import { QuizQuestionUpdateResponse } from 'src/app/Models/Course/Lesson/QuizQuestion/QuizQuestionUpdateResponse';

@Component({
  selector: 'app-create-quiz',
  templateUrl: './create-quiz.component.html',
  styleUrls: ['./create-quiz.component.scss']
})
export class CreateQuizComponent {
  @Input({ required: true }) quizzes: QuizQuestionUpdateResponse[] = [];


}
