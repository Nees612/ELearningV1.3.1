import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  @Input() isActiveRegistration: boolean;
  @Input() isActiveLogin: boolean;
  @Input() userName: string;
  @Input() userRole: string;

  @Output() authorized = new EventEmitter();
  isAuthorized: boolean = false;

  @Output() registration = new EventEmitter();
  @Output() login = new EventEmitter();
  @Output() homeTabClicked = new EventEmitter();
  @Output() usersTabClicked = new EventEmitter();
  @Output() assigmentsTabClicked = new EventEmitter();
  @Output() myProfileClicked = new EventEmitter();



  constructor(private cookieService: CookieService) { }

  ngOnInit() {
    if (this.cookieService.check('tokenCookie')) {
      this.isAuthorized = true
      this.authorized.emit();
    }
  }

  onRegistrationClick() {
    this.isActiveLogin = false;
    this.isActiveRegistration = true;
    this.registration.emit();
  }

  onLoginClick() {
    this.isActiveRegistration = false;
    this.isActiveLogin = true;
    this.login.emit();
  }

  onLogout() {
    this.cookieService.deleteAll();
    location.reload();
  }

  onHomeClick() {
    this.homeTabClicked.emit();
  }

  onAssigmentsClick() {
    this.assigmentsTabClicked.emit();
  }

  onOtherStudentsClick() {
    this.usersTabClicked.emit();
  }

  onMyProfileClick() {
    this.myProfileClicked.emit();
  }

}
