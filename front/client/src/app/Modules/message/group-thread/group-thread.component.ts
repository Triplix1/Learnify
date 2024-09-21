import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HubConnectionState } from '@microsoft/signalr';
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

  constructor(
    public messageService: MessageService,
    private readonly authService: AuthService,
    private readonly fb: FormBuilder
  ) { }

  ngOnInit(): void {
    this.initializeForm();

    // Subscribe to authService and manage connection creation
    this.authService.tokenData$.pipe(takeUntil(this.ngUnsubscribe)).subscribe(
      (data: AuthResponse) => {
        debugger;
        // Ensure that a connection is only created if it's disconnected or hasn't been created yet
        if (this.messageService.hubConnection?.state !== HubConnectionState.Connected) {
          this.messageService.createHubConnection(data, this.groupName);
        }
      }
    );
  }

  initializeForm() {
    this.messageForm = this.fb.group({
      message: ['', []],
    });
  }

  ngOnDestroy(): void {
    // Clean up on component destruction
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();

    // Stop the hub connection only if it exists and is connected
    if (this.messageService.hubConnection?.state === HubConnectionState.Connected) {
      this.messageService.stopHubConnection();
    }
  }

  sendMessage() {
    const messageCreateRequest: MessageCreateRequest = {
      Content: this.messageForm.controls['message'].value,
      GroupName: this.groupName
    };

    this.messageService.sendMessage(messageCreateRequest).then(
      // Optionally handle success or failure
    );
  }
}
