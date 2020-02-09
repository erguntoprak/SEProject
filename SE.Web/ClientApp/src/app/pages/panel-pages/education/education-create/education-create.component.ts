import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray, FormControl, AbstractControl } from '@angular/forms';
import { v4 as uuid } from 'uuid';
import { BaseService } from '../../../../shared/base.service';
import { ResponseModel } from '../../../../shared/response-model';
import { NgxSpinnerService } from 'ngx-spinner';
import { AngularEditorConfig } from '@kolkov/angular-editor';

@Component({
  selector: 'se-education-create',
  templateUrl: './education-create.component.html'
})
export class EducationCreateComponent implements OnInit, AfterViewInit {

  educationForm: FormGroup;
  errorList = [];
  imageMaxSizeMessages = [];
  submitted = false;
  category: Array<Object>;
  attributeList: object;
  cityList: Array<Object> = [];
  districtList: Array<Object>;
  urlImages: KeyValueModel[] = [];
  questionItems: FormArray;
  nextStepOneControl: boolean = true;
  nextStepTwoControl: boolean = false;
  nextStepThreeControl: boolean = false;
  nextStepOneValidation: boolean = false;
  nextStepThreeValidation: boolean = false;



  constructor(private formBuilder: FormBuilder, private baseService: BaseService, private spinner: NgxSpinnerService) { }

  ngOnInit() {
    this.spinner.show();
    this.educationForm = this.formBuilder.group({
      generalInformation: this.formBuilder.group(
        {
          educationName: ['', Validators.required],
          educationType: [0, Validators.min(1)],
          description: [''],
        }
      ),
      attributes: this.formBuilder.array([]),
      images: this.formBuilder.array([]),
      addressInformation: this.formBuilder.group(
        {
          address: ['', Validators.required],
          cityId: [0, Validators.min(1)],
          districtId: [0, Validators.min(1)]
        }
      ),
      contactInformation: this.formBuilder.group(
        {
          authorizedName: ['', Validators.required],
          authorizedEmail: ['', [Validators.required, Validators.email]],
          phoneOne: ['', Validators.required],
          phoneTwo: ['', Validators.required],
          educationEmail: ['', [Validators.required, Validators.email]],
          educationWebsite: ['',Validators.required]
        }
      ),
      questions: this.formBuilder.array([this.createQuestionItem()])
    });
    this.getAllCallMethod();
  }
  ngAfterViewInit() {
    this.spinner.hide();
  }

  onSubmit() {
    this.spinner.show();
    this.submitted = false;
    if (this.educationForm.invalid) {
      this.nextStepThreeValidation = true;
      this.spinner.hide();
      return;
    }
    this.baseService.post<ResponseModel>("Education/AddEducation", this.educationForm.value).subscribe(data => {

    });
    this.spinner.hide();

  }

  createQuestionItem() {
    return this.formBuilder.group({
      question: '',
      answer :''
    });
  }
  addQuestionItem(): void {
    this.questionItems = this.educationForm.get('questions') as FormArray;
    this.questionItems.push(this.createQuestionItem());
  }

  //steps
  nextStepOneClick() {
    if (this.educationForm.controls.generalInformation.status == 'VALID' && this.urlImages.length > 0) {
      window.scroll(0, 0);
      let imagesData = this.urlImages.map(({ value }) => value);
      const images = <FormArray>this.educationForm.controls.images;
      for (let image of imagesData) {
        images.push(new FormControl(image));
      }
      this.nextStepOneControl = false;
      this.nextStepTwoControl = true;
    }
    else {
      this.nextStepOneValidation = true;
    }
  }
  nextStepTwoClick() {
    window.scroll(0, 0);
    this.nextStepTwoControl = false;
    this.nextStepThreeControl = true;
  }
  previousStepTwoClick() {
    window.scroll(0, 0);
    this.nextStepOneControl = true;
    this.nextStepTwoControl = false;
  }
  previousStepThreeClick() {
    window.scroll(0, 0);
    this.nextStepTwoControl = true;
    this.nextStepThreeControl = false;
  }

  //remove selected image
  removeImage(id) {
    this.urlImages = this.urlImages.filter(el => el.key !== id);
    var deleteImage = document.getElementById(id);
    deleteImage.remove();
  }
  //select image
  onSelectFile(event) {
    this.imageMaxSizeMessages = [];
    if (event.target.files && event.target.files[0]) {
      var filesAmount = event.target.files.length;
      for (let i = 0; i < filesAmount; i++) {
        var reader = new FileReader();
        if (event.target.files[i].size < 211111) {
          reader.onload = (event: any) => {
            this.urlImages.push({ key: uuid(), value: event.target.result });
          }
          reader.readAsDataURL(event.target.files[i]);
        }
        else {
          this.imageMaxSizeMessages.push(event.target.files[i].name);
        }

      }
    }
  }
  //Checkbox change checked type
  onChange(id: string, isChecked: boolean) {
    const attributes = <FormArray>this.educationForm.controls.attributes;
    if (isChecked) {
      attributes.push(new FormControl(id));
    } else {
      let index = attributes.controls.findIndex(x => x.value == id)
      attributes.removeAt(index);
    }
  }
  //Dropdown selectedId change attributes
  educationTypeOnChange(selectedId) {
    this.baseService.getAll<ResponseModel>("Attribute/GetAllAttributeByEducationCategoryId?categoryId=" + selectedId).subscribe(responseModel => {
      this.attributeList = responseModel.data;
      if (responseModel.errorMessage.length > 0) {
        this.errorList = this.errorList.concat(responseModel.errorMessage);
      }
    });
  }
  //Gel All Method
  getAllCallMethod() {
    this.baseService.getAll<ResponseModel>("Category/GetAllCategoryList").subscribe(responseModel => {
      this.category = responseModel.data;
      if (responseModel.errorMessage.length > 0) {
        this.errorList = this.errorList.concat(responseModel.errorMessage);
      }
    });
    this.baseService.getAll<ResponseModel>("Address/GetCityNameDistricts").subscribe(responseModel => {
      this.cityList.push(responseModel.data.cityDto);
      this.districtList = responseModel.data.districtDtoList;
      if (responseModel.errorMessage.length > 0) {
        this.errorList = this.errorList.concat(responseModel.errorMessage);
      }
    });
  }
  editorConfig: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '15rem',
    minHeight: '5rem',
    maxHeight: 'auto',
    width: 'auto',
    minWidth: '0',
    translate: 'no',
    enableToolbar: true,
    showToolbar: true,
    placeholder: 'Buraya metin giriniz...',
    defaultParagraphSeparator: 'p',
    defaultFontName: '',
    defaultFontSize: '',
    fonts: [
      
    ],
    customClasses: [
      {
        name: 'quote',
        class: 'quote',
      },
      {
        name: 'redText',
        class: 'redText'
      },
      {
        name: 'titleText',
        class: 'titleText',
        tag: 'h1',
      },
    ],
    uploadUrl: 'v1/image',
    sanitize: true,
    toolbarPosition: 'top',
    toolbarHiddenButtons: [
      ['bold', 'italic'],
      ['fontSize']
    ]
  };
}
