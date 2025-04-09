import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ModeratorsManagingComponent } from './moderators-managing/moderators-managing.component';
import { ModeratorsRoutingModule } from './moderators-routing.module';
import { SharedModule } from 'src/app/Shared/shared.module';
import { PaymentsComponent } from './payments/payments.component';



@NgModule({
  declarations: [
    ModeratorsManagingComponent,
    PaymentsComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    ModeratorsRoutingModule,
  ]
})
export class ModeratorsModule { }
