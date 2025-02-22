import { Component, ElementRef, Input, ViewChild } from '@angular/core';
import { MediaService } from 'src/app/Core/services/media.service';
import { BaseComponent } from 'src/app/Models/BaseComponent';
import { SubtitlesResponse } from 'src/app/Models/Course/Lesson/Subtitle/SubtitlesResponse';

@Component({
  selector: 'app-video-player',
  templateUrl: './video-player.component.html',
  styleUrls: ['./video-player.component.scss']
})
export class VideoPlayerComponent extends BaseComponent {
  @Input({ required: true }) fileId: number;
  @Input() classList: string;
  @Input() availableSubtitles: SubtitlesResponse[] = [];
  loading: boolean = true;
  error: string = '';
  videoUrl: string;
  @ViewChild('videoPlayer') videoPlayer!: ElementRef<HTMLVideoElement>;

  selectedSubtitleId: string = ''; // Holds selected subtitle Id

  constructor(private readonly mediaService: MediaService) {
    super();
  }

  // ngOnInit(): void {
  //   this.mediaService.getVideoUrl(this.fileId)
  //     .subscribe(resp => {
  //       this.videoUrl = resp.data.url;
  //     });
  // }

  onVideoLoad(): void {
    this.loading = false;
  }

  onVideoError(event: ErrorEvent): void {
    this.error = 'Error loading video: ' + event.message;
  }

  ngAfterViewInit(): void {
    // Load default subtitles (optional)
  }

  loadSubtitles(fileId: number) {
    this.mediaService.getFileStream(fileId).subscribe(
      (subtitleBlob) => {
        const subtitleBlobUrl = URL.createObjectURL(subtitleBlob);

        // Remove existing track elements
        const video = this.videoPlayer.nativeElement;
        const existingTracks = video.getElementsByTagName('track');
        for (let i = existingTracks.length - 1; i >= 0; i--) {
          video.removeChild(existingTracks[i]);
        }

        // Create and append new track
        const track = document.createElement('track');
        track.kind = 'subtitles';
        track.label = this.availableSubtitles.find(sub => sub.subtitleFileId === fileId)?.language.toString() || 'Subtitle';
        track.srclang = this.availableSubtitles.find(sub => sub.subtitleFileId === fileId)?.language.toString() || 'en';
        track.src = subtitleBlobUrl;
        track.default = true;

        video.appendChild(track);
      },
      (error) => console.error('Error loading subtitles:', error)
    );
  }

  onSubtitleChange(event: Event) {
    const selectedFileId = +(event.target as HTMLSelectElement).value;
    this.loadSubtitles(selectedFileId);
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
