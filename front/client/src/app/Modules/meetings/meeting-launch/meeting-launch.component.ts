import { Component, Input, OnInit } from '@angular/core';
import { pipe, take, takeUntil } from 'rxjs';
import { MeetingTokenService } from 'src/app/Core/services/meeting-token.service';
import { MeetingService } from 'src/app/Core/services/meeting.service';
import { BaseComponent } from 'src/app/Models/BaseComponent';

@Component({
  selector: 'app-meeting-launch',
  templateUrl: './meeting-launch.component.html',
  styleUrls: ['./meeting-launch.component.scss']
})
export class MeetingLaunchComponent extends BaseComponent implements OnInit {
  @Input({ required: true }) courseId: number;
  @Input({ required: true }) isAuthor: boolean;

  sessionId: string;
  token: string;

  constructor(private readonly meetingService: MeetingService, private readonly _meetingTokenService: MeetingTokenService) {
    super();
  }

  ngOnInit(): void {
    this.meetingService.getSessionForCourse(this.courseId).pipe(take(1), takeUntil(this.destroySubject)).subscribe(sessionId => this.sessionId = sessionId.data.sessionId);
  }

  sessionCreated(session: string) {
    this.sessionId = session;
    this.joinSession();
  }

  joinSession() {
    this._meetingTokenService.generateToken(this.sessionId).pipe(take(1), takeUntil(this.destroySubject)).subscribe(response => this.token = response.data.token);
  }

  loggedOutFromTheMeeting() {
    this.token = null;
  }
}
