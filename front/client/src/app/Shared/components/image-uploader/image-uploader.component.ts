import { Component, EventEmitter, Input, Output } from '@angular/core';
import { map, Observable, of, switchMap, take, tap } from 'rxjs';
import { ImageCropperDialogComponent } from '../image-cropper-dialog/image-cropper-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { ImageService } from 'src/app/Core/services/image.service';
import { v4 as uuidv4 } from 'uuid';
import { PrivateFileBlobCreateRequest } from 'src/app/Models/File/PrivateFileBlobCreateRequest';
import { CropperParams } from 'src/app/Models/CropperParams';

@Component({
  selector: 'app-image-uploader',
  templateUrl: './image-uploader.component.html',
  styleUrls: ['./image-uploader.component.scss']
})
export class ImageUploaderComponent {
  @Input() cropperParams: CropperParams;
  @Output() imageChanged: EventEmitter<File> = new EventEmitter<File>();
  @Input({ required: true }) uploadMethod: (file: PrivateFileBlobCreateRequest) => Observable<Object>
  @Output() fileUploaded: EventEmitter<Observable<any>> = new EventEmitter<Observable<any>>(null);


  constructor(private readonly dialog: MatDialog, private readonly imageService: ImageService) { }

  onFileChange = (privateFileBlobCreateRequest: PrivateFileBlobCreateRequest): Observable<any> => {
    if (privateFileBlobCreateRequest && this.cropperParams) {
      const _file = URL.createObjectURL(privateFileBlobCreateRequest.content);

      return this.openAvatarEditor(_file).pipe(
        switchMap(result => {
          if (result) {
            return this.imageService.getImage(result).pipe(
              take(1),
              map(blob => {
                // Update the privateFileBlobCreateRequest content
                privateFileBlobCreateRequest.content = new File(
                  [blob],
                  `${uuidv4()}.${this.imageService.getExtensionFromMimeType(blob.type)}`,
                  { type: blob.type }
                );
                this.imageChanged.emit(privateFileBlobCreateRequest.content);
                return privateFileBlobCreateRequest; // Return the updated request
              })
            );
          }
          return of(null); // If no result, return the original request
        }),
        switchMap(updatedRequest => {
          // Now call the uploadMethod with the updated request
          return this.uploadMethod(updatedRequest);
        })
      );
    }

    // If cropping is not needed, directly call the uploadMethod
    return this.uploadMethod(privateFileBlobCreateRequest);
  };


  onFileUploaded(event: Observable<any>) {
    this.fileUploaded.emit(event);
  }

  openAvatarEditor(image: string): Observable<any> {
    const dialogRef = this.dialog.open(ImageCropperDialogComponent, {
      maxWidth: '70vw',
      maxHeight: '90vh',
      data: { image: image, cropperParams: this.cropperParams },
    });

    return dialogRef.afterClosed();
  }
}
