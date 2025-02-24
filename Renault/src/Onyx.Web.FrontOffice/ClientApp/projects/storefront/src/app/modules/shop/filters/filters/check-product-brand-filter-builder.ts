import { ProductBrandsClient } from 'projects/storefront/src/app/web-api-client';
import { AbstractFilterBuilder } from './abstract-filter-builder';
import { BaseFilterItem, CheckProductFilter } from 'projects/storefront/src/app/interfaces/filter';
import { Brand } from 'projects/storefront/src/app/interfaces/brand';
import { Observable, of, switchMap, forkJoin, map } from 'rxjs';
import { delayResponse } from 'projects/storefront/src/fake-server/utils';
import { ProductBrandmapperService } from 'projects/storefront/src/app/mapServieces/brandsCluster/product-brand-mapper.service';

export class CheckProductBrandFilterBuilder extends AbstractFilterBuilder {

    public override value: string[] = [];
    private brands : Brand[] = [];
    
    constructor(
        slug: string,
        name: string,
        private productBrandsClient : ProductBrandsClient,
        private productBrandmapperService : ProductBrandmapperService
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
    
        return this.productBrandsClient.getAllProductBrands().pipe(
            map((res) => 
                this.productBrandmapperService.mapAllProductBrands(res ?? [])
            )
        );
    }
    
    build(): CheckProductFilter {
      
        return {
            type: 'productBrand',
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
        if (this.slug === 'productBrand') {
            return brand ? [{
                slug: brand.slug,
                name: brand.name,
                count: 0,
            }] : [];
        }

        throw Error();
    }
}
