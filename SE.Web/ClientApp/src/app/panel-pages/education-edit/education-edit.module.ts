import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { EducationEditComponent } from './education-edit.component';
import { NgxMaskModule, IConfig } from 'ngx-mask'
import { AngularEditorModule } from '@kolkov/angular-editor';


export let options: Partial<IConfig> | (() => Partial<IConfig>);

@NgModule({
  declarations: [
    EducationEditComponent
  ],
  imports: [SharedModule, FormsModule, ReactiveFormsModule, NgxMaskModule.forRoot(options), AngularEditorModule ],
  exports: [
    EducationEditComponent
  ]
})
export class EducationEditModule { }
