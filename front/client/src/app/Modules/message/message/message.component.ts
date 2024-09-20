import { Component, Input } from '@angular/core';
import { Message } from 'src/app/Models/Message/Message';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.scss']
})
export class MessageComponent {
  @Input() message: Message;

  actions = [
    { label: 'Action 1', value: 'action1' },
    { label: 'Action 2', value: 'action2' },
    { label: 'Action 3', value: 'action3' }
  ];

  menuVisible = false;
  menuPositionX = 0;
  menuPositionY = 0;

  onRightClick(event: MouseEvent) {
    event.preventDefault(); // Prevent the browser's context menu
    this.menuPositionX = event.clientX;
    this.menuPositionY = event.clientY;
    this.menuVisible = true;
  }

  handleActionClick(action: any) {
    console.log('Action clicked:', action);
    this.menuVisible = false; // Hide the menu after an action is selected
  }

  // Optionally, you can hide the context menu when the user clicks outside
  onClickOutside(event: MouseEvent) {
    this.menuVisible = false;
  }
}
