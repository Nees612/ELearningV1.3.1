import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { environment } from '../../environments/environment';
import { HeadersService } from './headers.service';

@Injectable({
  providedIn: 'root'
})
export class ModulesService {

  constructor(private http: Http, private headersService: HeadersService) { }

  getAllModules() {
    return this.http.get(environment.API_MODULES_URL, { headers: this.headersService.getHeaders() });
  }


}

