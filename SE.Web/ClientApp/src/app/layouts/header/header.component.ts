import { Component, OnInit } from '@angular/core';
import { CategoryModel } from '../../shared/models';
import { BaseService } from '../../shared/base.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html'
})
export class HeaderComponent implements OnInit {

  categories: CategoryModel[];

  constructor(private baseService: BaseService) {

  }
  ngOnInit(): void {
    this.baseService.getAll<CategoryModel[]>("Category/GetAllCategoryList").subscribe(categories => {
      this.categories = categories;
    });
  }

}
