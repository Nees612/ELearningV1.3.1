import { Component, OnInit } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { UsersService } from '../services/users.service';
import * as jwt_decode from "jwt-decode";

@Component({
  selector: 'app-other-users',
  templateUrl: './other-users.component.html',
  styleUrls: ['./other-users.component.css']
})
export class OtherUsersComponent implements OnInit {

  allUsers: any[];
  isAdmin: boolean;

  selectedUser: any;

  constructor(private usersService: UsersService, private cookieService: CookieService) { }

  ngOnInit() {
    let decodedToken = jwt_decode(this.cookieService.get('tokenCookie'));
    if (decodedToken.user_role === "Admin") {
      this.isAdmin = true;
    }
    this.getAllusers();
  }

  onDelete(userName) {
    this.usersService.deleteUser(userName).subscribe(response => {
      this.getAllusers();
    });
  }

  onSeeProfile(userName) {
    this.usersService.getUserData(userName).subscribe(response => {
      this.selectedUser = response.json().user;
    });
  }

  private getAllusers() {
    this.usersService.getAllUsers().subscribe(response => {
      this.allUsers = response.json().users;
    });
  }

  onCancel() {
    this.selectedUser = null;
  }
}
