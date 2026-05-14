import {
  HTTP_INTERCEPTORS,
  HttpEvent,
  HttpHandler,
  HttpHeaders,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { StorageService } from '../Storage.Service';

import { Constants } from '../../Helpers/Constants';
import { Injectable } from '@angular/core';
@Injectable()
export class HeaderInterceptor implements HttpInterceptor {
  constructor(private storageService: StorageService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    req = req.clone({
      withCredentials: false,
      headers: headers,
    });

    return next.handle(req);
  }
}
export const httpInterceptorProviders = [
  { provide: HTTP_INTERCEPTORS, useClass: HeaderInterceptor, multi: true },
];
