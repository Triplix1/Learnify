import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { QuizItem } from 'src/app/Models/Course/Lesson/QuizQuestion/Anwers/QuizItem';
import { UserLessonQuizAnswerResponse } from 'src/app/Models/Course/Lesson/QuizQuestion/Anwers/UserLessonQuizAnswerResponse';
import { QuizQuestionResponse } from 'src/app/Models/Course/Lesson/QuizQuestion/QuizQuestionResponse';

@Component({
  selector: 'app-single-quiz',
  templateUrl: './single-quiz.component.html',
  styleUrls: ['./single-quiz.component.scss']
})
export class SingleQuizComponent implements OnInit {
  ngOnInit(): void {
    console.log(this.quizItem);
  }
  @Input({ required: true }) quizItem: QuizQuestionResponse;
  @Output() quizItemChange = new EventEmitter<QuizQuestionResponse>();

  setCorrectAnswer(index: number) {
    this.quizItem.userAnswer = {
      answerIndex: index,
      isCorrect: null
    }
    this.quizItemChange.emit(this.quizItem);
  }
}
