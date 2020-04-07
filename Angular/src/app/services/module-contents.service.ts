import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { environment } from '../../environments/environment';
import { HeadersService } from './headers.service';

@Injectable({
  providedIn: 'root'
})
export class ModuleContentsService {

  constructor(private http: Http, private headersService: HeadersService) { }

  getModuleContentByModuleId(moduleId: number) {
    return this.http.get(environment.API_MODULECONTENTS_URL + '/' + moduleId, { headers: this.headersService.getHeaders() })
  }

  getAllModuleContents() {
    return this.http.get(environment.API_MODULECONTENTS_URL + '/AllModuleContents', { headers: this.headersService.getHeaders() });
  }

  addModuleContent(moduleContent) {
    return this.http.post(environment.API_MODULECONTENTS_URL, moduleContent, { headers: this.headersService.getHeaders() });
  }

  deleteModuleContent(moduleContentId) {
    return this.http.delete(environment.API_MODULECONTENTS_URL + '/' + moduleContentId, { headers: this.headersService.getHeaders() })
  }
}
