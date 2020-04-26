import { Component, OnInit, AfterViewInit, HostListener } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CategoryModel, EducationListModel, DistrictModel, AddressModel } from '../../shared/models';
import { BaseService } from '../../shared/base.service';
import { ActivatedRoute } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { SwiperConfigInterface } from 'ngx-swiper-wrapper';
import { AcdcLoadingService } from 'acdc-loading';
declare var $: any;
@Component({
  selector: 'se-educacation-detail',
  templateUrl: './education-detail.component.html'
})
export class EducationDetailComponent implements OnInit, AfterViewInit {

  educationDetailModel: any;
  imageObject: Array<object> = [];
  constructor(private formBuilder: FormBuilder, private baseService: BaseService, private route: ActivatedRoute, private acdcLoadingService: AcdcLoadingService) {
  }
  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.acdcLoadingService.showLoading();
      this.baseService.getByName("Education/GetEducationDetailModelBySeoUrl?seoUrl=", params['name']).subscribe(data => {
        this.educationDetailModel = data;
        setTimeout(() => {
          this.educationDetailModel.images.forEach(image => {
            this.imageObject.push({
              image: `https://localhost:44362/images/${image}_1000x600.jpg`,
              thumbImage: `https://localhost:44362/images/${image}_1000x600.jpg`
            });
            this.acdcLoadingService.hideLoading();
          });
        }, 500)
        
      })
    });
  }
  ngAfterViewInit(): void {
  }

  onSearchSubmit() {

  }
  blankLinkClick(url) {
    window.open(url,"_blank");
  }
  getAllCallMethod():void{
    
  }
 
}
