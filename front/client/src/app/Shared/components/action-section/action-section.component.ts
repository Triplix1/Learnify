import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-action-section',
  templateUrl: './action-section.component.html',
  styleUrls: ['./action-section.component.scss']
})
export class ActionSectionComponent {
  @Input() classList: string;
}
