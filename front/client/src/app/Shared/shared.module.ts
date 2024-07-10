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


@NgModule({
  declarations: [
    NavbarComponent,
    HeaderComponent,
    FooterComponent,
    TextInputComponent,
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
  ]
})
export class SharedModule { }
