import { Injectable, OnDestroy, Optional, SkipSelf } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';
import { Vehicle } from '../interfaces/vehicle';

@Injectable({
    providedIn: 'root',
})
export class CurrentVehicleService {
    value$: BehaviorSubject<Vehicle | null>;

    constructor() {
        const items = localStorage.getItem('currentVehicle');
        this.value$ = new BehaviorSubject<Vehicle | null>(
            items != null ? JSON.parse(items) : null
        );
    }

    get value(): Vehicle | null {
        return this.value$.value;
    }

    set value(value: Vehicle | null) {
        localStorage.setItem('currentVehicle', JSON.stringify(value));
        this.value$.next(value);
    }
}
