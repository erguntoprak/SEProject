import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PanelLayoutComponent } from './layouts/panel-layout/panel-layout.component';
import { SiteLayoutComponent } from './layouts/site-layout/site-layout.component';


const routes: Routes = [
  {
    path: '',
    component: SiteLayoutComponent,
    children: [
      {
        path: '',
        loadChildren: () => import('./site-pages/home/home.module').then(m => m.HomeModule)
      },
      {
        path: 'giris',
        loadChildren: () => import('./site-pages/login/login.module').then(m => m.LoginModule)
      },
      {
        path: 'kayit-ol',
        loadChildren: () => import('./site-pages/register/register.module').then(m => m.RegisterModule)
      }
    ]
  },
  {
    path: 'panel',
    component: PanelLayoutComponent,
    children: [
      {
        path: '',
        loadChildren: () => import('./panel-pages/dashboard/dashboard.module').then(m => m.DashBoardModule)
      },
      {
        path: 'egitim-ekle',
        loadChildren: () => import('./panel-pages/education-create/education-create.module').then(m => m.EducationCreateModule)
      },
      {
        path: 'egitimler',
        loadChildren: () => import('./panel-pages/education-list/education-list.module').then(m => m.EducationListModule)
      }
    ]
  },
  {
    path: '',
    redirectTo: '',
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
