import { Component, OnInit } from '@angular/core';
import { Http, Headers } from '@angular/http';


// ToDo //

@Component({
  selector: 'app-questions',
  templateUrl: './questions.component.html',
  styleUrls: ['./questions.component.css']
})
export class QuestionsComponent implements OnInit {

  questions: any[];

  constructor(private http: Http) { }

  ngOnInit() {
    let key = localStorage.getItem("token");
    if (key !== null) {
      const myHeaders = new Headers({
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + key
      })
      this.http.get("http://localhost:5000/api/Questions", { headers: myHeaders }).subscribe(response => {
        this.questions = response.json();
      })
    }
  }
}
