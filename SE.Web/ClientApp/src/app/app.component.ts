import { Component } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router, NavigationStart, NavigationEnd } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  constructor(private spinner: NgxSpinnerService, private _router: Router) {

  }
  ngOnInit() {
    this._router.events.subscribe((route) => {
      if (route instanceof NavigationStart) {
        this.spinner.show();
      }
      if (route instanceof NavigationEnd) {
        this.spinner.hide();
      }
    });
  }
}
