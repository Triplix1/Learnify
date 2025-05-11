import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export function lettersOnlyValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        const value = control.value;
        if (!value) return null; // Don't validate empty values (use required if needed)

        const lettersOnlyRegex = /^[a-zA-Z]+$/;
        const valid = lettersOnlyRegex.test(value);

        return valid ? null : { lettersOnly: true };
    };
}
