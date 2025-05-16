import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function numberValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        const value = control.value;

        // Allow empty values to be handled by required validator
        if (value === null || value === '') {
            return null;
        }

        // Check if value is a valid number
        const isValid = !isNaN(value) && isFinite(value);

        return isValid ? null : { notANumber: true };
    };
}
