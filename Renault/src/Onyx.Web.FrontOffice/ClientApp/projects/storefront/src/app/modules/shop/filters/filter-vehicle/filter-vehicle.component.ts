import { Component, forwardRef, HostBinding, Input, OnDestroy, OnInit } from '@angular/core';
import { ControlValueAccessor, FormControl, NG_VALUE_ACCESSOR } from '@angular/forms';
import { VehicleFilter } from '../../../../interfaces/filter';
import { Observable, Subject, observable, switchMap } from 'rxjs';
import { distinctUntilChanged, map, takeUntil } from 'rxjs/operators';
import { VehiclePickerModalService } from '../../../../services/vehicle-picker-modal.service';
import { CurrentVehicleService } from '../../../../services/current-vehicle.service';
import { Vehicle } from '../../../../interfaces/vehicle';

@Component({
    selector: 'app-filter-vehicle',
    templateUrl: './filter-vehicle.component.html',
    styleUrls: ['./filter-vehicle.component.scss'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => FilterVehicleComponent),
            multi: true,
        },
    ],
})
export class FilterVehicleComponent implements OnInit, OnDestroy, ControlValueAccessor {
    private destroy$: Subject<void> = new Subject<void>();

    control: FormControl = new FormControl(false);

    vehicle$!: Observable<Vehicle|null>;

    @Input() set options(options: VehicleFilter) {
        
        // if (options.vehicle) {
        //     this.currentVehicle.value = options.vehicle;
        // }
    }

    @HostBinding('class.filter-vehicle') classFilterVehicle = true;

    changeFn: (_: number|null) => void = () => {};

    touchedFn: () => void = () => {};

    constructor(
        private currentVehicle: CurrentVehicleService,
        public vehiclePicker: VehiclePickerModalService,
    ) {
        this.vehicle$ = this.currentVehicle.value$;
     }

    ngOnInit(): void {
        this.control.valueChanges.pipe(
            switchMap(() => this.vehicle$.pipe(
                map(vehicle => this.control.value && vehicle ? vehicle.kindId : null),
                distinctUntilChanged(),
            )),
            takeUntil(this.destroy$),
        ).subscribe(vehicleId => this.changeFn(vehicleId));
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    registerOnChange(fn: any): void {
        this.changeFn = fn;
    }

    registerOnTouched(fn: any): void {
        this.touchedFn = fn;
    }

    writeValue(obj: any): void {
        this.control.setValue(!!obj, { emitEvent: false });
    }

    showPicker(): void {
        this.vehicle$.subscribe(c => {
            this.vehiclePicker.show(c).select$.pipe(
                takeUntil(this.destroy$),
            ).subscribe(vehicle => {
                
                this.currentVehicle.value = vehicle;
    
                if (!vehicle) {
                    this.control.setValue(null, { emitEvent: false });
                }
    
                if (this.control.value) {
                    this.control.updateValueAndValidity();
                }
            });
        });
    }
}
