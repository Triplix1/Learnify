import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Language } from 'src/app/Models/enums/Language';
import { SelectorOption } from 'src/app/Models/SelectorOption';

@Component({
  selector: 'app-select-language',
  templateUrl: './select-language.component.html',
  styleUrls: ['./select-language.component.scss']
})
export class SelectLanguageComponent {
  @Input({ required: true }) formControl: FormControl<any>;
  @Output() changedInput: EventEmitter<void> = new EventEmitter<void>()

  updatedInput() {
    this.changedInput.emit();
  }

  selectorOptions: SelectorOption[] = Object.keys(Language)
    .filter(key => isNaN(Number(key)))  // Filter out numeric keys
    .map(key => {
      return { label: key, value: Language[key as keyof typeof Language] };
    });

}
