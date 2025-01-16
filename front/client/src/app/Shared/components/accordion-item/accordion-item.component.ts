import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-accordion-item',
  templateUrl: './accordion-item.component.html',
  styleUrls: ['./accordion-item.component.scss']
})
export class AccordionItemComponent {
  @Input({ required: true }) index: number;
  @Output() itemOpened: EventEmitter<boolean> = new EventEmitter<boolean>();

  opened() {
    this.itemOpened.emit(true);
  }

  closed() {
    this.itemOpened.emit(false);
  }
}
