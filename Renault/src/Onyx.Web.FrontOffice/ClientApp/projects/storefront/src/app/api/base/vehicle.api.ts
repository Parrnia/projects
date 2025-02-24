import { Observable } from 'rxjs';
import { Vehicle } from '../../interfaces/vehicle';

export abstract class VehicleApi {
    abstract userVehicles$: Observable<Vehicle[]>;

    abstract currentVehicle$: Observable<Vehicle|null>;

    abstract getYears(): Observable<string[]>;

    abstract getMakes(year: string): Observable<string[]>;

    abstract getModels(year: string, make: string): Observable<string[]>;

    abstract getVehicles(year: string, make: string, model: string): Observable<Vehicle[]>;

    abstract getVehicleByVin(vin: string): Observable<Vehicle>;

    abstract addUserVehicle(vehicleId: number): Observable<void>;

    abstract removeUserVehicle(vehicleId: number): Observable<void>;

    abstract setCurrentVehicle(vehicle: Vehicle|null): void;
}
