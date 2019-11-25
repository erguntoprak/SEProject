import { NgModule } from '@angular/core';
import { SharedModule } from '../../../../shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { EducationCreateComponent } from './education-create.component';
import { SeTextBox } from '../../../../layouts/elements/se-textbox/se-textbox.component';
import { SeDropdown } from 'src/app/layouts/elements/se-dropdown/se-dropdown.component';
import { SeCheckBox } from 'src/app/layouts/elements/se-checkbox/se-checkbox.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {MatStepperModule} from '@angular/material';


@NgModule({
  declarations: [
    EducationCreateComponent,
    SeTextBox,
    SeDropdown,
    SeCheckBox
  ],
    imports: [SharedModule, FormsModule, ReactiveFormsModule, BrowserAnimationsModule, MatStepperModule],
  exports: [
    EducationCreateComponent,
    SeTextBox,
    SeDropdown,
    SeCheckBox
  ]
})
export class EducationCreateModule { }
