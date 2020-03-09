import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { ModulesService } from '../services/modules.service';
import { UsersService } from '../services/users.service';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  header: string = "Welcome! Feel free to choose a module.";

  currentModuleId: number;

  isUserLoggedIn: boolean = false;

  isSelectedModule: boolean = false;

  modules: any[];

  constructor(private modulesService: ModulesService, private usersService: UsersService) { }

  ngOnInit() {
    if (this.usersService.isUserLoggedIn()) {
      this.isUserLoggedIn = true;
      this.modulesService.getAllModules().subscribe(response => {
        this.modules = response.json();
      }, _error => {
        alert('Your login has expeired please login again !');
        location.reload();
      });
    } else {
      this.header = "You need to login or create an account !"
    }
  }

  onModuleClick(id: number) {
    this.currentModuleId = id;
    this.isSelectedModule = true;
  }

  onModuleCancel() {
    this.isSelectedModule = false;
  }

}
