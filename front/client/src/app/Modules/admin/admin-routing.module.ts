import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authGuard } from 'src/app/Core/guards/auth.guard';
import { AdminsManagingComponent } from './admins-managing/admins-managing.component';

const routes: Routes = [
  {
    path: 'admins', canActivate: [authGuard],
    children: [
      { path: 'managing', component: AdminsManagingComponent },
    ]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { bindToComponentInputs: true })],
  exports: [RouterModule]
})
export class AdminRoutingModule { }