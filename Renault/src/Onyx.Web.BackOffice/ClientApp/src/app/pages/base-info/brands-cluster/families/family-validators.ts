import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { Observable, map, of, switchMap } from 'rxjs';
import { FamiliesClient } from 'src/app/web-api-client';

export class FamilyValidators {
    /**
     *
     */
    constructor() {}
    static validFamilyLocalizedName(familiesClient: FamiliesClient, familyId : number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> =>  {
        const localizedName = control.value;
        return familiesClient.isUniqueFamilyLocalizedName(familyId, localizedName).pipe(
            map(isAvailable => (isAvailable ? null : { localizedNameTaken: true }))
        );
    }
    }
    static validFamilyName(familiesClient: FamiliesClient, familyId : number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> =>  {
        const name = control.value;
        return familiesClient.isUniqueFamilyName(familyId, name).pipe(
            map(isAvailable => (isAvailable ? null : { nameTaken: true }))
        );
    }
    }
}
