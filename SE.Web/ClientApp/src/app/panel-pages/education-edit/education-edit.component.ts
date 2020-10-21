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
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import * as _ from 'lodash';

@Component({
  selector: 'se-education-edit',
  templateUrl: './education-edit.component.html'
})
export class EducationEditComponent implements OnInit, AfterViewInit {

  educationForm: FormGroup;
  errorList = [];
  imageUploadErrorMessage = [];
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
  youtubeVideoOneId: string = "";
  youtubeVideoTwoId: string = "";
  iframeMapCode: SafeHtml = "";
  nextStepOneControl: boolean = true;
  nextStepOneValidation: boolean = false;
  nextStepTwoControl: boolean = false;
  nextStepThreeControl: boolean = false;
  nextStepThreeValidation: boolean = false;
  nextStepFourControl: boolean = false;
  nextStepFourValidation: boolean = false;
  nextStepFiveControl: boolean = false;



  constructor(private formBuilder: FormBuilder, private baseService: BaseService, private router: Router, private toastr: ToastrService, private acdcLoadingService: AcdcLoadingService, private route: ActivatedRoute, private sanitizer: DomSanitizer) { }

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
      socialInformation: this.formBuilder.group(
        {
          youtubeVideoOne: [''],
          youtubeVideoTwo: [''],
          facebookAccountUrl: [''],
          instagramAccountUrl: [''],
          twitterAccountUrl: [''],
          youtubeAccountUrl: [''],
          mapCode: ['']
        }),
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
          socialInformation: educationUpdateModel.socialInformation,
          userId:educationUpdateModel.userId
        });
        if (educationUpdateModel.socialInformation.mapCode != "") {
          this.iframeMapCode = this.sanitizer.bypassSecurityTrustHtml(educationUpdateModel.socialInformation.mapCode);
        }
        this.youtubeVideoOneId = educationUpdateModel.socialInformation.youtubeVideoOne.split("watch?v=")[1];
        this.youtubeVideoTwoId = educationUpdateModel.socialInformation.youtubeVideoTwo.split("watch?v=")[1];

        if (this.youtubeVideoOneId == undefined) {
          this.youtubeVideoOneId = '';
        }
        if (this.youtubeVideoTwoId == undefined) {
          this.youtubeVideoTwoId = '';
        }
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

    this.educationForm.get("socialInformation").get("youtubeVideoOne").valueChanges.subscribe(value => {
      if (value == "") {
        this.youtubeVideoOneId = "";
      }
      else {
        this.youtubeVideoOneId = value.split("watch?v=")[1];
      }
    });
    this.educationForm.get("socialInformation").get("youtubeVideoTwo").valueChanges.subscribe(value => {
      if (value == "") {
        this.youtubeVideoTwoId = "";
      }
      else {
        this.youtubeVideoTwoId = value.split("watch?v=")[1];
      }
    });
    this.educationForm.get("socialInformation").get("mapCode").valueChanges.subscribe(value => {
      if (value.startsWith("<iframe") && value.endsWith("</iframe>")) {
        this.iframeMapCode = this.sanitizer.bypassSecurityTrustHtml(value);
      }
      else if (value == "") {
        this.iframeMapCode = "";
      }
      else {
        this.iframeMapCode = null;
      }
    });
  }

  onSubmit() {
    this.acdcLoadingService.showLoading();
    if (this.educationForm.invalid) {
      this.nextStepFourValidation = true;
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
        this.nextStepFourControl = false;
        this.nextStepFiveControl = true;
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
  nextStepThreeClick() {
    if (this.iframeMapCode == null || this.youtubeVideoOneId == undefined || this.youtubeVideoTwoId == undefined) {
      this.nextStepThreeValidation = true;
      return;
    }
    window.scroll(0, 0);
    this.nextStepThreeControl = false;
    this.nextStepFourControl = true;
  }
  previousStepThreeClick() {
    window.scroll(0, 0);
    this.nextStepTwoControl = true;
    this.nextStepThreeControl = false;
  }
  previousStepFourClick() {
    window.scroll(0, 0);
    this.nextStepThreeControl = true;
    this.nextStepFourControl = false;
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
    this.imageUploadErrorMessage = [];
    const max_size = 1024000;

    if (event.target.files.length > 20) {
      this.imageUploadErrorMessage.push('En fazla 20 görsele izin verilmektedir.');
      this.imageUploadErrorMessage = [...this.imageUploadErrorMessage];
      return;
    }
    const images = <FormArray>this.educationForm.controls.images;
    const allowed_types = ['image/png', 'image/jpeg'];

    if (event.target.files && event.target.files[0]) {
      for (let i = 0; i < event.target.files.length; i++) {
        if (!_.includes(allowed_types, event.target.files[i].type)) {
          this.imageUploadErrorMessage.push('Sadece ( JPG | PNG ) uzantılar kabul edilmektedir.');
          this.imageUploadErrorMessage = [...this.imageUploadErrorMessage];
        }
        else {
          if (event.target.files[i].size >= max_size) {
            this.imageUploadErrorMessage.push(event.target.files[i].name + ' adlı görsel 1 MB değerinden büyüktür. 1 MB değerinden küçük veya eşit olmalıdır.');
            this.imageUploadErrorMessage = [...this.imageUploadErrorMessage];
          }
          else {
            var reader = new FileReader();
            reader.onload = (event: any) => {
              if (this.urlImages.length > 19) {
                this.imageUploadErrorMessage.push('En fazla 20 görsele izin verilmektedir.');
                this.imageUploadErrorMessage = [...this.imageUploadErrorMessage];
              }
              else {
                this.urlImages.push({ key: uuid(), value: event.target.result });
                images.push(new FormControl(event.target.result));
              }
            }
            reader.readAsDataURL(event.target.files[i]);
          }

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
