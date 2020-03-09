import { Component, OnInit, EventEmitter, Output, Input } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { UsersService } from '../services/users.service';

@Component({
  selector: 'app-my-profile',
  templateUrl: './my-profile.component.html',
  styleUrls: ['./my-profile.component.css']
})
export class MyProfileComponent implements OnInit {

  isEditActive: boolean = false;

  oldUserName: string;
  oldEmail: string;
  oldPhoneNumber: string;
  userRole: string;

  errors: string[] = [];

  user: any;

  constructor(private usersService: UsersService) { }

  ngOnInit() {
    this.usersService.getCurrentUserData().subscribe(response => {
      this.user = response.json().user;
      this.oldUserName = this.user.userName;
      this.oldEmail = this.user.email;
      this.oldPhoneNumber = this.user.phoneNumber;
      this.userRole = this.user.role;
    });
  }

  onEditClick() {
    this.isEditActive = true;
  }

  onSaveClick() {
    this.errors = [];
    let data = {
      OldUserName: this.oldUserName,
      OldEmail: this.oldEmail,
      OldPhoneNumber: this.oldPhoneNumber,
      NewUsername: this.user.userName,
      NewEmail: this.user.email,
      NewPhoneNumber: this.user.phoneNumber
    }
    this.usersService.updateUser(data).subscribe(response => {
      this.oldUserName = this.user.userName;
      this.oldEmail = this.user.email;
      this.oldPhoneNumber = this.user.phoneNumber;
      this.isEditActive = false;
      this.usersService.raiseTokenEvent()
    }, error => {
      let errors = error.json().errors;
      for (let key in errors) {
        let value = errors[key];
        this.errors.push(value);
      }
    });
  }

  onCancel() {
    this.user.userName = this.oldUserName;
    this.user.email = this.oldEmail;
    this.user.phoneNumber = this.oldPhoneNumber;
    this.isEditActive = false;
  }

  onUploadPhoto() {

  }
}
