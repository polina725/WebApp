import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject } from 'rxjs';
import { take } from 'rxjs/operators';
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

  constructor(private toastr: ToastrService, private router: Router) { }

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
      this.onlineUsers$.pipe(take(1)).subscribe(usernames => {
        this.onlineUSresSource.next([...usernames, username]);
      })
    })

    this.hubConnection.on('UserIsOffline', username => { 
      this.onlineUsers$.pipe(take(1)).subscribe(usernames => {
        this.onlineUSresSource.next([...usernames.filter(_username => _username !== username)]);
      })
    })

    this.hubConnection.on('GetOnlineUsers', (usernames: string[]) => {
      this.onlineUSresSource.next(usernames);
    })

    this.hubConnection.on("NewMessageReceived", ({username, knownAs}) => {
      this.toastr.info(knownAs + ' has sent you a new message')
        .onTap
        .pipe(take(1))
        .subscribe(() => this.router.navigateByUrl('/members/' + username + '?tab=3'));
    })
  }

  stopHubConnection(){
    this.hubConnection
      .stop()
      .catch(err => console.log(err));
  }
}
