import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { Observable, map, of, switchMap } from 'rxjs';
import { ThemesClient } from 'src/app/web-api-client';

export class ThemeValidators {
    /**
     *
     */
    constructor() { }
    static validThemeTitle(themesClient: ThemesClient, themeId: number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> => {
            const value = control.value;
            return themesClient.isUniqueThemeTitle(themeId, value).pipe(
                map(isAvailable => (isAvailable ? null : { titleTaken: true }))
            );
        }
    }
}

