import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateCourseComponent } from './create/create-course.component';


const routes: Routes = [
  {
    path: "course",
    children:
      [
        { path: 'create', component: CreateCourseComponent }
      ]
  }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class CourseRoutingModule { }
