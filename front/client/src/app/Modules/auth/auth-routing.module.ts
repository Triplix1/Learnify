import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginButtonComponent } from './login/login.component';
import { LogoutButtonComponent } from './logout-button/logout-button.component';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { RegisterComponent } from './register/register.component';

const routes: Routes = [
  { path: "login", component: LoginButtonComponent },
  { path: "logout", component: LogoutButtonComponent },
  { path: "user-profile", component: UserProfileComponent },
  { path: "register", component: RegisterComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AuthRoutingModule { }