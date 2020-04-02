import { Component, OnInit } from '@angular/core';
import { UsersService } from '../services/users.service';

@Component({
  selector: 'app-my-profile',
  templateUrl: './my-profile.component.html',
  styleUrls: ['./my-profile.component.css']
})
export class MyProfileComponent implements OnInit {

  isEditActive: boolean = false;

  errors: string[] = [];

  userName: string;
  email: string;
  phoneNumber: string;
  role: string;

  newUserName: string;
  newEmail: string;
  newPhoneNumber: string;
  newRole: string;

  constructor(private usersService: UsersService) { }

  ngOnInit() {
    this.usersService.isUserLoggedIn().subscribe(() => {
      this.setUserData();

    }, () => {
      this.usersService.raiseLogoutEvent();
    })
  }

  private setUserData() {
    this.usersService.getCurrentUserData().subscribe(response => {
      let user = response.json();
      this.newUserName = this.userName = user.userName;
      this.newEmail = this.email = user.email;
      this.newPhoneNumber = this.phoneNumber = user.phoneNumber;
      this.role = user.role;
    });
  }

  onEditClick() {
    this.isEditActive = true;
  }

  onSaveClick() {
    this.errors = [];
    let data = {
      UserName: this.newUserName,
      Email: this.newEmail,
      PhoneNumber: this.newPhoneNumber,
    }
    this.usersService.updateUser(data).subscribe(() => {
      this.usersService.raiseTokenEvent();
      this.setUserData();
      this.isEditActive = false;
    }, error => {
      let errors = error.json().errors;
      for (let key in errors) {
        let value = errors[key];
        this.errors.push(value);
      }
    });
  }

  onCancel() {
    this.newUserName = this.userName;
    this.newEmail = this.email;
    this.newPhoneNumber = this.phoneNumber;
    this.errors = [];
    this.isEditActive = false;
  }

  onUploadPhoto() {
    //ToDo
  }
}
