import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray, FormControl } from '@angular/forms';
import { v4 as uuid } from 'uuid';
import { BaseService } from '../../shared/base.service';
import { KeyValueModel, CategoryAttributeListModel, CategoryModel, AddressModel, CityModel, DistrictModel, ImageModel } from '../../shared/models';
import { NgxSpinnerService } from 'ngx-spinner';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { HttpErrorResponse } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AcdcLoadingService } from 'acdc-loading';

@Component({
  selector: 'se-education-edit',
  templateUrl: './education-edit.component.html'
})
export class EducationEditComponent implements OnInit, AfterViewInit {

  educationForm: FormGroup;
  errorList = [];
  categories: CategoryModel[];
  savedImageList: ImageModel[];
  educationId: number;
  selectedImageModel: ImageModel = { id: 0, firstVisible: false, imageUrl: "", title: "",educationId:0};
  isSelectFirstVisibleImage: boolean = false;
  attributeList: CategoryAttributeListModel[];
  existingAttributeIds: number[];
  city: CityModel;
  districtList: DistrictModel[];
  urlImages: KeyValueModel[] = [];
  questionItems: FormArray;
  nextStepOneControl: boolean = true;
  nextStepTwoControl: boolean = false;
  nextStepThreeControl: boolean = false;
  nextStepOneValidation: boolean = false;
  nextStepThreeValidation: boolean = false;
  nextStepFourValidation: boolean = false;



