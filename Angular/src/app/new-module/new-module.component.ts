import { Component, OnInit } from '@angular/core';
import { ModulesService } from '../services/modules.service';
import { UsersService } from '../services/users.service';
import { INewModule } from '../Interfaces/INewModule';
import { Router } from '@angular/router';

@Component({
  selector: 'app-new-module',
  templateUrl: './new-module.component.html',
  styleUrls: ['./new-module.component.css']
})
export class NewModuleComponent implements OnInit {

  isAdminLoggedIn: boolean = false;

  newModule: INewModule = { Title: null, Description: null };

  errors: any[] = [];

  constructor(private modulesService: ModulesService, private usersService: UsersService, private router: Router) { }

  ngOnInit(): void {
    this.usersService.isUserLoggedIn().subscribe(() => {
      this.isAdminLoggedIn = this.usersService.isAdmin();
    })
  }

  onSubmit() {
    this.errors = [];
    this.modulesService.addModule(this.newModule).subscribe(() => {
      alert('Module added succesfully.');
      this.router.navigate(['/home']);
    }, error => {
      let errors = error.json().errors;
      for (let key in errors) {
        let value = errors[key];
        this.errors.push(value);
      }
    });
  }

}
