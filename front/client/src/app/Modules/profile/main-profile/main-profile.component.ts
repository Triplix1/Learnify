import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { switchMap, take, takeUntil } from 'rxjs';
import { AuthService } from 'src/app/Core/services/auth.service';
import { ProfileService } from 'src/app/Core/services/profile.service';
import { BaseComponent } from 'src/app/Models/BaseComponent';
import { DropdownItem } from 'src/app/Models/DropdownItem';
import { UserType } from 'src/app/Models/enums/UserType';
import { ProfileResponse } from 'src/app/Models/Profile/ProfileResponse';
import { ProfileUpdateRequest } from 'src/app/Models/Profile/ProfileUpdateRequest';
import { UpdateUserRoleRequest } from 'src/app/Models/Profile/UpdateUserRoleRequest';
import { SelectorOptions } from 'src/app/Models/SelectorOptions';

@Component({
  selector: 'app-main-profile',
  templateUrl: './main-profile.component.html',
  styleUrls: ['./main-profile.component.scss']
})
export class MainProfileComponent extends BaseComponent implements OnInit {
  profileData: ProfileResponse;
  profileForm: FormGroup;
  teacherType: UserType = UserType.Teacher;
  studentType: UserType = UserType.Student;
  selectorOptions: SelectorOptions<UserType>[] = [
    { label: "Teacher", value: UserType.Teacher },
    { label: "Student", value: UserType.Student },
  ]

  userTypeFormControl: FormGroup;
  initialForm: any;

  constructor(private readonly authService: AuthService,
    private readonly profileService: ProfileService,
    private readonly fb: FormBuilder) {
    super();
  }

  ngOnInit(): void {
    this.authService.userData$.pipe(
      take(1),
      takeUntil(this.destroySubject),
      switchMap(
        response => {
          return this.profileService.getById(response.id).pipe(take(1));
        }
      )
    ).subscribe(
      profile => {
        this.handleProfileUpdate(profile.data);
      }
    )
  }

  handleProfileUpdate(profileResponse: ProfileResponse) {
    this.profileData = profileResponse;
    this.handleUserTypeUpdate(profileResponse);
    this.initialiseForm();
  }

  handleUserTypeUpdate(profileResponse: ProfileResponse) {
    this.profileData = profileResponse;
    this.updateUserTypeForm();
    this.initialForm = this.userTypeFormControl.value;
    this.authService.updateUserRole(profileResponse.type);
  }

  initialiseForm() {
    this.profileForm = this.fb.group({
      name: [this.profileData.name, [Validators.required]],
      surname: [this.profileData.surname, [Validators.required]],
      company: [this.profileData.company, [Validators.required]],
      username: [this.profileData.userName, [Validators.required]],
    });

    if (this.profileData.type === UserType.Teacher) {
      this.profileForm.addControl(
        'cardNumber',
        new FormControl(this.profileData.cardNumber, Validators.required) // Note the empty string and the parentheses after required
      );
    }
  }

  updateUserTypeForm() {
    this.userTypeFormControl = this.fb.group({
      role: [this.profileData.type]
    });

    // this.userTypeFormControl.controls["role"].valueChanges.subscribe(
    //   value => this.updateUserType(UserType[value as keyof typeof UserType])
    // );
  }

  updateProfile() {
    const profileUpdateRequest: ProfileUpdateRequest = {
      id: this.profileData.id,
      name: this.profileForm.get('name').value,
      surname: this.profileForm.get('surname').value,
      company: this.profileForm.get('company').value,
      cardNumber: this.profileForm.get('cardNumber')?.value,
      userName: this.profileForm.get('username').value,
    }

    this.profileService.update(profileUpdateRequest).subscribe(response => this.handleProfileUpdate(response.data));
  }

  updateUserType(userTypeString: string) {
    const userType = UserType[userTypeString as keyof typeof UserType];

    const userTypeUpdateRequest: UpdateUserRoleRequest = {
      role: userType
    };

    this.profileService.updateUserRole(userTypeUpdateRequest).subscribe(
      response => this.handleUserTypeUpdate(response.data),
      error => this.userTypeFormControl.setValue(this.initialForm)
    );
  }

  discardChanges() {
    this.initialiseForm();
  }
}