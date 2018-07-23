import { Directive, Input } from '@angular/core';
import { NG_VALIDATORS, Validator, AbstractControl } from '@angular/forms';

@Directive({
  selector: '[appMatchValue]',
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: MatchValueValidator,
      multi: true
    }
  ]
})
export class MatchValueValidator implements Validator {
  @Input('appMatchValue') valueToMatch: string;
  
  constructor() { }

  validate(control: AbstractControl): {[key: string]: any} | null {
    let isValid = control.value === this.valueToMatch;
    if (isValid) {
      return null;
    }
    else {
      return {
        appMatchValue: {
          valid: false
        }
      }
    }
  }
}
