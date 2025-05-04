import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { take } from 'rxjs';
import { AnswersService } from 'src/app/Core/services/answers.service';
import { CurrentLessonUpdatedResponse } from 'src/app/Models/Course/Lesson/CurrentLessonUpdatedResponse';
import { AnswerAddOrUpdateRequest } from 'src/app/Models/Course/Lesson/QuizQuestion/Anwers/AnswerAddOrUpdateRequest';
import { AnswersUpdateResponse } from 'src/app/Models/Course/Lesson/QuizQuestion/Anwers/AnswersUpdateResponse';

@Component({
  selector: 'app-create-quiz-answers',
  templateUrl: './create-quiz-answers.component.html',
  styleUrls: ['./create-quiz-answers.component.scss']
})
export class CreateQuizAnswersComponent implements OnChanges {
  @Input({ required: true }) lessonId: string;
  @Input({ required: true }) quizId: string;
  @Input({ required: true }) answersResponse: AnswersUpdateResponse;
  @Output() currentLessonUpdated: EventEmitter<CurrentLessonUpdatedResponse> = new EventEmitter<CurrentLessonUpdatedResponse>();

  constructor(private readonly answersService: AnswersService, private readonly spinnerService: NgxSpinnerService) { }

  ngOnChanges(changes: SimpleChanges): void {
    const answersResponse = changes['answersResponse'].currentValue;
    if (!answersResponse) {
      this.answersResponse = { correctAnswer: 0, options: [''] }
    }
  }

  addAnswer() {
    this.answersResponse.options.push('New answer');
  }

  updateAnser(answer: string, index: number) {
    this.answersResponse.options[index] = answer;
    this.save();
  }

  setCorrectAnswer(index: number) {
    this.answersResponse.correctAnswer = index;
    this.save();
  }

  deleteAnswer(index: number) {
    this.answersResponse.options = this.answersResponse.options.filter(x => x !== this.answersResponse.options[index]);

    if (this.answersResponse.correctAnswer === index) {
      this.answersResponse.correctAnswer = this.answersResponse.options.length - 1;
    }
    this.save();
  }

  save() {
    const answersUpdateRequest: AnswerAddOrUpdateRequest = {
      correctAnswer: this.answersResponse.correctAnswer,
      options: this.answersResponse.options,
      lessonId: this.lessonId,
      quizId: this.quizId
    }

    this.spinnerService.show(`loadingAnswers${this.quizId}`);

    this.answersService.updateAnwers(answersUpdateRequest).pipe(take(1)).subscribe(response => {
      this.answersResponse = response.data;
      this.currentLessonUpdated.emit(response.data.currentLessonUpdated);
      this.spinnerService.hide(`loadingAnswers${this.quizId}`);
    });
  }

  isCurrentAnswerCorrect(index: number) {
    return this.answersResponse.correctAnswer === index;
  }
}
