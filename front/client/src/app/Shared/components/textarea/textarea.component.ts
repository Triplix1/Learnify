import { Component, EventEmitter, Input, OnInit, Output, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl } from '@angular/forms';
import { debounceTime, of } from 'rxjs';

@Component({
  selector: 'app-textarea',
  templateUrl: './textarea.component.html',
  styleUrls: ['./textarea.component.scss']
})
export class TextareaComponent implements ControlValueAccessor, OnInit {
  @Input({ required: true }) label = '';
  @Input() hiddenLabel = false;
  @Input() hiddenError: boolean = false;
  @Input() classList = '';
  @Input() placeholder = '';
  @Input() minRows: number = 4;
  @Input() maxRows: number = 1000000;
  @Input() cols: number = 20;
  @Output() changedTextarea: EventEmitter<any> = new EventEmitter<any>();
  oldValue: any;

  constructor(@Self() public ngControl: NgControl) {
    this.ngControl.valueAccessor = this;
  }

  ngOnInit(): void {
    this.oldValue = this.ngControl.value;
    this.ngControl.valueChanges.pipe(debounceTime(500)).subscribe(value => {
      if (this.ngControl.valid && this.oldValue !== value)
        this.changedTextarea.emit(value);
      this.oldValue = value;
    });
  }

  writeValue(obj: any): void { }

  registerOnChange(fn: any): void { }

  registerOnTouched(fn: any): void { }

  get control(): FormControl {
    return this.ngControl.control as FormControl;
  }

  changed() {
    if (this.ngControl.valid && this.ngControl.dirty)
      of(this.ngControl.value).pipe(debounceTime(500)).subscribe(value => this.changedTextarea.emit(value));
  }
}
