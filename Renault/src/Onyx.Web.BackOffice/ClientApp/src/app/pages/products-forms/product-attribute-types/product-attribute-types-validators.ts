import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { Observable, map, of, switchMap } from 'rxjs';
import { ProductAttributeTypesClient } from 'src/app/web-api-client';

export class ProductAttributeTypeValidators {
    /**
     *
     */
    constructor() { }
    static validProductAttributeTypeName(productAttributeTypesClient: ProductAttributeTypesClient, productAttributeTypeId: number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> => {
            debugger;
            const name = control.value;
            return productAttributeTypesClient.isUniqueProductAttributeTypeName(productAttributeTypeId, name).pipe(
                map(isAvailable => (isAvailable ? null : { nameTaken: true }))
            );
        }
    }
}
