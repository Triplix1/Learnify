import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs';
import { AuthService } from 'src/app/Core/services/auth.service';
import { AuthorizationDeepLinkingService } from 'src/app/Core/services/authorization-deep-linking.service';

@Component({
  selector: 'app-login-button',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginButtonComponent implements OnInit {
  loginForm: FormGroup = new FormGroup({});
  returnUrl: string | undefined;
  authorizationService: AuthorizationDeepLinkingService = new AuthorizationDeepLinkingService(this.route);

  constructor(private readonly authService: AuthService, private readonly fb: FormBuilder, private readonly router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.initializeForm();
    this.returnUrl = this.authorizationService.getReturnUrl();
  }

  initializeForm() {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
    });
  }

  login() {
    this.authService.login(this.loginForm.value).pipe(take(1)).subscribe({
      next: _ => {
        this.router.navigateByUrl(this.returnUrl || "/");
      },
    });
  }

  cancel() {
    this.router.navigateByUrl(this.returnUrl || "/");
  }
}