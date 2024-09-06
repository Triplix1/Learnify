import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { switchMap, take } from 'rxjs';
import { LessonService } from 'src/app/Core/services/lesson.service';
import { MediaService } from 'src/app/Core/services/media.service';
import { LessonAddOrUpdateRequest } from 'src/app/Models/Course/Lesson/LessonAddOrUpdateRequest';
import { LessonTitleResponse } from 'src/app/Models/Course/Lesson/LessonTitleResponse';
import { LessonUpdateResponse } from 'src/app/Models/Course/Lesson/LessonUpdateResponse';
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
    if (!this.lessonId) {
      this.lessonService.createOrUpdateLesson({ id: null, editedLessonId: null, paragraphId: this.paragraphId, quizzes: [], title: null, video: null }).pipe(take(1))
        .subscribe(response => {
          this.lessonResponse = this.lessonResponse;
          this.lessonId = this.lessonResponse.id;
        })
    }
    else{
      this.lessonService.getLessonForUpdateById(this.lessonId).pipe(take(1))
      .subscribe(response =>{
        this.lessonResponse = response.data;
      });
    }
  }

  initializeForm() {
    this.lessonForm = this.fb.group({
      title: [this.lessonResponse?.title ?? '', [Validators.required]],
    });
  }

  handleFileInput(imageInput: File | null) {
    if (imageInput) {
      var fileCreateRequest: PrivateFileBlobCreateRequest = {
        content: imageInput,
        contentType: imageInput.type,
        courseId: this.courseId
      };

      this.mediaService.create(fileCreateRequest).pipe(
        switchMap(response => {
          response.
        })
      )
        .subscribe(response => {

        })
    }
  }
}
