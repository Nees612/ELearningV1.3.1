import { Component, OnInit } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { UsersService } from '../services/users.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  userName: string;
  userRole: string;

  isAuthorized: boolean = false;

  constructor(private cookieService: CookieService, private usersService: UsersService, private router: Router) { }

  ngOnInit() {
    this.usersService.gotUserTokenEvent.subscribe(userInfo => {
      this.isAuthorized = true;
      this.userName = userInfo.userName;
      this.userRole = userInfo.userRole;
    });
    this.usersService.logoutEvent.subscribe(() => {
      this.isAuthorized = false;
      this.userName = null;
      this.userRole = null;
      this.router.navigate(['/home']);
    })
    this.isAuthorized = this.usersService.isUserLoggedIn();
    if (this.isAuthorized) {
      this.userName = this.usersService.getCurrentUserName();
      this.userRole = this.usersService.getCurrentUserRole();
    }
  }

  onLogout() {
    this.cookieService.delete('tokenCookie');
    this.usersService.raiseLogoutEvent();
  }
}
