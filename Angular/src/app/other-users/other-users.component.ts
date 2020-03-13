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
    this.getAllusers();
  }

  onDelete(userName) {
    this.usersService.deleteUser(userName).subscribe(response => {
      this.getAllusers();
    });
  }

  onSeeProfile(userName) {
    this.router.navigate(['/profile', { userName: userName }]);
  }

  private getAllusers() {
    this.usersService.getAllUsers().subscribe(response => {
      let jsonResponse = response.json();
      this.admins = jsonResponse.admins;
      this.students = jsonResponse.students;
    });
  }

}
