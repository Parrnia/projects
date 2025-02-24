import { Injectable } from '@angular/core';
import {
    ActivatedRouteSnapshot,
    Resolve,
    RouterStateSnapshot,
} from '@angular/router';
import { ProductsList } from '../../../interfaces/list';
import { Observable } from 'rxjs';
import { parseProductsListParams } from '../../../functions/utils';
import { ShopApi } from '../../../api';
import { FilterService } from '../filters/filter.service';
import { Filter } from '../../../interfaces/filter';

@Injectable()
export class ProductsListResolver implements Resolve<ProductsList> {
    constructor(private shop: ShopApi, private filterService: FilterService) {}

    resolve(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<ProductsList> {
        const categorySlug =
            route.params['categorySlug'] || route.data['categorySlug'];
        const options = parseProductsListParams(route.queryParams);

        let result = this.filterService.getProductsList({
            ...options,
            filters: {
                ...options.filters,
                category: categorySlug,
            },
        });
        debugger;
        return result;
    }
}
