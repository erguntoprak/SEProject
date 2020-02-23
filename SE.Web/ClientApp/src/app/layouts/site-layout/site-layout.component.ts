import { Component, OnInit } from '@angular/core';
import { LazyLoadService } from '../../_services/lazy-load.service';

@Component({
  selector: 'app-site-layout',
  templateUrl: './site-layout.component.html'
})
export class SiteLayoutComponent implements OnInit {


  constructor(private lazyLoadService: LazyLoadService) {

  }
  ngOnInit(): void {
    let scripts = ["assets/js/jquery-3.2.1.min.js", "assets/js/propper.js",
    "assets/js/bootstrap.min.js", "assets/vendors/bootstrap-selector/js/bootstrap-select.min.js",
    "assets/vendors/wow/wow.min.js", "assets/vendors/sckroller/jquery.parallax-scroll.js",
    "assets/vendors/owl-carousel/owl.carousel.min.js", "assets/vendors/nice-select/jquery.nice-select.min.js",
    "assets/vendors/imagesloaded/imagesloaded.pkgd.min.js", "assets/vendors/isotope/isotope-min.js",
    "assets/vendors/magnify-pop/jquery.magnific-popup.min.js", "assets/vendors/circle-progress/circle-progress.js",
    "assets/vendors/counterup/jquery.counterup.min.js", "assets/vendors/counterup/jquery.waypoints.min.js",
    "assets/vendors/counterup/appear.js", "assets/vendors/scroll/jquery.mCustomScrollbar.concat.min.js",
    "assets/js/plugins.js", "assets/vendors/multiscroll/jquery.easings.min.js",
    "assets/vendors/multiscroll/multiscroll.responsiveExpand.limited.min.js", "assets/vendors/multiscroll/jquery.multiscroll.extensions.min.js","assets/js/main.js"];


    let cssFiles = ["assets/css/bootstrap.min.css", "assets/vendors/bootstrap-selector/css/bootstrap-select.min.css", "assets/vendors/themify-icon/themify-icons.css",
      "assets/vendors/elagent/style.css", "assets/vendors/flaticon/flaticon.css", "assets/vendors/animation/animate.css",
      "assets/vendors/owl-carousel/assets/owl.carousel.min.css", "assets/vendors/nice-select/nice-select.css", "assets/vendors/magnify-pop/magnific-popup.css",
      "assets/vendors/scroll/jquery.mCustomScrollbar.min.css", "assets/css/style.css", "assets/css/responsive.css"];
    this.lazyLoadService.loadScripts(scripts);
    this.lazyLoadService.loadCss(cssFiles); 
  }
}
