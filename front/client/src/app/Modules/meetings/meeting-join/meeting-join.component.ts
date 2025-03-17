import { Component, EventEmitter, Output } from '@angular/core';
import { MeetingTokenService } from 'src/app/Core/services/meeting-token.service';

@Component({
  selector: 'app-meeting-join',
  templateUrl: './meeting-join.component.html',
  styleUrls: ['./meeting-join.component.scss']
})
export class MeetingJoinComponent {
  @Output() joinRequested: EventEmitter<void> = new EventEmitter();

  constructor(private readonly _meetingTokenService: MeetingTokenService) { }

  emitJoinRequested() {
    this.joinRequested.emit();
  }
}
