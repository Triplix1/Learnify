import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { BehaviorSubject, map, take, takeUntil } from 'rxjs';
import { convertBlobToText } from 'src/app/Core/helpers/fileHelper';
import { LessonService } from 'src/app/Core/services/lesson.service';
import { MediaService } from 'src/app/Core/services/media.service';
import { BaseComponent } from 'src/app/Models/BaseComponent';
import { LessonResponse } from 'src/app/Models/Course/Lesson/LessonResponse';
import { SubtitlesSelected } from 'src/app/Models/Course/Lesson/Subtitle/SubtitlesSelected';

@Component({
  selector: 'app-course-study-lesson',
  templateUrl: './course-study-lesson.component.html',
  styleUrls: ['./course-study-lesson.component.scss']
})
export class CourseStudyLessonComponent extends BaseComponent implements OnChanges {
  @Input({ required: true }) lessonId: string;
  lesson: LessonResponse;
  transcriptions: Map<number, string> = new Map<number, string>();
  currentTranscription: number;
  subtitleText: string = '';
  summaryText$ = new BehaviorSubject<string | null>(null);
  loadingSummary: boolean = false;

  constructor(private readonly lessonService: LessonService, private readonly mediaService: MediaService, private readonly spinnerService: NgxSpinnerService) {
    super();
  }

  ngOnChanges(changes: SimpleChanges): void {
    const currentLessonId = changes['lessonId'];
    if (currentLessonId.currentValue) {
      this.spinnerService.show("lessonLoading");
      this.lessonService.getLessonById(currentLessonId.currentValue).pipe(takeUntil(this.destroySubject), take(1)).subscribe(
        response => {
          this.spinnerService.hide("lessonLoading");
          this.lesson = response.data;
          this.loadTranscription(this.lesson.video.subtitles.find(x => x.language === this.lesson.primaryLanguage).transcriptionFileId);
          this.loadSummary();
        }
      )
    }
  }

  loadTranscription(transcriptionFileId: number): string {
    this.currentTranscription = transcriptionFileId;

    if (transcriptionFileId !== null && transcriptionFileId !== undefined) {
      if (this.transcriptions.has(transcriptionFileId)) {
        return this.transcriptions.get(transcriptionFileId);
      }

      this.mediaService.getFile(transcriptionFileId).pipe(response =>
        response.pipe(map(blob => convertBlobToText(blob)))
      ).subscribe(
        response => {
          response.then(c => this.transcriptions.set(transcriptionFileId, c))
        });
    }

    return '';
  }

  get transcription() {
    return this.transcriptions.get(this.currentTranscription);
  }

  handleSubtitlesExtracted(subtitles: SubtitlesSelected) {
    console.log(subtitles);
    this.subtitleText = subtitles.text;
  }

  // get summary() {
  //   if (this.lesson.video?.summaryFileId && !this.loadingSummary) {
  //     this.loadingSummary = true
  //     this.spinnerService.show('summaryLoading')
  //     this.mediaService.getFile(this.lesson.video?.summaryFileId).subscribe(
  //       async blob => {
  //         this.summaryText = await blob.text();
  //         this.spinnerService.hide('summaryLoading');
  //       }
  //     )
  //   }

  //   return this.summaryText;
  // }

  loadSummary() {
    if (this.lesson.video?.summaryFileId) {
      this.loadingSummary = true;
      this.spinnerService.show('summaryLoading');

      this.mediaService.getFile(this.lesson.video?.summaryFileId).subscribe(
        async blob => {
          const text = await blob.text();
          this.summaryText$.next(text);
          this.loadingSummary = false;
          this.spinnerService.hide('summaryLoading');
        },
        error => {
          this.loadingSummary = false;
          this.spinnerService.hide('summaryLoading');
        }
      );
    }
  }
}
