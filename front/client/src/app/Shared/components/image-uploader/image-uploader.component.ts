import { Component, EventEmitter, Output } from '@angular/core';
import { Observable } from 'rxjs';
import { ImageCropperComponent } from '../image-cropper/image-cropper.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-image-uploader',
  templateUrl: './image-uploader.component.html',
  styleUrls: ['./image-uploader.component.scss']
})
export class ImageUploaderComponent {
  file: string = '';
  @Output() imageChanged: EventEmitter<File> = new EventEmitter<File>();

  constructor(private dialog: MatDialog) { }

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
              this.imageChanged.emit(files[0]);
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
