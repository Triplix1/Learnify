import { Component, OnInit } from '@angular/core';
import { take } from 'rxjs';
import { CourseService } from 'src/app/Core/services/course.service';
import { CourseTitleResponse } from 'src/app/Models/Course/CourseTitleResponse';
import { PagedList } from 'src/app/Models/PagedList';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  courseTitles: PagedList<CourseTitleResponse>;

  constructor(private readonly courseService: CourseService) { }

  ngOnInit(): void {
    this.courseService.getCourseTitles().pipe(take(1))
      .subscribe(
        response => this.courseTitles = response.data
      );
  }
}
