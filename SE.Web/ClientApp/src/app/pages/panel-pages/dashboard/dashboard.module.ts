import { NgModule } from '@angular/core';
import { DashBoardComponent } from './dashboard.component';
import { SharedModule } from '../../../shared/shared.module';
@NgModule({
  declarations: [
    DashBoardComponent
  ],
  imports: [SharedModule],
  exports: [
    DashBoardComponent
  ]
})
export class DashBoardModule { }
