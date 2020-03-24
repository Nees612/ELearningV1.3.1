import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { Headers } from '@angular/http';

@Injectable({
  providedIn: 'root'
})
export class HeadersService {

  constructor(private cookieService: CookieService) {

  }

  getHeaders() {
    return new Headers({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + this.cookieService.get('tokenCookie')
    });
  }
}
