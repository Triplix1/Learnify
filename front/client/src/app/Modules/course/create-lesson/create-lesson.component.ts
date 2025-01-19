import { Component, ElementRef, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable, switchMap, take, takeUntil } from 'rxjs';
import { LessonService } from 'src/app/Core/services/lesson.service';
import { MediaService } from 'src/app/Core/services/media.service';
import { ApiResponseWithData } from 'src/app/Models/ApiResponse';
import { AttachmentResponse } from 'src/app/Models/Attachment/AttachmentResponse';
import { BaseComponent } from 'src/app/Models/BaseComponent';
import { LessonAddOrUpdateRequest } from 'src/app/Models/Course/Lesson/LessonAddOrUpdateRequest';
import { LessonStepAddOrUpdateRequest } from 'src/app/Models/Course/Lesson/LessonStepAddOrUpdateRequest';
import { LessonTitleResponse } from 'src/app/Models/Course/Lesson/LessonTitleResponse';
import { LessonUpdateResponse } from 'src/app/Models/Course/Lesson/LessonUpdateResponse';
import { VideoAddOrUpdateRequest } from 'src/app/Models/Course/Lesson/Video/VideoAddOrUpdateRequest';
import { Language } from 'src/app/Models/enums/Language';
import { PrivateFileBlobCreateRequest } from 'src/app/Models/File/PrivateFileBlobCreateRequest';
import { PrivateFileDataResponse } from 'src/app/Models/File/PrivateFileDataResponse';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-create-lesson',
  templateUrl: './create-lesson.component.html',
  styleUrls: ['./create-lesson.component.scss']
})
export class CreateLessonComponent extends BaseComponent implements OnInit, OnChanges {
  @Input({ required: true }) currentLessonEditing: LessonStepAddOrUpdateRequest;
  @Input({ required: true }) courseId: number;
  @Input({ required: true }) possibleToCreateNewLesson: boolean = true;
  @Output() possibleToCreateNewLessonChange = new EventEmitter<boolean>();
  @Output() onUpdate: EventEmitter<LessonTitleResponse> = new EventEmitter<LessonTitleResponse>(null);

  lessonResponse: LessonUpdateResponse;
  lessonUpdatedTitleResponse: LessonTitleResponse;
  lessonForm: FormGroup = new FormGroup({});

  constructor(private readonly lessonService: LessonService, private readonly mediaService: MediaService, private readonly fb: FormBuilder) {
    super();
  }

  set possibleToCreateNewLessonValue(possibleToCreateNewLesson: boolean) {
    this.possibleToCreateNewLesson = possibleToCreateNewLesson;
    this.possibleToCreateNewLessonChange.emit(possibleToCreateNewLesson);
  }

  ngOnInit(): void {
    this.initializeComponent()
  }

