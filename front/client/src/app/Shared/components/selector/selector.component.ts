import { Component, Input } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-selector',
  templateUrl: './selector.component.html',
  styleUrls: ['./selector.component.scss']
})
export class SelectorComponent {
  @Input({ required: true }) options: { value: any, label: string }[] = [];
  @Input() placeholder: string = 'Select an option';
  @Input({ required: true }) formControl: FormControl = new FormControl();
}
