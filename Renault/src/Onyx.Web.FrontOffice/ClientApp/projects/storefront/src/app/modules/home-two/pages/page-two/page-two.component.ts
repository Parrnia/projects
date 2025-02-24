import { CorporationInfosClient } from './../../../../web-api-client';
import { Component, OnInit } from '@angular/core';
import { BlogApi } from '../../../../api';
import { Post } from '../../../../interfaces/post';
import { BehaviorSubject, EMPTY, Observable, of, timer } from 'rxjs';
import { Brand } from '../../../../interfaces/brand';
import { filter, map, mergeMap, switchMap, tap } from 'rxjs/operators';
import { Product } from '../../../../interfaces/product';
import { SectionHeaderGroup } from '../../../shared/components/section-header/section-header.component';
import { Category, ShopCategory } from '../../../../interfaces/category';
import { CarouselsClient, CustomerTypeEnum, ProductBrandsClient, ProductCategoriesClient, ProductsClient, VehicleBrandsClient, } from 'projects/storefront/src/app/web-api-client';
import { ProductmapperService } from 'projects/storefront/src/app/mapServieces/productsCluster/productmapper.service';
import { CategorymapperService } from 'projects/storefront/src/app/mapServieces/categoriesCluster/categorymapper.service';
import { LoadingService } from 'projects/storefront/src/app/services/loader.service';
import { ProductBrandmapperService } from 'projects/storefront/src/app/mapServieces/brandsCluster/product-brand-mapper.service';
import { VehicleBrandmapperService } from 'projects/storefront/src/app/mapServieces/brandsCluster/vehicle-brand-mapper.service';
import { ImageService } from 'projects/storefront/src/app/mapServieces/image.service';
import { Carousel } from 'projects/storefront/src/app/interfaces/carousel';

interface ProductsCarouselGroup extends SectionHeaderGroup {
    products$: Observable<Product[]>;
}

interface ProductsCarouselData {
    subject$: BehaviorSubject<ProductsCarouselGroup>;
    products$: Observable<Product[]>;
    loading: boolean;
    groups: ProductsCarouselGroup[];
}

interface DeferredData<T> {
    loading: boolean;
    data$: Observable<T>;
}

@Component({
    selector: 'app-page-two',
    templateUrl: './page-two.component.html',
    styleUrls: ['./page-two.component.scss'],
})
export class PageTwoComponent implements OnInit {
    carousels!: Carousel[];
    brandsForBlock$: Observable<Brand[]> = of([]);

    brands$: Observable<Brand[]> = of([]);

    popularCategories$: Observable<ShopCategory[]> = of([]);
    Categories$: Observable<Category[]> = of([]);

    featuredProducts!: ProductsCarouselData;

    blockSale!: DeferredData<Product[]>;

    latestPosts!: DeferredData<Post[]>;

    columnTopRated$!: Observable<Product[]>;
    columnSpecialOffers$!: Observable<Product[]>;
    columnBestsellers$!: Observable<Product[]>;
    sliderBackgroundImage: string | undefined;
    isLoading = false;
    private readonly currentCustomerTypeEnum: CustomerTypeEnum;
    constructor(
        private blogApi: BlogApi,
        private carouselClient: CarouselsClient,
        private imageService: ImageService,
        private productBrandsClient: ProductBrandsClient,
        private productBrandmapperService: ProductBrandmapperService,
        private vehicleBrandsClient: VehicleBrandsClient,
        private vehicleBrandmapperService: VehicleBrandmapperService,
        private productsClient: ProductsClient,
        private productCategoriesClient: ProductCategoriesClient,
        private productmapperService: ProductmapperService,
        private categorymapperService: CategorymapperService,
        private corporationInfosClient: CorporationInfosClient,
        private loadingService: LoadingService
    ) {
        this.currentCustomerTypeEnum = parseInt(
            localStorage.getItem('currentCustomerCustomerTypeEnum') ??
            CustomerTypeEnum.Personal.toString(),
            10
        ) as CustomerTypeEnum;
    }

