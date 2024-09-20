import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-context-menu',
  templateUrl: './context-menu.component.html',
  styleUrls: ['./context-menu.component.scss']
})
export class ContextMenuComponent {
  @Input() actions: any[] = [];
  @Input() visible = false;
  @Input() positionX = 0;
  @Input() positionY = 0;
  @Output() actionClick = new EventEmitter<any>();

  onActionClick(action: any) {
    this.actionClick.emit(action);
    this.visible = false; // Hide the menu after an action is clicked
  }
}