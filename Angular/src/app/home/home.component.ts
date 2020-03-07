import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { ModulesService } from '../services/modules.service';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  header: string = "Welcome! Feel free to choose a module.";

  modules: any[];

  @Output() selectChange = new EventEmitter();

  constructor(private modulesService: ModulesService, private cookieService: CookieService) { }

  ngOnInit() {
    if (this.cookieService.check('tokenCookie')) {
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

  onModuleClick(id) {
    this.selectChange.emit(id);
  }

}
