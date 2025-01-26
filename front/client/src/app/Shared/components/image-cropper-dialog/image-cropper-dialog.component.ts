import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import Cropper from 'cropperjs';
import { CropperParams } from 'src/app/Models/CropperParams';

@Component({
  selector: 'app-image-cropper-dialog',
  templateUrl: './image-cropper-dialog.component.html',
  styleUrls: ['./image-cropper-dialog.component.scss']
})
export class ImageCropperDialogComponent implements OnInit {
  cropper!: Cropper;
  sanitizedUrl: SafeUrl;
  constructor(
    public dialogRef: MatDialogRef<ImageCropperDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { image: string, cropperParams: CropperParams },
    private sanitizer: DomSanitizer
  ) { }

  ngOnInit(): void {
    this.sanitizedUrl = this.sanitizer.bypassSecurityTrustUrl(this.data.image);
    console.log("Hello from cropper!")
  }

  ngAfterViewInit() {
    this.initCropper();
  }

  initCropper() {
    const image = document.getElementById('image') as HTMLImageElement;
    this.cropper = new Cropper(image, {
      aspectRatio: this.data.cropperParams.isConstantAspectRatio ? this.data.cropperParams.minWidth / this.data.cropperParams.minHeight : NaN,
      viewMode: 1,
      guides: false,
      minContainerWidth: this.data.cropperParams.minWidth > 400 ? this.data.cropperParams.minWidth : 400,  // Set minimum container width
      minContainerHeight: this.data.cropperParams.minHeight > 400 ? this.data.cropperParams.minHeight : 400, // Set minimum container height
      minCropBoxWidth: this.data.cropperParams.minWidth,    // Set minimum crop box width
      minCropBoxHeight: this.data.cropperParams.minHeight    // Set minimum crop box height
    });
  }

  getRoundedCanvas(sourceCanvas: any) {
    var canvas = document.createElement('canvas');
    var context: any = canvas.getContext('2d');
    var width = sourceCanvas.width;
    var height = sourceCanvas.height;

    canvas.width = width;
    canvas.height = height;
    context.imageSmoothingEnabled = true;
    context.drawImage(sourceCanvas, 0, 0, width, height);
    context.globalCompositeOperation = 'destination-in';
    context.beginPath();
    context.arc(
      width / 2,
      height / 2,
      Math.min(width, height) / 2,
      0,
      2 * Math.PI,
      true
    );
    context.fill();
    return canvas;
  }

  //get the cropped image and closes the dialog 
  //returning an url or null if no image

  crop() {
    let croppedCanvas = this.cropper.getCroppedCanvas();

    if (this.data.cropperParams.isCircle)
      croppedCanvas = this.getRoundedCanvas(croppedCanvas);

    let roundedImage = document.createElement('img');

    if (roundedImage) {
      this.dialogRef.close(croppedCanvas.toDataURL());
    } else {
      return this.dialogRef.close(null);
    }
  }

  // resets the cropper
  reset() {
    this.cropper.clear();
    this.cropper.crop();
  }
}
