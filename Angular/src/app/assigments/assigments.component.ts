import { Component, OnInit } from '@angular/core';
import { AssigmentsService } from '../services/assigments.service';
import { UsersService } from '../services/users.service';
import { IAssigment } from '../Interfaces/IAssigment';
import { ModulesService } from '../services/modules.service';
import { IModule } from '../Interfaces/IModule';

@Component({
  selector: 'app-assigments',
  templateUrl: './assigments.component.html',
  styleUrls: ['./assigments.component.css']
})
export class AssigmentsComponent implements OnInit {

  isUserLoggedIn: boolean = false;
  isAdmin: boolean = false;

  assigmentsByModule: { [title: string]: IAssigment; } = {};

  constructor(private assigmentsService: AssigmentsService, private usersService: UsersService, private modulesService: ModulesService) { }

  ngOnInit() {
    this.usersService.isUserLoggedIn().subscribe(() => {
      this.isUserLoggedIn = true;
      this.isAdmin = this.usersService.isAdmin();
      this.getAllAssigmentSortedByModule();
    }, () => {
      this.usersService.raiseLogoutEvent();
      this.isUserLoggedIn = false;
    })
  }

  private getAllAssigmentSortedByModule() {
    this.modulesService.getAllModules().subscribe(response => {
      let modules = response.json();
      modules = <IModule[]>modules;
      for (let module of modules) {
        this.assigmentsService.getAssigmentsByModuleName(module.title).subscribe(response => {
          this.assigmentsByModule[module.title] = response.json();
        })
      }
    })
  }

  onDelete(id) {
    this.assigmentsService.deleteAssigment(id).subscribe(() => {
      this.getAllAssigmentSortedByModule();
    })
  }

}
