import { Component, Input } from '@angular/core';

@Component({
  selector: 'se-button',
  templateUrl: './se-button.component.html'
})
export class SeButton {
  @Input() text: string;
  @Input() link: string;

}
