import { Component, OnInit } from '@angular/core';
import { ModulesService } from '../services/modules.service';
import { UsersService } from '../services/users.service';
import { AssigmentsService } from '../services/assigments.service';
import { Router } from '@angular/router';
import { IModule } from '../Interfaces/IModule';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  currentModuleId: number;

  isUserLoggedIn: boolean = false;
  userName: string;

  isAdmin: boolean = false;

  selectedModule: IModule;
  selectedForChangeOrder: IModule;

  modules: IModule[];

  collapsed: boolean = false;

  constructor(private modulesService: ModulesService, private usersService: UsersService, private assigmentsService: AssigmentsService, private router: Router) { }

  ngOnInit() {
    this.usersService.logoutEvent.subscribe(() => {
      this.isUserLoggedIn = false;
    })
    this.usersService.isUserLoggedIn().subscribe(() => {
      this.isUserLoggedIn = true;
      this.isAdmin = this.usersService.isAdmin();
      this.userName = this.usersService.getCurrentUserName();
      this.getModules();
    }, () => {
      this.usersService.raiseLogoutEvent();
      this.router.navigate(['/login']);
    });
  }

  private getModules() {
    this.modulesService.getAllModules().subscribe(response => {
      this.modules = response.json();
    });
  }

  onModuleClick(module: IModule) {
    this.selectedForChangeOrder = null;
    this.selectedModule = module;
    this.collapsed = true;
  }

  onModuleCancel() {
    this.selectedModule = null;
    this.collapsed = false;
  }

  onSolveRandomAssigment(moduleName: string) {
    this.assigmentsService.getRandomAssigmentByModule(moduleName).subscribe(response => {
      let url = response.json().url;
      window.open(url, "_blank");
    })
  }

  onChangeOrder(module: IModule) {
    this.selectedModule = null;
    this.selectedForChangeOrder = module;
  }

  onClosedChangeOrder() {
    this.selectedForChangeOrder = null;
  }

}
