import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-meeting-admin-bar',
  templateUrl: './meeting-admin-bar.component.html',
  styleUrls: ['./meeting-admin-bar.component.scss']
})
export class MeetingAdminBarComponent {
  @Input({ required: true }) publisher: OT.Publisher;
  @Input({ required: true }) videoEnabled: boolean;
  @Input({ required: true }) audioEnabled: boolean;
  @Input({ required: true }) screenShared: boolean;
  @Output() screenShareRequested: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() onLogout: EventEmitter<void> = new EventEmitter<void>();

  changeVideo() {
    this.videoEnabled = !this.videoEnabled;
    this.publisher.publishVideo(this.videoEnabled);
  }

  changeAudio() {
    this.audioEnabled = !this.audioEnabled;
    this.publisher.publishAudio(this.audioEnabled);
  }

  changeScreenShare() {
    this.screenShared = !this.screenShared;
    this.screenShareRequested.emit(this.screenShared);
  }

  logout() {
    this.onLogout.emit();
  }
}
