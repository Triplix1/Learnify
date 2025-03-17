import { Component, Input, OnInit } from '@angular/core';
import { MeetingTokenService } from 'src/app/Core/services/meeting-token.service';
import { MeetingService } from 'src/app/Core/services/meeting.service';

@Component({
  selector: 'app-meeting-launch',
  templateUrl: './meeting-launch.component.html',
  styleUrls: ['./meeting-launch.component.scss']
})
export class MeetingLaunchComponent implements OnInit {
  @Input({ required: true }) courseId: number;
  @Input({ required: true }) isAuthor: boolean;

  sessionId: string;
  token: string;

  constructor(private readonly meetingService: MeetingService, private readonly _meetingTokenService: MeetingTokenService) { }

  ngOnInit(): void {
    this.meetingService.getSessionForCourse(this.courseId).subscribe(sessionId => this.sessionId = sessionId.data.sessionId);
  }

  sessionCreated(session: string) {
    this.sessionId = session;
    this.joinSession();
  }

  joinSession() {
    this._meetingTokenService.generateToken(this.sessionId).subscribe(response => this.token = response.data.token);
  }
}
