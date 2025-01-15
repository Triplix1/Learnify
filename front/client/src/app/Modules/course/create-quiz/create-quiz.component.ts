import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { QuizQuestionResponse } from 'src/app/Models/Course/Lesson/QuizQuestion/QuizQuestionResponse';
import { QuizQuestionUpdateResponse } from 'src/app/Models/Course/Lesson/QuizQuestion/QuizQuestionUpdateResponse';

@Component({
  selector: 'app-create-quiz',
  templateUrl: './create-quiz.component.html',
  styleUrls: ['./create-quiz.component.scss']
})
export class CreateQuizComponent implements OnInit {
  @Input({ required: true }) lessonId: string;
  @Input({ required: true }) quizzes: QuizQuestionUpdateResponse[] = [];


  ngOnInit(): void {
    // if (!this.quizzes)
  }

  addQuiz(): void {
    this.quizzes.push({ answers: { correctAnswer: 0, options: [""] }, media: null, question: "", quizId: null });
  }
}
