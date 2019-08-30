import { ValidatorFn, FormGroup, ValidationErrors } from '@angular/forms';

export const passwordMatch: ValidatorFn = (control: FormGroup): ValidationErrors | null => {
    const password = control.get('password');
    const confirmPassword = control.get('confirmPassword');

    let entered = (password.value.length > 0 && confirmPassword.value.length > 0)
        || (password.value.length === 0 && confirmPassword.value.length > 0);

    return entered && password && confirmPassword && password.value !== confirmPassword.value ? { 'passwordMatchFail': true } : null;
};