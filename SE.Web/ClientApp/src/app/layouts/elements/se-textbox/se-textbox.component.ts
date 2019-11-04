import { Component, Input, forwardRef} from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { BaseControlValueAccessor } from '../base-control-value-accessor';

@Component({
  selector: 'se-textbox',
  templateUrl: './se-textbox.component.html',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => SeTextBox),
      multi: true
    }
  ]
})
export class SeTextBox extends BaseControlValueAccessor<string>{

  @Input() placeholder = '';
  @Input() disabled: boolean;

}
