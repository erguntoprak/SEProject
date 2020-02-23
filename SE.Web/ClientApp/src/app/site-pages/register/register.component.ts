import { Router } from '@angular/router';
import { AuthService } from './../../_services/auth.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RegisterModel } from '../../shared/models';
import { MustMatch } from '../../_helpers/must-match.validator';




@Component({
    selector: 'se-register',
    templateUrl: './register.component.html'
})
export class RegisterComponent implements OnInit {

    registerForm: FormGroup;
    errorList = [];
    registerModel: RegisterModel;
    constructor(private formBuilder: FormBuilder, private authService: AuthService, private router: Router) { }

    ngOnInit() {
        this.registerForm = this.formBuilder.group({
            email: ['', [Validators.required, Validators.email]],
            phone: ['', [Validators.required]],
            password: ['', [Validators.required, Validators.minLength(6)]],
            confirmPassword: ['', [Validators.required, Validators.minLength(6)]]
        },
            {
                validators: MustMatch('password', 'confirmPassword')
            });
    }

    onRegisterSubmit() {
        if (this.registerForm.invalid) {
            return;
        }
        this.registerModel = {
            email: this.registerForm.value.email,
            phone: this.registerForm.value.phone,
            password: this.registerForm.value.password
        }
        this.authService.signup(this.registerModel).subscribe(responseModel => {
            if (responseModel.errorMessage.length > 0) {
                for (var error in responseModel.errorMessage) 
                {
                    this.errorList.push(error);
                }  
            }
            this.router.navigate(['/giris']);
        });

    }
}
