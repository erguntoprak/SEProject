import { Component, Input, forwardRef, OnInit } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { BaseControlValueAccessor } from './base-control-value-accessor';
import { v4 as uuid } from 'uuid';
import * as _ from 'lodash';
@Component({
  selector: 'se-dropdown',
  template:
    `
<div class="nice-select form-control selectpickers" tabindex="0">
  <span class="current">{{title}}</span>
  <ul class="list">
    <li *ngFor="let item of dataList" (click)="selectedValue(item.id)" class="option">{{item.name}}</li>
  </ul>
</div>
`,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => SeDropdown),
      multi: true
    }
  ]
})
export class SeDropdown extends BaseControlValueAccessor<number> {

  @Input() title: string;
  @Input() dataList: Array<Object>;

  selectedValue(newValue: number) {
    this.onChanged(newValue);
  }
}
