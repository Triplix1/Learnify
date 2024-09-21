import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
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
  hubConnection?: HubConnection;
  private messageThreadSouce = new BehaviorSubject<Message[]>([]);
  messageThread$ = this.messageThreadSouce.asObservable();

  constructor(private http: HttpClient) { }

  createHubConnection(authResponse: AuthResponse, groupName: string) {
    if (this.hubConnection?.state == HubConnectionState.Connected)
      this.hubConnection.stop();

    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + '/message?group=' + groupName, {
        accessTokenFactory: () => authResponse.token
      })
      .build();
    this.hubConnection.start().catch(error => console.log(error));

    this.hubConnection.on('ReceiveMessageThread', messages => {
      this.messageThreadSouce.next(messages);
    })

    this.hubConnection.onclose(error => {
      console.log('Connection closed:', error);
    });
    this.hubConnection.onreconnected(connectionId => {
      console.log('Reconnected:', connectionId);
    });

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
      this.messageThreadSouce.next([]);
    }
  }

  // getMessages(groupName: string): Observable<Message[]> {
  //   return this.http.get<Message[]>(this.baseUrl + 'messages/' + groupName);
  // }

  // getMessageThread(groupName: string) {
  //   return this.http.get<Message[]>(this.baseUrl + '/messages/thread/' + groupName);
  // }

  async sendMessage(messageCreateRequest: MessageCreateRequest) {
    return this.hubConnection?.invoke('SendMessage', messageCreateRequest)
      .catch(error => console.log(error));
  }

  async deleteMessage(id: number) {
    return this.hubConnection?.invoke('DeleteMessage', { id })
      .catch(error => console.log(error));
  }
}
