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
    let userInfo = {
      userName: this.token.user,
      userRole: this.token.user_role
    }
    this.gotUserTokenEvent.emit(userInfo);
  }

  getUserId() {
    return this.token.id;
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

  getUsersByRole(role: string) {
    return this.http.get(environment.API_USERS_URL + '/Users_by_role/' + role, { headers: this.headersService.getHeaders() });
  }

  getAllUsers() {
    return this.http.get(environment.API_USERS_URL + '/All', { headers: this.headersService.getHeaders() });
  }

  getCurrentUserData() {
    return this.http.get(environment.API_USERS_URL + '/' + this.token.id);
  }

  getUserData(id: string) {
    return this.http.get(environment.API_USERS_URL + '/' + id);
  }

  createUser(data) {
    return this.http.post(environment.API_USERS_URL + '/Registration', data);
  }

  loginUser(data) {
    return this.http.post(environment.API_USERS_URL + '/Login', data, { withCredentials: true });
  }

  deleteUser(id: string) {
    return this.http.delete(environment.API_USERS_URL + '/' + id, { headers: this.headersService.getHeaders() });
  }

  updateUser(data) {
    return this.http.put(environment.API_USERS_URL + '/' + this.getUserId(), data, { headers: this.headersService.getHeaders(), withCredentials: true });
  }

}
