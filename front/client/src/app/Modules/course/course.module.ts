import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateCourseComponent } from './managing/create/create-course.component';
import { CourseRoutingModule } from './course-routing.module';
import { SharedModule } from 'src/app/Shared/shared.module';
import { CreateParagraphComponent } from './managing/create-paragraph/create-paragraph.component';
import { CreateLessonComponent } from './managing/create-lesson/create-lesson.component';
import { CourseCartComponent } from './course-cart/course-cart.component';
import { CreateQuizComponent } from './managing/create-quiz/create-quiz.component';
import { LessonTitleComponent } from './managing/lesson-title/lesson-title.component';
import { CreateSingleQuizComponent } from './managing/create-single-quiz/create-single-quiz.component';
import { CreateQuizOptionComponent } from './managing/create-quiz-option/create-quiz-option.component';
import { CreateQuizAnswersComponent } from './managing/create-quiz-answers/create-quiz-answers.component';
import { MainCoursePageComponent } from './representation/main-course-page/main-course-page.component';
import { PaymentSuccessComponent } from './payment-success/payment-success.component';
import { PaymentCancelledComponent } from './payment-cancelled/payment-cancelled.component';
import { CourseStudyPageComponent } from './course-study/course-study-page/course-study-page.component';
import { CourseStudyLessonComponent } from './course-study/course-study-lesson/course-study-lesson.component';



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
    CreateQuizAnswersComponent,
    MainCoursePageComponent,
    PaymentSuccessComponent,
    PaymentCancelledComponent,
    CourseStudyPageComponent,
    CourseStudyLessonComponent,
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
