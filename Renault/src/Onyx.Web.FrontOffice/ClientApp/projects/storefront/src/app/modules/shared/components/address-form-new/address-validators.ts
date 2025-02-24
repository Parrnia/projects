import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { AddressesClient } from 'projects/storefront/src/app/web-api-client';
import { Observable, map, of, switchMap } from 'rxjs';

export class AddressValidators {
    /**
     *
     */
    constructor() {}
    static validAddressTitle(addressesClient: AddressesClient, addressId : number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> =>  {
        const title = control.value;
        return addressesClient.isUniqueAddressTitle(localStorage.getItem('userId')!, addressId, title).pipe(
            map(isAvailable => (isAvailable ? null : { titleTaken: true }))
        );
    }
    }
    static validAddressPostcode(addressesClient: AddressesClient, addressId : number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> =>  {
        const postcode = control.value;
        return addressesClient.isUniqueAddressPostcode(localStorage.getItem('userId')!, addressId, postcode).pipe(
            map(isAvailable => (isAvailable ? null : { postcodeTaken: true }))
        );
    }
    }
}
