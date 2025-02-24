import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { Observable, map, of, switchMap } from 'rxjs';
import { ProductBrandsClient } from 'src/app/web-api-client';

export class ProductBrandValidators {
    /**
     *
     */
    constructor() { }
    static validProductBrandLocalizedName(productBrandsClient: ProductBrandsClient, productBrandId: number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> => {
            debugger;
            const localizedName = control.value;
            return productBrandsClient.isUniqueProductBrandLocalizedName(productBrandId, localizedName).pipe(
                map(isAvailable => (isAvailable ? null : { localizedNameTaken: true }))
            );
        }
    }
    static validProductBrandName(productBrandsClient: ProductBrandsClient, productBrandId: number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> => {
            const name = control.value;
            return productBrandsClient.isUniqueProductBrandName(productBrandId, name).pipe(
                map(isAvailable => (isAvailable ? null : { nameTaken: true }))
            );
        }
    }
}
