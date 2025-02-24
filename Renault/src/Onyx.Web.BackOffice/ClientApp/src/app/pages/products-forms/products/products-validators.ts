import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { Observable, map, of, switchMap } from 'rxjs';
import { ProductsClient } from 'src/app/web-api-client';

export class ProductValidators {
    /**
     *
     */
    constructor() { }
    static validProductLocalizedName(productsClient: ProductsClient, productId: number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> => {
            debugger;
            const localizedName = control.value;
            return productsClient.isUniqueProductLocalizedName(productId, localizedName).pipe(
                map(isAvailable => (isAvailable ? null : { localizedNameTaken: true }))
            );
        }
    }
    static validProductName(productsClient: ProductsClient, productId: number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> => {
            const name = control.value;
            return productsClient.isUniqueProductName(productId, name).pipe(
                map(isAvailable => (isAvailable ? null : { nameTaken: true }))
            );
        }
    }
}
