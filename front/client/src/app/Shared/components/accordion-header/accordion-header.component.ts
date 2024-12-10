import { CdkAccordionItem } from '@angular/cdk/accordion';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-accordion-header',
  templateUrl: './accordion-header.component.html',
  styleUrls: ['./accordion-header.component.scss']
})
export class AccordionHeaderComponent {
  @Input({ required: true }) accordionItem: CdkAccordionItem;
}
