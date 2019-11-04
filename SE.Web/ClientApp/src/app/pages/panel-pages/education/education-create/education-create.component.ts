import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';
@Component({
  selector: 'se-education-create',
  templateUrl: './education-create.component.html'
})
export class EducationCreateComponent implements OnInit {
    educationForm: FormGroup;
    public Editor = ClassicEditor;
    submitted = false;
    category = [{"Text":'İlk Okul',"Value":1},
    {"Text":'Orta Okul',"Value":2},
    {"Text":'Lise',"Value":3},
    {"Text":'Üniversite',"Value":4},
    {"Text":'Dil Okulları',"Value":5}
  ];
    constructor(private formBuilder: FormBuilder) { }

    ngOnInit() {
        this.educationForm = this.formBuilder.group({
          educationName: ['asdasd', Validators.required],
          educationType : [0,Validators.required],
          description: ['',Validators.required]

        });
    }
    onSubmit() {
      debugger;
      this.submitted = true;
      if (this.educationForm.invalid) {
          return;
      }

      // display form values on success
      alert('SUCCESS!! :-)\n\n' + JSON.stringify(this.educationForm.value, null, 4));
  }
}
