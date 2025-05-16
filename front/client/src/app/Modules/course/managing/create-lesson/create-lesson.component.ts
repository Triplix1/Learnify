import { Component, ElementRef, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Observable, of, switchMap, take, takeUntil } from 'rxjs';
import { convertStringToLanguage } from 'src/app/Core/helpers/lessonHelper';
import { LessonService } from 'src/app/Core/services/lesson.service';
import { MediaService } from 'src/app/Core/services/media.service';
import { ApiResponseWithData } from 'src/app/Models/ApiResponse';
import { AttachmentResponse } from 'src/app/Models/Attachment/AttachmentResponse';
import { BaseComponent } from 'src/app/Models/BaseComponent';
import { CurrentLessonUpdatedResponse } from 'src/app/Models/Course/Lesson/CurrentLessonUpdatedResponse';
import { LessonAddOrUpdateRequest } from 'src/app/Models/Course/Lesson/LessonAddOrUpdateRequest';
import { LessonStepAddOrUpdateRequest } from 'src/app/Models/Course/Lesson/LessonStepAddOrUpdateRequest';
import { LessonTitleResponse } from 'src/app/Models/Course/Lesson/LessonTitleResponse';
import { LessonUpdateResponse } from 'src/app/Models/Course/Lesson/LessonUpdateResponse';
import { VideoAddOrUpdateRequest } from 'src/app/Models/Course/Lesson/Video/VideoAddOrUpdateRequest';
import { Language } from 'src/app/Models/enums/Language';
import { PrivateFileBlobCreateRequest } from 'src/app/Models/File/PrivateFileBlobCreateRequest';
import { PrivateFileDataResponse } from 'src/app/Models/File/PrivateFileDataResponse';
import { AcceptDialogComponent } from 'src/app/Shared/components/accept-dialog/accept-dialog.component';
import { ConfirmDialogComponent } from 'src/app/Shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-create-lesson',
  templateUrl: './create-lesson.component.html',
  styleUrls: ['./create-lesson.component.scss']
})
export class CreateLessonComponent extends BaseComponent implements OnChanges {
  @Input({ required: true }) currentLessonEditing: LessonStepAddOrUpdateRequest;
  @Input({ required: true }) courseId: number;
  @Input({ required: true }) possibleToCreateNewLesson: boolean = true;
  @Output() possibleToCreateNewLessonChange = new EventEmitter<boolean>();
  @Output() onUpdate: EventEmitter<LessonTitleResponse> = new EventEmitter<LessonTitleResponse>(null);
  @Output() lessonSaved: EventEmitter<void> = new EventEmitter<void>(null);

  initialLessonId: string;
  lessonResponse: LessonUpdateResponse;
  lessonUpdatedTitleResponse: LessonTitleResponse;
  lessonForm: FormGroup = new FormGroup({});

  constructor(private readonly lessonService: LessonService,
    private readonly fb: FormBuilder,
    private readonly dialog: MatDialog,
    private readonly mediaService: MediaService,
    private readonly toastrService: ToastrService) {
    super();
  }

  set possibleToCreateNewLessonValue(possibleToCreateNewLesson: boolean) {
    this.possibleToCreateNewLesson = possibleToCreateNewLesson;
    this.possibleToCreateNewLessonChange.emit(possibleToCreateNewLesson);
  }

  ngOnChanges(changes: SimpleChanges): void {
    const currentLessonEditing = changes['currentLessonEditing'];
    if (currentLessonEditing && currentLessonEditing.currentValue !== currentLessonEditing.previousValue) {
      this.initializeComponent();
    }
  }

  initializeComponent() {
    this.initializeForm();

    this.lessonForm.controls['language'].valueChanges.pipe(takeUntil(this.destroySubject))
      .subscribe(r => {
        if (r != this.lessonResponse?.primaryLanguage) {
          const lessonAddOrUpdateRequest = this.prepareLessonToUpdateDto();
          lessonAddOrUpdateRequest.primaryLanguage = this.lessonForm.controls['language'].value ?? lessonAddOrUpdateRequest.primaryLanguage;

          this.saveDraft(lessonAddOrUpdateRequest)
        }
      });

    this.lessonResponse = null;
    this.lessonUpdatedTitleResponse = null;
    this.possibleToCreateNewLessonValue = true;

    if (!this.currentLessonEditing.id) {
      this.lessonService.saveDraft({ id: null, editedLessonId: null, primaryLanguage: this.lessonForm.controls["language"].value, paragraphId: this.currentLessonEditing.paragraphId, title: null, video: null }).pipe(take(1))
        .subscribe(response => {
          this.handleLessonUpdate(response.data);
        })
    }
    else {
      this.lessonService.getLessonForUpdateById(this.currentLessonEditing.id).pipe(take(1))
        .subscribe(response => {
          this.handleLessonUpdate(response.data);
        });
    }
  }

  uploadContentFacade = (privateFileBlobCreateRequest: PrivateFileBlobCreateRequest): Observable<object> => {
    privateFileBlobCreateRequest.courseId = this.courseId;

    return this.mediaService.create(privateFileBlobCreateRequest);
  }

