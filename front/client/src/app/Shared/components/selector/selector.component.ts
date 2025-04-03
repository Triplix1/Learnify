import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormControl } from '@angular/forms';
import { SelectorOptions } from 'src/app/Models/SelectorOptions';

@Component({
  selector: 'app-selector',
  templateUrl: './selector.component.html',
  styleUrls: ['./selector.component.scss']
})
export class SelectorComponent<T> {
  @Input({ required: true }) options: SelectorOptions<T>[] = [];
  @Input() placeholder: string = 'Select an option';
  @Input({ required: true }) formControl: FormControl = new FormControl();
  @Output() valueUpdated: EventEmitter<string> = new EventEmitter<string>();

  optionSelected(event: Event) {
    const selectElement = event.target as HTMLSelectElement;
    this.valueUpdated.emit(selectElement.value);
  }
}
