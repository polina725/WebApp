import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {
  hubUrl = environment.hubUrl;
  private hubConnection: HubConnection;
  private onlineUSresSource = new BehaviorSubject<string[]>([])
  onlineUsers$ = this.onlineUSresSource.asObservable();

  constructor(private toastr: ToastrService) { }

  createHubConnection(user: User){
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'presence', {
        accessTokenFactory: () => user.token
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .catch(err => console.log(err));

    this.hubConnection.on('UserIsOnline', username => {
      this.toastr.info(username + ' connected');
    })

    this.hubConnection.on('UserIsOffline', username => {
      this.toastr.warning(username + ' disconnected');
    })

    this.hubConnection.on('GetOnlineUsers', (usernames: string[]) => {
      this.onlineUSresSource.next(usernames);
    })
  }

  stopHubConnection(){
    this.hubConnection
      .stop()
      .catch(err => console.log(err));
  }
}
