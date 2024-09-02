import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { take } from 'rxjs';
import { LessonService } from 'src/app/Core/services/lesson.service';
import { ParagraphService } from 'src/app/Core/services/paragraph.service';
import { LessonTitleResponse } from 'src/app/Models/Course/Lesson/LessonTitleResponse';
import { ParagraphCreateRequest } from 'src/app/Models/Course/Paragraph/ParagraphCreateRequest';
import { ParagraphResponse } from 'src/app/Models/Course/Paragraph/ParagraphResponse';
import { ParagraphUpdateRequest } from 'src/app/Models/Course/Paragraph/ParagraphUpdateRequest';
import { ParagraphUpdated } from 'src/app/Models/ParagraphUpdated';

@Component({
  selector: 'app-create-paragraph',
  templateUrl: './create-paragraph.component.html',
  styleUrls: ['./create-paragraph.component.scss']
})
export class CreateParagraphComponent {
  @Input() paragraphResponse: ParagraphResponse | null = null;
  @Input({ required: true }) index: number;
  @Input({ required: true }) courseId: number = null;
  @Output() onUpdate: EventEmitter<ParagraphUpdated> = new EventEmitter<ParagraphUpdated>(null);

  editingMode: boolean = false;
  paragraphForm: FormGroup = new FormGroup({});
  lessons: LessonTitleResponse[] = [];
  private _lessonsLoaded: boolean = false;
  errorWhileLoadingLessons: boolean = false;

  constructor(private readonly fb: FormBuilder, private readonly paragraphService: ParagraphService, private readonly lessonService: LessonService) { }

  ngOnInit(): void {
    this.initializeForm();

    if (!this.paragraphResponse) {
      this.editingMode = true;
    }
  }

  initializeForm() {
    this.paragraphForm = this.fb.group({
      name: [this.paragraphResponse?.name ?? '', [Validators.required]],
    });
  }

  loadLessons() {
    this.lessonService.getLessonTitlesForParagraph(this.paragraphResponse.id).pipe(take(1))
      .subscribe(
        lessonTitles => {
          this.lessons = lessonTitles.data;
          this._lessonsLoaded = true;
        },
        _ => this.errorWhileLoadingLessons = true);
  }

  expanded(value: boolean) {
    if (value && this.lessons !== null || this._lessonsLoaded)
      return;

    if (this.paragraphResponse === null) {
      this.lessons = [];
      return;
    }

    this.loadLessons();
  }

  save() {
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

  cancel() {
    this.initializeForm();
  }

  private handleUpdate(response: ParagraphResponse) {
    this.paragraphResponse = response;
    this.onUpdate.emit({ paragraph: response, index: this.index });
  }
}
