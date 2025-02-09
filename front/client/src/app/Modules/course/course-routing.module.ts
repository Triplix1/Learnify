import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateCourseComponent } from './managing/create/create-course.component';
import { authGuard } from 'src/app/Core/guards/auth.guard';
import { MainCoursePageComponent } from './representation/main-course-page/main-course-page.component';
import { PaymentSuccessComponent } from './payment-success/payment-success.component';
import { PaymentCancelledComponent } from './payment-cancelled/payment-cancelled.component';
import { CourseStudyPageComponent } from './course-study/course-study-page/course-study-page.component';


const routes: Routes = [
  {
    path: "course",
    children:
      [
        { path: 'managing/:courseId', component: CreateCourseComponent, canActivate: [authGuard] },
        { path: 'managing', component: CreateCourseComponent, canActivate: [authGuard] },
        { path: 'main-info/:courseId', component: MainCoursePageComponent },
        { path: 'study/:courseId', component: CourseStudyPageComponent },
        { path: 'study/:courseId/:lessonId', component: CourseStudyPageComponent },
        { path: 'payment-success/:courseId', component: PaymentSuccessComponent },
        { path: 'payment-cancelled/:courseId', component: PaymentCancelledComponent },
      ]
  }

];

@NgModule({
  imports: [RouterModule.forRoot(routes, { bindToComponentInputs: true })],
  exports: [RouterModule]
})

export class CourseRoutingModule { }
