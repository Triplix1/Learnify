import { Component, Input } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs';
import { AdminsService } from 'src/app/Core/services/admins.service';
import { AuthService } from 'src/app/Core/services/auth.service';
import { AuthorizationDeepLinkingService } from 'src/app/Core/services/authorization-deep-linking.service';
import { ModeratorsService } from 'src/app/Core/services/morderators.service';
import { UserType } from 'src/app/Models/enums/UserType';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  @Input() role: string;
  registerForm: FormGroup = new FormGroup({});
  returnUrl: string | undefined;
  authorizationService: AuthorizationDeepLinkingService = new AuthorizationDeepLinkingService(this.route);

  constructor(private readonly authService: AuthService,
    private readonly moderatorsService: ModeratorsService,
    private readonly adminsService: AdminsService,
    private readonly fb: FormBuilder,
    private readonly router: Router,
    private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.initializeForm();
    this.returnUrl = this.authorizationService.getReturnUrl();
  }

  initializeForm() {
    this.registerForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      username: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(25)]],
      name: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(25)]],
      surname: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(25)]],
      password: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(25)]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]]
    });
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.get(matchTo)?.value ? null : { isMatching: true }
    }
  }

  register() {
    if (this.role === UserType.Moderator) {
      this.moderatorsService.create(this.registerForm.value).pipe(take(1)).subscribe(
        this.handleRegistration,
      );
    }
    else if (this.role === UserType.Admin) {
      this.adminsService.create(this.registerForm.value).pipe(take(1)).subscribe(
        this.handleRegistration,
      );
    }
    else {
      this.authService.register(this.registerForm.value).pipe(take(1)).subscribe(
        this.handleRegistration,
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
