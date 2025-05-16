import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { take, takeUntil } from 'rxjs';
import { AuthService } from 'src/app/Core/services/auth.service';
import { AuthorizationDeepLinkingService } from 'src/app/Core/services/authorization-deep-linking.service';
import { BaseComponent } from 'src/app/Models/BaseComponent';

@Component({
  selector: 'app-login-button',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginButtonComponent extends BaseComponent implements OnInit {
  loginForm: FormGroup = new FormGroup({});
  returnUrl: string | undefined;
  authorizationService: AuthorizationDeepLinkingService = new AuthorizationDeepLinkingService(this.route);

  constructor(private readonly authService: AuthService, private readonly fb: FormBuilder, private readonly router: Router, private route: ActivatedRoute) {
    super();
  }

  ngOnInit(): void {
    this.initializeForm();
    this.returnUrl = this.authorizationService.getReturnUrl();
  }

  initializeForm() {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(25)]],
    });
  }

  login() {
    this.authService.login(this.loginForm.value).pipe(take(1), takeUntil(this.destroySubject)).subscribe({
      next: _ => {
        this.router.navigateByUrl(this.returnUrl || "/");
      },
    });
  }

  cancel() {
    this.router.navigateByUrl(this.returnUrl || "/");
  }
}