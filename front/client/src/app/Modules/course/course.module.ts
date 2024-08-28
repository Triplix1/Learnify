import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateComponent } from './create/create.component';
import { CourseRoutingModule } from './course-routing.module';
import { SharedModule } from 'src/app/Shared/shared.module';



@NgModule({
  declarations: [
    CreateComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    CourseRoutingModule,
  ]
})
export class CourseModule { }
