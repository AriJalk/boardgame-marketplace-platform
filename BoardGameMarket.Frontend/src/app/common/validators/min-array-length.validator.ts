import { AbstractControl, FormArray, ValidatorFn } from "@angular/forms";

export function minArrayLength(min: number): ValidatorFn {
  return (control: AbstractControl) => {
    if (control instanceof FormArray) {
      return control.length >= min ? null : { minArrayLength: { requiredLength: min, actualLength: control.length } };
    }
    return null;
  };
}
