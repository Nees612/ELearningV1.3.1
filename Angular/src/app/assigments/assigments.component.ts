import { Component, OnInit } from '@angular/core';
import { AssigmentsService } from '../services/assigments.service';
import { UsersService } from '../services/users.service';

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
      this.assigmentsService.getAllAssigments().subscribe(response => {
        let jsonReponse = response.json();
        this.prgBasicAssigments = jsonReponse.prgBasicAssigments;
        this.webAssigments = jsonReponse.webAssigments;
        this.oopAssigments = jsonReponse.oopAssigments;
      });
    }
  }

}
