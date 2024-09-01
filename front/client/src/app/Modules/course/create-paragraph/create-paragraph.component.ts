import { Component, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LessonResponse } from 'src/app/Models/Course/Lesson/LessonResponse';
import { LessonUpdateResponse } from 'src/app/Models/Course/Lesson/LessonUpdateResponse';
import { ParagraphResponse } from 'src/app/Models/Course/Paragraph/ParagraphResponse';

@Component({
  selector: 'app-create-paragraph',
  templateUrl: './create-paragraph.component.html',
  styleUrls: ['./create-paragraph.component.scss']
})
export class CreateParagraphComponent {
  @Input() paragraphResponse: ParagraphResponse | null = null;
  paragraphForm: FormGroup = new FormGroup({});
  lessons: 

  constructor(private readonly fb: FormBuilder) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.paragraphForm = this.fb.group({
      name: [this.paragraphResponse?.name ?? '', [Validators.required]],
    });
  }

  get lesson 
}
