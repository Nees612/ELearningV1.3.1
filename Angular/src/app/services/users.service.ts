import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { CookieService } from 'ngx-cookie-service';
import { environment } from '../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class UsersService {

  constructor(private http: Http, private cookieService: CookieService) { }

  getAllUsers() {
    let token = this.cookieService.get('tokenCookie');
    const myHeaders = new Headers({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + token
    })
    return this.http.get(environment.API_USERS_URL + '/All', { headers: myHeaders });
  }

  getUserData(userName: string) {
    return this.http.get(environment.API_USERS_URL + '/' + userName);
  }

  createUser(data) {
    return this.http.post(environment.API_USERS_URL + '/Registration', data);
  }

  loginUser(data) {
    return this.http.post(environment.API_USERS_URL + '/Login', data, { withCredentials: true });
  }

  deleteUser(userName: string) {
    let token = this.cookieService.get('tokenCookie');
    const myHeaders = new Headers({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + token
    })
    return this.http.delete(environment.API_USERS_URL + '/' + userName, { headers: myHeaders });
  }

  updateUser(data) {
    let token = this.cookieService.get('tokenCookie');
    const myHeaders = new Headers({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + token
    })
    return this.http.put(environment.API_USERS_URL, data, { headers: myHeaders, withCredentials: true });
  }

}