  constructor(private formBuilder: FormBuilder, private baseService: BaseService, private router: Router, private toastr: ToastrService, private acdcLoadingService: AcdcLoadingService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.educationForm = this.formBuilder.group({
      generalInformation: this.formBuilder.group(
        {
          id: [0, Validators.required],
          educationName: ['', Validators.required],
          educationType: [0, Validators.min(1)],
          description: [''],
        }
      ),
      attributes: this.formBuilder.array([]),
      images: this.formBuilder.array([]),
      userId: new FormControl(),
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
          educationWebsite: ['', Validators.required]
        }
      ),
      questions: this.formBuilder.array([])
    });
    this.getAllCallMethod();
    this.route.params.subscribe(params => {
      this.acdcLoadingService.showLoading();
      this.baseService.getByName<any>("Education/GetEducationUpdateModelBySeoUrl?seoUrl=", params['name']).subscribe(educationUpdateModel => {
        this.getAttributeList(educationUpdateModel.generalInformation.educationType)
        this.educationForm.patchValue({
          generalInformation: educationUpdateModel.generalInformation,
          addressInformation: educationUpdateModel.addressInformation,
          contactInformation: educationUpdateModel.contactInformation,
          userId:educationUpdateModel.userId
        });
        const images = <FormArray>this.educationForm.controls.images;
        educationUpdateModel.images.forEach(image => {
          this.urlImages.push({
            key: uuid(), value: `https://localhost:44362/images/${image}_1000x600.jpg`});
          images.push(new FormControl(image));
        });
        this.existingAttributeIds = educationUpdateModel.attributes;

        educationUpdateModel.attributes.forEach(attributeId => {
          this.onChange(attributeId, true);
        });
        educationUpdateModel.questions.forEach(question => {
          this.addExistingQuestionItem(question.question, question.answer);
        });
        this.acdcLoadingService.hideLoading();
      });
    });
  }
  ngAfterViewInit() {
  }

  onSubmit() {
    this.acdcLoadingService.showLoading();
    if (this.educationForm.invalid) {
      this.nextStepThreeValidation = true;
      this.acdcLoadingService.hideLoading();
      return;
    }
    this.baseService.post("Education/UpdateEducation", this.educationForm.value).subscribe(educationId => {
      this.educationId = educationId;
      this.baseService.getById<ImageModel[]>("Education/GetEducationImagesByEducationId?educationId=", educationId).subscribe(imageModel => {
        this.savedImageList = imageModel;
        if (imageModel.filter(d => d.firstVisible == true)[0] !== undefined) {
          this.selectedImageModel = imageModel.filter(d => d.firstVisible == true)[0];
        }
        this.nextStepThreeControl = false;
        this.nextStepFourValidation = true;
        this.acdcLoadingService.hideLoading();
      });
    },
      error => {
        console.log(error);
      });
  }
  saveSelectedFirsImage() {
    if (this.selectedImageModel.id === 0) {
      this.isSelectFirstVisibleImage = true;
      return;
    }
    this.acdcLoadingService.showLoading();
    this.baseService.post("Education/UpdateFirstVisibleImage", this.selectedImageModel).subscribe(data => {
      this.router.navigate(['/panel/egitimler']);
      this.toastr.success('Güncelleme işlemi gerçekleşti.', 'Başarılı!');
      this.acdcLoadingService.hideLoading();
    });
  }
  getQuestionControl() {
    let questionItems = this.educationForm.get('questions') as FormArray;
    return questionItems.controls;
  }
  createQuestionItem() {
    return this.formBuilder.group({
      question: '',
      answer: ''
    });
  }
  addQuestionItem(): void {
    this.questionItems = this.educationForm.get('questions') as FormArray;
    this.questionItems.push(this.createQuestionItem());
  }
  addExistingQuestionItem(question: string, answer: string) {
    this.questionItems = this.educationForm.get('questions') as FormArray;
    this.questionItems.push(this.formBuilder.group({
      question: question,
      answer: answer
    }));
  }
  //steps
  nextStepOneClick() {
    if (this.educationForm.controls.generalInformation.status == 'VALID' && this.urlImages.length > 0) {
      window.scroll(0, 0);
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
  removeImage(id, value) {
    const images = <FormArray>this.educationForm.controls.images;
    this.urlImages = this.urlImages.filter(el => el.key !== id);
    var deleteImage = document.getElementById(id);
    deleteImage.remove();
    let index = images.controls.findIndex(x => x.value == value);
    images.removeAt(index);
  }
  selectFirstVisibleImage(imageModel: ImageModel) {
    this.selectedImageModel = imageModel;
  }
  //select image
  onSelectFile(event) {
    const images = <FormArray>this.educationForm.controls.images;
    if (event.target.files && event.target.files[0]) {
      var filesAmount = event.target.files.length;
      for (let i = 0; i < filesAmount; i++) {
        var reader = new FileReader();
          reader.onload = (event: any) => {
            this.urlImages.push({ key: uuid(), value: event.target.result });
            images.push(new FormControl(event.target.result));
          }
          reader.readAsDataURL(event.target.files[i]);
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
  educationTypeOnChange(event) {
    var categoryId = event.target.value.split(": ");
    this.baseService.getAll<CategoryAttributeListModel[]>("Attribute/GetAllAttributeByEducationCategoryId?categoryId=" + categoryId[1]).subscribe(attributeList => {
      this.attributeList = attributeList;
    });
  }
  getAttributeList(categoryId) {
    this.baseService.getAll<CategoryAttributeListModel[]>("Attribute/GetAllAttributeByEducationCategoryId?categoryId=" + categoryId).subscribe(attributeList => {
      this.attributeList = attributeList;
    });
  }
  //Gel All Method
  getAllCallMethod() {
    this.baseService.getAll<CategoryModel[]>("Category/GetAllCategoryList").subscribe(categories => {
      this.categories = categories;
    });
    this.baseService.getAll<AddressModel>("Address/GetCityNameDistricts").subscribe(addressModel => {
      this.city = addressModel.cityModel;
      this.districtList = addressModel.districtListModel;
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
      [
        'subscript',
        'superscript',
        'justifyFull',
        'heading',
        'fontName'],
      [
        'customClasses',
        'insertImage',
        'insertVideo',
        'insertHorizontalRule',
        'toggleEditorMode'
      ]
    ]
  };

}
