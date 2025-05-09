import { CdkAccordionItem } from '@angular/cdk/accordion';
import { AfterContentChecked, AfterContentInit, AfterViewChecked, AfterViewInit, Component, EventEmitter, Input, OnChanges, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { debounceTime, take } from 'rxjs';
import { QuizService } from 'src/app/Core/services/quiz.service';
import { CurrentLessonUpdatedResponse } from 'src/app/Models/Course/Lesson/CurrentLessonUpdatedResponse';
import { QuizQuestionAddOrUpdateRequest } from 'src/app/Models/Course/Lesson/QuizQuestion/QuizQuestionAddOrUpdateRequest';
import { QuizQuestionUpdatedResponse } from 'src/app/Models/Course/Lesson/QuizQuestion/QuizQuestionUpdatedResponse';
import { QuizQuestionUpdateResponse } from 'src/app/Models/Course/Lesson/QuizQuestion/QuizQuestionUpdateResponse';
import { QuizDeleted } from 'src/app/Models/QuizDeleted';

@Component({
  selector: 'app-create-single-quiz',
  templateUrl: './create-single-quiz.component.html',
  styleUrls: ['./create-single-quiz.component.scss']
})
export class CreateSingleQuizComponent implements OnInit {
  @Input({ required: true }) lessonId: string;
  @Input({ required: true }) quiz: QuizQuestionUpdateResponse;
  @Input({ required: true }) index: number;
  @Output() deleted: EventEmitter<QuizDeleted> = new EventEmitter<QuizDeleted>();
  @Output() lessonUpdated: EventEmitter<CurrentLessonUpdatedResponse> = new EventEmitter<CurrentLessonUpdatedResponse>();

  quizUpdateRequest: QuizQuestionAddOrUpdateRequest;
  initialState: QuizQuestionAddOrUpdateRequest;
  quizForm: FormGroup = new FormGroup({});
  editingMode: boolean = false;
  expanded: boolean = false;

  constructor(private readonly fb: FormBuilder, private readonly quizService: QuizService) { }

  ngOnInit(): void {
    this.handleUpdate(this.quiz);

    if (this.quiz.id === null) {
      this.save(this.quizUpdateRequest);
    }

    this.quizForm.valueChanges.pipe(debounceTime(500)).subscribe(value => {
      if (this.quizForm.valid && this.quizUpdateRequest.question != value)
        this.saveForm();
    })

    this.initialState = { ...this.quizUpdateRequest };

    if (this.quiz.question === "Питання")
      this.editingMode = true;

    this.initializeForm(this.quiz);
    this.editingMode = false;
    this.expanded = false;
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
      quizId: this.quiz.id,
      question: quiz.question,
    }
  }

  saveForm(final: boolean = false) {
    this.quizUpdateRequest.question = this.quizForm.controls['question'].value;
    this.save(this.quizUpdateRequest);

    if (final) {
      this.initialState = { ...this.quiz, lessonId: this.lessonId };
      this.editingMode = false;
    }
  }

  save(quizQuestionAddOrUpdateRequest: QuizQuestionAddOrUpdateRequest) {
    this.quizService.addOrUpdate(quizQuestionAddOrUpdateRequest).pipe(take(1)).subscribe(response => {
      this.handleUpdate(response.data);
      this.handleLessonUpdate(response.data.currentLessonUpdated)
    });
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

  deleteToggle() {
    if (this.quiz.id) {
      this.quizService.delete(this.quiz.id, this.lessonId).pipe(take(1)).subscribe(r => {
        this.deleted.emit({ index: this.index, currentLessonUpdated: r.data });
      });
    }
  }

  handleLessonUpdate(lessonUpdated: CurrentLessonUpdatedResponse) {
    this.lessonUpdated.emit(lessonUpdated);
  }
}
