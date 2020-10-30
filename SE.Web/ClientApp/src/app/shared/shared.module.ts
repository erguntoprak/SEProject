import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SeCheckBox } from '../_helpers/se-checkbox.component';

@NgModule({
  declarations: [
    SeCheckBox
  ],
  imports: [CommonModule],
  exports: [
    CommonModule,
    SeCheckBox
  ]
})
export class SharedModule { }
