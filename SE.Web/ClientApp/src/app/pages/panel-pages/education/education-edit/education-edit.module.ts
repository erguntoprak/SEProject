import { NgModule } from '@angular/core';
import { SharedModule } from '../../../../shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { EducationEditComponent } from './education-edit.component';
import { SeTextBox } from '../../../../layouts/elements/se-textbox/se-textbox.component';
import { SeDropdown } from 'src/app/layouts/elements/se-dropdown/se-dropdown.component';
import { SeCheckBox } from 'src/app/layouts/elements/se-checkbox/se-checkbox.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgxMaskModule, IConfig } from 'ngx-mask'
import { AngularEditorModule } from '@kolkov/angular-editor';


export let options: Partial<IConfig> | (() => Partial<IConfig>);

@NgModule({
  declarations: [
    EducationEditComponent,
    SeTextBox,
    SeDropdown,
    SeCheckBox
  ],
  imports: [SharedModule, FormsModule, ReactiveFormsModule, BrowserAnimationsModule, NgxMaskModule.forRoot(options), AngularEditorModule ],
  exports: [
    EducationEditComponent,
    SeTextBox,
    SeDropdown,
    SeCheckBox
  ]
})
export class EducationEditModule { }
