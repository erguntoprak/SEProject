import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { LoginComponent } from './login.component';
import { ReactiveFormsModule } from '@angular/forms';
import { NgxMaskModule, IConfig } from 'ngx-mask'

export let options: Partial<IConfig> | (() => Partial<IConfig>);

@NgModule({
  declarations: [
    LoginComponent
  ],
  imports: [SharedModule, ReactiveFormsModule, NgxMaskModule.forRoot(options)],
  exports: [
    LoginComponent
  ]
})
export class LoginModule { }
