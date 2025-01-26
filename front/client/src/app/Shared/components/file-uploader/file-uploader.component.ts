import { HttpEventType, HttpResponse } from '@angular/common/http';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { filter, map, Observable, switchMap, take, takeUntil, tap } from 'rxjs';
import { MediaService } from 'src/app/Core/services/media.service';
import { ApiResponseWithData } from 'src/app/Models/ApiResponse';
import { AttachmentResponse } from 'src/app/Models/Attachment/AttachmentResponse';
import { BaseComponent } from 'src/app/Models/BaseComponent';
import { PrivateFileBlobCreateRequest } from 'src/app/Models/File/PrivateFileBlobCreateRequest';
import { PrivateFileDataResponse } from 'src/app/Models/File/PrivateFileDataResponse';

@Component({
  selector: 'app-file-uploader',
  templateUrl: './file-uploader.component.html',
  styleUrls: ['./file-uploader.component.scss']
})
export class FileUploaderComponent extends BaseComponent {
  @Input() accept = "*"
  @Input({ required: true }) uploadMethod: (file: PrivateFileBlobCreateRequest) => Observable<Object>
  @Output() fileUploaded: EventEmitter<Observable<any>> = new EventEmitter<Observable<any>>(null);
  uploadProgress: number;

  constructor() {
    super();
  }

  handleFileInput(imageInputFiles: FileList) {

    for (let i = 0; i < imageInputFiles.length; i++) {
      if (imageInputFiles[i]) {

        const fileCreateRequest: PrivateFileBlobCreateRequest = {
          content: imageInputFiles[i],
          contentType: imageInputFiles[i].type,
          courseId: null
        };

        var fileUploadObservable = this.uploadMethod(fileCreateRequest).pipe(
          tap((event: any) => {
            if (event.type === HttpEventType.UploadProgress && event.total) {
              this.uploadProgress = Math.round((100 * event.loaded) / event.total); // Update progress bar
            }
          }),
          filter(event => event.type === HttpEventType.Response),
          map(response => response.body),
          takeUntil(this.destroySubject)
        );

        this.fileUploaded.emit(fileUploadObservable);
      }
    }
  }
}
