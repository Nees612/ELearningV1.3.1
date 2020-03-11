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
    return this.http.get(environment.API_ASSIGMENTS_URL + '/All', { headers: this.headersService.Headers })

  }

  getRandomAssigmentByModule(moduleName: string) {
    return this.http.get(environment.API_ASSIGMENTS_URL + '/Random/' + moduleName, { headers: this.headersService.Headers })
  }
}
