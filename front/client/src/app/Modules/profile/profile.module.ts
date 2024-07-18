import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/Shared/shared.module';
import { ProfileRoutingModule } from './profile-routing.module';
import { MainProfileComponent } from './main-profile/main-profile.component';


@NgModule({
  declarations: [
    MainProfileComponent,
  ],
  imports: [
    ProfileRoutingModule,
    SharedModule,
  ],
  exports: [
    MainProfileComponent,
  ]
})
export class ProfileModule { }
