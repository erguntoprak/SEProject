import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { LoginComponent } from './login.component';
import { ReactiveFormsModule } from '@angular/forms';
@NgModule({
  declarations: [
    LoginComponent
  ],
  imports: [SharedModule, ReactiveFormsModule],
  exports: [
    LoginComponent
  ]
})
export class LoginModule { }
