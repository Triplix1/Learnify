import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ManagingComponent } from './managing/managing.component';
import { ModeratorsRoutingModule } from './moderators-routing.module';
import { SharedModule } from 'src/app/Shared/shared.module';



@NgModule({
  declarations: [
    ManagingComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    ModeratorsRoutingModule,
  ]
})
export class ModeratorsModule { }
