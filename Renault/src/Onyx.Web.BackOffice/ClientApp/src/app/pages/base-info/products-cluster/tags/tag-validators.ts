import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { Observable, map, of, switchMap } from 'rxjs';
import { TagsClient } from 'src/app/web-api-client';

export class TagValidators {
    /**
     *
     */
    constructor() { }
    static validTagFaTitle(tagsClient: TagsClient, tagId: number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> => {
            const faTitle = control.value;
            return tagsClient.isUniqueTagFaTitleValidator(tagId, faTitle).pipe(
                map(isAvailable => (isAvailable ? null : { faTitleTaken: true }))
            );
        }
    }
    static validTagEnTitle(tagsClient: TagsClient, tagId: number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> => {
            const enTitle = control.value;
            return tagsClient.isUniqueTagEnTitleValidator(tagId, enTitle).pipe(
                map(isAvailable => (isAvailable ? null : { enTitleTaken: true }))
            );
        }
    }
}
