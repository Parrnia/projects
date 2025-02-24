import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { Observable, map, of, switchMap } from 'rxjs';
import { ProductTypeAttributeGroupsClient } from 'src/app/web-api-client';

export class ProductAttributeGroupValidators {
    /**
     *
     */
    constructor() { }
    static validProductAttributeGroupName(productAttributeGroupsClient: ProductTypeAttributeGroupsClient, productAttributeGroupId: number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> => {
            debugger;
            const name = control.value;
            return productAttributeGroupsClient.isUniqueProductTypeAttributeGroupName(productAttributeGroupId, name).pipe(
                map(isAvailable => (isAvailable ? null : { nameTaken: true }))
            );
        }
    }
}
