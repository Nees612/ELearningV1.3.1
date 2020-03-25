import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { environment } from '../../environments/environment';
import { HeadersService } from './headers.service';
import { debug } from 'util';

@Injectable({
  providedIn: 'root'
})
export class ModulesService {

  constructor(private http: Http, private headersService: HeadersService) { }

  getAllModules() {
    return this.http.get(environment.API_MODULES_URL, { headers: this.headersService.getHeaders() });
  }

  getModuleContentByModuleId(moduleId: number) {
    return this.http.get(environment.API_MODULES_URL + '/' + moduleId, { headers: this.headersService.getHeaders() })
  }

  getAllModuleContents() {
    return this.http.get(environment.API_MODULES_URL + '/AllModuleContents', { headers: this.headersService.getHeaders() });
  }
}

