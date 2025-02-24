import { Directive, EventEmitter, OnDestroy, Output } from '@angular/core';
import { Vehicle } from '../../../interfaces/vehicle';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { CustomersClient } from '../../../web-api-client';
import { UserVehicleService } from '../../../services/user-vehicle.service';
import { CurrentVehicleService } from '../../../services/current-vehicle.service';

@Directive({
    selector: '[appRemoveVehicle]',
    exportAs: 'removeVehicle',
})
export class RemoveVehicleDirective implements OnDestroy {
    private destroy$: Subject<void> = new Subject<void>();

    inProgress = false;

    @Output() vehicleRemoved: EventEmitter<number> = new EventEmitter<number>();

    constructor(
        private userVehicleService: UserVehicleService,
        private customersClient: CustomersClient,
        private currentVehicleService: CurrentVehicleService
    ) {}

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    remove(event: MouseEvent, vehicle: Vehicle): void {
        event.stopPropagation();

        if (this.inProgress) {
            return;
        }
        let token = localStorage.getItem('token');
        this.inProgress = true;

        if (token) {
            this.customersClient
                .removeVehicleFromCustomer(vehicle.id)
                .pipe(takeUntil(this.destroy$))
                .subscribe({
                    next: () => this.vehicleRemoved.emit(1),
                    complete: () => (this.inProgress = false),
                });
        } else {
            this.userVehicleService
                .remove(vehicle)
                .pipe(takeUntil(this.destroy$))
                .subscribe({
                    next: () => {
                        this.vehicleRemoved.emit(1);
                    },
                    complete: () => (this.inProgress = false),
                });
        }
    }
}
