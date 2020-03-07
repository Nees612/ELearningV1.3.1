import { Component, OnInit } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import * as jwt_decode from "jwt-decode";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Home';

  userName: string;
  userRole: string;



  isRegistration: boolean = false;
  isLogin: boolean = false;
  isAuthorized: boolean = false;
  isSelectedModule: boolean = false;
  isUsersTabActive: boolean = false;
  isMyProfileTabActive: boolean = false;
  isAssigmentsTabActive: boolean = false;
  isHomeTabActive: boolean = true;
  moduleId: number;

  constructor(private cookieService: CookieService) { }

  ngOnInit() {
    if (this.cookieService.check('tokenCookie')) {
      let decodedToken = jwt_decode(this.cookieService.get('tokenCookie'));
      this.userName = decodedToken.user;
      this.userRole = decodedToken.user_role;
    }
  }


  onRegistration() {
    this.isLogin = false;
    this.isRegistration = true;
  }

  onLogin() {
    this.isRegistration = false;
    this.isLogin = true;
  }

  onCancel() {
    this.isLogin = false;
    this.isRegistration = false;
  }


  onAuthorized() {
    this.isAuthorized = true;
  }

  onModuleSelected(moduleId) {
    this.isSelectedModule = true;
    this.moduleId = moduleId;
  }

  onModuleCancel() {
    this.isSelectedModule = false;
  }

  onUsersClick() {
    this.isHomeTabActive = false;
    this.isAssigmentsTabActive = false;
    this.isMyProfileTabActive = false;
    this.isUsersTabActive = true;
  }

  onAssigmentsClick() {
    this.isHomeTabActive = false;
    this.isUsersTabActive = false;
    this.isMyProfileTabActive = false;
    this.isAssigmentsTabActive = true;
  }

  onHomeClick() {
    this.isLogin = false;
    this.isRegistration = false;
    this.isUsersTabActive = false;
    this.isAssigmentsTabActive = false;
    this.isMyProfileTabActive = false;
    this.isHomeTabActive = true;
  }

  onMyProfileClick() {
    this.isUsersTabActive = false;
    this.isHomeTabActive = false;
    this.isAssigmentsTabActive = false;
    this.isMyProfileTabActive = true;
  }

  onUpdatedProfile() {
    this.userName = jwt_decode(this.cookieService.get('tokenCookie')).user;
  }
}
