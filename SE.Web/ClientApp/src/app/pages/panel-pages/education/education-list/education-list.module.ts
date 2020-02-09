import { NgModule } from '@angular/core';
import { EducationListComponent } from './education-list.component';
import { SharedModule } from '../../../../shared/shared.module';
import { AppRoutingModule } from '../../../../app-routing.module';
@NgModule({
  declarations: [
    EducationListComponent
  ],
  imports: [SharedModule, AppRoutingModule],
  exports: [
    EducationListComponent
  ]
})
export class EducationListModule { }
