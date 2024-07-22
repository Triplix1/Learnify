import { Component, OnInit } from '@angular/core';
import { switchMap, take } from 'rxjs';
import { AuthService } from 'src/app/Core/services/auth.service';
import { ProfileService } from 'src/app/Core/services/profile.service';
import { ProfileResponse } from 'src/app/Models/ProfileResponse';
import { ProfileUpdateRequest } from 'src/app/Models/ProfileUpdateRequest';

@Component({
  selector: 'app-main-profile',
  templateUrl: './main-profile.component.html',
  styleUrls: ['./main-profile.component.scss']
})
export class MainProfileComponent implements OnInit {
  profileData: ProfileResponse;

  constructor(private readonly authService: AuthService,
    private readonly profileService: ProfileService) { }

  ngOnInit(): void {
    this.authService.userData$.pipe(
      switchMap(
        response => {
          return this.profileService.getById(response.id);
        }
      )
    ).subscribe(
      profile => {
        this.profileData = profile.data;
      }
    )
  }

  imagechanged(image: File): void {
    const profileUpdateRequest = {
      id: this.profileData.id,
      cardNumber: this.profileData.cardNumber,
      company: this.profileData.company,
      file: image,
      name: this.profileData.name,
      surname: this.profileData.surname
    } as ProfileUpdateRequest

    this.profileService.update(profileUpdateRequest).pipe(take(1)).subscribe();
  }
}
