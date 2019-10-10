import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { HomeComponent } from './home.component';
import { ReactiveFormsModule } from '@angular/forms';
@NgModule({
  declarations: [
    HomeComponent
  ],
  imports: [SharedModule, ReactiveFormsModule],
  exports: [
    HomeComponent
  ]
})
export class HomeModule { }
