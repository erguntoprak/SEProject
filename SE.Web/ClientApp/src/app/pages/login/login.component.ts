import { Router } from '@angular/router';
import { AuthService } from './../../_services/auth.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators, FormControl, FormGroupDirective } from '@angular/forms';
import { BaseService } from '../../shared/base.service';
import { MustMatch } from '../../shared/helpers/must-match.validator';
import { NgxSpinnerService } from "ngx-spinner";




@Component({
    selector: 'se-login',
    templateUrl: './login.component.html'
})
export class LoginComponent implements OnInit {

    registerForm: FormGroup;
    loginForm: FormGroup;
    submittedLogin = false;
    submittedRegister = false;
    errorList = [];
    loginModel: LoginModel;
    registerModel: RegisterModel;
    constructor(private formBuilder: FormBuilder, private baseService: BaseService, private spinner: NgxSpinnerService, private authService: AuthService, private router: Router) { }

    ngOnInit() {
        this.loginForm = this.formBuilder.group({
            email: ['', [Validators.required, Validators.email]],
            password: ['', Validators.required],
        });

        this.registerForm = this.formBuilder.group({
            userName: ['', Validators.required],
            email: ['', [Validators.required, Validators.email]],
            phone: ['', [Validators.required]],
            password: ['', [Validators.required, Validators.minLength(6)]],
            confirmPassword: ['', [Validators.required, Validators.minLength(6)]]
        },
            {
                validators: MustMatch('password', 'confirmPassword')
            });
    }
    onLoginSubmit() {
        this.submittedLogin = true;
        this.spinner.show();
        if (this.loginForm.invalid) {
            this.spinner.hide();
            return;
        }
        this.loginModel = {
            email: this.loginForm.value.email,
            password: this.loginForm.value.password
        };
        this.authService.login(this.loginModel).subscribe(data => {
            this.spinner.hide();
            this.router.navigate(['/panel']);
        });
    }

    onRegisterSubmit() {
        debugger;
        this.submittedLogin = true;
        this.submittedRegister = true;
        if (this.registerForm.invalid) {
            return;
        }
        this.registerModel = {
            userName: this.registerForm.value.userName,
            email: this.registerForm.value.email,
            phone: this.registerForm.value.phone,
            password: this.registerForm.value.password
        }
        this.authService.signup(this.registerModel).subscribe(responseModel => {
            debugger;
            if (responseModel.errorMessage.length > 0) {
                for (var error in responseModel.errorMessage) 
                {
                    this.errorList.push(error);
                }  
            }
            this.submittedLogin = false;
            this.router.navigate(['/giris']);
        });

    }
}
