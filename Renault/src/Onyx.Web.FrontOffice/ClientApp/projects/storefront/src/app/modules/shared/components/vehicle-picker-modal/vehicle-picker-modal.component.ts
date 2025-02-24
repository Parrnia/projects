import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Observable, Subject, of } from 'rxjs';
import {
    VehiclePickerModalService,
    VehiclePickerModalSession,
} from '../../../../services/vehicle-picker-modal.service';
import { first, map, switchMap, takeUntil } from 'rxjs/operators';
import { Vehicle } from '../../../../interfaces/vehicle';
import { FormControl } from '@angular/forms';
import { VehiclesClient } from 'projects/storefront/src/app/web-api-client';
import { VehiclemapperService } from 'projects/storefront/src/app/mapServieces/brandsCluster/vehiclemapper.service';
import { CurrentVehicleService } from 'projects/storefront/src/app/services/current-vehicle.service';
import { VehicleApi } from 'projects/storefront/src/app/api';
import { UserVehicleService } from 'projects/storefront/src/app/services/user-vehicle.service';
import { UserVehicleAccountService } from 'projects/storefront/src/app/services/user-vehicle-account.service';
import { AuthService } from 'projects/storefront/src/app/services/authService/auth.service';


@Component({
    selector: 'app-vehicle-picker-modal',
    templateUrl: './vehicle-picker-modal.component.html',
    styleUrls: ['./vehicle-picker-modal.component.scss'],
})
export class VehiclePickerModalComponent implements OnInit, OnDestroy, AfterViewInit {
    private destroy$: Subject<void> = new Subject<void>();

    onSelectClick$: Subject<void> = new Subject<void>();

    session: VehiclePickerModalSession | null = null;

    vehicles$!: Observable<Vehicle[]>;
    vehicles: Vehicle[] = [];
    currentVehicleControl: FormControl = new FormControl(null);

    currentPanel: 'list' | 'form' = 'list';

    addVehicleControl: FormControl = new FormControl(null);

    @ViewChild('modal') modal!: ModalDirective;
    constructor(
        private service: VehiclePickerModalService,
        private userVehicleService: UserVehicleService,
        private userVehicleAccountService: UserVehicleAccountService,
        private authService: AuthService,
        private currentVehicleService: CurrentVehicleService

    ) { }

    ngOnInit(): void {

        this.loadVehicles();
        
        this.onSelectClick$.pipe(
            switchMap(() => this.vehicles$.pipe(
                first(),
                map(vehicles => vehicles.find(x => x.kindId === this.currentVehicleControl.value?.kindId) || null)),
            ),
        ).subscribe(vehicle => {
            this.loadVehicles();
            this.select(vehicle);
            
            this.currentVehicleService.value = vehicle;
        });
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    ngAfterViewInit(): void {
        this.service.show$.pipe(
            takeUntil(this.destroy$),
        ).subscribe(session => {
            
            this.session = session;
            this.currentVehicleControl.setValue(this.currentVehicleService.value, { emitEvent: false });
            this.modal.show();
            //this.currentVehicleControl.setValue(session.currentVehicle?.id || null, { emitEvent: false });
        });

        this.modal.onHidden.pipe(
            takeUntil(this.destroy$),
        ).subscribe(() => {
            this.currentPanel = 'list';
        });
    }

    onVehicleAdded(vehicle: Vehicle): void {
        this.currentPanel = 'list';
        this.loadVehicles();
        this.vehicles$.subscribe({
            next: (res) => {
                
                this.currentVehicleControl.setValue(res.filter(c => c.kindId == vehicle.kindId), { emitEvent: false });
            }
        });
    }

    select(vehicle: Vehicle | null): void {
        if (this.session) {
            this.session.select$.emit(vehicle);
        }
        
        this.close();
    }

    close(): void {
        
        if (this.session) {
            this.session.select$.complete();
            this.session.close$.emit();
            this.session.close$.complete();
            this.session = null;
        }

        this.modal.hide();
    }
    loadVehicles() {
        this.authService.isLoggedIn().subscribe({
            next: (res) => {
                if (res) {
                    this.vehicles$ = this.userVehicleAccountService.vehicles$;
                } else {
                    this.vehicles$ = this.userVehicleService.vehicles$;
                }
            }
        });
    }
}
