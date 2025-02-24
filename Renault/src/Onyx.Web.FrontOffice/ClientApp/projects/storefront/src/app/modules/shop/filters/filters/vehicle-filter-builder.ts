
import { VehicleFilter, VehicleFilterValue } from 'projects/storefront/src/app/interfaces/filter';
import { AbstractFilterBuilder } from './abstract-filter-builder';
import { Vehicle } from 'projects/storefront/src/app/interfaces/vehicle';
import { Product } from 'projects/storefront/src/app/interfaces/product';
import { VehiclesClient } from 'projects/storefront/src/app/web-api-client';
import { VehiclemapperService } from 'projects/storefront/src/app/mapServieces/brandsCluster/vehiclemapper.service';
import { Observable, of } from 'rxjs';
import { UserVehicleAccountService } from 'projects/storefront/src/app/services/user-vehicle-account.service';
import { UserVehicleService } from 'projects/storefront/src/app/services/user-vehicle.service';
import { AuthService } from 'projects/storefront/src/app/services/authService/auth.service';
import { CurrentVehicleService } from 'projects/storefront/src/app/services/current-vehicle.service';

export class VehicleFilterBuilder extends AbstractFilterBuilder {
    public override value: VehicleFilterValue = null;
    public vehicle: Vehicle|null = null;
    constructor(
        slug: string,
        name: string,
        private userVehicleService : UserVehicleService,
        private userVehicleAccountService: UserVehicleAccountService,
        private authService: AuthService,
        private currentVehicleService: CurrentVehicleService
    ) {
        super(slug,name);
    }



    makeItems(value: string, isLoaded : boolean): Observable<void> {
        
        this.currentVehicleService.value$.subscribe({
            next: (res) => {
                this.vehicle = res;
            }
        }
        )
        
        if(typeof +value === 'number'){
            this.value = parseInt(value);
        }
        else{
            this.value = null;
        }
        return of(undefined);
    }

    build(): VehicleFilter {
        return {
            type: 'vehicle',
            slug: this.slug,
            name: this.name,
            value: this.value,
            vehicle: this.vehicle,
        };
    }
}