    ngOnInit(): void {
        const isFirefox =
            navigator.userAgent.toLowerCase().indexOf('firefox') > -1;
        const preloader = document.querySelector('.site-preloader');
        if (!isFirefox) {
            preloader?.classList.remove('site-preloader__fade');
            window.onload = () => {
                preloader?.classList.add('site-preloader__fade');
            };
        }

        this.carouselClient.getAllCarousels().subscribe(
            (result) => {
                this.carousels = result;
                this.carousels.forEach(c => {
                    c.desktopImage = this.imageService.makeImageUrl(c.desktopImage);
                    c.mobileImage = this.imageService.makeImageUrl(c.mobileImage);
                });
            },
            (error) => console.error(error)
        );

        this.brandsForBlock$ = this.productBrandsClient
            .getProductBrandsForBlock(16)
            .pipe(
                map((res) =>
                    this.productBrandmapperService.mapProductBrandsForBlock(res.items ?? [])
                )
            );

        this.brands$ = this.vehicleBrandsClient
            .getVehicleBrandsWithPagination(1, 16)
            .pipe(
                map((res) =>
                    this.vehicleBrandmapperService.mapVehicleBrandsWithPaginationDto(res.items ?? [])
                )
            );

        this.popularCategories$ = this.productCategoriesClient
            .getPopularFirstProductCategoriesWithPagination(1, 6)
            .pipe(
                map(res =>
                    this.categorymapperService.mapProductCategoriesByProductParentCategoryId(
                        res.items ?? [],
                        'products'
                    )
                )
            );


        this.Categories$ = this.productCategoriesClient
            .getFeaturedFirstProductCategoriesWithPagination(1, 3)
            .pipe(
                map((res) => {
                    return this.categorymapperService.mapAllProductCategoriesWithPagination(
                        res.items ?? []
                    );
                })
            );

        this.Categories$.subscribe({
            next: (res) => {
                let category1: Category;
                let category2: Category;
                let category3: Category;
                category1 = res.slice(0, 1)[0];
                category2 = res.slice(1, 2)[0];
                category3 = res.slice(2, 3)[0];

                this.featuredProducts = this.makeCarouselData([
                    {
                        label: 'All',
                        products$: this.productsClient
                            .getFeaturedProductsWithPagination(
                                1,
                                8,
                                this.currentCustomerTypeEnum
                            )
                            .pipe(
                                switchMap((res) => [
                                    this.productmapperService.mapFeaturedProductsWithPagination(
                                        res.items ?? []
                                    ),
                                ])
                            ),
                    },
                    {
                        label: category1?.name ?? '',
                        products$: this.productsClient
                            .getFeaturedProductsByProductCategoryIdWithPagination(
                                category1?.id,
                                1,
                                8,
                                this.currentCustomerTypeEnum
                            )
                            .pipe(
                                switchMap((res) => [
                                    this.productmapperService.mapProductsByProductCategoryIdWithPagination(
                                        res.items ?? []
                                    ),
                                ])
                            ),
                    },
                    {
                        label: category2?.name ?? '',
                        products$: this.productsClient
                            .getFeaturedProductsByProductCategoryIdWithPagination(
                                category2?.id,
                                1,
                                8,
                                this.currentCustomerTypeEnum
                            )
                            .pipe(
                                switchMap((res) => [
                                    this.productmapperService.mapProductsByProductCategoryIdWithPagination(
                                        res.items ?? []
                                    ),
                                ])

                            )
                    },
                    {
                        label: category3?.name ?? '',
                        products$: this.productsClient
                            .getFeaturedProductsByProductCategoryIdWithPagination(
                                category3?.id,
                                1,
                                8,
                                this.currentCustomerTypeEnum
                            )
                            .pipe(
                                switchMap((res) => [
                                    this.productmapperService.mapProductsByProductCategoryIdWithPagination(
                                        res.items ?? []
                                    ),
                                ])
                            )
                    },
                ]);
            },
            error: (err) => {
                console.log(err);
            },
        });
        this.blockSale = this.makeDeferredData(
            this.productsClient
                .getSalesProductsWithPagination(1, 10, this.currentCustomerTypeEnum)
                .pipe(
                    map((res) => this.productmapperService.mapSalesProductsWithPagination(res.items ?? [])
                    )
                ));



        this.latestPosts = this.makeDeferredData(
            this.blogApi.getLatestPosts(8)
        );

        this.columnTopRated$ = this.productsClient
            .getTopRatedProductsWithPagination(1, 6, this.currentCustomerTypeEnum)
            .pipe(
                switchMap((res) => [
                    this.productmapperService.mapTopRatedProductsWithPagination(
                        res.items ?? []
                    ),
                ])
            );
        this.columnSpecialOffers$ = this.productsClient
            .getSpecialOffersProductsWithPagination(
                1,
                6,
                this.currentCustomerTypeEnum
            )
            .pipe(
                switchMap((res) => [
                    this.productmapperService.mapSpecialOffersProductsWithPagination(
                        res.items ?? []
                    ),
                ])
            );
        this.columnBestsellers$ = this.productsClient
            .getBestSellersProductsWithPagination(
                1,
                6,
                this.currentCustomerTypeEnum
            )
            .pipe(
                switchMap((res) => [
                    this.productmapperService.mapBestSellerProductsWithPagination(
                        res.items ?? []
                    ),
                ])
            );

        this.corporationInfosClient.getCorporationInfo().subscribe({
            next: (res) => {
                this.sliderBackgroundImage = this.imageService.makeImageUrl(res?.sliderBackGroundImage);
            },
        });

    }

    groupChange(
        carousel: ProductsCarouselData,
        group: SectionHeaderGroup
    ): void {
        carousel.subject$.next(group as ProductsCarouselGroup);
    }

    private makeCarouselData(
        groups: ProductsCarouselGroup[]
    ): ProductsCarouselData {
        const subject = new BehaviorSubject<ProductsCarouselGroup>(groups[0]);
        const carouselData: ProductsCarouselData = {
            subject$: subject,
            products$: subject.pipe(
                filter((x) => x !== null),
                tap(() => (carouselData.loading = true)),
                switchMap((group) => group.products$),
                tap(() => (carouselData.loading = false))
            ),
            loading: true,
            groups,
        };

        return carouselData;
    }

    private makeDeferredData<T>(dataSource: Observable<T>): DeferredData<T> {
        const data = {
            loading: true,
            data$: EMPTY as Observable<T>,
        };

        data.data$ = timer(0).pipe(
            mergeMap(() => dataSource.pipe(tap(() => (data.loading = false))))
        );

        return data;
    }
}
