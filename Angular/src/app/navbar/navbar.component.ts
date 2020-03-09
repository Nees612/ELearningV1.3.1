import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import * as jwt_decode from "jwt-decode";
import { UsersService } from '../services/users.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  //@Input() isActiveRegistration: boolean;
  //@Input() isActiveLogin: boolean;
  userName: string;
  userRole: string;

  isAuthorized: boolean = false;

  //@Output() authorized = new EventEmitter();
  //@Output() registration = new EventEmitter();
  //@Output() login = new EventEmitter();
  //@Output() homeTabClicked = new EventEmitter();
  //@Output() usersTabClicked = new EventEmitter();
  //@Output() assigmentsTabClicked = new EventEmitter();
  //@Output() myProfileClicked = new EventEmitter();



  constructor(private cookieService: CookieService, private usersService: UsersService, private router: Router) { }

  ngOnInit() {
    this.usersService.gotUserTokenEvent.subscribe(userInfo => {
      this.isAuthorized = true;
      this.userName = userInfo.userName;
      this.userRole = userInfo.userRole;
    });
    this.isAuthorized = !(this.usersService.getCurrentUserName() === null && this.usersService.getCurrentUserRole() === null);
    if (this.isAuthorized) {
      this.userName = this.usersService.getCurrentUserName();
      this.userRole = this.usersService.getCurrentUserRole();
    }
  }

  //onRegistrationClick() {
  //  this.isActiveLogin = false;
  //  this.isActiveRegistration = true;
  //  this.registration.emit();
  //}

  //onLoginClick() {
  //  this.isActiveRegistration = false;
  //  this.isActiveLogin = true;
  //  this.login.emit();
  //}

  onLogout() {
    this.cookieService.delete('tokenCookie');
    this.router.navigate(['/home'])
  }

  //onHomeClick() {
  //  this.homeTabClicked.emit();
  //}

  //onAssigmentsClick() {
  //  this.assigmentsTabClicked.emit();
  //}

  //onOtherStudentsClick() {
  //  this.usersTabClicked.emit();
  //}

  //onMyProfileClick() {
  //  this.myProfileClicked.emit();
  //}

}
