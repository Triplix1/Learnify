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
import { InputComponent } from './components/input/input.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DropdownComponent } from './components/dropdown/dropdown.component';
import { ImageUploaderComponent } from './components/image-uploader/image-uploader.component';
import { ImageCropperDialogComponent } from './components/image-cropper-dialog/image-cropper-dialog.component';
import { SelectorComponent } from './components/selector/selector.component';
import { ButtonComponent } from './components/button/button.component';
import { NgxSpinnerModule } from 'ngx-spinner';
import { StepperComponent } from './components/stepper/stepper.component';
import { StepComponent } from './components/step/step.component';
import { ContextMenuComponent } from './components/context-menu/context-menu.component';
import { VideoPlayerComponent } from './components/video-player/video-player.component';
import { FileUploaderComponent } from './components/file-uploader/file-uploader.component';
import { ArrowComponent } from './components/arrow/arrow.component';
import { AccordionItemComponent } from './components/accordion-item/accordion-item.component';
import { AccordionHeaderComponent } from './components/accordion-header/accordion-header.component';
import { AccordionBodyComponent } from './components/accordion-body/accordion-body.component';
import { MediaPipe } from './pipes/media.pipe';
import { MediaComponent } from './components/media/media.component';
import { ImageComponent } from './components/image/image.component';
import { TruncatePipe } from './pipes/truncate.pipe';
import { NestedOptionComponent } from './components/nested-option/nested-option.component';
import { DragDropDirective } from './directives/drag-drop.directive';
import { ConfirmDialogComponent } from './components/confirm-dialog/confirm-dialog.component';
import { TextareaComponent } from './components/textarea/textarea.component';
import { VgCoreModule } from '@videogular/ngx-videogular/core';
import { VgControlsModule } from '@videogular/ngx-videogular/controls';
import { VgOverlayPlayModule } from '@videogular/ngx-videogular/overlay-play';
import { VgBufferingModule } from '@videogular/ngx-videogular/buffering';
import { FilePathPipe } from './pipes/file-path.pipe';
import { SelectLanguageComponent } from './components/select-language/select-language.component';
import { SelectLanguagesListComponent } from './components/select-languages-list/select-languages-list.component';
import { ActionSectionComponent } from './components/action-section/action-section.component';
import { PaginationComponent } from './components/pagination/pagination.component';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { AcceptDialogComponent } from './components/accept-dialog/accept-dialog.component';

@NgModule({
  declarations: [
    NavbarComponent,
    HeaderComponent,
    FooterComponent,
    InputComponent,
    DropdownComponent,
    ImageUploaderComponent,
    ImageCropperDialogComponent,
    SelectorComponent,
    ButtonComponent,
    StepperComponent,
    StepComponent,
    ContextMenuComponent,
    VideoPlayerComponent,
    FileUploaderComponent,
    ArrowComponent,
    AccordionItemComponent,
    AccordionHeaderComponent,
    AccordionBodyComponent,
    MediaPipe,
    MediaComponent,
    ImageComponent,
    TruncatePipe,
    NestedOptionComponent,
    DragDropDirective,
    ConfirmDialogComponent,
    TextareaComponent,
    FilePathPipe,
    SelectLanguageComponent,
    SelectLanguagesListComponent,
    ActionSectionComponent,
    PaginationComponent,
    AcceptDialogComponent,
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
    }),
    VgCoreModule,
    VgControlsModule,
    VgOverlayPlayModule,
    VgBufferingModule,
    BsDropdownModule.forRoot(),
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
    InputComponent,
    FormsModule,
    ReactiveFormsModule,
    DropdownComponent,
    ImageCropperDialogComponent,
    ImageUploaderComponent,
    SelectorComponent,
    ButtonComponent,
    NgxSpinnerModule,
    StepComponent,
    StepperComponent,
    ContextMenuComponent,
    VideoPlayerComponent,
    FileUploaderComponent,
    AccordionItemComponent,
    ArrowComponent,
    MediaPipe,
    TruncatePipe,
    MediaComponent,
    NestedOptionComponent,
    DragDropDirective,
    ConfirmDialogComponent,
    TextareaComponent,
    VgCoreModule,
    VgControlsModule,
    VgOverlayPlayModule,
    VgBufferingModule,
    FilePathPipe,
    SelectLanguageComponent,
    SelectLanguagesListComponent,
    ActionSectionComponent,
    BsDropdownModule,
    PaginationComponent
  ]
})
export class SharedModule { }
