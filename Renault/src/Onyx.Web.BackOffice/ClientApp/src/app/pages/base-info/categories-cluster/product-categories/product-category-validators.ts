import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { Observable, map, of, switchMap } from 'rxjs';
import { ProductCategoriesClient } from 'src/app/web-api-client';

export class ProductCategoryValidators {
    /**
     *
     */
    constructor() {}
    static validProductCategoryLocalizedName(productCategorysClient: ProductCategoriesClient, productCategoryId : number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> =>  {
        const localizedName = control.value;
        return productCategorysClient.isUniqueProductCategoryLocalizedName(productCategoryId, localizedName).pipe(
            map(isAvailable => (isAvailable ? null : { localizedNameTaken: true }))
        );
    }
    }
    static validProductCategoryName(productCategorysClient: ProductCategoriesClient, productCategoryId : number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> =>  {
        const name = control.value;
        return productCategorysClient.isUniqueProductCategoryName(productCategoryId, name).pipe(
            map(isAvailable => (isAvailable ? null : { nameTaken: true }))
        );
    }
    }
}
