import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateCourseComponent } from './create/create-course.component';
import { authGuard } from 'src/app/Core/guards/auth.guard';


const routes: Routes = [
  {
    path: "course",
    children:
      [
        { path: 'managing/:courseId', component: CreateCourseComponent, canActivate: [authGuard] },
        { path: 'managing', component: CreateCourseComponent, canActivate: [authGuard] },
      ]
  }

];

@NgModule({
  imports: [RouterModule.forRoot(routes, { bindToComponentInputs: true })],
  exports: [RouterModule]
})

export class CourseRoutingModule { }
