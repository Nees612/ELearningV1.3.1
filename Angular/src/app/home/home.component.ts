import { Component, OnInit } from '@angular/core';
import { ModulesService } from '../services/modules.service';
import { UsersService } from '../services/users.service';
import { AssigmentsService } from '../services/assigments.service';
import { Router } from '@angular/router';
import { debug } from 'util';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  currentModuleId: number;

  isUserLoggedIn: boolean = false;
  userName: string;

  isSelectedModule: boolean = false;

  modules: any[];

  constructor(private modulesService: ModulesService, private usersService: UsersService, private assigmentsService: AssigmentsService, private router: Router) { }

  ngOnInit() {
    this.usersService.logoutEvent.subscribe(() => {
      this.isUserLoggedIn = false;
    });
    console.log(this.usersService.isUserLoggedIn())
    //danger
    if (this.usersService.isUserLoggedIn()) {
      this.isUserLoggedIn = true;
      this.userName = this.usersService.getCurrentUserName();
      this.getModules();
    } else {
      this.router.navigate(['/login']);
    }
  }

  private getModules() {
    debugger
    this.modulesService.getAllModules().subscribe(response => {
      this.modules = response.json().modules;
    });
  }

  onModuleClick(id: number) {
    this.currentModuleId = id;
    this.isSelectedModule = true;
  }

  onModuleCancel() {
    this.isSelectedModule = false;
  }

  onSolveRandomAssigment(moduleName: string) {
    this.assigmentsService.getRandomAssigmentByModule(moduleName).subscribe(response => {
      let url = response.json().randomAssigmentUrl;
      window.open(url, "_blank");
    })
  }

}
