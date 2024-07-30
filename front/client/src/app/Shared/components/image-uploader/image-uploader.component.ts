import { Component, EventEmitter, Output } from '@angular/core';
import { Observable, take } from 'rxjs';
import { ImageCropperComponent } from '../image-cropper/image-cropper.component';
import { MatDialog } from '@angular/material/dialog';
import { ImageService } from 'src/app/Core/services/image.service';
import { v4 as uuidv4 } from 'uuid';

@Component({
  selector: 'app-image-uploader',
  templateUrl: './image-uploader.component.html',
  styleUrls: ['./image-uploader.component.scss']
})
export class ImageUploaderComponent {
  file: string = '';
  @Output() imageChanged: EventEmitter<File> = new EventEmitter<File>();

  constructor(private readonly dialog: MatDialog, private readonly imageService: ImageService) { }

  onFileChange(event: any) {
    const files = event.target.files as FileList;

    if (files.length > 0) {
      const _file = URL.createObjectURL(files[0]);
      this.resetInput();
      this.openAvatarEditor(_file)
        .subscribe(
          (result) => {
            if (result) {
              this.file = result;
              this.imageService.getImage(result).pipe(take(1)).subscribe(blob => {
                const file = new File([blob], uuidv4() + this.imageService.getExtensionFromMimeType(blob.type), { type: blob.type });
                this.imageChanged.emit(file);
              })
            }
          }
        )
    }
  }

  openAvatarEditor(image: string): Observable<any> {
    const dialogRef = this.dialog.open(ImageCropperComponent, {
      maxWidth: '70vw',
      maxHeight: '90vh',
      data: image,
    });

    return dialogRef.afterClosed();
  }

  resetInput() {
    const input = document.getElementById('avatar-input-file') as HTMLInputElement;
    if (input) {
      input.value = "";
    }
  }
}
