import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/Shared/shared.module';
import { ProfileRoutingModule } from './profile-routing.module';
import { MainProfileComponent } from './main-profile/main-profile.component';
import { MyCoursesComponent } from './my-courses/my-courses.component';
import { CourseModule } from '../course/course.module';
import { MySubscribedCoursesComponent } from './my-subscribed-courses/my-subscribed-courses.component';


@NgModule({
  declarations: [
    MainProfileComponent,
    MyCoursesComponent,
    MySubscribedCoursesComponent,
  ],
  imports: [
    ProfileRoutingModule,
    SharedModule,
    CourseModule,
  ],
  exports: [
    MainProfileComponent,
  ]
})
export class ProfileModule { }
