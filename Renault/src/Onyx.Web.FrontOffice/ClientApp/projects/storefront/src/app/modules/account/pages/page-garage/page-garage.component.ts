import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Observable, Subject } from 'rxjs';
import { Vehicle } from '../../../../interfaces/vehicle';
import { map, switchMap } from 'rxjs/operators';
import { UrlService } from '../../../../services/url.service';
import { VehiclemapperService } from 'projects/storefront/src/app/mapServieces/brandsCluster/vehiclemapper.service';
import { VehiclesClient } from 'projects/storefront/src/app/web-api-client';
import { UserVehicleAccountService } from 'projects/storefront/src/app/services/user-vehicle-account.service';

@Component({
    selector: 'app-page-garage',
    templateUrl: './page-garage.component.html',
    styleUrls: ['./page-garage.component.scss'],
})
export class PageGarageComponent implements OnInit, OnDestroy {
    private destroy$: Subject<void> = new Subject<void>();

    vehicles$!: Observable<Vehicle[]>;

    hasVehicles$!: Observable<boolean>;

    vehicle: FormControl = new FormControl(null);

    constructor(
        public url: UrlService,
        private userVehicleAccountService: UserVehicleAccountService
    ) { }

    ngOnInit(): void {
        this.vehicles$ = this.userVehicleAccountService.vehicles$;
        this.hasVehicles$ = this.vehicles$.pipe(map(x => x.length > 0));
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }
    loadVehicles() {
        this.userVehicleAccountService.load();
        this.vehicles$ = this.userVehicleAccountService.vehicles$;
    }
}
