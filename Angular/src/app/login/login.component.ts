import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { UsersService } from '../services/users.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  username: string;
  password: string;

  errors: any[];

  @Output() cancel = new EventEmitter();

  constructor(private usersService: UsersService) {
  }

  ngOnInit() {
  }

  onSubmit() {
    this.errors = [];
    let data = {
      Username: this.username,
      Password: this.password
    }
    this.usersService.loginUser(data).subscribe(response => {
      alert("Login was successfull.");
      location.reload();
    }, error => {
      let errors = error.json().errors;
      for (let key in errors) {
        let value = errors[key];
        this.errors.push(value);
      }
    });
  }

  onCancel() {
    this.cancel.emit();
  }
}

