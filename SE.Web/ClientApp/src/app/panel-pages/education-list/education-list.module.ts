import { NgModule } from '@angular/core';
import { EducationListComponent } from './education-list.component';
import { SharedModule } from '../../shared/shared.module';
import { RouterModule, Routes } from '@angular/router';
import { BaseService } from '../../shared/base.service';
import { HttpClient } from '@angular/common/http';
const routes: Routes = [
  {
    path: '',
    component: EducationListComponent
  }
];
@NgModule({
  declarations: [
    EducationListComponent
  ],
  imports: [SharedModule, RouterModule.forChild(routes)],
  exports: [
    EducationListComponent
  ],
  providers: [BaseService]
})
export class EducationListModule { }
