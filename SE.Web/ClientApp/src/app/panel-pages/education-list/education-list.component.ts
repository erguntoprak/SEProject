import { Component, OnInit } from '@angular/core';
import { BaseService } from '../../shared/base.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { EducationListModel } from '../../shared/models';
import Swal from 'sweetalert2';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'se-education-list',
  templateUrl: './education-list.component.html'
})
export class EducationListComponent implements OnInit {

  educationList : EducationListModel[];
  errorList = [];

  constructor(private baseService: BaseService, private spinner: NgxSpinnerService) {

  }
  ngOnInit(): void {
    this.baseService.getAll<EducationListModel[]>("Education/GetAllEducationList").subscribe(educationList => {
      this.educationList = educationList;
    });
  }
  deleteEducation(educationId: number) {
    Swal.fire({
      title: 'Silmek istediğinize emin misiniz ?',
      text: "Bu işlemi gerçekleştirdiğinizde geri alınamaz.",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#6754e2',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Evet, sil!',
      cancelButtonText:'Hayır'
    }).then((result) => {
      if (result.value) {
        this.baseService.delete("Education/DeleteEducation?educationId=", educationId).subscribe(data => {
          Swal.fire(
            'Silindi!',
            '',
            'success'
          )
        }, (error: HttpErrorResponse) => {this.errorList.push(error.error)}
        )
       
      }
    })
  }

}
