import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { Observable, map, of, switchMap } from 'rxjs';
import { TestimonialsClient } from 'src/app/web-api-client';

export class TestimonialValidators {
    /**
     *
     */
    constructor() { }
    static validTestimonialName(testimonialsClient: TestimonialsClient, testimonialId: number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> => {
            const name = control.value;
            return testimonialsClient.isUniqueTestimonialName(testimonialId, name).pipe(
                map(isAvailable => (isAvailable ? null : { nameTaken: true }))
            );
        }
    }
}

