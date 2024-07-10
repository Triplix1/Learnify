import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs';
import { AuthService } from 'src/app/Core/services/auth.service';
import { AuthorizationDeepLinkingService } from 'src/app/Core/services/authorization-deep-linking.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  registerForm: FormGroup = new FormGroup({});
  returnUrl: string | undefined;
  authorizationService: AuthorizationDeepLinkingService = new AuthorizationDeepLinkingService(this.route);

  constructor(private readonly authService: AuthService, private readonly fb: FormBuilder, private readonly router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.initializeForm();
    this.returnUrl = this.authorizationService.getReturnUrl();
  }

  initializeForm() {
    this.registerForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      nickname: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(25)]],
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
    this.authService.register(this.registerForm.value).pipe(take(1)).subscribe({
      next: _ => {
        this.router.navigateByUrl(this.returnUrl || "/");
      },
    });
  }

  cancel() {
    this.router.navigateByUrl(this.returnUrl || "/");
  }
}
