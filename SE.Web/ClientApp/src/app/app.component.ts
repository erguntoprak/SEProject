import { Component, AfterViewInit, OnInit } from '@angular/core';
import { Router, NavigationStart, NavigationEnd } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ScriptLoaderService } from './_services/script-loader.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, AfterViewInit {
  title = 'ClientApp';

  constructor(private _script: ScriptLoaderService,private _router: Router, private spinner: NgxSpinnerService) {
    this._router.events.subscribe((route) => {
      if (route instanceof NavigationStart) {     
          this.spinner.show();
      }
      if (route instanceof NavigationEnd) {
        this.spinner.hide();
      }
    });
  }

  ngOnInit() {
   
  }
  ngAfterViewInit() {
    this._script.loadScripts('body', ['assets/js/se-plugin.js'], true)
      .then(result => {
        this._script.loadScripts('body', [' assets/js/se-main.js'], true);
        this._script.loadScripts('body', [' assets/js/dashboard.js'], true);
      });
  }
}
