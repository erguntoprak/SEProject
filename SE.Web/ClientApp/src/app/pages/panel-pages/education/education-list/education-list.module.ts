import { NgModule } from '@angular/core';
import { EducationListComponent } from './education-list.component';
import { SharedModule } from '../../../../shared/shared.module';
@NgModule({
  declarations: [
    EducationListComponent
  ],
  imports: [SharedModule],
  exports: [
    EducationListComponent
  ]
})
export class EducationListModule { }
