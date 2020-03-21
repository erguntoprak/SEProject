import { Router } from '@angular/router';
import { BaseService } from './../shared/base.service';
import { Injectable } from '@angular/core';
import { BehaviorSubject, throwError } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { UserModel, LoginModel, RegisterModel } from '../shared/models';
import { map } from 'rxjs/operators';
@Injectable({ providedIn: 'root' })
export class AuthService {
  public currentUserSubject: BehaviorSubject<UserModel>;
 
  constructor(private baseService:BaseService,private router:Router){
    this.currentUserSubject = new BehaviorSubject<UserModel>(JSON.parse(localStorage.getItem('currentUser')));
  }
  public get currentUserValue(){
    return this.currentUserSubject;
  }
  login(loginModel:LoginModel){
    return this.baseService.post<UserModel>("Account/Login", loginModel)
      .pipe(map(user => {
        if (user && user.token) {
          localStorage.setItem('currentUser', JSON.stringify(user));
          this.currentUserSubject.next(user);
        }
        return user;
      }));
  }
  logout() {
    this.router.navigate(['/']);
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }

  signup(registerModel:RegisterModel) {
    return this.baseService.post<RegisterModel>("Account/Register", registerModel);
  }
}
