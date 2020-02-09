import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SiteLayoutComponent } from './layouts/site-layout/site-layout.component';
import { HomeComponent } from './pages/home/home.component';
import { LoginComponent } from './pages/login/login.component';
import { PanelLayoutComponent } from './layouts/panel-layout/panel-layout.component';
import { DashBoardComponent } from './pages/panel-pages/dashboard/dashboard.component';
import { EducationListComponent } from './pages/panel-pages/education/education-list/education-list.component';
import { EducationCreateComponent } from './pages/panel-pages/education/education-create/education-create.component';
import { AuthGuard } from './_services/auth-guard.service';

const routes: Routes = [
  {
    path: '',
    component: SiteLayoutComponent,
    children: [
      {
        path: '',
        loadChildren: () => import('./pages/home/home.module').then(m => m.HomeModule)
      },
      {
        path: 'giris', component: LoginComponent
      }
    ]
  },
  {
    path: 'panel',
    component: PanelLayoutComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: '', component: DashBoardComponent
      },
      {
        path: 'egitim-listesi', component: EducationListComponent
      },
      {
        path: 'egitim-ekle', component: EducationCreateComponent
      }
    ]
  },
  {
    path: '',
    redirectTo: '',
    pathMatch: 'full'
  }

  ,
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
