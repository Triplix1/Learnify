import { Component, Input } from '@angular/core';
import { MediaService } from 'src/app/Core/services/media.service';
import { AttachmentResponse } from 'src/app/Models/Attachment/AttachmentResponse';
import { MediaType } from 'src/app/Models/enums/MediaType';

@Component({
  selector: 'app-media',
  templateUrl: './media.component.html',
  styleUrls: ['./media.component.scss']
})
export class MediaComponent {
  @Input({ required: true }) media: AttachmentResponse;
  MediaType = MediaType;

  constructor(private readonly mediaService: MediaService) { }

  get mediaType() {
    return this.mediaService.getMediaType(this.media?.contentType);
  }
}
