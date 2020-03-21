import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PanelLayoutComponent } from './layouts/panel-layout/panel-layout.component';
import { SiteLayoutComponent } from './layouts/site-layout/site-layout.component';
import { AuthGuard } from './_services/auth-guard.service';


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
        loadChildren: () => import('./panel-pages/dashboard/dashboard.module').then(m => m.DashBoardModule),
        canLoad: [AuthGuard]
      },
      {
        path: 'egitim-ekle',
        loadChildren: () => import('./panel-pages/education-create/education-create.module').then(m => m.EducationCreateModule),
        canLoad: [AuthGuard]
      },
      {
        path: 'egitim-duzenle/:name',
        loadChildren: () => import('./panel-pages/education-edit/education-edit.module').then(m => m.EducationEditModule),
        canLoad: [AuthGuard]
      },
      {
        path: 'egitimler',
        loadChildren: () => import('./panel-pages/education-list/education-list.module').then(m => m.EducationListModule),
        canLoad: [AuthGuard]
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
