import { Router } from '@angular/router';
import { AuthService } from './../../_services/auth.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators, FormControl, FormGroupDirective } from '@angular/forms';
import { BaseService } from '../../shared/base.service';
import { NgxSpinnerService } from "ngx-spinner";
import { LoginModel, RegisterModel } from '../../shared/models';
import { HttpErrorResponse } from '@angular/common/http';
import { AcdcLoadingService } from 'acdc-loading';


@Component({
  selector: 'se-login',
  templateUrl: './login.component.html'
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup;
  errorList = [];
  loginModel: LoginModel;
  submitted = false;
  constructor(private formBuilder: FormBuilder, private authService: AuthService, private router: Router, private acdcLoadingService: AcdcLoadingService) { }

  ngOnInit() {
    this.acdcLoadingService.hideLoading();
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
  }
  onLoginSubmit() {
    this.submitted = true;

    this.loginModel = {
      email: this.loginForm.value.email,
      password: this.loginForm.value.password
    };
    this.authService.login(this.loginModel).subscribe(
      data => {
        this.router.navigate(['/panel']);
      },
      (error : HttpErrorResponse) => {
        console.log(error);
      }
    );
  }
}
