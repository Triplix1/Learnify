import { CdkAccordionItem } from '@angular/cdk/accordion';
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { debounceTime } from 'rxjs';
import { QuizService } from 'src/app/Core/services/quiz.service';
import { QuizQuestionAddOrUpdateRequest } from 'src/app/Models/Course/Lesson/QuizQuestion/QuizQuestionAddOrUpdateRequest';
import { QuizQuestionUpdateResponse } from 'src/app/Models/Course/Lesson/QuizQuestion/QuizQuestionUpdateResponse';

@Component({
  selector: 'app-create-single-quiz',
  templateUrl: './create-single-quiz.component.html',
  styleUrls: ['./create-single-quiz.component.scss']
})
export class CreateSingleQuizComponent implements OnInit {
  @Input({ required: true }) lessonId: string;
  @Input({ required: true }) quiz: QuizQuestionUpdateResponse;
  @Input({ required: true }) index: number;

  quizUpdateRequest: QuizQuestionAddOrUpdateRequest;
  initialState: QuizQuestionAddOrUpdateRequest;
  quizForm: FormGroup = new FormGroup({});
  editingMode: boolean = false;
  expanded: boolean = false;

  constructor(private readonly fb: FormBuilder, private readonly quizService: QuizService) { }

  ngOnInit(): void {
    this.handleUpdate(this.quiz);

    this.quizForm.valueChanges.pipe(debounceTime(500)).subscribe(value => {
      if (this.quizForm.valid && this.quizUpdateRequest.question != value)
        this.saveForm();
    })

    this.initialState = { ...this.quizUpdateRequest };

    if (!this.quiz.question)
      this.editingMode = true;
  }

  initializeForm(quiz: QuizQuestionUpdateResponse) {
    this.quizForm = this.fb.group({
      question: [quiz?.question ?? '', [Validators.required]],
    });
  }

  handleUpdate(quiz: QuizQuestionUpdateResponse) {
    this.quiz = quiz;
    this.initializeForm(quiz);
    this.quizUpdateRequest = {
      lessonId: this.lessonId,
      quizId: this.quiz.quizId,
      question: quiz.question,
      media: quiz.media,
    }
  }

  saveForm(final: boolean = false) {
    this.quizUpdateRequest.question = this.quizForm.get('question').value;
    this.save(this.quizUpdateRequest);

    if (final) {
      this.initialState = { ...this.quiz, lessonId: this.lessonId };
      this.editingMode = false;
    }
  }

  save(quizQuestionAddOrUpdateRequest: QuizQuestionAddOrUpdateRequest) {
    this.quizService.addOrUpdate(quizQuestionAddOrUpdateRequest).subscribe(response => this.handleUpdate(response.data));
  }

  cancel() {
    this.save(this.initialState);
    this.editingMode = false;
    this.initializeForm(this.quiz);
  }

  changeExpanded(opened: boolean) {
    this.expanded = opened;
  }

  editingToggle() {
    this.editingMode = true;
  }

}
