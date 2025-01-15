import { CdkAccordionItem } from '@angular/cdk/accordion';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-accordion-body',
  templateUrl: './accordion-body.component.html',
  styleUrls: ['./accordion-body.component.scss']
})
export class AccordionBodyComponent {
  @Input({ required: true }) accordionItem: CdkAccordionItem;
  @Input({ required: true }) index: number;
}
