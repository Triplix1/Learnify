import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminsManagingComponent } from './admins-managing/admins-managing.component';
import { SharedModule } from 'src/app/Shared/shared.module';
import { AdminRoutingModule } from './admin-routing.module';



@NgModule({
  declarations: [
    AdminsManagingComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    AdminRoutingModule,
  ]
})
export class AdminModule { }