  handleUpdateOfTitle() {
    const lessonAddOrUpdateRequest = this.prepareLessonToUpdateDto();
    lessonAddOrUpdateRequest.title = this.lessonForm.controls['title'].value ?? lessonAddOrUpdateRequest.title;

    this.saveDraft(lessonAddOrUpdateRequest)
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
        lessonAddOrUpdateRequest.video = { attachment: attachment, subtitles: [lessonAddOrUpdateRequest.primaryLanguage] };

        return this.lessonService.saveDraft(lessonAddOrUpdateRequest);
      })
    )
      .subscribe(response => {
        this.handleLessonUpdate(response.data);
        this.possibleToCreateNewLessonValue = true;
      });
  }

  initializeForm() {
    this.lessonForm = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(50)]],
      language: [Language.English, Validators.required],
    })
  }

  dropVideo() {
    const lessonAddOrUpdateRequest = this.prepareLessonToUpdateDto();

    lessonAddOrUpdateRequest.video = null;
    this.saveDraft(lessonAddOrUpdateRequest);
  }

  saveDraft(lessonAddOrUpdateRequest: LessonAddOrUpdateRequest) {
    this.possibleToCreateNewLessonValue = false;

    this.lessonService.saveDraft(lessonAddOrUpdateRequest).pipe(take(1))
      .subscribe(response => {
        this.handleLessonUpdate(response.data);
        this.possibleToCreateNewLessonValue = true;
      });
  }

  save() {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '450px',
      data: {
        title: "Ви впевнені що хочете зберегти зміни?"
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        const lessonAddOrUpdateRequest = this.prepareLessonToUpdateDto();
        this.possibleToCreateNewLessonValue = false;

        this.lessonService.createOrUpdateLesson(lessonAddOrUpdateRequest).pipe(take(1)).subscribe(
          response => {
            this.handleLessonUpdate(response.data);
            this.possibleToCreateNewLessonValue = true;
            this.lessonSaved.emit();
          },
          error => this.dialog.open(AcceptDialogComponent, {
            width: '450px',
            data: {
              text: error.error.errorData.join("\n")
            }
          })
        );
      }
    });
  }

  cancel() {
    if (this.lessonResponse.originalLessonId) {
      const dialogRef = this.dialog.open(ConfirmDialogComponent, {
        width: '450px',
        data: {
          title: "Ви впевнені що хочете відмінити внесені зміни?"
        }
      });

      dialogRef.afterClosed().subscribe(result => {
        if (result) {
          this.lessonService.deleteLesson(this.lessonResponse.id).pipe(take(1), switchMap(_ => {
            if (this.initialLessonId)
              return this.lessonService.getLessonForUpdateById(this.initialLessonId)
            return of(null)
          }
          )).subscribe(response => {
            if (response)
              this.handleLessonUpdate(response.data);
          });
        }
      });
    }
  }

  private prepareLessonToUpdateDto(): LessonAddOrUpdateRequest {
    let lessonAddOrUpdateRequest: LessonAddOrUpdateRequest;

    if (this.lessonResponse) {
      let video: VideoAddOrUpdateRequest;

      if (this.lessonResponse.video) {
        video = {
          attachment: this.lessonResponse.video.attachment,
          subtitles: this.lessonResponse.video.subtitles.map(s => s.language)
        }
      }

      lessonAddOrUpdateRequest = {
        editedLessonId: this.lessonResponse.editedLessonId,
        id: this.lessonResponse.id,
        paragraphId: this.lessonResponse.paragraphId,
        primaryLanguage: this.lessonForm.controls['language'].value ?? this.lessonResponse.primaryLanguage,
        video: video,
        title: this.lessonForm.controls['title'].value
      }
    }
    else {
      lessonAddOrUpdateRequest = {
        editedLessonId: null,
        paragraphId: this.currentLessonEditing.paragraphId,
        primaryLanguage: this.lessonForm.controls['language'].value,
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
    this.lessonForm.controls['title'].setValue(lessonResponse.title);

    this.lessonForm.controls['language'].setValue(lessonResponse.primaryLanguage);

    this.lessonService.$lessonAddedOrUpdated.next(this.lessonResponse);
    this.lessonForm.markAsUntouched();

    this.initialLessonId = lessonResponse.isDraft ? lessonResponse.originalLessonId : lessonResponse.id;

    this.lessonUpdatedTitleResponse = { originalLessonId: lessonResponse.originalLessonId, id: lessonResponse.id, title: lessonResponse.title }
    this.onUpdate.emit(this.lessonUpdatedTitleResponse);
  }

  handleImplicitLessonUpdate(currentLessonUpdatedResponse: CurrentLessonUpdatedResponse) {
    if (this.lessonResponse.id !== currentLessonUpdatedResponse.lessonId) {
      this.lessonResponse.originalLessonId = this.lessonResponse.id;
      this.lessonResponse.id = currentLessonUpdatedResponse.lessonId;
      this.lessonResponse.isDraft = true;
    }
  }

  updateSelectedLanguages(languages: Language[]) {
    const lessonAddOrUpdateRequest = this.prepareLessonToUpdateDto();
    lessonAddOrUpdateRequest.video.subtitles = languages;

    this.saveDraft(lessonAddOrUpdateRequest)
  }

  get subtitlesLanguages(): Language[] {
    return this.lessonResponse?.video?.subtitles.map(s => s.language) ?? []
  }
  get constantLanguages(): Language[] {
    return [this.lessonResponse?.primaryLanguage];
  }
}
