import { Component, OnInit } from '@angular/core';
import { BaseService } from '../../shared/base.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ResponseModel } from '../../shared/response-model';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'se-education-list',
  templateUrl: './education-list.component.html'
})
export class EducationListComponent implements OnInit {

  educationList = [];
  errorList = [];

  constructor(private baseService: BaseService, private spinner: NgxSpinnerService) {

  }
  ngOnInit(): void {
    this.baseService.getAll<ResponseModel>("Education/GetAllEducationList").subscribe(responseModel => {
      this.educationList = responseModel.data;
      if (responseModel.errorMessage.length > 0) {
       this.errorList.push(responseModel.errorMessage);
      }
    });
  }

}
