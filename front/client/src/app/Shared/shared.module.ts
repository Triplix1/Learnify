import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { MaterialModule } from '../Material/material.module';
import { RouterModule } from '@angular/router';
import { ToastrModule } from 'ngx-toastr';
import { NavbarComponent } from './components/navbar/navbar.component';
import { MatIconModule } from '@angular/material/icon';
import { TextInputComponent } from './components/text-input/text-input.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DropdownComponent } from './components/dropdown/dropdown.component';
import { ImageUploaderComponent } from './components/image-uploader/image-uploader.component';
import { ImageCropperComponent } from './components/image-cropper/image-cropper.component';


@NgModule({
  declarations: [
    NavbarComponent,
    HeaderComponent,
    FooterComponent,
    TextInputComponent,
    DropdownComponent,
    ImageUploaderComponent,
    ImageCropperComponent,
  ],
  imports: [
    CommonModule,
    BrowserModule,
    HttpClientModule,
    MaterialModule,
    RouterModule,
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right'
    }),
    MatIconModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  exports: [
    CommonModule,
    BrowserModule,
    HttpClientModule,
    MaterialModule,
    NavbarComponent,
    HeaderComponent,
    FooterComponent,
    RouterModule,
    ToastrModule,
    MatIconModule,
    TextInputComponent,
    FormsModule,
    ReactiveFormsModule,
    DropdownComponent,
    ImageUploaderComponent,
  ]
})
export class SharedModule { }
