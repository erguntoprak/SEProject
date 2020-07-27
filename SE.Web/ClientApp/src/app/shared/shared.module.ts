import { NgModule } from '@angular/core';
import { NgxSpinnerModule } from 'ngx-spinner';
import { BaseService } from './base.service';
import { CommonModule } from '@angular/common';
import { SeCheckBox } from '../_helpers/se-checkbox.component';
import { SeDropdown } from '../_helpers/se-dropdown.component';

@NgModule({
  declarations: [
    SeCheckBox,
    SeDropdown
  ],
  imports: [CommonModule],
  exports: [
    CommonModule,
    SeCheckBox,
    SeDropdown
  ]
})
export class SharedModule { }
