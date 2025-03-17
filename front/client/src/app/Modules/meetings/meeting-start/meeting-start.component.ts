import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MeetingService } from 'src/app/Core/services/meeting.service';

@Component({
  selector: 'app-meeting-start',
  templateUrl: './meeting-start.component.html',
  styleUrls: ['./meeting-start.component.scss']
})
export class MeetingStartComponent {
  @Input({ required: true }) courseId: number;
  @Input({ required: true }) isAuthor: boolean;
  @Output() sessionCreated: EventEmitter<string> = new EventEmitter<string>();

  constructor(private readonly meetingService: MeetingService) { }

  createSession() {
    if (!this.isAuthor)
      return;

    this.meetingService.createSession(this.courseId).subscribe(sessionResponse => {
      this.sessionCreated.emit(sessionResponse.data.sessionId);
    });
  }
}
