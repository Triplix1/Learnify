import { AfterViewInit, Component, ElementRef, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { MediaService } from 'src/app/Core/services/media.service';
import { SubtitlesService } from 'src/app/Core/services/subtitles.service';
import { BaseComponent } from 'src/app/Models/BaseComponent';
import { SubtitlesResponse } from 'src/app/Models/Course/Lesson/Subtitle/SubtitlesResponse';
import { SubtitlesSelected } from 'src/app/Models/Course/Lesson/Subtitle/SubtitlesSelected';

@Component({
  selector: 'app-video-player',
  templateUrl: './video-player.component.html',
  styleUrls: ['./video-player.component.scss']
})
export class VideoPlayerComponent extends BaseComponent implements AfterViewInit {
  @Input({ required: true }) fileId: number;
  @Input() classList: string;
  @Input() availableSubtitles: SubtitlesResponse[] = [];
  @Output() subtitleSelected: EventEmitter<SubtitlesSelected> = new EventEmitter<SubtitlesSelected>();// Send data to parent
  loading: boolean = true;
  error: string = '';
  videoUrl: string;
  @ViewChild('videoPlayer') videoPlayer!: ElementRef<HTMLVideoElement>;

  private activeSubtitleTrack: TextTrack | null = null;
  private currentSubtitleFileId: number;


  constructor(private readonly subtitlesService: SubtitlesService) {
    super();
  }

  ngAfterViewInit(): void {
    const video: HTMLVideoElement = this.videoPlayer.nativeElement;

    video.textTracks.addEventListener('change', (event: any) => {
      this.onCueChange();
    });
  }

  onCueChange() {
    const video: HTMLVideoElement = this.videoPlayer.nativeElement;

    this.activeSubtitleTrack = this.getActiveTrack(video.textTracks);
    if (!this.activeSubtitleTrack) return;

    const activeCues = this.activeSubtitleTrack.activeCues;
    if (activeCues) {
      this.subtitleSelected.emit({ id: this.currentSubtitleFileId, text: this.subtitlesService.extractSubtitles(this.activeSubtitleTrack) });
    }
  }

  getActiveTrack(textTracks: TextTrackList): TextTrack | null {
    for (let i = 0; i < textTracks.length; i++) {
      if (textTracks[i].mode === 'showing') {
        return textTracks[i];
      }
    }
    return null;
  }

  onVideoLoad(): void {
    this.loading = false;
  }

  onVideoError(event: ErrorEvent): void {
    this.error = 'Error loading video: ' + event.message;
  }

  getSrclang(language: string) {
    language = language.toLowerCase();

    const langMap: any = {
      "english": "en",
      "ukrainian": "uk",
      "spanish": "es",
      "italian": "it",
      "french": "fr"
    };

    return langMap[language] || null; // Returns null if the language is not found
  }
}
