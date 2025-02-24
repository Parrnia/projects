import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from '@angular/router';
import { Product } from '../../../interfaces/product';
import { EMPTY, Observable, throwError } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';
import { CustomerTypeEnum, ProductsClient } from '../../../web-api-client';
import { ProductmapperService } from '../../../mapServieces/productsCluster/productmapper.service';


@Injectable()
export class ProductResolver implements Resolve<Product> {

    constructor(
        private router: Router,
        private productsClient: ProductsClient,
        private productmapperService: ProductmapperService
    ) { }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Product> {
        debugger;
        const slug = route.params['productSlug'] || route.data['productSlug'];
        const name = route.params['productName'] || route.data['productName'];
        let products = this.productsClient.getProductBySlug(slug, name, CustomerTypeEnum.Personal).pipe(switchMap(res => [this.productmapperService.mapProductsBySlug([res ?? []])])).pipe(catchError(error => {
            if (error instanceof HttpErrorResponse && error.status === 404) {
                this.router.navigateByUrl('/').then();
                
                return EMPTY;
            }
            
            return throwError(error);
        }));
        debugger;
        return products.pipe(
            map(products => products[0])
          );
    }
}
