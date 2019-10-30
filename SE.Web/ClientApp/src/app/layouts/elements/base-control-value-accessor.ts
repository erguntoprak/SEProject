import { ControlValueAccessor } from '@angular/forms';

export class BaseControlValueAccessor<T> implements ControlValueAccessor {
  public disabled = false;

  /**
   * Call when value has changed programmatically
   */
  private onChanged() {}
  private onTouched() {}
  public value: T;



  /**
   * Model -> View changes
   */
  public writeValue(obj: T): void { debugger; this.value = obj; }
  public registerOnChange(fn: any): void {debugger; this.onChanged = fn; }
  public registerOnTouched(fn: any): void {debugger; this.onTouched = fn; }
  public setDisabledState?(isDisabled: boolean): void { this.disabled = isDisabled; }
}
