import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { Observable, map, of, switchMap } from 'rxjs';
import { AboutUsClient, TestimonialsClient } from 'src/app/web-api-client';

export class AboutUsValidators {
    /**
     *
     */
    constructor() { }
    static validAboutUsTitle(aboutUsClient: AboutUsClient, aboutUsId: number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> => {
            const title = control.value;
            return aboutUsClient.isUniqueAboutUsTitle(aboutUsId, title).pipe(
                map(isAvailable => (isAvailable ? null : { titleTaken: true }))
            );
        }
    }
}

