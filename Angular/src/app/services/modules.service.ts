import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { environment } from '../../environments/environment';
import { UsersService } from './users.service';
import { HeadersService } from './headers.service';
import { debug } from 'util';

@Injectable({
  providedIn: 'root'
})
export class ModulesService {

  constructor(private http: Http, private usersService: UsersService, private headersService: HeadersService) { }

  getAllModules() {
    if (this.usersService.isUserLoggedIn()) {
      return this.http.get(environment.API_MODULES_URL, { headers: this.headersService.Headers });
    }
  }

  getModuleContentByModuleId(moduleId: number) {
    if (this.usersService.isUserLoggedIn()) {
      return this.http.get(environment.API_MODULES_URL + '/' + moduleId, { headers: this.headersService.Headers })
    }
  }
}

