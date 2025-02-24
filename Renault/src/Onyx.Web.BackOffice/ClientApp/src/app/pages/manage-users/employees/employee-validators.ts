import { AbstractControl, AsyncValidatorFn, ValidationErrors, ValidatorFn } from '@angular/forms';
import { Observable , map } from 'rxjs';
import { AuthenticationService } from 'src/app/core/services/authService/auth.service';
import { UniqueNationalCodeValidator } from 'src/app/core/services/authService/models/validators/UniqueNationalCodeValidator';
import { UniqueUserNameValidator } from 'src/app/core/services/authService/models/validators/UniqueUserNameValidator';

export class EmployeeValidators {
    /**
     *
     */
    constructor() {}
    static mustMatchValidator(first: string, second: string): ValidatorFn {
        return (control: AbstractControl): ValidationErrors | null => {
            const firstControl = control.get(first);
            const secondControl = control.get(second);
    
            if (firstControl && secondControl) {
                if (firstControl.value !== secondControl.value && firstControl.value && secondControl.value) {
                    const errors = secondControl.errors || {};
    
                    secondControl.setErrors(Object.assign({}, errors, { mustMatch: true }));
                } else if (secondControl.errors) {
                    const errors = Object.assign({}, secondControl.errors);
    
                    delete errors['mustMatch'];
    
                    if (Object.keys(errors).length > 0) {
                        secondControl.setErrors(errors);
                    } else {
                        secondControl.setErrors(null);
                    }
                }
            }
    
            return null;
        }
    }

    static validUniqueUserName(authenticationService: AuthenticationService, userId: string): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> => {
            debugger;
            let uniqueUserNameValidator = new UniqueUserNameValidator();
            uniqueUserNameValidator.UserId = userId;
            uniqueUserNameValidator.UserName = control.value;
            return authenticationService.IsUserUniqueUserName(uniqueUserNameValidator).pipe(
                map(isAvailable => (isAvailable ? null : { userNameTaken: true }))
            );
        }
    }
    static validUniqueNationalCode(authenticationService: AuthenticationService, userId: string): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> => {
            debugger;
            let uniqueNationalCodeValidator = new UniqueNationalCodeValidator();
            uniqueNationalCodeValidator.UserId = userId;
            uniqueNationalCodeValidator.NationalCode = control.value;
            return authenticationService.isUserUniqueNationalCode(uniqueNationalCodeValidator).pipe(
                map(isAvailable => (isAvailable ? null : { nationalCodeTaken: true }))
            );
        }
    }
}
