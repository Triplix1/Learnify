import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authGuard } from 'src/app/Core/guards/auth.guard';
import { ModeratorsManagingComponent } from './moderators-managing/moderators-managing.component';

const routes: Routes = [
  {
    path: 'moderators', canActivate: [authGuard],
    children: [
      { path: 'managing', component: ModeratorsManagingComponent },
    ]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { bindToComponentInputs: true })],
  exports: [RouterModule]
})
export class ModeratorsRoutingModule { }