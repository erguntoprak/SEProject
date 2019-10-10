import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptorService } from './_services/auth-interceptor.service';
import { ScriptLoaderService } from './_services/script-loader.service';

@NgModule({
  providers: [ScriptLoaderService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptorService,
      multi: true
    }
  ]
})
export class CoreModule { }
