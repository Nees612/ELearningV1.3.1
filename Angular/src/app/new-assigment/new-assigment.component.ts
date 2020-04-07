import { Component, OnInit } from '@angular/core';
import { ModulesService } from '../services/modules.service';
import { AssigmentsService } from '../services/assigments.service';
import { UsersService } from '../services/users.service';

@Component({
  selector: 'app-new-assigment',
  templateUrl: './new-assigment.component.html',
  styleUrls: ['./new-assigment.component.css']
})
export class NewAssigmentComponent implements OnInit {

  modules: any[];
  selectedModuleId: number = 1;

  isAdminLoggedIn: boolean = false;

  title: string;
  description: string;
  url: string;

  errors: any[] = [];

  constructor(private modulesService: ModulesService, private assigmentsService: AssigmentsService, private usersService: UsersService) { }

  ngOnInit() {
    this.usersService.isUserLoggedIn().subscribe(() => {
      if (this.usersService.isAdmin) {
        this.isAdminLoggedIn = true;
        this.modulesService.getAllModules().subscribe(response => {
          this.modules = response.json();
          this.modules.pop();
        });
      }
    }, () => {
      this.usersService.raiseLogoutEvent();
      this.isAdminLoggedIn = false;
    });
  }


  onSubmit() {
    this.errors = [];
    let newAssigment = {
      title: this.title,
      description: this.description,
      url: this.url,
      moduleId: +this.selectedModuleId
    }
    this.assigmentsService.addAssigment(newAssigment).subscribe(() => {
      alert('Assigment added succesfully.');
    }, error => {
      let errors = error.json().errors;
      for (let key in errors) {
        let value = errors[key];
        this.errors.push(value);
      }
    });
  }
}
