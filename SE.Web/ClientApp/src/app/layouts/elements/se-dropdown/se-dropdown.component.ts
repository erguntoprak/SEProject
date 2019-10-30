import { Component, Input } from '@angular/core';

@Component({
  selector: 'se-dropdown',
  templateUrl: './se-dropdown.component.html'
})
export class SeDropdown {
  @Input() title: string;
  @Input() dataList: Array<Object>;
}
