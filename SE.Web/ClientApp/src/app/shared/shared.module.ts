import { NgModule } from '@angular/core';
import { NgxSpinnerModule } from 'ngx-spinner';
import { BaseService } from './base.service';
import { CommonModule } from '@angular/common';

@NgModule({
  imports: [CommonModule],
  exports: [
    CommonModule
  ]
})
export class SharedModule { }
