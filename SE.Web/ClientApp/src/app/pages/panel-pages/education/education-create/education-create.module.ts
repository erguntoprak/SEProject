import { NgModule } from '@angular/core';
import { SharedModule } from '../../../../shared/shared.module';
import { EducationCreateComponent } from './education-create.component';
import { SeTextBox } from '../../../../layouts/elements/se-textbox/se-textbox.component';
@NgModule({
  declarations: [
    EducationCreateComponent,
    SeTextBox
  ],
  imports: [SharedModule],
  exports: [
    EducationCreateComponent,
    SeTextBox
  ]
})
export class EducationCreateModule { }
