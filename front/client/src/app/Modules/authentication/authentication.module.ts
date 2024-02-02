import { NgModule } from '@angular/core';
import { AuthenticationRoutingModule } from './authentication-routing.module';
import { AuthCallbackComponent } from './auth-callback/auth-callback.component';
import { SharedModule } from 'src/app/Shared/shared.module';
import { SignoutRedirectCallbackComponent } from './signout-redirect-callback/signout-redirect-callback.component';



@NgModule({
  declarations: [
    AuthCallbackComponent,
    SignoutRedirectCallbackComponent
  ],
  imports: [
    AuthenticationRoutingModule,
    SharedModule,
  ],
  exports: [
    AuthCallbackComponent,
    SignoutRedirectCallbackComponent
  ]
})
export class AuthenticationModule { }
