import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subject, takeUntil } from 'rxjs';
import { AuthService } from 'src/app/Core/services/auth.service';
import { MessageService } from 'src/app/Core/services/message.service';
import { AuthResponse } from 'src/app/Models/Auth/AuthReponse';
import { MessageCreateRequest } from 'src/app/Models/Message/MessageCreateRequest';

@Component({
  selector: 'app-group-thread',
  templateUrl: './group-thread.component.html',
  styleUrls: ['./group-thread.component.scss']
})
export class GroupThreadComponent implements OnInit, OnDestroy {
  @Input() groupName: string;
  private ngUnsubscribe = new Subject<void>();

  messageForm: FormGroup = new FormGroup({});

  constructor(public messageService: MessageService, private readonly authService: AuthService, private readonly fb: FormBuilder) { }

  ngOnInit(): void {
    this.initializeForm();
    this.authService.tokenData$.pipe(takeUntil(this.ngUnsubscribe)).subscribe(
      data => this.messageService.createHubConnection(data, this.groupName)
    );
  }

  initializeForm() {
    this.messageForm = this.fb.group({
      message: ['', []],
    });
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
    this.messageService.stopHubConnection();
  }

  sendMessage() {
    const messageCreateRequest: MessageCreateRequest = {
      content: this.messageForm.controls['message'].value,
      groupName: this.groupName
    }

    this.messageService.sendMessage(messageCreateRequest).then(
    );
  }
}
