import { NgModule } from '@angular/core';
import { SharedModule } from '../../../../shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { EducationCreateComponent } from './education-create.component';
import { SeTextBox } from '../../../../layouts/elements/se-textbox/se-textbox.component';
import { SeDropdown } from 'src/app/layouts/elements/se-dropdown/se-dropdown.component';
import { FroalaEditorModule, FroalaViewModule } from 'angular-froala-wysiwyg';
@NgModule({
  declarations: [
    EducationCreateComponent,
    SeTextBox,
    SeDropdown
  ],
  imports: [SharedModule,FormsModule,ReactiveFormsModule,FroalaEditorModule.forRoot(), FroalaViewModule.forRoot()],
  exports: [
    EducationCreateComponent,
    SeTextBox,
    SeDropdown
  ]
})
export class EducationCreateModule { }
