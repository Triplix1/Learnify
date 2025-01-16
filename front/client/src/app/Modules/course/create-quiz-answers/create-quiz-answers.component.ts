import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { AnswersService } from 'src/app/Core/services/answers.service';
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

  answersUpdateRequest: AnswerAddOrUpdateRequest;

  constructor(private readonly answersService: AnswersService) { }

  ngOnChanges(changes: SimpleChanges): void {
    const answersResponse = changes['answersResponse'].currentValue;
    if (!answersResponse) {
      this.answersResponse = { correctAnswer: 0, options: [''] }
      this.save();
    }

    this.answersUpdateRequest = {
      correctAnswer: this.answersResponse.correctAnswer,
      options: this.answersResponse.options,
      lessonId: this.lessonId,
      quizId: this.quizId
    }
  }

  addAnswer() {
    this.answersUpdateRequest.options.push('New answer');
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
    this.answersService.updateAnwers(this.answersUpdateRequest).subscribe(response => this.answersResponse = response.data);
  }
}
