import { Component, Input, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { BaseControlValueAccessor } from '../base-control-value-accessor';
import { v4 as uuid } from 'uuid';
@Component({
  selector: 'se-checkbox',
  templateUrl: './se-checkbox.component.html',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => SeCheckBox),
      multi: true
    }
  ]
})
export class SeCheckBox extends BaseControlValueAccessor<number> {
    @Input() text: string;
    id = uuid();
}
