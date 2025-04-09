import { CdkAccordionItem } from '@angular/cdk/accordion';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-arrow',
  templateUrl: './arrow.component.html',
  styleUrls: ['./arrow.component.scss']
})
export class ArrowComponent {
  @Input({ required: true }) expanded: boolean;
}
