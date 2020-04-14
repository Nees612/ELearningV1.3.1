import { Component, OnInit } from '@angular/core';
import { UsersService } from '../services/users.service';
import { IUser } from '../Interfaces/IUser';
import { IUpdatedUser } from '../Interfaces/IUpdatedUser';

@Component({
  selector: 'app-my-profile',
  templateUrl: './my-profile.component.html',
  styleUrls: ['./my-profile.component.css']
})
export class MyProfileComponent implements OnInit {

  isEditActive: boolean = false;

  user: IUser;
  updatedUser: IUpdatedUser;

  errors: string[] = [];

  constructor(private usersService: UsersService) { }

  ngOnInit() {
    this.usersService.isUserLoggedIn().subscribe(() => {
      this.setUserData();
    }, () => {
      this.usersService.raiseLogoutEvent();
    })
  }

  private setUpdatedUserData() {
    this.updatedUser = {
      UserName: this.user.userName,
      Email: this.user.email,
      PhoneNumber: this.user.phoneNumber
    }
  }

  private setUserData() {
    this.usersService.getCurrentUserData().subscribe(response => {
      this.user = response.json();
      this.setUpdatedUserData();
    });
  }

  onEditClick() {
    this.isEditActive = true;
  }

  onSaveClick() {
    this.errors = [];
    this.usersService.updateUser(this.updatedUser).subscribe(() => {
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
    this.setUpdatedUserData();
    this.errors = [];
    this.isEditActive = false;
  }

  onUploadPhoto() {
    //ToDo
  }
}
