import { NgModule } from '@angular/core';
import { SharedModule } from '../../../../shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { EducationCreateComponent } from './education-create.component';
import { SeTextBox } from '../../../../layouts/elements/se-textbox/se-textbox.component';
@NgModule({
  declarations: [
    EducationCreateComponent,
    SeTextBox
  ],
  imports: [SharedModule,FormsModule,ReactiveFormsModule],
  exports: [
    EducationCreateComponent,
    SeTextBox
  ]
})
export class EducationCreateModule { }
