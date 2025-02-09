import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { delay, map, Observable, of, pipe, take, takeUntil, tap } from 'rxjs';
import { CourseService } from 'src/app/Core/services/course.service';
import { LessonService } from 'src/app/Core/services/lesson.service';
import { ApiResponseWithData } from 'src/app/Models/ApiResponse';
import { BaseComponent } from 'src/app/Models/BaseComponent';
import { CourseStudyResponse } from 'src/app/Models/Course/CourseStudyResponse';
import { LessonTitleResponse } from 'src/app/Models/Course/Lesson/LessonTitleResponse';
import { ParagraphResponse } from 'src/app/Models/Course/Paragraph/ParagraphResponse';
import { SidebarItem } from 'src/app/Models/SidebarItems';

@Component({
  selector: 'app-course-study-page',
  templateUrl: './course-study-page.component.html',
  styleUrls: ['./course-study-page.component.scss']
})
export class CourseStudyPageComponent extends BaseComponent implements OnInit {
  @Input({ required: true }) courseId: number;
  @Input() lessonId: string;

  lessonTitles: Map<number, LessonTitleResponse[]> = new Map<number, LessonTitleResponse[]>();
  isSidebarVisible = true;
  isSectionsLoaded: boolean = false;
  courseStudyResponse: CourseStudyResponse;
  itemSelected: { paragraphIndex: number, lessonIndex: number };

  sections: SidebarItem[] = [
    { title: 'Назва розділу', expanded: false, items: ['Назва пункту', 'Назва пункту', 'Назва пункту'] },
    { title: 'Назва розділу', expanded: false, items: ['Назва пункту', 'Назва пункту', 'Назва пункту'] },
    { title: 'Назва розділу', expanded: false, items: ['Назва пункту', 'Назва пункту', 'Назва пункту'] },
    { title: 'Назва розділу', expanded: false, items: ['Назва пункту', 'Назва пункту', 'Назва пункту'] },
    { title: 'Назва розділу', expanded: false, items: ['Назва пункту', 'Назва пункту', 'Назва пункту'] },
    { title: 'Назва розділу', expanded: false, items: ['Назва пункту', 'Назва пункту', 'Назва пункту'] },
    { title: 'Назва розділу', expanded: false, items: ['Назва пункту', 'Назва пункту', 'Назва пункту'] },
    { title: 'Назва розділу', expanded: false, items: ['Назва пункту', 'Назва пункту', 'Назва пункту'] },
    { title: 'Назва розділу', expanded: false, items: ['Назва пункту', 'Назва пункту', 'Назва пункту'] },
    { title: 'Назва розділу', expanded: false, items: ['Назва пункту', 'Назва пункту', 'Назва пункту'] },
    { title: 'Назва розділу', expanded: false, items: ['Назва пункту', 'Назва пункту', 'Назва пункту'] },
    { title: 'Назва розділу', expanded: false, items: ['Назва пункту', 'Назва пункту', 'Назва пункту'] },
    { title: 'Назва розділу', expanded: false, items: ['Назва пункту', 'Назва пункту', 'Назва пункту'] },
    { title: 'Назва розділу', expanded: false, items: ['Назва пункту', 'Назва пункту', 'Назва пункту'] },
    { title: 'Назва розділу', expanded: false, items: ['Назва пункту', 'Назва пункту', 'Назва пункту'] },
  ];

  constructor(private readonly courseService: CourseService,
    private readonly lessonService: LessonService,
    private readonly router: Router,
    private readonly spinnerService: NgxSpinnerService) {
    super();
  }

  ngOnInit(): void {
    console.log(this.lessonId);
    this.spinnerService.show('sidebarSpinner');
    this.courseService.getStudyResponse(this.courseId).pipe(takeUntil(this.destroySubject), take(1)).subscribe(
      response => {
        this.isSectionsLoaded = true;
        this.courseStudyResponse = response.data;
        this.mapResponseList(response.data.paragraphs);
        this.spinnerService.hide('sidebarSpinner');
      }
    );
  }

  toggleSection(section: any) {
    section.expanded = !section.expanded;
  }

  toggleSidebar() {
    this.isSidebarVisible = !this.isSidebarVisible;
  }

  openParagraph(paragraphIndex: number) {
    if (this.sections[paragraphIndex].items.length == 0) {
      this.loadlessonTitlesIfNotExists(paragraphIndex);
    }

    this.sections[paragraphIndex].expanded = !this.sections[paragraphIndex].expanded;
  }

  selectLesson(paragraphIndex: number, lessonIndex: number) {
    this.lessonId = this.lessonTitles.get(paragraphIndex)[lessonIndex].id;
    this.itemSelected = {
      lessonIndex: lessonIndex,
      paragraphIndex: paragraphIndex
    }

    this.router.navigate([`/course/study/${this.courseId}/${this.lessonId}`])
  }

  loadlessonTitlesIfNotExists(paragraphIndex: number) {
    this.loadlessonTitlesObservable(paragraphIndex).subscribe();
  }

  loadlessonTitlesObservable(paragraphIndex: number): Observable<LessonTitleResponse[]> {
    if (!this.lessonTitles.has(paragraphIndex) && this.isSectionsLoaded) {
      this.spinnerService.show("sidebarLessonLoader");
      return this.lessonService.getLessonTitlesForParagraph(this.courseStudyResponse.paragraphs[paragraphIndex].id, false).pipe(takeUntil(this.destroySubject), take(1), tap(
        response => {
          this.spinnerService.hide("sidebarLessonLoader");
          this.lessonTitles.set(paragraphIndex, response.data);
          this.sections[paragraphIndex].items = response.data.map(x => x.title);
          if (response.data.length == 0) {
            throw "this paragraph doesn't contains any lessons";
          }
        }),
        map(
          response => response.data
        ));
    }

    return of(this.lessonTitles.get(paragraphIndex));
  }

  private mapResponseList(paragraphs: ParagraphResponse[]) {
    this.sections = [];
    for (let paragraph of paragraphs) {
      this.sections.push({
        title: paragraph.name,
        expanded: false,
        items: []
      });
    }
    if (this.sections.length > 0) {
      this.sections[0].expanded = true;
      this.loadlessonTitlesObservable(0).subscribe(
        _ => {
          this.selectLesson(0, 0);
        }
      );
    }
  }
}
