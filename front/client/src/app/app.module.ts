import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeModule } from './Modules/home/home.module';
import { AuthenticationModule } from './Modules/auth/auth.module';
import { SharedModule } from './Shared/shared.module';
import { NoopAnimationsModule, BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HTTP_INTERCEPTORS, provideHttpClient } from '@angular/common/http';
import { RefreshInterceptor } from './Core/interceptors/refresh.interceptor';
import { ErrorHandlerInterceptor } from './Core/interceptors/error-handler.interceptor';
import { ProfileModule } from './Modules/profile/profile.module';
import { CourseModule } from './Modules/course/course.module';
import { MeetingModule } from './Modules/meetings/meeting.module';
import { ModeratorsModule } from './Modules/moderators/moderators.module';
import { AdminModule } from './Modules/admin/admin.module';

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    SharedModule,
    AppRoutingModule,
    AuthenticationModule,
    ProfileModule,
    HomeModule,
    NoopAnimationsModule,
    BrowserAnimationsModule,
    CourseModule,
    MeetingModule,
    ModeratorsModule,
    AdminModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: ErrorHandlerInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: RefreshInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
