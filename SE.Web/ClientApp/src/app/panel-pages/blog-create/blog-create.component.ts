import { Component, OnInit } from '@angular/core';
import { BaseService } from '../../shared/base.service';
import { AcdcLoadingService } from 'acdc-loading';
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'se-blog-create',
  templateUrl: './blog-create.component.html'
})
export class BlogCreateComponent implements OnInit {

  blogInsertForm: FormGroup;
  errorList = [];
  submitted = false;


  constructor(private baseService: BaseService, private formBuilder: FormBuilder, private acdcLoadingService: AcdcLoadingService, private router: Router, private toastr: ToastrService) {

  }
  ngOnInit(): void {
    this.blogInsertForm = this.formBuilder.group({
      title: ['', Validators.required],
      firstVisibleImageName : ['',Validators.required],
      blogItems: this.formBuilder.array([this.createBlogItem()])
    });
  }
  onInsertBlogSubmit() {
    this.submitted = true;
    if (this.blogInsertForm.invalid) {
      return;
    }
    this.acdcLoadingService.showLoading();
    this.baseService.post("Blog/InsertBlog", this.blogInsertForm.value).subscribe(data => {
      this.router.navigate(['/panel/blog-listesi']);
      this.toastr.success('Kayıt işlemi gerçekleşti.', 'Başarılı!');
      this.acdcLoadingService.hideLoading();
    });
  }

  createBlogItem() {
    return this.formBuilder.group({
      imageName: [''],
      description: ['']
    });
  }
  addBlogItem(): void {
    (this.blogInsertForm.get('blogItems') as FormArray).push(this.createBlogItem());
  }
  getBlogItemControl() {
    let blogItems = this.blogInsertForm.get('blogItems') as FormArray;
    return blogItems.controls;
  }
  //select image
  onFirstVisibleImageSelectFile(event) {
    if (event.target.files && event.target.files.length > 0) {
      var reader = new FileReader();
      const [file] = event.target.files;
      reader.readAsDataURL(file);
      reader.onload = (event: any) => {
        this.blogInsertForm.get('firstVisibleImageName').setValue(reader.result);
      }
    }
  }
  onSelectFile(event, i) {
    if (event.target.files && event.target.files.length > 0) {
      var reader = new FileReader();
      const [file] = event.target.files;
      reader.readAsDataURL(file);
      reader.onload = (event: any) => {
        (<FormGroup>(<FormArray>this.blogInsertForm.controls.blogItems).controls[i]).patchValue({
          imageName: reader.result
        });
      }
    }
  }
  removeImage(i) {
    (<FormGroup>(<FormArray>this.blogInsertForm.controls.blogItems).controls[i]).patchValue({
      imageName: ''
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
    translate: 'yes',
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
