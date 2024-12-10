import { CdkAccordionItem } from '@angular/cdk/accordion';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-accordion-arrow',
  templateUrl: './accordion-arrow.component.html',
  styleUrls: ['./accordion-arrow.component.scss']
})
export class AccordionArrowComponent {
  @Input({ required: true }) accordionItem: CdkAccordionItem;
}
