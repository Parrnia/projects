import { Injectable } from '@angular/core';
import { ShopCategory } from '../../../interfaces/category';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { Observable, observable, of, switchMap } from 'rxjs';
import { ProductCategoriesClient } from '../../../web-api-client';
import { CategorymapperService } from '../../../mapServieces/categoriesCluster/categorymapper.service';
import { prepareCategory } from 'projects/storefront/src/fake-server/endpoints';

@Injectable()
export class CategoryResolver implements Resolve<ShopCategory> {
    constructor(
        private productCategoriesClient : ProductCategoriesClient,
        private categorymapperService : CategorymapperService
    ) { }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<ShopCategory> {
        
        if(route.params['categorySlug'] == null && route.data['categorySlug'] == null){
            //let res = this.productCategoriesClient.getAllProductCategories().pipe(switchMap((res => this.categorymapperService.mapAllProductCategories(res))));
            let res = of(new ShopCategory());
            return res;
        }
        return this.productCategoriesClient.getProductCategoryBySlug(route.params['categorySlug'] || route.data['categorySlug'])
        .pipe(switchMap((res => [prepareCategory(this.categorymapperService.mapProductCategoryBySlug(res),2)])));
    }
}
