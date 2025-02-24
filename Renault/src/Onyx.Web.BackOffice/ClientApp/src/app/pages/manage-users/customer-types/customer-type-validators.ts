import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { Observable, map, of, switchMap } from 'rxjs';
import { CustomerTypesClient } from 'src/app/web-api-client';

export class CustomerTypeValidators {
    /**
     *
     */
    constructor() { }
    static validCustomerType(customerTypesClient: CustomerTypesClient, customerTypeId: number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> => {
            debugger;
            const customerType = control.value;
            return customerTypesClient.isUniqueCustomerType(customerType, customerTypeId).pipe(
                map(isAvailable => (isAvailable ? null : { customerTypeEnumTaken: true }))
            );
        }
    }
}
