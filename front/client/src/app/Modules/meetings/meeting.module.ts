import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MeetingComponent } from './meeting/meeting.component';
import { MeetingLaunchComponent } from './meeting-launch/meeting-launch.component';
import { MeetingJoinComponent } from './meeting-join/meeting-join.component';
import { MeetingStartComponent } from './meeting-start/meeting-start.component';
import { SharedModule } from 'src/app/Shared/shared.module';
import { MeetingAdminBarComponent } from './meeting-admin-bar/meeting-admin-bar.component';
import { MeetingAdminBarIconComponent } from './meeting-admin-bar-icon/meeting-admin-bar-icon.component';

@NgModule({
  declarations: [
    MeetingComponent,
    MeetingLaunchComponent,
    MeetingJoinComponent,
    MeetingStartComponent,
    MeetingAdminBarComponent,
    MeetingAdminBarIconComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
  ],
  exports: [
    MeetingLaunchComponent,
  ]
})
export class MeetingModule { }
