import { Component, OnInit } from '@angular/core';
import { UsersService } from '../services/users.service';
import { Route } from '@angular/compiler/src/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-other-users',
  templateUrl: './other-users.component.html',
  styleUrls: ['./other-users.component.css']
})
export class OtherUsersComponent implements OnInit {

  admins: any[];
  students: any[];

  isAdmin: boolean;


  constructor(private usersService: UsersService, private router: Router) { }

  ngOnInit() {
    this.isAdmin = this.usersService.isAdmin();
    this.getAdmins();
    this.getAllUsers();
  }

  onDelete(userName) {
    this.usersService.deleteUser(userName).subscribe(response => {
      this.getAllUsers();
    });
  }

  onSeeProfile(userName) {
    this.router.navigate(['/profile', { userName: userName }]);
  }

  private getAllUsers() {
    this.getAdmins();
    this.getStudents();
  }

  private getAdmins() {
    this.usersService.getUsersByRole('Admin').subscribe(response => {
      this.admins = response.json().users;
    });
  }

  private getStudents() {
    this.usersService.getUsersByRole('Student').subscribe(response => {
      this.students = response.json().users;
    })
  }

}
