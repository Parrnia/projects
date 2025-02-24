import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { Observable, map, of, switchMap } from 'rxjs';
import { ProductStatusesClient } from 'src/app/web-api-client';

export class ProductStatusValidators {
    /**
     *
     */
    constructor() {}
    static validProductStatusLocalizedName(productStatussClient: ProductStatusesClient, productStatusId : number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> =>  {
        const localizedName = control.value;
        return productStatussClient.isUniqueProductStatusLocalizedName(productStatusId, localizedName).pipe(
            map(isAvailable => (isAvailable ? null : { localizedNameTaken: true }))
        );
    }
    }
    static validProductStatusName(productStatussClient: ProductStatusesClient, productStatusId : number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> =>  {
        const name = control.value;
        return productStatussClient.isUniqueProductStatusName(productStatusId, name).pipe(
            map(isAvailable => (isAvailable ? null : { nameTaken: true }))
        );
    }
    }
}
