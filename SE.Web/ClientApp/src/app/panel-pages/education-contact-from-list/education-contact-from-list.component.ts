import { Component, OnInit } from '@angular/core';
import { BaseService } from '../../shared/base.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { EducationListModel, EducationContactFormListModel } from '../../shared/models';
import Swal from 'sweetalert2';
import { HttpErrorResponse } from '@angular/common/http';
import * as _ from 'lodash';
import { AcdcLoadingService } from 'acdc-loading';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'se-education-contact-from-list',
  templateUrl: './education-contact-from-list.component.html'
})
export class EducationContactFormListComponent implements OnInit {

  educationContactFormList: EducationContactFormListModel[];

  constructor(private baseService: BaseService, private route: ActivatedRoute, private acdcLoadingService: AcdcLoadingService) {

  }
  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.baseService.getByName<EducationContactFormListModel[]>("Education/GetEducationContactFormListModelBySeoUrl?seoUrl=", params['name']).subscribe(educationContactFormList => {
        this.educationContactFormList = educationContactFormList;
      });
    });

  }
}
