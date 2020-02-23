import { Component, Input, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { BaseControlValueAccessor } from './base-control-value-accessor';
import { v4 as uuid } from 'uuid';
@Component({
  selector: 'se-checkbox',
  template:
    ` <div class="checkbox">
         <input class="inp-cbx" id="{{id}}" type="checkbox" style="display: none;">
               <label class="cbx" for="{{id}}">
                    <span>
                        <svg width="12px" height="10px" viewbox="0 0 12 10">
                             <polyline points="1.5 6 4.5 9 10.5 1"></polyline>
                        </svg>
                    </span>
                    {{text}}
               </label>
     </div>
`,
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
