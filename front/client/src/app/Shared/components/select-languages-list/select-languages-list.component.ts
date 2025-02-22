import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { ButtonsList } from 'src/app/Models/ButtonsList';
import { Language } from 'src/app/Models/enums/Language';

@Component({
  selector: 'app-select-languages-list',
  templateUrl: './select-languages-list.component.html',
  styleUrls: ['./select-languages-list.component.scss']
})

export class SelectLanguagesListComponent implements OnChanges {
  @Input() languagesList: Language[] = [];
  @Input() constantlySelectedLanguages: Language[] = [];
  @Output() languagesListChange: EventEmitter<Language[]> = new EventEmitter<Language[]>();

  ngOnChanges(changes: SimpleChanges): void {
    const shouldUpdateList = changes["constantlySelectedLanguages"].currentValue != changes["constantlySelectedLanguages"].previousValue ||
      changes["languagesList"].currentValue != changes["languagesList"].previousValue;

    if (shouldUpdateList) {
      this.languagesList = Array.from(new Set([...this.languagesList, ...this.constantlySelectedLanguages]));
    }
  }

  selectorOptions: ButtonsList[] = Object.keys(Language)
    .filter(key => isNaN(Number(key)))
    .map(key => {
      return { label: key, value: Language[key as keyof typeof Language] };
    });

  updateListLaguages(language: Language) {
    if (this.languagesList.includes(language)) {
      this.languagesList = this.languagesList.filter(l => l != language);
    }
    else {
      this.languagesList.push(language);
    }

    this.languagesListChange.emit(this.languagesList);
  }
}