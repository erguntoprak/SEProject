import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'se-education-create',
  templateUrl: './education-create.component.html'
})
export class EducationCreateComponent {
    educationForm: FormGroup;
    submitted = false;

    constructor(private formBuilder: FormBuilder) { }

    ngOnInit() {
        this.educationForm = this.formBuilder.group({
          educationName: ['asdasd', Validators.required]
        });
    }
    onSubmit() {
      this.submitted = true;

      // stop here if form is invalid
      if (this.educationForm.invalid) {
          return;
      }

      // display form values on success
      alert('SUCCESS!! :-)\n\n' + JSON.stringify(this.educationForm.value, null, 4));
  }
}
