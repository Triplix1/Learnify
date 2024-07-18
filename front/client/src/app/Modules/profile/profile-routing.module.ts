import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainProfileComponent } from './main-profile/main-profile.component';

const routes: Routes = [
  {
    path: "profile", children: [
      { path: "", component: MainProfileComponent },
      { path: "main", component: MainProfileComponent },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class ProfileRoutingModule { }