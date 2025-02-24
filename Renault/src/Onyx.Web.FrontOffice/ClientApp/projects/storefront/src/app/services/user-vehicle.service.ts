import { Vehicle } from './../interfaces/vehicle';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { Product } from '../interfaces/product';
import { isPlatformBrowser } from '@angular/common';
import { BehaviorSubject, Subject, Observable, of } from 'rxjs';


interface VehicleData {
    vehicles: Vehicle[]
}

@Injectable({
    providedIn: 'root',
})
export class UserVehicleService {
    private data: VehicleData = {
        vehicles: []
    };
    private vehiclesSubject$: BehaviorSubject<Vehicle[]> = new BehaviorSubject(this.data.vehicles);
    private onAddingSubject$: Subject<Vehicle> = new Subject();

    get vehicles(): Array<Vehicle> {
        return this.data.vehicles;
    }
    set vehicles(vehicles : Vehicle[]){
        this.data.vehicles = vehicles;
    }
    
    readonly vehicles$: Observable<Vehicle[]> = this.vehiclesSubject$.asObservable();

    constructor(
        @Inject(PLATFORM_ID) private platformId: any,
    ) {
        if (isPlatformBrowser(this.platformId)) {
            this.load();
        }
    }
    
    add(vehicle : Vehicle): Observable<Vehicle | null> {
        
        let isExist = this.vehicles.find(v => v.engine === vehicle.engine && v.kindId === vehicle.kindId && v.make === vehicle.make && v.model === v.model && v.year === vehicle.year);
        this.onAddingSubject$.next(vehicle);
         
        let item = new Vehicle();

        if (isExist) {
            return of(null);
        } else {
            let newVehicleResult = new Vehicle();
            vehicle.id = vehicle.id;
            newVehicleResult = {...vehicle};
            
            item = newVehicleResult;
            this.data.vehicles.push(item);
        }
        this.save();

        return of(item);
    }


    remove(item: Vehicle): Observable<void> {
        this.data.vehicles = this.data.vehicles.filter(eachItem => eachItem.kindId !== item.kindId);
        
        this.save();
        return of(null) as unknown as Observable<void>;
    }


    public save(): void {
         
        localStorage.setItem('vehicles', JSON.stringify(this.data.vehicles));
        this.load();
    }

    public load(): void {
         
        const items = localStorage.getItem('vehicles');
         
        if (items) {
            this.vehiclesSubject$.next(JSON.parse(items));
            this.vehicles = JSON.parse(items);
        }
    }
}
