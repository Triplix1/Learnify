import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { switchMap, take } from 'rxjs';
import { LessonService } from 'src/app/Core/services/lesson.service';
import { MediaService } from 'src/app/Core/services/media.service';
import { AttachmentResponse } from 'src/app/Models/Attachment/AttachmentResponse';
import { LessonAddOrUpdateRequest } from 'src/app/Models/Course/Lesson/LessonAddOrUpdateRequest';
import { LessonResponse } from 'src/app/Models/Course/Lesson/LessonResponse';
import { LessonTitleResponse } from 'src/app/Models/Course/Lesson/LessonTitleResponse';
import { LessonUpdateResponse } from 'src/app/Models/Course/Lesson/LessonUpdateResponse';
import { Language } from 'src/app/Models/enums/Language';
import { PrivateFileBlobCreateRequest } from 'src/app/Models/File/PrivateFileBlobCreateRequest';

@Component({
  selector: 'app-create-lesson',
  templateUrl: './create-lesson.component.html',
  styleUrls: ['./create-lesson.component.scss']
})
export class CreateLessonComponent implements OnInit {
  @Input() lessonId: string;
  @Input({ required: true }) paragraphId: number;
  @Input({ required: true }) courseId: number;
  @Output() onUpdate: EventEmitter<LessonTitleResponse> = new EventEmitter<LessonTitleResponse>(null);

  lessonResponse: LessonUpdateResponse;
  lessonAddOrUpdateRequest: LessonAddOrUpdateRequest;
  lessonForm: FormGroup = new FormGroup({});

  constructor(private readonly lessonService: LessonService, private readonly mediaService: MediaService, private readonly fb: FormBuilder) { }

  ngOnInit(): void {
    this.initializeForm();

    if (!this.lessonId) {
      this.lessonService.saveDraft({ id: null, editedLessonId: null, paragraphId: this.paragraphId, quizzes: [], title: null, video: null }).pipe(take(1))
        .subscribe(response => {
          this.lessonResponse = this.lessonResponse;
          this.lessonId = this.lessonResponse.id;
        })
    }
    else {
      this.lessonService.getLessonForUpdateById(this.lessonId).pipe(take(1))
        .subscribe(response => {
          this.handleLessonUpdate(response.data);
        });
    }
  }

  initializeForm() {
    this.lessonForm = this.fb.group({
      title: [this.lessonResponse?.title ?? '', [Validators.required]],
      language: this.lessonResponse?.video.primaryLanguage ? Language[this.lessonResponse.video.primaryLanguage as keyof typeof Language] : Language.English,
    });
  }

  handleFileInput(imageInput: File | null, quizIndex: number | null) {
    if (imageInput) {
      var fileCreateRequest: PrivateFileBlobCreateRequest = {
        content: imageInput,
        contentType: imageInput.type,
        courseId: this.courseId
      };

      this.mediaService.create(fileCreateRequest).pipe(
        take(1),
        switchMap(response => {
          const attachment: AttachmentResponse = {
            contentType: response.data.contentType,
            fileId: response.data.id
          }

          this.prepareLessonToUpdateDto();

          if (quizIndex === null)
            this.lessonAddOrUpdateRequest.video.attachment = attachment;
          else
            this.lessonAddOrUpdateRequest.quizzes[quizIndex].media = attachment;

          return this.lessonService.saveDraft(this.lessonAddOrUpdateRequest);
        })
      )
        .subscribe(response => {
          this.handleLessonUpdate(response.data);
        })
    }
  }

  private prepareLessonToUpdateDto() {
    this.lessonAddOrUpdateRequest.editedLessonId = this.lessonResponse.editedLessonId;
    this.lessonAddOrUpdateRequest.id = this.lessonResponse.id;
    this.lessonAddOrUpdateRequest.quizzes = this.lessonResponse.quizzes;
    this.lessonAddOrUpdateRequest.video.attachment = this.lessonResponse.video.attachment;
    this.lessonAddOrUpdateRequest.video.primaryLanguage = this.lessonResponse.video.primaryLanguage;

    this.lessonAddOrUpdateRequest.video.subtitles = this.lessonResponse.video.subtitles.map(s => Language[s.language as keyof typeof Language]);

    this.lessonAddOrUpdateRequest.title = this.lessonForm.controls['title'].value;
  }

  private handleLessonUpdate(lessonResponse: LessonUpdateResponse) {
    this.lessonId = lessonResponse.id
    this.lessonResponse = lessonResponse;
    this.lessonForm.controls['title'].setValue(lessonResponse.title);
    this.lessonForm.controls['language'].setValue(Language[lessonResponse.video.primaryLanguage as keyof typeof Language]);
  }
}
