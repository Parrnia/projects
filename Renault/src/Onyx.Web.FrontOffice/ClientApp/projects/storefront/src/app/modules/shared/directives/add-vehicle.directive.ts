import { Directive, EventEmitter, OnDestroy, Output } from '@angular/core';
import { Subject } from 'rxjs';
import { VehicleApi } from '../../../api';
import { takeUntil } from 'rxjs/operators';
import {
    AddVehicleToCustomerCommand,
    CreateVehicleCommand,
    CustomersClient,
    VehiclesClient,
} from '../../../web-api-client';
import { Vehicle } from '../../../interfaces/vehicle';
import { UserVehicleService } from '../../../services/user-vehicle.service';
import { UserVehicleAccountService } from '../../../services/user-vehicle-account.service';
import { CurrentVehicleService } from '../../../services/current-vehicle.service';

@Directive({
    selector: '[appAddVehicle]',
    exportAs: 'addVehicle',
})
export class AddVehicleDirective implements OnDestroy {
    private destroy$: Subject<void> = new Subject<void>();

    inProgress = false;

    @Output() vehicleAdded: EventEmitter<Vehicle> = new EventEmitter<Vehicle>();

    constructor(
        private userVehicleService: UserVehicleService,
        private userVehicleAccountService: UserVehicleAccountService,
        private currentVehicleService: CurrentVehicleService,
        private customersClient: CustomersClient,
        private VehiclesClient: VehiclesClient
    ) {}

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    addVehicle(vehicle: Vehicle): void {
        if (this.inProgress) {
            return;
        }
        this.inProgress = true;

        let userId = localStorage.getItem('userId');
        let token = localStorage.getItem('token');

        if (token) {
            if (!vehicle.id) {
                let cmd = new Vehicle();
                cmd.kindId = vehicle.kindId;
                this.userVehicleAccountService.add(cmd).subscribe({
                    next: (res) => this.vehicleAdded.emit(vehicle),
                    complete: () => (this.inProgress = false),
                    error: (err) => {
                        console.log(err);
                        this.inProgress = false;
                    },
                });
                this.currentVehicleService.value = vehicle;
                return;
            }

            let vehicleCommand = new AddVehicleToCustomerCommand();
            vehicleCommand.customerId = userId!;
            vehicleCommand.vehicleId = vehicle.id;
            vehicleCommand.kindId = vehicle.kindId;
            this.customersClient
                .addVehicleToCustomer(vehicleCommand)
                .pipe(takeUntil(this.destroy$))
                .subscribe({
                    next: () => this.vehicleAdded.emit(vehicle),
                    complete: () => (this.inProgress = false),
                    error: (err) => {
                        console.log(err);
                        this.inProgress = false;
                    },
                });
        } else {
            this.inProgress = true;
            this.userVehicleService
                .add(vehicle)
                .pipe(takeUntil(this.destroy$))
                .subscribe({
                    next: () => this.vehicleAdded.emit(vehicle),
                    complete: () => (this.inProgress = false),
                });
        }
        this.currentVehicleService.value = vehicle;
    }
}
