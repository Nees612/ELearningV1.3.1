import { Component, OnInit } from '@angular/core';
import { ModulesService } from '../services/modules.service';
import { AssigmentsService } from '../services/assigments.service';
import { UsersService } from '../services/users.service';
import { IModule } from '../Interfaces/IModule';
import { INewAssigment } from '../Interfaces/INewAssigment';
import { Router } from '@angular/router';

@Component({
  selector: 'app-new-assigment',
  templateUrl: './new-assigment.component.html',
  styleUrls: ['./new-assigment.component.css']
})
export class NewAssigmentComponent implements OnInit {

  modules: IModule[];
  selectedModuleId: number ;

  isAdminLoggedIn: boolean = false;

  newAssigment: INewAssigment = { Title: '', Description: '', Url: null, ModuleId: 0 };

  errors: any[] = [];

  constructor(private modulesService: ModulesService, private assigmentsService: AssigmentsService, private usersService: UsersService, private router: Router) { }

  ngOnInit() {
    this.usersService.isUserLoggedIn().subscribe(() => {
      if (this.usersService.isAdmin) {
        this.isAdminLoggedIn = true;
        this.modulesService.getAllModules().subscribe(response => {
          this.modules = response.json();
          this.selectedModuleId = this.modules[0].id;
        });
      }
    }, () => {
      this.usersService.raiseLogoutEvent();
      this.isAdminLoggedIn = false;
    });
  }


  onSubmit() {
    this.errors = [];
    this.newAssigment.ModuleId = +this.selectedModuleId;
    this.assigmentsService.addAssigment(this.newAssigment).subscribe(() => {
      alert('Assigment added succesfully.');
      this.router.navigate(['/assigments']);
    }, error => {
      let errors = error.json().errors;
      for (let key in errors) {
        let value = errors[key];
        this.errors.push(value);
      }
    });
  }
}
