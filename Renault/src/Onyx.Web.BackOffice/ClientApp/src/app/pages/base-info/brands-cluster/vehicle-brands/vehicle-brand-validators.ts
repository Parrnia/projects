import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { Observable, map, of, switchMap } from 'rxjs';
import { VehicleBrandsClient } from 'src/app/web-api-client';

export class VehicleBrandValidators {
    /**
     *
     */
    constructor() { }
    static validVehicleBrandLocalizedName(vehicleBrandsClient: VehicleBrandsClient, vehicleBrandId: number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> => {
            debugger;
            const localizedName = control.value;
            return vehicleBrandsClient.isUniqueVehicleBrandLocalizedName(vehicleBrandId, localizedName).pipe(
                map(isAvailable => (isAvailable ? null : { localizedNameTaken: true }))
            );
        }
    }
    static validVehicleBrandName(vehicleBrandsClient: VehicleBrandsClient, vehicleBrandId: number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> => {
            const name = control.value;
            return vehicleBrandsClient.isUniqueVehicleBrandName(vehicleBrandId, name).pipe(
                map(isAvailable => (isAvailable ? null : { nameTaken: true }))
            );
        }
    }
}
