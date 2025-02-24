import { PaginatedListOfNewProductWithPaginationDto, CustomerTypeEnum } from './../../../../web-api-client';
import { ProductmapperService } from 'projects/storefront/src/app/mapServieces/productsCluster/productmapper.service';
import { Component, HostBinding, Input, OnDestroy, OnInit } from '@angular/core';
import { BehaviorSubject, of, Subject } from 'rxjs';
import { switchMap, takeUntil } from 'rxjs/operators';
import { Product } from '../../../../interfaces/product';
import { UrlService } from '../../../../services/url.service';
import { ProductsClient } from 'projects/storefront/src/app/web-api-client';

@Component({
    selector: 'app-analogs-table',
    templateUrl: './analogs-table.component.html',
    styleUrls: ['./analogs-table.component.scss'],
})
export class AnalogsTableComponent implements OnInit, OnDestroy {
    private destroy$: Subject<void> = new Subject<void>();
    private productId$: BehaviorSubject<number|null> = new BehaviorSubject<number|null>(null);

    analogs: Product[] = [];

    @Input() set productId(value: number) {
        if (value !== this.productId$.value) {
            this.productId$.next(value);
        }
    }

    @HostBinding('class.analogs-table') classAnalogsTable = true;
    constructor(
        public url: UrlService,
        public productsClient : ProductsClient,
        public productmapperService : ProductmapperService
    ) {}

    ngOnInit(): void {
        this.productId$.pipe(
            switchMap(productId => {
                if (!productId) {
                    return of(new PaginatedListOfNewProductWithPaginationDto);
                }

                return this.productsClient.getNewProductsWithPagination(1,5,CustomerTypeEnum.Personal);
            }),
            takeUntil(this.destroy$),
        ).subscribe(x => this.analogs = this.productmapperService.mapNewProductsWithPagination(x.items ?? []));
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }
}
