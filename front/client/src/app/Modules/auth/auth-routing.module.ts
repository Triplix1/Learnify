import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginButtonComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';

const routes: Routes = [
  { path: "login", component: LoginButtonComponent },
  { path: "register", component: RegisterComponent },
  { path: "register/:role", component: RegisterComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { bindToComponentInputs: true })],
  exports: [RouterModule]
})
export class AuthRoutingModule { }