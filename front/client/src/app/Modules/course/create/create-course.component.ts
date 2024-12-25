import { Component, ElementRef, Input, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { map, Observable, switchMap, take } from 'rxjs';
import { CourseService } from 'src/app/Core/services/course.service';
import { LessonService } from 'src/app/Core/services/lesson.service';
import { MediaService } from 'src/app/Core/services/media.service';
import { ApiResponseWithData } from 'src/app/Models/ApiResponse';
import { AttachmentResponse } from 'src/app/Models/Attachment/AttachmentResponse';
import { BaseComponent } from 'src/app/Models/BaseComponent';
import { CourseCreateRequest } from 'src/app/Models/Course/CourseCreateRequest';
import { CourseResponse } from 'src/app/Models/Course/CourseResponse';
import { CourseUpdateRequest } from 'src/app/Models/Course/CourseUpdateRequest';
import { LessonAddOrUpdateRequest } from 'src/app/Models/Course/Lesson/LessonAddOrUpdateRequest';
import { LessonStepAddOrUpdateRequest } from 'src/app/Models/Course/Lesson/LessonStepAddOrUpdateRequest';
import { LessonTitleResponse } from 'src/app/Models/Course/Lesson/LessonTitleResponse';
import { LessonUpdateResponse } from 'src/app/Models/Course/Lesson/LessonUpdateResponse';
import { VideoAddOrUpdateRequest } from 'src/app/Models/Course/Lesson/Video/VideoAddOrUpdateRequest';
import { ParagraphResponse } from 'src/app/Models/Course/Paragraph/ParagraphResponse';
import { Language } from 'src/app/Models/enums/Language';
import { PrivateFileBlobCreateRequest } from 'src/app/Models/File/PrivateFileBlobCreateRequest';
import { PrivateFileDataResponse } from 'src/app/Models/File/PrivateFileDataResponse';
import { ParagraphUpdated } from 'src/app/Models/ParagraphUpdated';
import { SelectorOption } from 'src/app/Models/SelectorOption';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-create-course',
  templateUrl: './create-course.component.html',
  styleUrls: ['./create-course.component.scss']
})
export class CreateCourseComponent extends BaseComponent {
  @Input() courseId: number = null;

  editingMode: boolean = false;
  courseResponse: CourseResponse = null;
  paragraphs: ParagraphResponse[] = [];
  courseForm: FormGroup = new FormGroup({});
  selectorOptions: SelectorOption[] = Object.keys(Language)
    .filter(key => isNaN(Number(key)))  // Filter out numeric keys
    .map(key => {
      return { label: key, value: Language[key as keyof typeof Language] };
    });
  firstFormGroup: any;
  secondFormGroup: any;

  currentLessonEditing: LessonStepAddOrUpdateRequest;
  possibleToCreateNewLesson: boolean = true;

  constructor(private readonly fb: FormBuilder,
    private readonly courseService: CourseService,
    private readonly spinner: NgxSpinnerService) {
    super();
  }

  ngOnInit(): void {
    console.log(this.courseId);

    this.initializeForm();

    if (this.courseId !== null && this.courseId !== undefined) {
      this.spinner.show();
      this.courseService.getForUpdate(this.courseId).pipe(take(1))
        .subscribe(
          response => {
            this.handleCourseUpdate(response.data);
            this.spinner.hide();
          }
        );
    }
  }

  saveChanges() {
    this.spinner.show();
    if (this.courseId === null || this.courseId === undefined) {
      const courseCreateRequest: CourseCreateRequest = {
        name: this.courseForm.controls["name"].value,
        price: +this.courseForm.controls["price"].value,
        primaryLanguage: this.courseForm.controls["language"].value as Language
      }

      this.courseService.createCourse(courseCreateRequest).pipe(take(1)).subscribe(
        courseResponse => {
          this.handleCourseUpdate(courseResponse.data);
          this.spinner.hide();
        }
      );
    }
    else {
      const courseUpdateRequest: CourseUpdateRequest = {
        id: this.courseId,
        name: this.courseForm.controls["name"].value,
        price: +this.courseForm.controls["price"].value,
        primaryLanguage: this.courseForm.controls["language"].value as Language
      }

      this.courseService.updateCourse(courseUpdateRequest).pipe(take(1)).subscribe(
        courseResponse => {
          this.handleCourseUpdate(courseResponse.data);
          this.spinner.hide();
        }
      );
    }
  }

  cancelChanges() {
    this.initializeForm();
  }

  publish(publish: boolean) {
    this.spinner.show();

    this.courseService.publishCourse(this.courseId, publish).pipe(take(1))
      .subscribe(
        response => {
          this.courseResponse = response.data;
          this.handleCourseUpdate(response.data);
          this.spinner.hide();
        }
      );
  }

  addParagraph() {
    this.paragraphs.push(null);
  }

  paragraphUpdated(paragraphUpdated: ParagraphUpdated) {
    this.paragraphs[paragraphUpdated.index] = paragraphUpdated.paragraph;
  }

  lessonDeleted(lessonTitleResponse: LessonTitleResponse) {
    if (this.currentLessonEditing.id === lessonTitleResponse.id) {
      this.clearLessonTab();
    }
  }

  lessonAddOrUpdateRequest(lessonStepAddOrUpdateRequest: LessonStepAddOrUpdateRequest) {
    if (!this.possibleToCreateNewLesson) {
      var confirmed = confirm('You will lost your unloaded changes');
      if (!confirmed)
        return;
    }

    this.currentLessonEditing = lessonStepAddOrUpdateRequest;
  }

  private clearLessonTab() {
    this.currentLessonEditing = null;
  }

  private handleCourseUpdate(courseResponse: CourseResponse) {
    this.courseId = courseResponse.id;
    this.courseResponse = courseResponse;
    this.editingMode = courseResponse.isPublished;
    this.paragraphs = courseResponse.paragraphs;

    if (this.paragraphs.length === 0)
      this.paragraphs = [null];

    this.initializeForm();
  }

  private initializeForm() {
    this.courseForm = this.fb.group({
      name: [this.courseResponse?.name ?? '', [Validators.required]],
      price: [this.courseResponse?.price ?? '', [Validators.required, Validators.pattern('^\d+$')]],
      language: this.courseResponse?.primaryLanguage ? Language[this.courseResponse.primaryLanguage as keyof typeof Language] : Language.English,
    });
  }
}
