import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { Observable, map, of, switchMap } from 'rxjs';
import { VehiclesClient } from 'src/app/web-api-client';

export class VehicleValidators {
    /**
     *
     */
    constructor() {}
    static validVehicleKindId(vehiclesClient: VehiclesClient, vehicleId : number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> =>  {
        const kindId = control.value;
        return vehiclesClient.isUniqueVehicleKindId(vehicleId, kindId).pipe(
            map(isAvailable => (isAvailable ? null : { kindIdTaken: true }))
        );
    }
    }
    static validVehicleVinNumber(vehiclesClient: VehiclesClient, vehicleId : number): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> =>  {
        const vinNumber = control.value;
        return vehiclesClient.isUniqueVehicleVinNumber(vehicleId, vinNumber).pipe(
            map(isAvailable => (isAvailable ? null : { vinNumberTaken: true }))
        );
    }
    }
}
