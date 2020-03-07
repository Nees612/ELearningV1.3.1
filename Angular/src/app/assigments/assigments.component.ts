import { Component, OnInit } from '@angular/core';
import { AssigmentsService } from '../services/assigments.service';

@Component({
  selector: 'app-assigments',
  templateUrl: './assigments.component.html',
  styleUrls: ['./assigments.component.css']
})
export class AssigmentsComponent implements OnInit {

  assigments: any[];

  constructor(private assigmentsService: AssigmentsService) { }

  ngOnInit() {
    this.assigmentsService.getAllAssigments().subscribe(response => {
      this.assigments = response.json().assigments;
    });
  }

}
