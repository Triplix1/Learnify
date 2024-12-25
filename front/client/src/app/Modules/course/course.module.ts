import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateCourseComponent } from './create/create-course.component';
import { CourseRoutingModule } from './course-routing.module';
import { SharedModule } from 'src/app/Shared/shared.module';
import { CreateParagraphComponent } from './create-paragraph/create-paragraph.component';
import { CreateLessonComponent } from './create-lesson/create-lesson.component';
import { CourseCartComponent } from './course-cart/course-cart.component';
import { CreateQuizComponent } from './create-quiz/create-quiz.component';
import { LessonTitleComponent } from './lesson-title/lesson-title.component';
import { CreateSingleQuizComponent } from './create-single-quiz/create-single-quiz.component';
import { CreateQuizOptionComponent } from './create-quiz-option/create-quiz-option.component';



@NgModule({
  declarations: [
    CreateCourseComponent,
    CreateParagraphComponent,
    CreateLessonComponent,
    CourseCartComponent,
    CreateQuizComponent,
    LessonTitleComponent,
    CreateSingleQuizComponent,
    CreateQuizOptionComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    CourseRoutingModule,
  ],
  exports: [
    CreateCourseComponent,
    CourseCartComponent,
  ]
})
export class CourseModule { }
