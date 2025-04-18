import { Component, Input, OnInit } from '@angular/core';
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
    if (!this.quizzes) {
      this.quizzes = [];
    }
  }

  addQuiz(): void {
    this.quizzes.push({ answers: { correctAnswer: 0, options: [""] }, media: null, question: "New question", id: null });
  }

  quizDeleted(index: number) {
    this.quizzes = this.quizzes.filter((_, i) => i !== index);
  }
}
