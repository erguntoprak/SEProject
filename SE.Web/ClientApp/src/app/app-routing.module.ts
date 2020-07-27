import { NgModule } from '@angular/core';
import { Routes, RouterModule, ExtraOptions } from '@angular/router';
import { PanelLayoutComponent } from './layouts/panel-layout/panel-layout.component';
import { SiteLayoutComponent } from './layouts/site-layout/site-layout.component';
import { AuthGuard } from './_services/auth-guard.service';

const routerOptions: ExtraOptions = {
  scrollPositionRestoration: 'enabled',
  anchorScrolling: 'enabled',
  scrollOffset: [0, 64],
};
const routes: Routes = [
  {
    path: 'panel',
    component: PanelLayoutComponent,
    canLoad: [AuthGuard],
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
      },
      {
        path: 'egitim-iletisim-formlari/:name',
        loadChildren: () => import('./panel-pages/education-contact-from-list/education-contact-from-list.module').then(m => m.EducationContactFormListModule),
        canLoad: [AuthGuard]
      },
      {
        path: 'blog-ekle',
        loadChildren: () => import('./panel-pages/blog-create/blog-create.module').then(m => m.BlogCreateModule),
        canLoad: [AuthGuard]
      },
      {
        path: 'blog-listesi',
        loadChildren: () => import('./panel-pages/blog-list/blog-list.module').then(m => m.BlogListModule),
        canLoad: [AuthGuard]
      },
      {
        path: 'blog-duzenle/:name',
        loadChildren: () => import('./panel-pages/blog-edit/blog-edit.module').then(m => m.BlogEditModule),
        canLoad: [AuthGuard]
      },
    ]
  },
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
      },
      {
        path: 'egitim-kurumu/:district/:category/:name',
        loadChildren: () => import('./site-pages/education-detail/education-detail.module').then(m => m.EducationDetailModule)
      },
      {
        path: 'blog/:name',
        loadChildren: () => import('./site-pages/blog-detail/blog-detail.module').then(m => m.BlogDetailModule)
      },
      {
        path: 'bloglar/:userName',
        loadChildren: () => import('./site-pages/blog-view-list/blog-view-list.module').then(m => m.BlogViewListModule)
      },
      {
        path: 'egitim-kurumlari/:district/:category',
        loadChildren: () => import('./site-pages/education-view-list/education-view-list.module').then(m => m.EducationViewListModule)
      },
      {
        path: 'egitim-kurumlari/:category',
        loadChildren: () => import('./site-pages/education-view-list/education-view-list.module').then(m => m.EducationViewListModule)
      }
    ]
  },
  
  {
    path: "**",
    loadChildren: () => import('./site-pages/login/login.module').then(m => m.LoginModule)
  },
  {
    path: '',
    redirectTo: '',
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, routerOptions)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
