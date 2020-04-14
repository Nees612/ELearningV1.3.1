import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { UsersService } from '../services/users.service';
import { Router } from '@angular/router';
import { INewUser } from '../Interfaces/INewUser';


@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  newUser: INewUser = { Username: '', Email: '', PhoneNumber: '', Password: '' }
  passwordAgain: string;

  errors: any[] = [];

  @Output() cancel = new EventEmitter();

  constructor(private usersService: UsersService, private router: Router) { }

  ngOnInit() {
  }

  onSubmit() {
    this.errors = [];
    if (this.newUser.Password === this.passwordAgain) {
      this.usersService.createUser(this.newUser).subscribe(() => {
        alert("Your account succesfully created !");
        this.router.navigate(['login']);
      }, error => {
        let errors = error.json().errors;
        for (let key in errors) {
          let value = errors[key];
          this.errors.push(value);
        }
      });
    } else {
      this.errors.push('Passwords do not match !');
    }
  }

  onCancel() {
    this.cancel.emit();
  }

}
