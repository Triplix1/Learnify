import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-create-quiz-option',
  templateUrl: './create-quiz-option.component.html',
  styleUrls: ['./create-quiz-option.component.scss']
})
export class CreateQuizOptionComponent implements OnInit {
  @Input({ required: true }) answer: string;
  @Input({ required: true }) isCorrect: boolean;
  @Output() correctAnswerChange = new EventEmitter(); // Emit correct index changes
  @Output() answerChange = new EventEmitter<string>(); // Emit updated answers
  @Output() answerDeleted = new EventEmitter(); // Emit updated answers

  answerForm: FormGroup = new FormGroup({});

  constructor(private readonly fb: FormBuilder) { }

  ngOnInit(): void {
    this.answerForm = this.fb.group({
      answer: [this.answer ?? '', [Validators.required]],
    });
  }

  editAnswer() {
    if (this.answerForm.valid)
      this.answerChange.emit(this.answerForm.value); // Notify parent of changes
  }

  deleteAnswer() {
    this.answerDeleted.emit();
  }

  setCorrectAnswer() {
    this.correctAnswerChange.emit(); // Notify parent
  }
}
