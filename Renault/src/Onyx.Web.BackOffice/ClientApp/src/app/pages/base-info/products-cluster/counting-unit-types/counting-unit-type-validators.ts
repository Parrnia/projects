import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { Observable, map, of, switchMap } from 'rxjs';
import { CountingUnitTypesClient } from 'src/app/web-api-client';

export class CountingUnitTypeValidators {
    /**
     *
     */
    constructor() {}
    static validCountingUnitTypeLocalizedName(countingUnitTypesClient: CountingUnitTypesClient, countingUnitTypeId : number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> =>  {
        const localizedName = control.value;
        return countingUnitTypesClient.isUniqueCountingUnitTypeLocalizedName(countingUnitTypeId, localizedName).pipe(
            map(isAvailable => (isAvailable ? null : { localizedNameTaken: true }))
        );
    }
    }
    static validCountingUnitTypeName(countingUnitTypesClient: CountingUnitTypesClient, countingUnitTypeId : number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> =>  {
        const name = control.value;
        return countingUnitTypesClient.isUniqueCountingUnitTypeName(countingUnitTypeId, name).pipe(
            map(isAvailable => (isAvailable ? null : { nameTaken: true }))
        );
    }
    }
}
