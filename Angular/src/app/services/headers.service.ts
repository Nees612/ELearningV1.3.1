import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { Headers } from '@angular/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class HeadersService {

  constructor(private cookieService: CookieService) {

  }

  getHeaders() {
    return new Headers({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + this.cookieService.get(environment.COOKIE_ID)
    });
  }
}
