import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { Observable, map, of, switchMap } from 'rxjs';
import { KindsClient } from 'src/app/web-api-client';

export class KindValidators {
    /**
     *
     */
    constructor() {}
    static validKindLocalizedName(kindsClient: KindsClient, kindId : number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> =>  {
        const localizedName = control.value;
        return kindsClient.isUniqueKindLocalizedName(kindId, localizedName).pipe(
            map(isAvailable => (isAvailable ? null : { localizedNameTaken: true }))
        );
    }
    }
    static validKindName(kindsClient: KindsClient, kindId : number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> =>  {
        const name = control.value;
        return kindsClient.isUniqueKindName(kindId, name).pipe(
            map(isAvailable => (isAvailable ? null : { nameTaken: true }))
        );
    }
    }
}
