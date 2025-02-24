
import { AbstractControl, AsyncValidatorFn, ValidationErrors, ValidatorFn } from '@angular/forms';
import { Observable, map } from 'rxjs';
import { UniqueUserNameValidator } from '../../services/authService/models/validators/UniqueUserNameValidator';
import { UniqueNationalCodeValidator } from '../../services/authService/models/validators/UniqueNationalCodeValidator';
import { AuthService } from '../../services/authService/auth.service';


export class AuthValidators {
    /**
     *
     */
    constructor() {}
    static validUniqueUserName(AuthService: AuthService, userId: string): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> => {
            debugger;
            let uniqueUserNameValidator = new UniqueUserNameValidator();
            uniqueUserNameValidator.UserId = userId;
            uniqueUserNameValidator.UserName = control.value;
            return AuthService.IsUserUniqueUserName(uniqueUserNameValidator).pipe(
                map(isAvailable => (isAvailable ? null : { userNameTaken: true }))
            );
        }
    }
    static validUniqueNationalCode(AuthService: AuthService, userId: string): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> => {
            debugger;
            let uniqueNationalCodeValidator = new UniqueNationalCodeValidator();
            uniqueNationalCodeValidator.UserId = userId;
            uniqueNationalCodeValidator.NationalCode = control.value;
            return AuthService.isUserUniqueNationalCode(uniqueNationalCodeValidator).pipe(
                map(isAvailable => (isAvailable ? null : { nationalCodeTaken: true }))
            );
        }
    }
}
