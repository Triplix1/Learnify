import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CurrentLessonUpdatedResponse } from 'src/app/Models/Course/Lesson/CurrentLessonUpdatedResponse';
import { QuizQuestionUpdateResponse } from 'src/app/Models/Course/Lesson/QuizQuestion/QuizQuestionUpdateResponse';
import { QuizDeleted } from 'src/app/Models/QuizDeleted';

@Component({
  selector: 'app-create-quiz',
  templateUrl: './create-quiz.component.html',
  styleUrls: ['./create-quiz.component.scss']
})
export class CreateQuizComponent implements OnInit {
  @Input({ required: true }) lessonId: string;
  @Input({ required: true }) quizzes: QuizQuestionUpdateResponse[] = [];
  @Output() currentLessonUpdated: EventEmitter<CurrentLessonUpdatedResponse> = new EventEmitter<CurrentLessonUpdatedResponse>();

  ngOnInit(): void {
    if (!this.quizzes) {
      this.quizzes = [];
    }
  }

  addQuiz(): void {
    this.quizzes.push({ answers: { correctAnswer: 0, options: [""] }, question: "New question", id: null });
  }

  quizDeleted(quizDeletedData: QuizDeleted) {
    this.quizzes = this.quizzes.filter((_, i) => i !== quizDeletedData.index);
    this.currentLessonUpdated.emit(quizDeletedData.currentLessonUpdated);
  }

  lessonUpdated(currentLessonUpdatedResponse: CurrentLessonUpdatedResponse) {
    this.currentLessonUpdated.emit(currentLessonUpdatedResponse);
  }
}
