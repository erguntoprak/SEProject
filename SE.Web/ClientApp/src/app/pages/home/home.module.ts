import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { HomeComponent } from './home.component';
import { ReactiveFormsModule } from '@angular/forms';
import { SiteLayoutComponent } from '../../layouts/site-layout/site-layout.component';
import { Routes, RouterModule } from '@angular/router';
import { SiteLayoutModule } from '../../layouts/site-layout/site-layout.module';
const routes: Routes = [
  {
    path:'',
    component: SiteLayoutComponent,
    children: [
      {
        path: '',
        component: HomeComponent
      }
    ]
  }
];
@NgModule({
  declarations: [
    HomeComponent
  ],
  imports: [SharedModule, ReactiveFormsModule, RouterModule.forChild(routes),SiteLayoutModule],
  exports: [
    HomeComponent
  ]
})
export class HomeModule { }
