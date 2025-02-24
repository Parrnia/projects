import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { Observable, map, of, switchMap } from 'rxjs';
import { ProductTypesClient } from 'src/app/web-api-client';

export class ProductTypeValidators {
    /**
     *
     */
    constructor() {}
    static validProductTypeLocalizedName(productTypesClient: ProductTypesClient, productTypeId : number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> =>  {
        const localizedName = control.value;
        return productTypesClient.isUniqueProductTypeLocalizedName(productTypeId, localizedName).pipe(
            map(isAvailable => (isAvailable ? null : { localizedNameTaken: true }))
        );
    }
    }
    static validProductTypeName(productTypesClient: ProductTypesClient, productTypeId : number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> =>  {
        const name = control.value;
        return productTypesClient.isUniqueProductTypeName(productTypeId, name).pipe(
            map(isAvailable => (isAvailable ? null : { nameTaken: true }))
        );
    }
    }
}
