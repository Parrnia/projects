import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { Observable, map, of, switchMap } from 'rxjs';
import { AddressesClient } from 'src/app/web-api-client';

export class AddressesValidators {
    /**
     *
     */
    constructor() { }
    static validAddressesTitle(addressessClient: AddressesClient, customerId: string, addressesId: number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> => {
            debugger;
            const title = control.value;
            return addressessClient.isUniqueAddressTitle(customerId, addressesId, title).pipe(
                map(isAvailable => (isAvailable ? null : { titleTaken: true }))
            );
        }
    }
    static validAddressesPostcode(addressessClient: AddressesClient, customerId: string, addressesId: number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> => {
            const postCode = control.value;
            return addressessClient.isUniqueAddressPostcode(customerId, addressesId, postCode).pipe(
                map(isAvailable => (isAvailable ? null : { postCodeTaken: true }))
            );
        }
    }
}
