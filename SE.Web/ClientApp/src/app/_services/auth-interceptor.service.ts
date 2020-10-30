import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpParams,

  HttpHeaders,
  HttpErrorResponse
} from '@angular/common/http';
import { take, exhaustMap, catchError } from 'rxjs/operators';
import Swal from 'sweetalert2';

import { AuthService } from './auth.service';
import { throwError } from 'rxjs';
import { Router } from '@angular/router';
import { AcdcLoadingService } from 'acdc-loading';

@Injectable()
export class AuthInterceptorService implements HttpInterceptor {

  constructor(private authService: AuthService, private router: Router, private acdcLoadingService: AcdcLoadingService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    return this.authService.currentUserSubject.pipe(
      take(1),
      exhaustMap(user => {
        let headers = new HttpHeaders();
        headers = headers.set('Content-Type', 'application/json');
        headers = headers.set('Access-Control-Allow-Origin', 'http://localhost:4200');

        if (!user) {
          const modifiedReq = req.clone({
            url: `https://localhost:44362/api/${req.url}`,
            headers: headers
          });
          return next.handle(modifiedReq);
        }
        headers = headers.set('Authorization', `Bearer ${user.token}`);
        const modifiedReq = req.clone({
          url: `https://localhost:44362/api/${req.url}`,
          headers: headers
        });
        return next.handle(modifiedReq).pipe(
          catchError((error: HttpErrorResponse) => {
            switch (error.status) {
              case 400:
                  Swal.fire({
                    icon: 'error',
                    title: 'Başarısız!',
                    text: error.error.message
                  });
                  this.acdcLoadingService.hideLoading();
                  break;
              case 404:      //login
                this.router.navigateByUrl("/giris-yap");
                this.acdcLoadingService.hideLoading();
                break;
              case 401:      //login
                this.router.navigateByUrl("/giris-yap");
                this.acdcLoadingService.hideLoading();
                break;
              case 403:     //forbidden
                this.router.navigateByUrl("/giris-yap");
                this.acdcLoadingService.hideLoading();
                break;
            }
            return throwError(error);
          }));
      })
    );
  }
}
