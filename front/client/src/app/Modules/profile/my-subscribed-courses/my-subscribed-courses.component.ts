import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { take, takeUntil } from 'rxjs';
import { CourseParamsDeepLinkingService } from 'src/app/Core/services/course-params-deep-linking.service';
import { CourseService } from 'src/app/Core/services/course.service';
import { BaseComponent } from 'src/app/Models/BaseComponent';
import { CourseTitleResponse } from 'src/app/Models/Course/CourseTitleResponse';
import { PagedList } from 'src/app/Models/PagedList';
import { CourseParams } from 'src/app/Models/Params/Course/CourseParams';

@Component({
  selector: 'app-my-subscribed-courses',
  templateUrl: './my-subscribed-courses.component.html',
  styleUrls: ['./my-subscribed-courses.component.scss']
})
export class MySubscribedCoursesComponent extends BaseComponent {
  courseParamsDeepLinkingService: CourseParamsDeepLinkingService = new CourseParamsDeepLinkingService(this.route, this.router);

  courseParams: CourseParams;
  courseTitles: PagedList<CourseTitleResponse>;

  constructor(private readonly courseService: CourseService, private readonly route: ActivatedRoute, private readonly router: Router) {
    super();
  }

  ngOnInit(): void {
    this.courseParams = this.courseParamsDeepLinkingService.getCourseParams();

    this.courseService.getMyCourseTitles(this.courseParams).pipe(take(1), takeUntil(this.destroySubject)).subscribe(r => this.courseTitles = r.data);
  }

  navigateToCourse(id: number) {
    this.router.navigate([`/course/study/${id}`]);
  }
}
