import { Component, Input, OnInit } from '@angular/core';
import { takeUntil } from 'rxjs';
import { MediaService } from 'src/app/Core/services/media.service';
import { BaseComponent } from 'src/app/Models/BaseComponent';

@Component({
  selector: 'app-video-player',
  templateUrl: './video-player.component.html',
  styleUrls: ['./video-player.component.scss']
})
export class VideoPlayerComponent extends BaseComponent {
  @Input({ required: true }) fileId: number;
  @Input() classList: string;
  loading: boolean = true;
  error: string = '';
  videoUrl: string;

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
}
