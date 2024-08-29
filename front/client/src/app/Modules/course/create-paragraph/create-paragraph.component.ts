import { Component, Input } from '@angular/core';
import { ParagraphResponse } from 'src/app/Models/Course/Paragraph/ParagraphResponse';

@Component({
  selector: 'app-create-paragraph',
  templateUrl: './create-paragraph.component.html',
  styleUrls: ['./create-paragraph.component.scss']
})
export class CreateParagraphComponent {
  @Input() paragraphResponse: ParagraphResponse | null = null;
}
