import { Component, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Language } from 'src/app/Models/enums/Language';
import { SelectorOption } from 'src/app/Models/SelectorOption';

@Component({
  selector: 'app-create-course',
  templateUrl: './create-course.component.html',
  styleUrls: ['./create-course.component.scss']
})
export class CreateCourseComponent {
  @Input() courseId: number;
  courseForm: FormGroup = new FormGroup({});
  selectorOptions: SelectorOption[] = Object.keys(Language)
    .filter(key => isNaN(Number(key)))  // Filter out numeric keys
    .map(key => {
      return { label: key, value: Language[key as keyof typeof Language] };
    });

  constructor(private readonly fb: FormBuilder) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.courseForm = this.fb.group({
      name: ['', [Validators.required]],
      price: ['', [Validators.required, Validators.pattern('[1-9]')]],
      language: [Language.English, [Validators.required]],
    });
  }
}
