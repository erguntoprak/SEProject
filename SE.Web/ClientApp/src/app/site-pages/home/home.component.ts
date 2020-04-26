import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CategoryModel, EducationListModel, DistrictModel, AddressModel } from '../../shared/models';
import { BaseService } from '../../shared/base.service';
declare var $: any;
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit, AfterViewInit {
   
  searchForm: FormGroup;
  categories: CategoryModel[];
  educationList: EducationListModel[];
  districtList: DistrictModel[];
  constructor(private formBuilder: FormBuilder,private baseService:BaseService) {
    this.searchForm = this.formBuilder.group({
      email: [''],
      password: ['']
    });
  }
  ngOnInit(): void {
    this.getAllCallMethod();
  }
  ngAfterViewInit(): void {
  }

  onSearchSubmit() {

  }

  getAllCallMethod() {
    this.baseService.getAll<CategoryModel[]>("Category/GetAllCategoryList").subscribe(categories => {
      this.categories = categories;
    });
    this.baseService.getAll<EducationListModel[]>("Education/GetAllEducationListByRandomCategoryId").subscribe(educationList => {
      this.educationList = educationList;
    });
    this.baseService.getAll<AddressModel>("Address/GetCityNameDistricts").subscribe(addressModel => {
      this.districtList = addressModel.districtListModel;
    });
  }
 
}
