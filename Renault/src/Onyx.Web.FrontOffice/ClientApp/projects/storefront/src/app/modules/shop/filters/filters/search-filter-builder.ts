
import { AbstractFilterBuilder } from './abstract-filter-builder';
import { ShopCategory } from 'projects/storefront/src/app/interfaces/category';
import { ColorFilterItem, SearchFilter } from 'projects/storefront/src/app/interfaces/filter';
import { CategorymapperService } from 'projects/storefront/src/app/mapServieces/categoriesCluster/categorymapper.service';
import { ProductCategoriesClient } from 'projects/storefront/src/app/web-api-client';
import { prepareCategory } from 'projects/storefront/src/fake-server/endpoints';
import { Observable, of } from 'rxjs';

export class SearchFilterBuilder extends AbstractFilterBuilder {
    public override value: string|null = null;

    constructor(
        slug: string,
        name: string
    ) {
        super(slug,name);
    }

    
    makeItems(value: string, isLoaded : boolean): Observable<void> {
        this.value = value;
        return of(undefined);
    }

    build(): SearchFilter {
        
        return {
            type: 'search',
            slug: this.slug,
            name: this.name,
            value: this.value ?? '',
            placeHolder : 'متن جستجو'
        };
    }

}

