import { Component, OnInit } from '@angular/core';
import { BaseService } from '../../shared/base.service';
import * as _ from 'lodash';
import { AcdcLoadingService } from 'acdc-loading';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'se-e-mail-confirmation-message',
  templateUrl: './e-mail-confirmation-message.component.html'
})
export class EmailConfirmationMessageComponent implements OnInit {

  isEmailConfirmationSuccessMessage = false;
  emailConfirmationSuccessMessage : string;
  errorList: string[] = [];


  constructor(private baseService: BaseService, private route: ActivatedRoute, private router:Router) {

  }
  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      if (params['userId'] && params['confirmation-token']) {
        this.baseService.post("Account/EmailConfirmation", { userId: params['userId'], confirmationToken: params['confirmation-token'] }).subscribe(message => {
          this.errorList = [];
          this.isEmailConfirmationSuccessMessage = true;
          this.emailConfirmationSuccessMessage = message.emailConfirmationSuccessMessage;
          setTimeout(() => {
            this.router.navigate(['/giris-yap']);
          }, 5000);
        },
          (error: HttpErrorResponse) => {
            this.errorList = [];
            this.errorList.push(error.error);
            this.errorList = [...this.errorList];
          }
        );
      }
    });
  }
}
