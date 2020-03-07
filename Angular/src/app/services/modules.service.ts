import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { CookieService } from 'ngx-cookie-service';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ModulesService {

  constructor(private http: Http, private cookieService: CookieService) { }

  getAllModules() {
    let token = this.cookieService.get('tokenCookie');
    const myHeaders = new Headers({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + token
    });
    return this.http.get(environment.API_MODULES_URL, { headers: myHeaders });
  }

  getModuleContentByModuleId(moduleId: number) {
    let token = this.cookieService.get('tokenCookie');
    const myHeaders = new Headers({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + token
    })
    return this.http.get(environment.API_MODULES_URL + '/' + moduleId, { headers: myHeaders })
  }
}

