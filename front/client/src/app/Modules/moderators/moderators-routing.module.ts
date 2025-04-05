import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authGuard } from 'src/app/Core/guards/auth.guard';
import { ManagingComponent } from './managing/managing.component';

const routes: Routes = [
  {
    path: 'moderators', canActivate: [authGuard],
    children: [
      { path: 'managing', component: ManagingComponent },
    ]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { bindToComponentInputs: true })],
  exports: [RouterModule]
})
export class ModeratorsRoutingModule { }