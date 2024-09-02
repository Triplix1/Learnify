import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-button',
  templateUrl: './button.component.html',
  styleUrls: ['./button.component.scss']
})
export class ButtonComponent {
  // Input for button type: 'accept', 'cancel', or custom class
  @Input() type: 'accept' | 'cancel' = 'accept';

  @Input() disabled: boolean = false;

  // Input for custom class list
  @Input() classList: string = '';

  // Output event emitter that will emit on click
  @Output() buttonClick = new EventEmitter<void>();

  // Function to emit the event when the button is clicked
  onClick() {
    // Only emit event if button is not disabled
    if (!this.disabled) {
      this.buttonClick.emit();
    }
  }

  // Method to return the appropriate Tailwind classes based on the type
  getButtonClasses(): string {
    let baseClass = "button-paddings text-white ";

    baseClass += this.type === 'accept'
      ? 'bg-buttonAccept hover:bg-buttonAcceptHover'
      : this.type === 'cancel'
        ? 'bg-buttonCancel hover:bg-buttonCancelHover'
        : '';

    const disabledClass = this.disabled ? 'opacity-50 cursor-not-allowed' : '';

    return `${baseClass} ${this.classList} ${disabledClass}`;

  }
}
