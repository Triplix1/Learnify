import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { JoinToGroupComponent } from './join-to-group/join-to-group.component';
import { GroupThreadComponent } from './group-thread/group-thread.component';
import { authGuard } from 'src/app/Core/guards/auth.guard';

const routes: Routes = [
  {
    path: 'messages', canActivate: [authGuard],
    children: [
      { path: 'join', component: JoinToGroupComponent },
      { path: ':groupName', component: GroupThreadComponent }
    ]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { bindToComponentInputs: true })],
  exports: [RouterModule]
})
export class MessageRoutingModule { }