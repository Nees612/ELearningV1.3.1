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
    this.isAuthorized = !(this.usersService.getCurrentUserName() === null && this.usersService.getCurrentUserRole() === null);
    if (this.isAuthorized) {
      this.userName = this.usersService.getCurrentUserName();
      this.userRole = this.usersService.getCurrentUserRole();
    }
  }

  onLogout() {
    this.cookieService.delete('tokenCookie');
    this.router.navigate(['/home'])
  }
}
