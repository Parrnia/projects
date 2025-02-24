import { AbstractFilterBuilder } from './abstract-filter-builder';
import { BaseFilterItem, CheckVehicleFilter } from 'projects/storefront/src/app/interfaces/filter';
import { Brand } from 'projects/storefront/src/app/interfaces/brand';
import { Observable, of, switchMap, forkJoin, map } from 'rxjs';
import { VehicleBrandmapperService } from 'projects/storefront/src/app/mapServieces/brandsCluster/vehicle-brand-mapper.service';
import { VehicleBrandsClient } from 'projects/storefront/src/app/web-api-client';

export class CheckVehicleBrandFilterBuilder extends AbstractFilterBuilder {

    public override value: string[] = [];
    private brands : Brand[] = [];
    
    constructor(
        slug: string,
        name: string,
        private vehicleBrandsClient : VehicleBrandsClient,
        private vehicleBrandmapperService : VehicleBrandmapperService
    ) {
        super(slug,name);
        
    }

    makeItems(value: string, isLoaded : boolean): Observable<void> {
         
        if(!isLoaded){
        this.getBrandsAsync(isLoaded).subscribe({
                next : (res) => {
                    this.brands = res;
                    this.brands.forEach(brand => this.extractItems(brand).forEach(item => {
                        if (!this.items.find(x => x.slug === item.slug)) {
                            this.items.push(item);
                        }
                    }));
                  
                },
                error : (err) => console.log(err)
        })}
        
        this.value = this.parseValue(value);
        return of(undefined);   
    }

    getBrandsAsync(isLoaded: boolean): Observable<Brand[]> {
        if (isLoaded) {
            return of(this.brands);
        }
    
        return this.vehicleBrandsClient.getAllVehicleBrands().pipe(
            map((res) => 
                this.vehicleBrandmapperService.mapAllVehicleBrands(res ?? [])
            )
        );
    }
    
    build(): CheckVehicleFilter {
      
        return {
            type: 'vehicleBrand',
            slug: this.slug,
            name: this.name,
            items: this.items,
            value: this.value,
        };
    }

    // noinspection JSMethodCanBeStatic
    private parseValue(value: string): string[] {
        return value ? value.split(',') : [];
    }

    private extractItems(brand: Brand): BaseFilterItem[] {
        if (this.slug === 'vehicleBrand') {
            return brand ? [{
                slug: brand.slug,
                name: brand.name,
                count: 0,
            }] : [];
        }

        throw Error();
    }
}
