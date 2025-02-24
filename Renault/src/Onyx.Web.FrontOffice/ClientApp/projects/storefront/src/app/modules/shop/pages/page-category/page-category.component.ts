import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ShopCategory } from '../../../../interfaces/category';
import { UrlService } from '../../../../services/url.service';
import { Observable, of, forkJoin } from 'rxjs';
import { map, mergeMap, switchMap, tap } from 'rxjs/operators';
import { Product } from '../../../../interfaces/product';
import { ShopApi } from '../../../../api';
import { Brand } from '../../../../interfaces/brand';
import { TranslateService } from '@ngx-translate/core';
import { LanguageService } from '../../../language/services/language.service';
import { getCategoryPath } from '../../../../functions/utils';
import { BreadcrumbItem } from '../../../shared/components/breadcrumb/breadcrumb.component';
import { CustomerTypeEnum, ProductBrandsClient, ProductsClient } from 'projects/storefront/src/app/web-api-client';
import { ProductmapperService } from 'projects/storefront/src/app/mapServieces/productsCluster/productmapper.service';
import { ProductBrandmapperService } from 'projects/storefront/src/app/mapServieces/brandsCluster/product-brand-mapper.service';

interface AsyncData<T> {
    data$: Observable<T>;
    loading: boolean;
}

function asyncData<T>(source$: Observable<T>): AsyncData<T> {
    const container: AsyncData<T> = {
        data$: of(null).pipe(
            tap(() => container.loading = true),
            switchMap(() => source$),
            tap(() => container.loading = false),
        ),
        loading: false,
    };

    return container;
}

export type PageCategoryLayout =
    'columns-3-sidebar' |
    'columns-4-sidebar' |
    'columns-5-sidebar' |
    'columns-4-full' |
    'columns-5-full' |
    'columns-6-full' |
    'columns-7-full';

export type PageCategorySidebarPosition = 'start' | 'end';

export interface PageCategoryData {
    layout?: PageCategoryLayout;
    sidebarPosition?: PageCategorySidebarPosition;
    category?: ShopCategory;
    children?: ShopCategory[];
}

@Component({
    selector: 'app-page-category',
    templateUrl: './page-category.component.html',
    styleUrls: ['./page-category.component.scss'],
})
export class PageCategoryComponent implements OnInit {
    layout: PageCategoryLayout = 'columns-4-full';

    sidebarPosition: PageCategorySidebarPosition = 'start';

    category$!: Observable<ShopCategory | null>;

    children$!: Observable<ShopCategory[]>;

    pageTitle$!: Observable<string>;

    breadcrumbs$!: Observable<BreadcrumbItem[]>;

    bestsellers!: AsyncData<Product[]>;

    featured!: AsyncData<Product[]>;

    brands$!: Observable<Brand[]>;

    latest$!: Observable<Product[]>;

    private currentCustomerTypeEnum = CustomerTypeEnum.Personal;
    get hasSidebar(): boolean {
        return this.layout.endsWith('-sidebar');
    }

    constructor(
        private route: ActivatedRoute,
        private shop: ShopApi,
        private translate: TranslateService,
        private language: LanguageService,
        public url: UrlService,
        private productsClient: ProductsClient,
        private productBrandsClient: ProductBrandsClient,
        private productmapperService: ProductmapperService,
        private productBrandmapperService: ProductBrandmapperService
    ) { }

    ngOnInit(): void {
        this.route.data.subscribe((data: PageCategoryData) => {
            this.layout = data.layout || 'columns-4-full';
            this.sidebarPosition = data.sidebarPosition || 'start';
        });

        this.category$ = this.route.data.pipe(
            map((data: PageCategoryData) => data.category || null),
        );

        this.children$ = this.route.data.pipe(
            map((data: PageCategoryData) => data.category ? data.category.children : data.children),
            map(categories => categories || []),
        );

        this.pageTitle$ = this.category$.pipe(
            mergeMap(category => category ? of(category.name) : this.translate.stream('HEADER_SHOP')),
        );

        this.breadcrumbs$ = this.language.current$.pipe(
            switchMap(() => this.category$.pipe(
                map(category => {
                    const categoryPath = category ? getCategoryPath(category) : [];

                    return [
                        { label: this.translate.instant('LINK_HOME'), url: this.url.home() },
                        { label: this.translate.instant('LINK_SHOP'), url: this.url.shop() },
                        ...categoryPath.map(x => ({ label: x.name, url: this.url.category(x) })),
                    ];
                }),
            )),
        );

        this.bestsellers = asyncData(this.productsClient.getPopularProductsWithPagination(1, 8, this.currentCustomerTypeEnum)
            .pipe(map(res => this.productmapperService.mapPopularProductsWithPagination(res.items ?? []))));

        this.featured = asyncData(this.productsClient.getFeaturedProductsWithPagination(1, 8, this.currentCustomerTypeEnum)
            .pipe(map(res => this.productmapperService.mapFeaturedProductsWithPagination(res.items ?? []))));

        let limit = (this.hasSidebar ? 7 : 8) * 2;

        this.brands$ = this.productBrandsClient.getProductBrandsForBlock(limit)
            .pipe(
                map((res) =>
                    this.productBrandmapperService.mapProductBrandsForBlock(res.items ?? [])
                )
            );


        if (this.hasSidebar) {
            this.latest$ = this.productsClient.getLatestProductsWithPagination(1, 5, this.currentCustomerTypeEnum)
                .pipe(map(res => this.productmapperService.mapLatestProductsWithPagination(res.items ?? [])));
        }
    }
}
