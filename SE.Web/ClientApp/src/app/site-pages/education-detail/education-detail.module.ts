import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';
import { EducationDetailComponent } from './education-detail.component';
import { NgImageSliderModule } from 'ng-image-slider';
import { NgxMaskModule, IConfig } from 'ngx-mask'
import { YouTubePlayerModule } from '@angular/youtube-player';

export let options: Partial<IConfig> | (() => Partial<IConfig>);
const routes: Routes = [
  {
    path: '',
    component: EducationDetailComponent
  }
];
@NgModule({
  declarations: [
    EducationDetailComponent
  ],
  imports: [SharedModule,
    ReactiveFormsModule,
    YouTubePlayerModule,
    NgImageSliderModule,
    NgxMaskModule.forRoot(options),
    RouterModule.forChild(routes)]
})
export class EducationDetailModule { }
