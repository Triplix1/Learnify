import { Component, Input, OnInit, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl } from '@angular/forms';

@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.scss']
})
export class TextInputComponent implements ControlValueAccessor {
  @Input({ required: true }) label = '';
  @Input() hiddenLabel = false;
  @Input() hiddenError: boolean = false;
  @Input() type = 'text';
  @Input() classList = '';
  @Input() placeholder = '';

  constructor(@Self() public ngControl: NgControl) {
    this.ngControl.valueAccessor = this;
  }

  writeValue(obj: any): void { }

  registerOnChange(fn: any): void { }

  registerOnTouched(fn: any): void { }

  get control(): FormControl {
    return this.ngControl.control as FormControl;
  }
}