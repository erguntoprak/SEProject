import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpParams,
  HttpHeaders
} from '@angular/common/http';
import { take, exhaustMap } from 'rxjs/operators';

import { AuthService } from './auth.service';

@Injectable()
export class AuthInterceptorService implements HttpInterceptor {

  constructor(private authService: AuthService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    return this.authService.user.pipe(
      take(1),
      exhaustMap(user => {
        let headers = new HttpHeaders();
        headers = headers.set('Content-Type', 'application/json');
        if (!user) {
          const modifiedReq = req.clone({
            headers: headers
          });
          return next.handle(modifiedReq);
        }
        headers = headers.set('Authorization', `Bearer ${user.token}`);
        const modifiedReq = req.clone({
          headers: headers
        });
        return next.handle(modifiedReq);
      })
    );
  }
}
