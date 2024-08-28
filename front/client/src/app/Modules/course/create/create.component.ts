import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Language } from 'src/app/Models/enums/Language';
import { SelectorOption } from 'src/app/Models/SelectorOption';

@Component({
  selector: 'app-create',
  templateUrl: './create.component.html',
  styleUrls: ['./create.component.scss']
})
export class CreateComponent {
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
      price: ['', [Validators.required]],
      language: [Language.English, [Validators.required]],
    });
  }
}
