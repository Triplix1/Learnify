import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateCourseComponent } from './create/create-course.component';
import { CourseRoutingModule } from './course-routing.module';
import { SharedModule } from 'src/app/Shared/shared.module';
import { CreateParagraphComponent } from './create-paragraph/create-paragraph.component';



@NgModule({
  declarations: [
    CreateCourseComponent,
    CreateParagraphComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    CourseRoutingModule,
  ]
})
export class CourseModule { }
