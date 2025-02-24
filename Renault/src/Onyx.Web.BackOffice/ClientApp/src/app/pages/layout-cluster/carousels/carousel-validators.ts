import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { Observable, map, of, switchMap } from 'rxjs';
import { CarouselsClient } from 'src/app/web-api-client';

export class CarouselValidators {
    /**
     *
     */
    constructor() {}

    static validCarouselTitle(carouselsClient: CarouselsClient, carouselId : number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> =>  {
        const title = control.value;
        return carouselsClient.isUniqueCarouselTitle(carouselId, title).pipe(
            map(isAvailable => (isAvailable ? null : { titleTaken: true }))
        );
    }
    }
}
