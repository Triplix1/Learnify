import { Component, EventEmitter, Input, Output } from '@angular/core';
import { getMediaType } from 'src/app/Core/helpers/mediaHelper';
import { MediaService } from 'src/app/Core/services/media.service';
import { AttachmentResponse } from 'src/app/Models/Attachment/AttachmentResponse';
import { MediaType } from 'src/app/Models/enums/MediaType';

@Component({
  selector: 'app-media',
  templateUrl: './media.component.html',
  styleUrls: ['./media.component.scss']
})
export class MediaComponent {
  @Input({ required: true }) fileId: number;
  @Input({ required: true }) contentType: string;
  @Input() showClose: boolean = false;
  @Output() closed: EventEmitter<null> = new EventEmitter();
  MediaType = MediaType;

  get mediaType() {
    return getMediaType(this.contentType);
  }

  onClosed() {
    this.closed.emit();
  }
}
