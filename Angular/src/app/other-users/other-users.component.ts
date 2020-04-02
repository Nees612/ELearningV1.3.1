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

  isAdmin: boolean = false;
  isUserLoggedIn: boolean = false;


  constructor(private usersService: UsersService, private router: Router) { }

  ngOnInit() {
    this.usersService.isUserLoggedIn().subscribe(() => {
      this.isUserLoggedIn = true;
      this.isAdmin = this.usersService.isAdmin();
      this.getAdmins();
      this.getAllUsers();
    }, () => {
      this.usersService.raiseLogoutEvent();
      this.isUserLoggedIn = false;
      this.isAdmin = false;
    });
  }

  onDelete(id) {
    this.usersService.deleteUser(id).subscribe(() => {
      this.getAllUsers();
    });
  }

  onSeeProfile(id) {
    this.router.navigate(['/profile', { Id: id }]);
  }

  private getAllUsers() {
    this.getAdmins();
    this.getStudents();
  }

  private getAdmins() {
    this.usersService.getUsersByRole('Admin').subscribe(response => {
      this.admins = response.json();
    });
  }

  private getStudents() {
    this.usersService.getUsersByRole('Student').subscribe(response => {
      this.students = response.json();
    })
  }

}
