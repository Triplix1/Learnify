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
import { SelectorComponent } from './components/selector/selector.component';
import { ButtonComponent } from './components/button/button.component';
import { NgxSpinnerModule } from 'ngx-spinner';
import { StepperComponent } from './components/stepper/stepper.component';
import { StepComponent } from './components/step/step.component';
import { ContextMenuComponent } from './components/context-menu/context-menu.component';


@NgModule({
  declarations: [
    NavbarComponent,
    HeaderComponent,
    FooterComponent,
    TextInputComponent,
    DropdownComponent,
    ImageUploaderComponent,
    ImageCropperComponent,
    SelectorComponent,
    ButtonComponent,
    StepperComponent,
    StepComponent,
    ContextMenuComponent,
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
    NgxSpinnerModule.forRoot({
      type: 'ball-clip-rotate'
    })
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
    SelectorComponent,
    ButtonComponent,
    NgxSpinnerModule,
    StepComponent,
    StepperComponent,
    ContextMenuComponent,
  ]
})
export class SharedModule { }
