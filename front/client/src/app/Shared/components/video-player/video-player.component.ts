import { Component, Input } from '@angular/core';
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
  videoUrl?: string;
  loading: boolean = true;
  error: string = '';
  constructor(private mediaService: MediaService) {
    super();
  }

  public ngOnInit() {
    this.getFilmUrl();
  }

  public ngOnChanges() {
    this.getFilmUrl();
  }

  onVideoLoad(): void {
    this.loading = false;
  }

  onVideoError(event: ErrorEvent): void {
    this.error = 'Error loading video: ' + event.message;
  }

  private getFilmUrl() {
    if (!this.fileId) {
      return;
    }

    this.mediaService.getFileUrl(this.fileId).pipe(takeUntil(this.destroySubject)).subscribe(url => this.videoUrl = url);
  }
}
