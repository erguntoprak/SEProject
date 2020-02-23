import { Router } from '@angular/router';
import { BaseService } from './../shared/base.service';
import { Injectable } from '@angular/core';
import { BehaviorSubject, throwError } from 'rxjs';
import { ResponseModel } from '../shared/response-model';
import { catchError,tap } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';
import { User, LoginModel, RegisterModel } from '../shared/models';
@Injectable({ providedIn: 'root' })
export class AuthService {
  user : BehaviorSubject<User>;
 
  constructor(private baseService:BaseService,private router:Router){
    this.user = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('userData')));
  }

  login(loginModel:LoginModel){
    return this.baseService.post<ResponseModel>("Account/Login", loginModel)
    .pipe(
      catchError(this.handleError),
      tap(res => {
        this.handleAuthentication(
          res.data.id,
          res.data.name,
          res.data.surname,
          res.data.email,
          res.data.userName,
          res.data.token
        );
      })
    );
  }
  logout(){
    this.user.next(null);
    this.router.navigate(['/']);
    localStorage.removeItem('userData');
  }

  signup(registerModel:RegisterModel) {
    return this.baseService.post<ResponseModel>("Account/Register", registerModel)
    .pipe(
      catchError(this.handleError)
    );
  }


  private handleAuthentication(
    id: string,
    name: string,
    surname:string,
    email:string,
    userName:string,
    token: string
  ) {
    const user = {
      id:id,
      name:name,
      surname:surname,
      email:email,
      userName:userName,
      token:token
    }
    this.user.next(user);
    localStorage.setItem('userData', JSON.stringify(user));
  }
  private handleError(errorRes: HttpErrorResponse) {
    let errorMessage = 'An unknown error occurred!';
    if (!errorRes.error || !errorRes.error.error) {
      return throwError(errorMessage);
    }
    switch (errorRes.error.error.message) {
      case 'EMAIL_EXISTS':
        errorMessage = 'This email exists already';
        break;
      case 'EMAIL_NOT_FOUND':
        errorMessage = 'This email does not exist.';
        break;
      case 'INVALID_PASSWORD':
        errorMessage = 'This password is not correct.';
        break;
    }
    return throwError(errorMessage);
  }
}
