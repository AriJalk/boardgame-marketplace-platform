import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export function requireControl(controlName: string): ValidatorFn {
    return (group: AbstractControl): ValidationErrors | null => {
        const control = group.get(controlName);
        if (!control) {
            return null;
        }
        return control.value ? null : { requiredField: controlName };
    };
}