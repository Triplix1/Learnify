import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-meeting-admin-bar-icon',
  templateUrl: './meeting-admin-bar-icon.component.html',
  styleUrls: ['./meeting-admin-bar-icon.component.scss']
})
export class MeetingAdminBarIconComponent {
  @Input({ required: true }) itemName: string;
  @Output() onClick: EventEmitter<void> = new EventEmitter<void>();

  clicked(): void {
    this.onClick.emit();
  }
}
