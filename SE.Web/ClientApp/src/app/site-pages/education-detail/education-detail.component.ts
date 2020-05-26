import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { EducationContactFormInsertModel} from '../../shared/models';
import { BaseService } from '../../shared/base.service';
import { ActivatedRoute } from '@angular/router';
import { AcdcLoadingService } from 'acdc-loading';
declare var $: any;
@Component({
  selector: 'se-educacation-detail',
  templateUrl: './education-detail.component.html'
})
export class EducationDetailComponent implements OnInit, AfterViewInit {

  educationDetailModel: any;
  imageObject: Array<object> = [];
  contactForm: FormGroup;
  submitted = false;
  contactFormSuccessMessage = false;
  contactFormDiv = true;
  educationContactFormInsertModel: EducationContactFormInsertModel;
  constructor(private formBuilder: FormBuilder, private baseService: BaseService, private route: ActivatedRoute, private acdcLoadingService: AcdcLoadingService) {
  }
  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.acdcLoadingService.showLoading();
      this.baseService.getByName("Education/GetEducationDetailModelBySeoUrl?seoUrl=", params['name']).subscribe(data => {
        this.educationDetailModel = data;
        this.educationDetailModel.blogList.map(blog => {
          blog.firstVisibleImageName = `https://localhost:44362/images/blog/${blog.firstVisibleImageName}_300x180.jpg`
        });
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
    this.contactForm = this.formBuilder.group({
      nameSurname: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', Validators.required]
    });
    
  }
  ngAfterViewInit(): void {
  }
  onContactFormSubmit() {
    this.submitted = true;
    if (this.contactForm.invalid) {
      return;
    }
    this.educationContactFormInsertModel = {
      nameSurname: this.contactForm.controls['nameSurname'].value,
      email: this.contactForm.controls['email'].value,
      phoneNumber: this.contactForm.controls['phoneNumber'].value,
      educationId: this.educationDetailModel.generalInformation.id,
      createDateTime: null
    };
    this.baseService.post("Education/InsertContactForm", this.educationContactFormInsertModel).subscribe(data => {
      this.contactFormSuccessMessage = true;
      this.contactFormDiv = false;
    });

  }
 
  blankLinkClick(url) {
    window.open(url,"_blank");
  }
  
 
}
