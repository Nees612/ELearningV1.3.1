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

  isUserLoggedIn: boolean = false;
  isAdmin: boolean = false;

  prgBasicAssigments: any[];
  webAssigments: any[];
  oopAssigments: any[];

  constructor(private assigmentsService: AssigmentsService, private usersService: UsersService) { }

  ngOnInit() {
    this.usersService.isUserLoggedIn().subscribe(() => {
      this.isUserLoggedIn = true;
      this.isAdmin = this.usersService.isAdmin();
      this.getAllAssigmentSorted();
    }, () => {
      this.usersService.raiseLogoutEvent();
      this.isUserLoggedIn = false;
    })
  }

  private getAllAssigmentSorted() {
    this.assigmentsService.getAssigmentsByModuleName(environment.PROGRAMMING_BASICS).subscribe(response => {
      this.prgBasicAssigments = response.json();
    })
    this.assigmentsService.getAssigmentsByModuleName(environment.WEB_TECHNOLOGIES).subscribe(response => {
      this.webAssigments = response.json();
    })
    this.assigmentsService.getAssigmentsByModuleName(environment.OOP).subscribe(response => {
      this.oopAssigments = response.json();
    })
  }

  onDelete(id) {
    this.assigmentsService.deleteAssigment(id).subscribe(() => {
      this.getAllAssigmentSorted();
    })
  }

}
