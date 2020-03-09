import { Component, OnInit } from '@angular/core';
import { UsersService } from '../services/users.service';

@Component({
  selector: 'app-other-users',
  templateUrl: './other-users.component.html',
  styleUrls: ['./other-users.component.css']
})
export class OtherUsersComponent implements OnInit {

  allUsers: any[];
  isAdmin: boolean;

  selectedUser: any;

  constructor(private usersService: UsersService) { }

  ngOnInit() {
    this.isAdmin = this.usersService.isAdmin();
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
