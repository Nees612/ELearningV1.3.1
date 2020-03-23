import { Component, OnInit } from '@angular/core';
import { AssigmentsService } from '../services/assigments.service';
import { UsersService } from '../services/users.service';
import { environment } from '../../environments/environment';

@Component({
  selector: 'app-assigments',
  templateUrl: './assigments.component.html',
  styleUrls: ['./assigments.component.css']
})
export class AssigmentsComponent implements OnInit {

  isCollapsed: boolean;

  prgBasicAssigments: any[];
  webAssigments: any[];
  oopAssigments: any[];

  constructor(private assigmentsService: AssigmentsService, private usersService: UsersService) { }

  ngOnInit() {
    if (this.usersService.isUserLoggedIn) {
      this.assigmentsService.getAssigmentsByModuleName(environment.PROGRAMMING_BASICS).subscribe(response => {
        this.prgBasicAssigments = response.json().assigments;
      })
      this.assigmentsService.getAssigmentsByModuleName(environment.WEB_TECHNOLOGIES).subscribe(response => {
        this.webAssigments = response.json().assigments;
      })
      this.assigmentsService.getAssigmentsByModuleName(environment.OOP).subscribe(response => {
        this.oopAssigments = response.json().assigments;
      })
    }
  }

}
