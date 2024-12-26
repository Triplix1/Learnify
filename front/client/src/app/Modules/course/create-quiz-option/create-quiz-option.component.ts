import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-create-quiz-option',
  templateUrl: './create-quiz-option.component.html',
  styleUrls: ['./create-quiz-option.component.scss']
})
export class CreateQuizOptionComponent {
  @Input() answers: string[] = [];
  @Input() correctAnswerIndex: number = 0;
  @Output() correctAnswerIndexChange = new EventEmitter<number>(); // Emit correct index changes
  @Output() answersChange = new EventEmitter<string[]>(); // Emit updated answers

  editAnswer(index: number, updatedText: string) {
    this.answers[index] = updatedText;
    this.answersChange.emit(this.answers); // Notify parent of changes
  }

  deleteAnswer(index: number) {
    this.answers.splice(index, 1);
    this.answersChange.emit(this.answers); // Notify parent of changes
    if (this.correctAnswerIndex >= this.answers.length) {
      this.setCorrectAnswerIndex(0); // Reset correct index if out of bounds
    }
  }

  setCorrectAnswerIndex(index: number) {
    this.correctAnswerIndex = index;
    this.correctAnswerIndexChange.emit(this.correctAnswerIndex); // Notify parent
  }

}
