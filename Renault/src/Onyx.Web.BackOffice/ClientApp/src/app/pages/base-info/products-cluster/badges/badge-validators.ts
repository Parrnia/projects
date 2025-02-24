import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { Observable, map, of, switchMap } from 'rxjs';
import { BadgesClient } from 'src/app/web-api-client';

export class BadgeValidators {
    /**
     *
     */
    constructor() { }
    static validBadgeValue(badgesClient: BadgesClient, badgeId: number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> => {
            const value = control.value;
            return badgesClient.isUniqueBadgeValue(badgeId, value).pipe(
                map(isAvailable => (isAvailable ? null : { valueTaken: true }))
            );
        }
    }
}

