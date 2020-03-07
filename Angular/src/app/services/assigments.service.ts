import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { CookieService } from 'ngx-cookie-service';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AssigmentsService {

  constructor(private http: Http, private cookieService: CookieService) { }

  getAllAssigments() {
    let token = this.cookieService.get('tokenCookie');
    const myHeaders = new Headers({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + token
    });
    return this.http.get(environment.API_ASSIGMENTS_URL + '/All', { headers: myHeaders })
  }
}
