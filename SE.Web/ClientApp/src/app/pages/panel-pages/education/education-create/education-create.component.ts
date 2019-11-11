import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { v4 as uuid } from 'uuid';

@Component({
  selector: 'se-education-create',
  templateUrl: './education-create.component.html'
})
export class EducationCreateComponent implements OnInit {
    educationForm: FormGroup;
    submitted = false;
    category = [{"Text":'İlk Okul',"Value":1},
    {"Text":'Orta Okul',"Value":2},
    {"Text":'Lise',"Value":3},
    {"Text":'Üniversite',"Value":4},
    {"Text":'Dil Okulları',"Value":5}
  ];
  urlImages: KeyValueModel[] = [];
  physicalFacilities = ["Elevator in building", "Air Conditioned", "Free Wi Fi", "Free Parking on premises", "Instant Book","Pet Friendly"]
  removeImage(id) {
    this.urlImages = this.urlImages.filter(el => el.key !== id);
    var deleteImage = document.getElementById(id);
    deleteImage.remove();
  }
  onSelectFile(event) {
    if (event.target.files && event.target.files[0]) {
      var filesAmount = event.target.files.length;
      for (let i = 0; i < filesAmount; i++) {
        var reader = new FileReader();

        reader.onload = (event: any) => {
          this.urlImages.push({ key: uuid() , value: event.target.result});
        }

        reader.readAsDataURL(event.target.files[i]);
      }
    }
  }

  constructor(private formBuilder: FormBuilder) { }

  ngOnInit() {
  
        this.educationForm = this.formBuilder.group({
          educationName: ['asdasd', Validators.required],
          educationType : [0,Validators.required],
          description: ['',Validators.required],
          physicalFacilities: this.formBuilder.array([])
        });
    }
    onSubmit() {
      this.submitted = true;
      //@ts-ignore
      this.educationForm.get('description').setValue(CKEDITOR.instances.editor.getData());

      if (this.educationForm.invalid) {
          return;
      }

      // display form values on success
      alert('SUCCESS!! :-)\n\n' + JSON.stringify(this.educationForm.value, null, 4));
  }
}
