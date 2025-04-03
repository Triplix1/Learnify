import { Component, EventEmitter, Input, OnInit, Output, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl } from '@angular/forms';
import { debounceTime, forkJoin, of, switchMap } from 'rxjs';

@Component({
  selector: 'app-input',
  templateUrl: './input.component.html',
  styleUrls: ['./input.component.scss']
})
export class InputComponent implements ControlValueAccessor, OnInit {
  @Input({ required: true }) label = '';
  @Input() hiddenLabel = false;
  @Input() hiddenError: boolean = false;
  @Input() type: 'text' | 'IBAN' | 'password' = 'text';
  @Input() classList = '';
  @Input() placeholder = '';
  @Output() changedInput: EventEmitter<any> = new EventEmitter<any>();
  oldValue: any;

  constructor(@Self() public ngControl: NgControl) {
    this.ngControl.valueAccessor = this;
  }

  ngOnInit(): void {
    this.oldValue = this.ngControl.value;
    this.ngControl.valueChanges.pipe(debounceTime(500)).subscribe(value => {
      if (this.ngControl.valid && this.oldValue != value)
        this.changedInput.emit(value);
      this.oldValue = value;
    })
  }

  writeValue(obj: any): void { }

  registerOnChange(fn: any): void { }

  registerOnTouched(fn: any): void { }

  get control(): FormControl {
    return this.ngControl.control as FormControl;
  }

  changed() {
    if (this.ngControl.valid && this.ngControl.dirty)
      of(this.ngControl.value).pipe(debounceTime(500)).subscribe(value => this.changedInput.emit(value));
  }
}