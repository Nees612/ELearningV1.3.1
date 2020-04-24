import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { environment } from '../../environments/environment';
import { HeadersService } from './headers.service';
import { INewModule } from '../Interfaces/INewModule';
import { IModule } from '../Interfaces/IModule';

@Injectable({
  providedIn: 'root'
})
export class ModulesService {

  constructor(private http: Http, private headersService: HeadersService) { }

  getAllModules() {
    return this.http.get(environment.API_MODULES_URL, { headers: this.headersService.getHeaders() });
  }

  addModule(module) {
    return this.http.post(environment.API_MODULES_URL, module, { headers: this.headersService.getHeaders() });
  }

  deleteModule(id: number) {
    return this.http.delete(environment.API_MODULES_URL + '/' + id, { headers: this.headersService.getHeaders() });
  }

  updateModule(module: IModule) {
    return this.http.put(environment.API_MODULES_URL + '/' + module.id, module, { headers: this.headersService.getHeaders() })
  }


}

