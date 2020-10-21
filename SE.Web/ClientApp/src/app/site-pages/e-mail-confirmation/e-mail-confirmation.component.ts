import { Component, OnInit } from '@angular/core';
import { BaseService } from '../../shared/base.service';
import * as _ from 'lodash';
import { AcdcLoadingService } from 'acdc-loading';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'se-e-mail-confirmation',
  templateUrl: './e-mail-confirmation.component.html'
})
export class EmailConfirmationComponent implements OnInit {

  emailConfirmationForm: FormGroup;
  submitted = false;
  emailConfirmationFormSuccessMessage = false;
  emailConfirmationFormDiv = true;
  errorList: string[] = [];


  constructor(private baseService: BaseService, private formBuilder: FormBuilder, private acdcLoadingService: AcdcLoadingService) {

  }
  ngOnInit(): void {
    this.emailConfirmationForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }
  onEmailConfirmationFormSubmit() {
    this.acdcLoadingService.showLoading();
    this.submitted = true;
    if (this.emailConfirmationForm.invalid) {
      return;
    }
    this.baseService.post("Account/ResendEmailConfirmation", this.emailConfirmationForm.get('email').value).subscribe(data => {
      this.errorList = [];
      this.emailConfirmationFormSuccessMessage = true;
      this.emailConfirmationFormDiv = false;
      this.acdcLoadingService.hideLoading();
    },
      (error: HttpErrorResponse) => {
        this.errorList = [];
        this.errorList.push(error.error);
        this.errorList = [...this.errorList];
        this.acdcLoadingService.hideLoading();
      }
    );

  }
}
