import { Component, EventEmitter, Input, Output } from '@angular/core';
import { take, takeUntil } from 'rxjs';
import { MeetingService } from 'src/app/Core/services/meeting.service';
import { BaseComponent } from 'src/app/Models/BaseComponent';

@Component({
  selector: 'app-meeting-start',
  templateUrl: './meeting-start.component.html',
  styleUrls: ['./meeting-start.component.scss']
})
export class MeetingStartComponent extends BaseComponent {
  @Input({ required: true }) courseId: number;
  @Input({ required: true }) isAuthor: boolean;
  @Output() sessionCreated: EventEmitter<string> = new EventEmitter<string>();

  constructor(private readonly meetingService: MeetingService) {
    super();
  }

  createSession() {
    if (!this.isAuthor)
      return;

    this.meetingService.createSession(this.courseId).pipe(take(1), takeUntil(this.destroySubject)).subscribe(sessionResponse => {
      this.sessionCreated.emit(sessionResponse.data.sessionId);
    });
  }
}
