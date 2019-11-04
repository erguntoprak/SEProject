import { Component, Input, forwardRef } from '@angular/core';
import { BaseControlValueAccessor } from '../base-control-value-accessor';
import { v4 as uuid } from 'uuid';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
@Component({
  selector: 'se-dropdown',
  templateUrl: './se-dropdown.component.html',
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

  selectedValue(newValue:number){
    this.onChanged(newValue);
  }
}
