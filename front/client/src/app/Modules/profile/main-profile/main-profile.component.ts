import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { switchMap, take, takeUntil } from 'rxjs';
import { lettersOnlyValidator } from 'src/app/Core/helpers/lettersOnlyValidator';
import { AuthService } from 'src/app/Core/services/auth.service';
import { ProfileService } from 'src/app/Core/services/profile.service';
import { BaseComponent } from 'src/app/Models/BaseComponent';
import { DropdownItem } from 'src/app/Models/DropdownItem';
import { UserType } from 'src/app/Models/enums/UserType';
import { ProfileResponse } from 'src/app/Models/Profile/ProfileResponse';
import { ProfileUpdateRequest } from 'src/app/Models/Profile/ProfileUpdateRequest';
import { UpdateUserRoleRequest } from 'src/app/Models/Profile/UpdateUserRoleRequest';
import { SelectorOptions } from 'src/app/Models/SelectorOptions';
import { AcceptDialogComponent } from 'src/app/Shared/components/accept-dialog/accept-dialog.component';

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
    private readonly fb: FormBuilder,
    private readonly dialog: MatDialog) {
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
    this.initialiseForm();
    this.initialForm = this.userTypeFormControl.value;
    this.authService.updateUserRole(profileResponse.type);
  }

  initialiseForm() {
    this.profileForm = this.fb.group({
      name: [this.profileData.name, [Validators.required, Validators.maxLength(25), lettersOnlyValidator()]],
      surname: [this.profileData.surname, [Validators.required, Validators.maxLength(25), lettersOnlyValidator()]],
      username: [this.profileData.userName, [Validators.required, Validators.maxLength(25)]],
    });

    if (this.profileData.type === UserType.Teacher) {
      this.profileForm.addControl(
        'cardNumber',
        new FormControl(this.profileData.cardNumber, Validators.required) // Note the empty string and the parentheses after required
      );
      this.profileForm.addControl(
        'company',
        new FormControl(this.profileData.company, [Validators.maxLength(100)])
      )
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
      company: this.profileForm.get('company')?.value,
      cardNumber: this.profileForm.get('cardNumber')?.value,
      userName: this.profileForm.get('username').value,
    }

    this.profileService.update(profileUpdateRequest).pipe(take(1)).subscribe(response => this.handleProfileUpdate(response.data));
  }

  updateUserType(userTypeString: string) {
    const userType = UserType[userTypeString as keyof typeof UserType];

    const userTypeUpdateRequest: UpdateUserRoleRequest = {
      role: userType
    };

    this.profileService.updateUserRole(userTypeUpdateRequest).pipe(take(1), takeUntil(this.destroySubject)).subscribe(
      response => this.handleUserTypeUpdate(response.data),
      error => {
        this.userTypeFormControl.setValue(this.initialForm);
        if (error.error.errorData) {
          this.dialog.open(AcceptDialogComponent, {
            width: '450px',
            data: {
              text: error.error.errorData
            }
          })
        }
      }
    );
  }

  discardChanges() {
    this.initialiseForm();
  }
}