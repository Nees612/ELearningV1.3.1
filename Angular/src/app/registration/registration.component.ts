import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { UsersService } from '../services/users.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  username: string;
  email: string;
  phoneNumber: string;
  password: string;
  passwordAgain: string;

  errors: any[] = [];

  @Output() cancel = new EventEmitter();

  constructor(private usersService: UsersService, private router: Router) { }

  ngOnInit() {
  }

  onSubmit() {
    this.errors = [];
    if (this.password === this.passwordAgain) {
      let data = {
        Username: this.username === '' ? null : this.username,
        Email: this.email === '' ? null : this.email,
        PhoneNumber: this.phoneNumber === '' ? null : this.phoneNumber,
        Password: this.password === '' ? null : this.password
      }
      this.usersService.createUser(data).subscribe(response => {
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
