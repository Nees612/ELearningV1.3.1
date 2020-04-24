import { Component, OnInit } from '@angular/core';
import { UsersService } from '../services/users.service';
import { Router } from '@angular/router';
import { IUser } from '../Interfaces/IUser';
import { Role } from '../Roles/Role';

@Component({
  selector: 'app-other-users',
  templateUrl: './other-users.component.html',
  styleUrls: ['./other-users.component.css']
})
export class OtherUsersComponent implements OnInit {

  admins: IUser[];
  students: IUser[];

  isAdmin: boolean = false;
  isUserLoggedIn: boolean = false;

  currentUserName: string;


  constructor(private usersService: UsersService, private router: Router) { }

  ngOnInit() {
    this.usersService.isUserLoggedIn().subscribe(() => {
      this.isUserLoggedIn = true;
      this.isAdmin = this.usersService.isAdmin();
      this.currentUserName = this.usersService.getCurrentUserName();
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
    this.usersService.getUsersByRole(Role.Admin).subscribe(response => {
      this.admins = response.json();
    });
  }

  private getStudents() {
    this.usersService.getUsersByRole(Role.Student).subscribe(response => {
      this.students = response.json();
    })
  }

  onPromote(student: IUser) {
    this.usersService.promoteUser(student.id).subscribe(() => {
      this.getAllUsers();
    })
  }

  onDemote(admin: IUser) {
    this.usersService.demoteUser(admin.id).subscribe(() => {
      this.getAllUsers();
    })
  }
}
