import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { take, takeUntil } from 'rxjs';
import { LessonService } from 'src/app/Core/services/lesson.service';
import { ParagraphService } from 'src/app/Core/services/paragraph.service';
import { BaseComponent } from 'src/app/Models/BaseComponent';
import { LessonStepAddOrUpdateRequest } from 'src/app/Models/Course/Lesson/LessonStepAddOrUpdateRequest';
import { LessonTitleResponse } from 'src/app/Models/Course/Lesson/LessonTitleResponse';
import { ParagraphCreateRequest } from 'src/app/Models/Course/Paragraph/ParagraphCreateRequest';
import { ParagraphResponse } from 'src/app/Models/Course/Paragraph/ParagraphResponse';
import { ParagraphUpdateRequest } from 'src/app/Models/Course/Paragraph/ParagraphUpdateRequest';
import { PublishParagraphRequest } from 'src/app/Models/Course/Paragraph/PublishParagraphRequest';
import { ParagraphUpdated } from 'src/app/Models/ParagraphUpdated';

@Component({
  selector: 'app-create-paragraph',
  templateUrl: './create-paragraph.component.html',
  styleUrls: ['./create-paragraph.component.scss']
})
export class CreateParagraphComponent extends BaseComponent {
  @Input() paragraphResponse: ParagraphResponse | null = null;
  @Input({ required: true }) index: number;
  @Input({ required: true }) courseId: number = null;
  @Input({ required: true }) possibleToCreateNewLesson: boolean = true;
  @Output() onUpdate: EventEmitter<ParagraphUpdated> = new EventEmitter<ParagraphUpdated>(null);
  @Output() onDelete: EventEmitter<number> = new EventEmitter<number>(null);
  @Output() onLessonAddOrUpdateRequest: EventEmitter<LessonStepAddOrUpdateRequest> = new EventEmitter<LessonStepAddOrUpdateRequest>(null);
  @Output() onLessonDelete: EventEmitter<LessonTitleResponse> = new EventEmitter<LessonTitleResponse>(null);

  editingMode: boolean = false;
  paragraphForm: FormGroup = new FormGroup({});
  lessons: LessonTitleResponse[] = [];
  private _lessonsLoaded: boolean = false;
  errorWhileLoadingLessons: boolean = false;

  constructor(private readonly fb: FormBuilder, private readonly paragraphService: ParagraphService, private readonly lessonService: LessonService) {
    super();
  }

  get lessonTitlesList() {
    return this.lessons.filter(l => l !== null);
  }

  ngOnInit(): void {
    this.initializeForm();

    if (!this.paragraphResponse) {
      this.editingMode = true;
    }

    this.lessonService.$lessonAddedOrUpdated.pipe().subscribe(
      updatedLesson => {
        if (this.lessons) {
          const lessonIndex = this.lessons.findIndex(l => l === null);

          if (lessonIndex !== -1) {
            this.lessons[lessonIndex] = updatedLesson;
          }
        }
      });
  }


  initializeForm() {
    this.paragraphForm = this.fb.group({
      name: [this.paragraphResponse?.name ?? '', [Validators.required]],
    });
  }

  loadLessons() {
    if (this._lessonsLoaded)
      return;

    if (this.paragraphResponse === null) {
      this.lessons = [];
      return;
    }

    this.lessonService.getLessonTitlesForParagraph(this.paragraphResponse.id, true).pipe(take(1)).subscribe(lessons => {
      this.lessons = lessons.data;
      this._lessonsLoaded = true;
    })
  }

  save(event: MouseEvent) {
    event.stopPropagation();
    if (this.paragraphResponse) {
      const paragraphUpdateRequest: ParagraphUpdateRequest = {
        id: this.paragraphResponse.id,
        name: this.paragraphForm.controls['name'].value
      }

      this.paragraphService.updateParagraph(paragraphUpdateRequest).pipe(take(1))
        .subscribe(
          response => this.handleUpdate(response.data)
        );
    }
    else {
      const paragraphCreateRequest: ParagraphCreateRequest = {
        courseId: this.courseId,
        name: this.paragraphForm.controls['name'].value
      }

      this.paragraphService.createParagraph(paragraphCreateRequest).pipe(take(1))
        .subscribe(
          response => this.handleUpdate(response.data)
        );
    }
  }

  cancel(event: MouseEvent) {
    event.stopPropagation();
    this.initializeForm();

    if (this.paragraphResponse)
      this.editingMode = false;
  }

  addLesson() {
    if (this.paragraphResponse) {
      var lessonStepAddOrUpdateRequest: LessonStepAddOrUpdateRequest = {
        paragraphId: this.paragraphResponse.id
      }

      this.lessons.push(null);

      this.onLessonAddOrUpdateRequest.emit(lessonStepAddOrUpdateRequest);
    }
  }

  editingToggle() {
    this.editingMode = true;
  }

  publish() {
    const publishParagraphRequest: PublishParagraphRequest = {
      paragraphId: this.paragraphResponse.id,
      publish: !this.paragraphResponse.isPublished
    };

    this.paragraphService.publishParagraph(publishParagraphRequest).pipe(take(1)).subscribe(r => this.handleUpdate(r.data));
  }

  delete() {
    this.paragraphService.deleteParagraph(this.paragraphResponse.id).pipe(take(1)).subscribe(r => {
      this.onDelete.emit(this.index);
    });
  }

  lessonDeleted(id: string) {
    let lesson = this.lessons.find(l => l.id === id);
    if (lesson) {
      this.lessons = this.lessons.filter(l => l.id !== id);
      this.onLessonDelete.emit(lesson);
    }
  }

  editLesson(id: string) {
    let lesson = this.lessons.find(l => l.id === id);
    if (lesson) {
      var lessonStepAddOrUpdateRequest: LessonStepAddOrUpdateRequest = {
        id: lesson.id,
        paragraphId: this.paragraphResponse.id,
      }

      this.onLessonAddOrUpdateRequest.emit(lessonStepAddOrUpdateRequest);
    }
  }

  private handleUpdate(response: ParagraphResponse) {
    this.paragraphResponse = response;
    this.editingMode = false;
    this.onUpdate.emit({ paragraph: response, index: this.index });
  }
}
