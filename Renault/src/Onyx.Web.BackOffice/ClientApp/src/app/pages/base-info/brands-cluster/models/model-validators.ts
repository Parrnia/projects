import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { Observable, map, of, switchMap } from 'rxjs';
import { ModelsClient } from 'src/app/web-api-client';

export class ModelValidators {
    /**
     *
     */
    constructor() {}
    static validModelLocalizedName(modelsClient: ModelsClient, modelId : number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> =>  {
        const localizedName = control.value;
        return modelsClient.isUniqueModelLocalizedName(modelId, localizedName).pipe(
            map(isAvailable => (isAvailable ? null : { localizedNameTaken: true }))
        );
    }
    }
    static validModelName(modelsClient: ModelsClient, modelId : number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> =>  {
        const name = control.value;
        return modelsClient.isUniqueModelName(modelId, name).pipe(
            map(isAvailable => (isAvailable ? null : { nameTaken: true }))
        );
    }
    }
}
