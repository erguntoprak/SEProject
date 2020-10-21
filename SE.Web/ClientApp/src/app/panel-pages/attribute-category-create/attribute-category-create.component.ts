import { Component, OnInit } from '@angular/core';
import { BaseService } from '../../shared/base.service';
import { AcdcLoadingService } from 'acdc-loading';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { MyErrorStateMatcher } from '../../_helpers/input-error-state-matcher';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';


@Component({
  selector: 'se-attribute-category-create',
  templateUrl: './attribute-category-create.component.html'
})
export class AttributeCategoryCreateComponent implements OnInit {

  attributeCategoryName: string;
  matcher = new MyErrorStateMatcher();
  attributeCategoryInsertForm: FormGroup;
  constructor(private formBuilder: FormBuilder, private baseService: BaseService, private acdcLoadingService: AcdcLoadingService, private toastr: ToastrService, private router: Router) {

  }
  ngOnInit(): void {
    this.attributeCategoryInsertForm = this.formBuilder.group({
      name: [null, Validators.required]
    });
  }

  onSubmit() {
    this.baseService.post("AttributeCategory/InsertAttributeCategory",
      this.attributeCategoryInsertForm.value).subscribe(data => {
        this.toastr.success('Özellik Kategori oluşturuldu.', 'Başarılı!');
        this.router.navigate(['/panel/ozellik-kategori-listesi']);
      },
        (error: HttpErrorResponse) => {
          this.toastr.success(error.error, 'Başarısız!');
          this.acdcLoadingService.hideLoading();
        });
  }
}

