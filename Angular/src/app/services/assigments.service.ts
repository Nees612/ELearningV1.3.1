import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { environment } from '../../environments/environment';
import { HeadersService } from './headers.service';

@Injectable({
  providedIn: 'root'
})
export class AssigmentsService {

  constructor(private http: Http, private headersService: HeadersService) { }

  getAllAssigments() {
    return this.http.get(environment.API_ASSIGMENTS_URL + '/All', { headers: this.headersService.getHeaders() })
  }

  getAssigmentsByModuleName(moduleName: string) {
    return this.http.get(environment.API_ASSIGMENTS_URL + '/' + moduleName, { headers: this.headersService.getHeaders() })
  }

  getRandomAssigmentByModule(moduleName: string) {
    return this.http.get(environment.API_ASSIGMENTS_URL + '/Random/' + moduleName, { headers: this.headersService.getHeaders() })
  }

  addAssigment(newAssigment) {
    return this.http.post(environment.API_ASSIGMENTS_URL, newAssigment, { headers: this.headersService.getHeaders() })
  }

  deleteAssigment(id) {
    return this.http.delete(environment.API_ASSIGMENTS_URL + '/' + id, { headers: this.headersService.getHeaders() })
  }
}
