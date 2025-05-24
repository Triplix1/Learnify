import { Component, Input, OnInit } from '@angular/core';
import { take, takeUntil } from 'rxjs';
import { QuizService } from 'src/app/Core/services/quiz.service';
import { BaseComponent } from 'src/app/Models/BaseComponent';
import { AnswersValidateRequest } from 'src/app/Models/Course/Lesson/QuizQuestion/Anwers/AnswersValidateRequest';
import { QuizItem } from 'src/app/Models/Course/Lesson/QuizQuestion/Anwers/QuizItem';
import { QuizValidateRequest } from 'src/app/Models/Course/Lesson/QuizQuestion/Anwers/QuizValidateRequest';
import { UserLessonQuizAnswerResponse } from 'src/app/Models/Course/Lesson/QuizQuestion/Anwers/UserLessonQuizAnswerResponse';
import { UserQuizAnswerResponse } from 'src/app/Models/Course/Lesson/QuizQuestion/Anwers/UserQuizAnswerResponse';
import { QuizQuestionResponse } from 'src/app/Models/Course/Lesson/QuizQuestion/QuizQuestionResponse';

@Component({
  selector: 'app-quizzes',
  templateUrl: './quizzes.component.html',
  styleUrls: ['./quizzes.component.scss']
})
export class QuizzesComponent extends BaseComponent {
  @Input({ required: true }) lessonId: string;
  @Input({ required: true }) quizzes: QuizQuestionResponse[];

  constructor(private readonly quizService: QuizService) {
    super();
  }

  get allAnswersSpecified() {
    return this.quizzes.every(l => l.userAnswer?.answerIndex !== null && l.userAnswer?.answerIndex !== undefined);
  }

  get allAnswersAreCorrect() {
    return this.quizzes.every(l => l.userAnswer?.isCorrect === true);
  }

  checkAnswers() {
    if (this.allAnswersSpecified) {
      const quizCheckRequests = this.quizzes.map<QuizValidateRequest>(x => {
        return { id: x.id, answer: x.userAnswer?.answerIndex }
      });

      const answerValidateRequest: AnswersValidateRequest = {
        lessonId: this.lessonId,
        quizValidateRequests: quizCheckRequests
      };

      this.quizService.check(answerValidateRequest).pipe(take(1), takeUntil(this.destroySubject)).subscribe(response => {
        this.handleUserQuizAnswerResponseUpdate(response.data);
      })
    }
  }

  private handleUserQuizAnswerResponseUpdate(userQuizAnswerResponses: UserQuizAnswerResponse[]) {
    for (let userQuizAnswer of userQuizAnswerResponses) {
      var currentAnswer = this.quizzes.find(x => x.id == userQuizAnswer.quizId);
      if (currentAnswer) {
        currentAnswer.userAnswer = {
          answerIndex: userQuizAnswer.answerIndex,
          isCorrect: userQuizAnswer.isCorrect
        };
      }
    }
  }
}
