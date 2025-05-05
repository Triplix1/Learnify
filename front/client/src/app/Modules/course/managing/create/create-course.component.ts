import { Component, ElementRef, Input, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { NgxSpinnerService } from 'ngx-spinner';
import { Observable, take, takeUntil } from 'rxjs';
import { CourseService } from 'src/app/Core/services/course.service';
import { ApiResponseWithData } from 'src/app/Models/ApiResponse';
import { BaseComponent } from 'src/app/Models/BaseComponent';
import { CourseCreateRequest } from 'src/app/Models/Course/CourseCreateRequest';
import { CourseResponse } from 'src/app/Models/Course/CourseResponse';
import { CourseUpdateRequest } from 'src/app/Models/Course/CourseUpdateRequest';
import { CourseUpdateResponse } from 'src/app/Models/Course/CourseUpdateResponse';
import { LessonStepAddOrUpdateRequest } from 'src/app/Models/Course/Lesson/LessonStepAddOrUpdateRequest';
import { LessonTitleResponse } from 'src/app/Models/Course/Lesson/LessonTitleResponse';
import { ParagraphResponse } from 'src/app/Models/Course/Paragraph/ParagraphResponse';
import { PublishCourseRequest } from 'src/app/Models/Course/PublishCourseRequest';
import { CropperParams } from 'src/app/Models/CropperParams';
import { Language } from 'src/app/Models/enums/Language';
import { PrivateFileBlobCreateRequest } from 'src/app/Models/File/PrivateFileBlobCreateRequest';
import { PrivateFileDataResponse } from 'src/app/Models/File/PrivateFileDataResponse';
import { ParagraphUpdated } from 'src/app/Models/ParagraphUpdated';
import { SelectorOption } from 'src/app/Models/SelectorOption';
import { AcceptDialogComponent } from 'src/app/Shared/components/accept-dialog/accept-dialog.component';

@Component({
  selector: 'app-create-course',
  templateUrl: './create-course.component.html',
  styleUrls: ['./create-course.component.scss']
})
export class CreateCourseComponent extends BaseComponent {
  @Input() courseId: number = null;
  @ViewChild('lessonTab') myLessonTab: ElementRef;

  editingMode: boolean = false;
  courseResponse: CourseUpdateResponse = null;
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
  lessonTitleUpdated: LessonTitleResponse;

  photoSetted: boolean;
  videoSetted: boolean;

  cropperParams: CropperParams = {
    isCircle: false,
    minHeight: 180,
    minWidth: 290,
    isConstantAspectRatio: true
  };

  constructor(private readonly fb: FormBuilder,
    private readonly courseService: CourseService,
    private readonly spinner: NgxSpinnerService,
    private readonly dialog: MatDialog) {
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

  lassonUpdated(lessonTitleUpdated: LessonTitleResponse) {
    this.lessonTitleUpdated = lessonTitleUpdated;
  }

  saveChanges() {
    this.spinner.show();
    if (this.courseId === null || this.courseId === undefined) {
      const courseCreateRequest: CourseCreateRequest = {
        name: this.courseForm.controls["name"].value,
        price: +this.courseForm.controls["price"].value,
        primaryLanguage: this.courseForm.controls["language"].value as Language,
        description: this.courseForm.controls["description"].value
      }

      this.courseService.createCourse(courseCreateRequest).pipe(take(1)).subscribe(
        courseResponse => {
          this.handleCourseUpdate(courseResponse.data);
          this.spinner.hide();
        },
        error => this.dialog.open(AcceptDialogComponent, {
          width: '450px',
          data: {
            text: error.error.errorData.join("\n")
          }
        })
      );
    }
    else {
      const courseUpdateRequest: CourseUpdateRequest = {
        id: this.courseId,
        name: this.courseForm.controls["name"].value,
        description: this.courseForm.controls["description"].value,
        price: +this.courseForm.controls["price"].value,
        primaryLanguage: this.courseForm.controls["language"].value as Language
      }

      this.courseService.updateCourse(courseUpdateRequest).pipe(take(1)).subscribe(
        courseResponse => {
          this.handleCourseUpdate(courseResponse.data);
          this.spinner.hide();
        },
        error => this.dialog.open(AcceptDialogComponent, {
          width: '450px',
          data: {
            text: error.error.errorData.join("\n")
          }
        })
      );
    }
  }

  cancelChanges() {
    this.initializeForm();
  }

  publish(publish: boolean) {
    this.spinner.show();

    const publishRequest: PublishCourseRequest = {
      courseId: this.courseId,
      publish: publish
    };

    this.courseService.publishCourse(publishRequest).pipe(take(1))
      .subscribe(
        response => {
          this.courseResponse.isPublished = !this.courseResponse.isPublished;
          this.spinner.hide();
        },
        error => this.dialog.open(AcceptDialogComponent, {
          width: '450px',
          data: {
            text: error.error.errorData.join("\n")
          }
        })

      );
  }

  addParagraph() {
    this.paragraphs.push(null);
  }

  paragraphUpdated(paragraphUpdated: ParagraphUpdated) {
    this.paragraphs[paragraphUpdated.index] = paragraphUpdated.paragraph;
  }

  paragraphDeleted(index: number) {
    const deletedParagraph = this.paragraphs.at(index);

    if (this.currentLessonEditing?.paragraphId === deletedParagraph.id) {
      this.currentLessonEditing = null;
    }

    this.paragraphs = this.paragraphs.filter(p => p !== deletedParagraph);
  }

  lessonDeleted(lessonTitleResponse: LessonTitleResponse) {
    if (this.currentLessonEditing.id === lessonTitleResponse.id) {
      this.clearLessonTab();
    }
  }

  uploadPhotoFacade = (privateFileBlobCreateRequest: PrivateFileBlobCreateRequest): Observable<Object> => {
    privateFileBlobCreateRequest.courseId = this.courseResponse.id;

    return this.courseService.updatePhoto(privateFileBlobCreateRequest);
  }

  photoUpdated(observe: Observable<ApiResponseWithData<PrivateFileDataResponse>>) {
    observe.pipe(takeUntil(this.destroySubject)).subscribe(s => {
      this.courseResponse.photo = s.data;
      this.handleCourseUpdate(this.courseResponse);
    })
  }

  photoUnsetted() {
    this.courseResponse.photo = null;
  }

  uploadVideoFacade = (privateFileBlobCreateRequest: PrivateFileBlobCreateRequest): Observable<Object> => {
    privateFileBlobCreateRequest.courseId = this.courseResponse.id;

    return this.courseService.updateVideo(privateFileBlobCreateRequest);
  }

  videoUnsetted() {
    this.courseResponse.video = null;
  }

  videoUpdated(observe: Observable<ApiResponseWithData<PrivateFileDataResponse>>) {
    observe.pipe(takeUntil(this.destroySubject)).subscribe(s => {
      this.courseResponse.video = s.data;
      this.handleCourseUpdate(this.courseResponse);
    })
  }

  lessonAddOrUpdateRequest(lessonStepAddOrUpdateRequest: LessonStepAddOrUpdateRequest) {
    if (!this.possibleToCreateNewLesson) {
      var confirmed = confirm('You will lost your unloaded changes');
      if (!confirmed)
        return;
    }

    this.currentLessonEditing = lessonStepAddOrUpdateRequest;

    this.myLessonTab.nativeElement.click();
  }

  private clearLessonTab() {
    this.currentLessonEditing = null;
  }

  unpublishedCourse() {
    this.courseResponse.isPublished = false;
  }

  private handleCourseUpdate(courseResponse: CourseResponse) {
    this.courseId = courseResponse.id;
    this.courseResponse = courseResponse;
    this.editingMode = courseResponse.isPublished;
    this.paragraphs = courseResponse.paragraphs;

    this.photoSetted = courseResponse.photo !== null && courseResponse.photo !== undefined;
    this.videoSetted = courseResponse.video !== null && courseResponse.video !== undefined;

    if (this.paragraphs.length === 0)
      this.paragraphs = [null];

    this.initializeForm();
  }

  private initializeForm() {
    this.courseForm = this.fb.group({
      name: [this.courseResponse?.name ?? '', [Validators.required]],
      description: [this.courseResponse?.description ?? '', [Validators.required]],
      price: [this.courseResponse?.price ?? '', [Validators.required, Validators.pattern('^\d+$')]],
      language: this.courseResponse?.primaryLanguage ? Language[this.courseResponse.primaryLanguage as keyof typeof Language] : Language.English,
    });
  }
}
