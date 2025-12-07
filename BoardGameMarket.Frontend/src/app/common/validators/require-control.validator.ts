import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";
import { required } from "@angular/forms/signals";

export function requireControl(controlName: string): ValidatorFn {
    return (group: AbstractControl): ValidationErrors | null => {
        const control = group.get(controlName);
        if (!control) {
            return null;
        }
        return control.value ? null : { requiredField: true };
    };
}