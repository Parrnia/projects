import { Vehicle } from './../interfaces/vehicle';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { Product } from '../interfaces/product';
import { isPlatformBrowser } from '@angular/common';
import { BehaviorSubject, Subject, Observable, of } from 'rxjs';
import { CreateVehicleCommand, VehiclesClient } from '../web-api-client';
import { VehiclemapperService } from '../mapServieces/brandsCluster/vehiclemapper.service';
import { AuthService } from './authService/auth.service';


interface VehicleData {
    vehicles: Vehicle[]
}

@Injectable({
    providedIn: 'root',
})
export class UserVehicleAccountService {
    private data: VehicleData = {
        vehicles: []
    };
    private vehiclesSubject$: BehaviorSubject<Vehicle[]> = new BehaviorSubject(this.data.vehicles);
    private onAddingSubject$: Subject<Vehicle> = new Subject();

    get vehicles(): Array<Vehicle> {
        return this.data.vehicles;
    }
    set vehicles(vehicles: Vehicle[]) {
        this.data.vehicles = vehicles;
    }

    readonly vehicles$: Observable<Vehicle[]> = this.vehiclesSubject$.asObservable();

    constructor(
        @Inject(PLATFORM_ID) private platformId: any,
        private vehiclesClient: VehiclesClient,
        private vehiclemapperService: VehiclemapperService,
        private authService: AuthService
    ) {
        if (isPlatformBrowser(this.platformId)) {
            this.load();
        }
    }

    add(vehicle: Vehicle): Observable<Vehicle | null> {

        let isExist = this.vehicles.find(v => v.kindId == vehicle.kindId);
        this.onAddingSubject$.next(vehicle);
         
        let item = new Vehicle();

        if (isExist) {
            return of(null);
        } else {
            let createVehicleCommand = new CreateVehicleCommand();
            createVehicleCommand.kindId = vehicle.kindId;
            createVehicleCommand.customerId = localStorage.getItem('userId')!;
            item = vehicle;
            this.vehiclesClient.create(createVehicleCommand).subscribe(c => this.load());
        }
        this.load();

        return of(item);
    }


    remove(item: Vehicle): Observable<void> {
        this.vehiclesClient.delete(item.id);

        this.load();
        return of(null) as unknown as Observable<void>;
    }

    public load(): void {
        this.authService.isLoggedIn().subscribe(res => {
            if (res) {
                this.vehiclesClient.getVehiclesByCustomerId()
                    .subscribe(c => {
                        this.vehicles = this.vehiclemapperService.mapVehiclesByUserId(c);
                        this.vehiclesSubject$.next(this.vehicles);
                    });
            };
        }
        )

    };
}
