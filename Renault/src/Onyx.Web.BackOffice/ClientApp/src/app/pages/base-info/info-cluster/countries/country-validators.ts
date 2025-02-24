import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { Observable, map, of, switchMap } from 'rxjs';
import { CountriesClient } from 'src/app/web-api-client';

export class CountryValidators {
    /**
     *
     */
    constructor() {}
    static validCountryLocalizedName(countriesClient: CountriesClient, countryId : number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> =>  {
        const localizedName = control.value;
        return countriesClient.isUniqueCountryLocalizedName(countryId, localizedName).pipe(
            map(isAvailable => (isAvailable ? null : { localizedNameTaken: true }))
        );
    }
    }
    static validCountryName(countriesClient: CountriesClient, countryId : number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> =>  {
        const name = control.value;
        return countriesClient.isUniqueCountryName(countryId, name).pipe(
            map(isAvailable => (isAvailable ? null : { nameTaken: true }))
        );
    }
    }
}
