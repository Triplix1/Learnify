import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, Observable, take } from 'rxjs';
import { Message } from 'src/app/Models/Message/Message';
import { Group } from 'src/app/Models/Message/Group';
import { AuthService } from './auth.service';
import { AuthResponse } from 'src/app/Models/Auth/AuthReponse';
import { MessageCreateRequest } from 'src/app/Models/Message/MessageCreateRequest';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl = environment.baseApiUrl;
  hubUrl = environment.hubUrl;
  private hubConnection?: HubConnection;
  private messageThreadSouce = new BehaviorSubject<Message[]>([]);
  messageThread$ = this.messageThreadSouce.asObservable();

  constructor(private http: HttpClient) { }

  createHubConnection(authResponse: AuthResponse, groupName: string) {
    console.log("creating hub conn");
    debugger;
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + '/message?group=' + groupName, {
        accessTokenFactory: () => authResponse.token
      })
      .withAutomaticReconnect()
      .build();
    this.hubConnection.start().catch(error => console.log(error));

    this.hubConnection.on('ReceiveMessageThread', messages => {
      this.messageThreadSouce.next(messages);
    })

    this.hubConnection.on('NewMessage', message => {
      this.messageThread$.pipe(take(1)).subscribe({
        next: messages => {
          this.messageThreadSouce.next([...messages, message])
        }
      })
    })

    this.hubConnection.on('MessageDeleted', messageId => {
      this.messageThread$.pipe(take(1)).subscribe({
        next: messages => {
          this.messageThreadSouce.next([...messages].filter(m => m.id !== messageId))
        }
      })
    })

    this.hubConnection.on('UpdatedGroup', (group: Group) => {
      if (group.name === groupName) {
        this.messageThread$.pipe(take(1)).subscribe({
          next: messages => {
            messages.forEach(message => {
            })
            this.messageThreadSouce.next([...messages]);
          }
        })
      }
    })
  }

  stopHubConnection() {
    if (this.hubConnection) {
      this.hubConnection?.stop();
    }
  }

  // getMessages(groupName: string): Observable<Message[]> {
  //   return this.http.get<Message[]>(this.baseUrl + 'messages/' + groupName);
  // }

  // getMessageThread(groupName: string) {
  //   return this.http.get<Message[]>(this.baseUrl + '/messages/thread/' + groupName);
  // }

  async sendMessage(messageCreateRequest: MessageCreateRequest) {
    return this.hubConnection?.invoke('SendMessage', { messageCreateRequest })
      .catch(error => console.log(error));
  }

  async deleteMessage(id: number) {
    return this.hubConnection?.invoke('DeleteMessage', { id })
      .catch(error => console.log(error));
  }
}
