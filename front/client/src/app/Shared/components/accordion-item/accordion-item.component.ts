import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-accordion-item',
  templateUrl: './accordion-item.component.html',
  styleUrls: ['./accordion-item.component.scss']
})
export class AccordionItemComponent {
  @Input({ required: true }) index: number;
  @Output() itemOpened: EventEmitter<number> = new EventEmitter<number>();

  opened() {
    this.itemOpened.emit(this.index);
  }
}
