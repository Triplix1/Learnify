import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-meeting-admin-bar',
  templateUrl: './meeting-admin-bar.component.html',
  styleUrls: ['./meeting-admin-bar.component.scss']
})
export class MeetingAdminBarComponent {
  @Input({ required: true }) publisher: OT.Publisher;
  @Input({ required: true }) videoEnabled: boolean;
  @Input({ required: true }) audioEnabled: boolean;

  changeVideo() {
    this.videoEnabled = !this.videoEnabled;
    this.publisher.publishVideo(this.videoEnabled);
  }

  changeAudio() {
    this.audioEnabled = !this.audioEnabled;
    this.publisher.publishAudio(this.audioEnabled);
  }
}
