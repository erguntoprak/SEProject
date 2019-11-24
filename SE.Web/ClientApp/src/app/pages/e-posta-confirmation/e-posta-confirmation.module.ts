import { NgModule } from '@angular/core';
import { EpostaConfirmationComponent } from './e-posta-confirmation.component';
import { SharedModule } from '../../shared/shared.module';
@NgModule({
  declarations: [
    EpostaConfirmationComponent
  ],
  imports: [SharedModule],
  exports: [
    EpostaConfirmationComponent
  ]
})
export class EducationListModule { }
