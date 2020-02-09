import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SeButton } from './layouts/elements/se-button/se-button.component';
import { HttpClientModule } from '@angular/common/http';
import { SharedModule } from './shared/shared.module';
import { CoreModule } from './core.module';
import { SiteLayoutModule } from './layouts/site-layout/site-layout.module';
import { NgxSpinnerModule } from 'ngx-spinner';
import { HomeModule } from './pages/home/home.module';
import { LoginModule } from './pages/login/login.module';
import { PanelLayoutModule } from './layouts/panel-layout/panel-layout.module';
import { DashBoardModule } from './pages/panel-pages/dashboard/dashboard.module';
import { EducationListModule } from './pages/panel-pages/education/education-list/education-list.module';
import { EducationCreateModule } from './pages/panel-pages/education/education-create/education-create.module';

@NgModule({
  declarations: [
    AppComponent,
    SeButton
  ],
  imports: [
    BrowserModule,
    CoreModule,
    AppRoutingModule,
    NgxSpinnerModule,
    HttpClientModule,
    SharedModule,
    SiteLayoutModule,
    LoginModule,
    PanelLayoutModule,
    DashBoardModule,
    EducationListModule,
    EducationCreateModule
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
