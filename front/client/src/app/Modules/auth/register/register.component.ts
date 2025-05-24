import { Component, Input } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { take, takeUntil } from 'rxjs';
import { lettersOnlyValidator } from 'src/app/Core/helpers/lettersOnlyValidator';
import { AdminsService } from 'src/app/Core/services/admins.service';
import { AuthService } from 'src/app/Core/services/auth.service';
import { AuthorizationDeepLinkingService } from 'src/app/Core/services/authorization-deep-linking.service';
import { ModeratorsService } from 'src/app/Core/services/morderators.service';
import { BaseComponent } from 'src/app/Models/BaseComponent';
import { UserType } from 'src/app/Models/enums/UserType';
import { AcceptDialogComponent } from 'src/app/Shared/components/accept-dialog/accept-dialog.component';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent extends BaseComponent {
  @Input() role: string;
  registerForm: FormGroup = new FormGroup({});
  returnUrl: string | undefined;
  authorizationService: AuthorizationDeepLinkingService = new AuthorizationDeepLinkingService(this.route);

  constructor(private readonly authService: AuthService,
    private readonly moderatorsService: ModeratorsService,
    private readonly adminsService: AdminsService,
    private readonly fb: FormBuilder,
    private readonly router: Router,
    private route: ActivatedRoute,
    private readonly dialog: MatDialog) {
    super();
  }

  ngOnInit(): void {
    this.initializeForm();
    this.returnUrl = this.authorizationService.getReturnUrl();

    this.registerForm.get('password')?.valueChanges.pipe(takeUntil(this.destroySubject)).subscribe(() => {
      this.registerForm.get('confirmPassword')?.updateValueAndValidity();
    });

  }

  initializeForm() {
    this.registerForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      username: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(25)]],
      name: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(25), lettersOnlyValidator()]],
      surname: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(25), lettersOnlyValidator()]],
      password: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(25)]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]]
    });
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      const parent = control?.parent;
      if (!parent) return null;

      const matchToControl = parent.get(matchTo);
      return control.value === matchToControl?.value ? null : { isMatching: true };
    };
  }

  register() {
    if (this.role === UserType.Moderator) {
      this.moderatorsService.create(this.registerForm.value).pipe(take(1)).subscribe(
        this.handleRegistration,
        error => {
          if (error.error.errorData) {
            this.dialog.open(AcceptDialogComponent, {
              width: '450px',
              data: {
                text: error.error.errorData.join("\n")
              }
            })
          }
        }
      );
    }
    else if (this.role === UserType.Admin) {
      this.adminsService.create(this.registerForm.value).pipe(take(1)).subscribe(
        this.handleRegistration,
        error => {
          if (error.error.errorData) {
            this.dialog.open(AcceptDialogComponent, {
              width: '450px',
              data: {
                text: error.error.errorData.join("\n")
              }
            })
          }
        }
      );
    }
    else {
      this.authService.register(this.registerForm.value).pipe(take(1)).subscribe(
        this.handleRegistration,
        error => {
          if (error.error.errorData) {
            this.dialog.open(AcceptDialogComponent, {
              width: '450px',
              data: {
                text: error.error.errorData.join("\n")
              }
            })
          }
        }
      );
    }
  }

  handleRegistration = (_: any) => {
    this.router.navigateByUrl(this.returnUrl || "/");
  }

  cancel() {
    this.router.navigateByUrl(this.returnUrl || "/");
  }
}
