import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { HomeComponent } from './home.component';
import { ReactiveFormsModule } from '@angular/forms';
import { SiteLayoutComponent } from '../../layouts/site-layout/site-layout.component';
import { Routes, RouterModule } from '@angular/router';
const routes: Routes = [
  {
    path:'',
    component: HomeComponent
  }
];
@NgModule({
  declarations: [
    HomeComponent
  ],
  imports: [SharedModule, ReactiveFormsModule, RouterModule.forChild(routes)],
  exports: [
    HomeComponent
  ]
})
export class HomeModule { }
