import { Injectable, EventEmitter } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { CookieService } from 'ngx-cookie-service';
import { environment } from '../../environments/environment';
import * as jwt_decode from "jwt-decode";
import { HeadersService } from './headers.service';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  private token: any;

  gotUserTokenEvent: EventEmitter<{ userName: string, userRole: string }>;
  logoutEvent = new EventEmitter();

  constructor(private http: Http, private cookieService: CookieService, private headersService: HeadersService) {
    if (cookieService.check('tokenCookie')) {
      this.token = jwt_decode(cookieService.get('tokenCookie'));
    }
    this.gotUserTokenEvent = new EventEmitter<{ userName: string, userRole: string }>();
  }

  raiseTokenEvent(): void {
    this.token = jwt_decode(this.cookieService.get('tokenCookie'));
    let UserInfo = {
      userName: this.token.user,
      userRole: this.token.user_role
    }
    this.gotUserTokenEvent.emit(UserInfo);
  }

  raiseLogoutEvent() {
    this.logoutEvent.emit();
  }

  isUserLoggedIn() {
    if (this.cookieService.check('tokenCookie')) {
      return true;
    }
    return false;
  }

  isAdmin() {
    return this.token.user_role === "Admin";
  }

  getCurrentUserName() {
    if (this.cookieService.check('tokenCookie')) {
      return this.token.user;
    }
    return null;
  }

  getCurrentUserRole() {
    if (this.cookieService.check('tokenCookie')) {
      return this.token.user_role;
    }
    return null;
  }

  getAllUsers() {
    return this.http.get(environment.API_USERS_URL + '/All', { headers: this.headersService.Headers });
  }

  getCurrentUserData() {
    return this.http.get(environment.API_USERS_URL + '/' + this.token.user);
  }

  getUserData(userName: string) {
    return this.http.get(environment.API_USERS_URL + '/' + userName);
  }

  createUser(data) {
    return this.http.post(environment.API_USERS_URL + '/Registration', data);
  }

  loginUser(data) {
    return this.http.post(environment.API_USERS_URL + '/Login', data, { withCredentials: true });
  }

  deleteUser(userName: string) {
    return this.http.delete(environment.API_USERS_URL + '/' + userName, { headers: this.headersService.Headers });
  }

  updateUser(data) {
    return this.http.put(environment.API_USERS_URL, data, { headers: this.headersService.Headers, withCredentials: true });
  }

}
