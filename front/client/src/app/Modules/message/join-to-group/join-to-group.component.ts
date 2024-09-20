import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { AuthService } from 'src/app/Core/services/auth.service';
import { MessageService } from 'src/app/Core/services/message.service';
import { AuthResponse } from 'src/app/Models/Auth/AuthReponse';

@Component({
  selector: 'app-join-to-group',
  templateUrl: './join-to-group.component.html',
  styleUrls: ['./join-to-group.component.scss']
})
export class JoinToGroupComponent implements OnInit, OnDestroy {
  joinForm: FormGroup = new FormGroup({});
  private ngUnsubscribe = new Subject<void>();
  private tokenData: AuthResponse;

  constructor(private readonly authService: AuthService, private readonly fb: FormBuilder, private readonly messageService: MessageService, private readonly router: Router) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.joinForm = this.fb.group({
      groupName: ['', [Validators.required]],
    });
  }

  join() {
    if (this.joinForm.valid) {
      this.router.navigate([`/messages/${this.joinForm.controls['groupName'].value}`]);
    }
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
    this.messageService.stopHubConnection();
  }

}
