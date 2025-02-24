import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { Observable, map, of, switchMap } from 'rxjs';
import { ProvidersClient } from 'src/app/web-api-client';

export class ProviderValidators {
    /**
     *
     */
    constructor() { }
    static validProviderLocalizedName(providersClient: ProvidersClient, providerId: number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> => {
            debugger;
            const localizedName = control.value;
            return providersClient.isUniqueProviderLocalizedName(providerId, localizedName).pipe(
                map(isAvailable => (isAvailable ? null : { localizedNameTaken: true }))
            );
        }
    }
    static validProviderName(providersClient: ProvidersClient, providerId: number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> => {
            const name = control.value;
            return providersClient.isUniqueProviderName(providerId, name).pipe(
                map(isAvailable => (isAvailable ? null : { nameTaken: true }))
            );
        }
    }
}
