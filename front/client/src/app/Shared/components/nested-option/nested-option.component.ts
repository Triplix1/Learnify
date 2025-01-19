import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-nested-option',
  templateUrl: './nested-option.component.html',
  styleUrls: ['./nested-option.component.scss']
})
export class NestedOptionComponent {
  @Input() classList: string;
}
