import { Component, OnInit } from '@angular/core';
import { BaseService } from '../../shared/base.service';
import * as _ from 'lodash';
import { AcdcLoadingService } from 'acdc-loading';
import { ActivatedRoute } from '@angular/router';
import { BlogListModel } from '../../shared/models';

@Component({
  selector: 'se-blog-view-list',
  templateUrl: './blog-view-list.component.html'
})
export class BlogViewListComponent implements OnInit {

  blogListModel: BlogListModel;

  constructor(private baseService: BaseService, private acdcLoadingService: AcdcLoadingService, private route: ActivatedRoute) {

  }
  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.acdcLoadingService.showLoading();
      this.baseService.getByName<BlogListModel>("Blog/GetAllBlogListByUserName?userName=", params['userName']).subscribe(data => {
        this.blogListModel = data;
        console.log(this.blogListModel)
        this.acdcLoadingService.hideLoading();
      })
    }); 
  }
}
