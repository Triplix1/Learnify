import { Pipe, PipeTransform } from '@angular/core';
import { AttachmentResponse } from 'src/app/Models/Attachment/AttachmentResponse';
import { MediaService } from '../../Core/services/media.service';
import { Observable } from 'rxjs';

@Pipe({
  name: 'media'
})
export class MediaPipe implements PipeTransform {

  constructor(private readonly mediaService: MediaService) { }

  transform(fileId: number): Observable<string> {
    return this.mediaService.getFileUrl(fileId);
  }

}
