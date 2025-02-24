import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { Observable, map, of, switchMap } from 'rxjs';
import { CountingUnitsClient } from 'src/app/web-api-client';

export class CountingUnitValidators {
    /**
     *
     */
    constructor() {}
    static validCountingUnitLocalizedName(countingUnitsClient: CountingUnitsClient, countingUnitId : number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> =>  {
        const localizedName = control.value;
        return countingUnitsClient.isUniqueCountingUnitLocalizedName(countingUnitId, localizedName).pipe(
            map(isAvailable => (isAvailable ? null : { localizedNameTaken: true }))
        );
    }
    }
    static validCountingUnitName(countingUnitsClient: CountingUnitsClient, countingUnitId : number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> =>  {
        const name = control.value;
        return countingUnitsClient.isUniqueCountingUnitName(countingUnitId, name).pipe(
            map(isAvailable => (isAvailable ? null : { nameTaken: true }))
        );
    }
    }
}