  initializeComponent() {

    this.initializeForm();

    this.lessonResponse = null;
    this.lessonUpdatedTitleResponse = null;
    this.possibleToCreateNewLessonValue = true;

    if (!this.currentLessonEditing.id) {
      this.lessonService.saveDraft({ id: null, editedLessonId: null, paragraphId: this.currentLessonEditing.paragraphId, title: null, video: null }).pipe(take(1))
        .subscribe(response => {
          this.handleLessonUpdate(response.data);
        })
    }
    else {
      this.lessonService.getLessonForUpdateById(this.currentLessonEditing.id).pipe(take(1))
        .subscribe(response => {
          this.handleLessonUpdate(response.data);
          this.lessonForm.markAsUntouched();
        });
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    const currentLessonEditing = changes['currentLessonEditing'];
    if (currentLessonEditing && currentLessonEditing.currentValue !== currentLessonEditing.previousValue) {
      this.initializeComponent();
    }
  }

  handleFileInput(fileUploadedEvent: Observable<ApiResponseWithData<PrivateFileDataResponse>>) {
    this.possibleToCreateNewLessonValue = false;

    fileUploadedEvent.pipe(
      take(1),
      takeUntil(this.destroySubject),
      switchMap((response: any) => {
        const attachment: AttachmentResponse = {
          contentType: response.data.contentType,
          fileId: response.data.id
        };

        const lessonAddOrUpdateRequest = this.prepareLessonToUpdateDto();

        return this.lessonService.saveDraft(lessonAddOrUpdateRequest);
      })
    )
      .subscribe(response => {
        this.handleLessonUpdate(response.data);
        this.possibleToCreateNewLessonValue = true;
      });
  }


  // lessonAddOrUpdateRequest(lessonStepAddOrUpdateRequest: LessonStepAddOrUpdateRequest) {
  //   this.currentLessonEditing = lessonStepAddOrUpdateRequest;
  //   this.lessonResponse = null;

  //   if (lessonStepAddOrUpdateRequest.id) {
  //     this.lessonService.getLessonForUpdateById(lessonStepAddOrUpdateRequest.id).pipe(take(1)).subscribe(response => {
  //       this.handleLessonUpdate(response.data)
  //     });
  //   }

  //   this.initializeForm();
  // }

  initializeForm() {
    this.lessonForm = this.fb.group({
      title: ['', Validators.required],
      language: [Language.English, Validators.required],
    })
  }

  saveDraft() {
    const lessonAddOrUpdateRequest = this.prepareLessonToUpdateDto();
    this.possibleToCreateNewLessonValue = false;

    lessonAddOrUpdateRequest.title = this.lessonForm.controls['title'].value ?? lessonAddOrUpdateRequest.title;

    if (lessonAddOrUpdateRequest.video)
      lessonAddOrUpdateRequest.video.primaryLanguage = this.lessonForm.controls['language'].value ?? lessonAddOrUpdateRequest.video.primaryLanguage;

    this.lessonService.saveDraft(lessonAddOrUpdateRequest).pipe(take(1))
      .subscribe(response => {
        this.handleLessonUpdate(response.data);
        this.possibleToCreateNewLessonValue = true;
      });
  }

  private prepareLessonToUpdateDto(): LessonAddOrUpdateRequest {
    let lessonAddOrUpdateRequest: LessonAddOrUpdateRequest;

    if (this.lessonResponse) {
      let video: VideoAddOrUpdateRequest;

      if (this.lessonResponse.video) {
        video = {
          attachment: this.lessonResponse.video.attachment,
          primaryLanguage: this.lessonResponse.video.primaryLanguage,
          subtitles: this.lessonResponse.video.subtitles.map(s => Language[s.language as keyof typeof Language])
        }
      }

      lessonAddOrUpdateRequest = {
        editedLessonId: this.lessonResponse.editedLessonId,
        id: this.lessonResponse.id,
        paragraphId: this.lessonResponse.paragraphId,
        video: video,
        title: this.lessonForm.controls['title'].value
      }
    }
    else {
      lessonAddOrUpdateRequest = {
        editedLessonId: null,
        paragraphId: this.currentLessonEditing.paragraphId,
        id: this.currentLessonEditing.id,
        title: null,
        video: null
      }
    }

    return lessonAddOrUpdateRequest;
  }

  private handleLessonUpdate(lessonResponse: LessonUpdateResponse) {
    this.currentLessonEditing = { id: lessonResponse.id, paragraphId: lessonResponse.paragraphId };
    this.lessonResponse = lessonResponse;
    this.lessonUpdatedTitleResponse = { id: lessonResponse.id, title: lessonResponse.title }
    this.lessonForm.controls['title'].setValue(lessonResponse.title);

    if (lessonResponse.video)
      this.lessonForm.controls['language'].setValue(Language[lessonResponse.video.primaryLanguage as keyof typeof Language]);

    this.lessonService.$lessonAddedOrUpdated.next(this.lessonResponse);
  }
}
