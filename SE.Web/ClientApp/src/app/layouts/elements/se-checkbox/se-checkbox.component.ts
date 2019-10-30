import { Component, Input } from '@angular/core';
import { v4 as uuid } from 'uuid';
@Component({
  selector: 'se-checkbox',
  templateUrl: './se-checkbox.component.html'
})
export class SeCheckBox {
  @Input() text: string;

  generateValue() {
    return uuid();
  }
}
