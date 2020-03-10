import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { environment } from '../../environments/environment';
import { UsersService } from './users.service';
import { HeadersService } from './headers.service';

@Injectable({
  providedIn: 'root'
})
export class AssigmentsService {

  constructor(private http: Http, private usersService: UsersService, private headersService: HeadersService) { }

  getAllAssigments() {
    if (this.usersService.isUserLoggedIn()) {
      return this.http.get(environment.API_ASSIGMENTS_URL + '/All', { headers: this.headersService.Headers })
    }
  }
}
