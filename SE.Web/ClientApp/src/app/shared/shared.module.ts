import { NgModule } from '@angular/core';
import { NgxSpinnerModule } from 'ngx-spinner';
import { BaseService } from './base.service';
import { CommonModule } from '@angular/common';

@NgModule({
  imports: [CommonModule, NgxSpinnerModule],
  exports: [
    CommonModule,
    NgxSpinnerModule
  ],
  providers: [BaseService]
})
export class SharedModule { }
