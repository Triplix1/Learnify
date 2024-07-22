import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/Shared/shared.module';
import { AuthModule } from '@auth0/auth0-angular';
import { LoginButtonComponent } from './login/login.component';
import { LogoutButtonComponent } from './logout-button/logout-button.component';
import { AuthRoutingModule } from './auth-routing.module';
import { RegisterComponent } from './register/register.component';


@NgModule({
  declarations: [
    LoginButtonComponent,
    LogoutButtonComponent,
    RegisterComponent
  ],
  imports: [
    AuthRoutingModule,
    SharedModule,
    AuthModule.forRoot({
      domain: 'dev-pxu5guvclvdrtofv.us.auth0.com',
      clientId: '2i944U35sPpnXzfVovUC2Z48RG2CHjPg',
      authorizationParams: {
        redirect_uri: window.location.origin
      }
    })
  ],
  exports: [
  ]
})
export class AuthenticationModule { }
