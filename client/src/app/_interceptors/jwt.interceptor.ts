import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AccountService } from '../_services/account.service';
import { User } from '../_models/user';
import { take } from 'rxjs/operators';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private accountService: AccountService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let currUser: User;

    this.accountService.currentUser$.pipe(take(1)).subscribe(user => currUser = user);
    if(currUser){
      request = request.clone({
        setHeaders:{
          Authorization: `Bearer ${currUser.token}`
        }
      })
    }

    return next.handle(request);
  }
}
