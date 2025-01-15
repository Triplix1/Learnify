import { Component, Input, OnInit } from '@angular/core';
import { AnswersService } from 'src/app/Core/services/answers.service';
import { AnswerAddOrUpdateRequest } from 'src/app/Models/Course/Lesson/QuizQuestion/Anwers/AnswerAddOrUpdateRequest';
import { AnswersUpdateResponse } from 'src/app/Models/Course/Lesson/QuizQuestion/Anwers/AnswersUpdateResponse';

@Component({
  selector: 'app-create-quiz-answers',
  templateUrl: './create-quiz-answers.component.html',
  styleUrls: ['./create-quiz-answers.component.scss']
})
export class CreateQuizAnswersComponent implements OnInit {
  @Input({ required: true }) lessonId: string;
  @Input({ required: true }) quizId: string;
  @Input({ required: true }) answersResponse: AnswersUpdateResponse;

  answersUpdateRequest: AnswerAddOrUpdateRequest;

  constructor(private readonly answersService: AnswersService) { }

  ngOnInit(): void {
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
  }

  setCorrectAnswer(index: number) {
    this.answersResponse.correctAnswer = index;
  }

  deleteAnswer(index: number) {
    this.answersResponse.options = this.answersResponse.options.filter(x => x !== this.answersResponse.options[index]);

    if (this.answersResponse.correctAnswer === index) {
      this.answersResponse.correctAnswer = this.answersResponse.options.length - 1;
    }
  }

  save() {
    this.answersService.updateAnwers(this.answersUpdateRequest).;
  }
}
