import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MessageRoutingModule } from './message-routing.module';
import { JoinToGroupComponent } from './join-to-group/join-to-group.component';
import { SharedModule } from 'src/app/Shared/shared.module';
import { GroupThreadComponent } from './group-thread/group-thread.component';
import { MessageComponent } from './message/message.component';

@NgModule({
  declarations: [
    JoinToGroupComponent,
    GroupThreadComponent,
    MessageComponent
  ],
  imports: [
    CommonModule,
    MessageRoutingModule,
    SharedModule,
  ]
})
export class MessageModule { }
