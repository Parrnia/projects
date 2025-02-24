import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { Observable, switchMap } from 'rxjs';
import { ShopCategory } from '../../../interfaces/category';
import { ProductCategoriesClient } from '../../../web-api-client';
import { CategorymapperService } from '../../../mapServieces/categoriesCluster/categorymapper.service';
import { prepareCategory } from 'projects/storefront/src/fake-server/endpoints/categories';

@Injectable()
export class RootCategoriesResolver implements Resolve<ShopCategory[]> {
    constructor(
        private productCategoriesClient : ProductCategoriesClient,
        private categorymapperService : CategorymapperService
    ) { }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<ShopCategory[]> {
        return this.productCategoriesClient.getProductCategoriesByProductParentCategoryId(null)
        .pipe(switchMap((res) => [this.categorymapperService.mapProductCategoriesByProductParentCategoryId(res ?? [].map(c => prepareCategory(c,1)))]));
    }
}
