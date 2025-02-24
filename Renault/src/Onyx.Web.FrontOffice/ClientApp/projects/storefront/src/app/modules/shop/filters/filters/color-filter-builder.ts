import { ProductOptionColorsClient, ProductOptionValueColorDto } from 'projects/storefront/src/app/web-api-client';

import { AbstractFilterBuilder } from './abstract-filter-builder';
import { ProductOptionColormapperService } from 'projects/storefront/src/app/mapServieces/productsCluster/optionsCluster/productOptionColormapper.service';
import { ColorFilter, ColorFilterItem } from 'projects/storefront/src/app/interfaces/filter';
import { Observable, of } from 'rxjs';


export class ColorFilterBuilder extends AbstractFilterBuilder {

    public override value: string[] = [];
    private colors : ProductOptionValueColorDto[] = [];
    public override items: ColorFilterItem[] = [];

    constructor(
        slug: string,
        name: string,
        private productOptionColorsClient : ProductOptionColorsClient
    ) {
        super(slug,name);
    }
    
    
    
    makeItems(value: string, isLoaded : boolean): Observable<void> {
            this.productOptionColorsClient.getAllProductOptionValueColors().subscribe({
                next : (res) => {
                  
                    this.colors = res;
                    this.items = this.extractItems();
                },
                error : (err) => console.log(err)
            });
        
        this.value = this.parseValue(value);
        return of(undefined);
    }


    build(): ColorFilter {
        return {
            type: 'color',
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

    private extractItems(): ColorFilterItem[] {
      
        return this.colors.map(value => ({
            slug: value.slug ?? '',
            name: value.name ?? '',
            color: value.color ?? '#000',
            count: 0,
        }));
    }

}
