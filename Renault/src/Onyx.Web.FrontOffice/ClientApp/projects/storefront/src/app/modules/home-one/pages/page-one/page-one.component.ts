import { Component, OnInit } from '@angular/core';
import { BlogApi, ShopApi } from '../../../../api';
import { SectionHeaderGroup } from '../../../shared/components/section-header/section-header.component';
import { BehaviorSubject, EMPTY, Observable, of, timer } from 'rxjs';
import { Product } from '../../../../interfaces/product';
import { filter, mergeMap, switchMap, tap } from 'rxjs/operators';
import { Category, ShopCategory } from '../../../../interfaces/category';
import { Post } from '../../../../interfaces/post';
import { Brand } from '../../../../interfaces/brand';
import { CategorymapperService } from 'projects/storefront/src/app/mapServieces/categoriesCluster/categorymapper.service';
import { ProductmapperService } from 'projects/storefront/src/app/mapServieces/productsCluster/productmapper.service';
import { ProductsClient, ProductCategoriesClient } from 'projects/storefront/src/app/web-api-client';

interface ProductsCarouselGroup extends SectionHeaderGroup {
    products$: Observable<Product[]>;
}

interface ProductsCarouselData {
    subject$: BehaviorSubject<ProductsCarouselGroup>;
    products$: Observable<Product[]>;
    loading: boolean;
    groups: ProductsCarouselGroup[];
}

interface BlockZoneData {
    image: string;
    mobileImage: string;
    category$: Observable<ShopCategory>;
}

interface DeferredData<T> {
    loading: boolean;
    data$: Observable<T>;
}

@Component({
    selector: 'app-page-one',
    templateUrl: './page-one.component.html',
    styleUrls: ['./page-one.component.scss'],
})
export class PageOneComponent implements OnInit {
    featuredProducts!: ProductsCarouselData;

    blockSale!: DeferredData<Product[]>;

    blockZones: BlockZoneData[] = [];

    newArrivals!: DeferredData<Product[]>;

    latestPosts!: DeferredData<Post[]>;

    brands$: Observable<Brand[]> = of([]);

    Categories$: Observable<Category[]> = of([]);


    columnTopRated$!: Observable<Product[]>;
    columnSpecialOffers$!: Observable<Product[]>;
    columnBestsellers$!: Observable<Product[]>;

    constructor(
        private shopApi: ShopApi,
        private blogApi: BlogApi,
        private brandsClient : BrandsClient,
        private productsClient : ProductsClient,
        private productCategoriesClient : ProductCategoriesClient,
        private productmapperService : ProductmapperService,
        private categorymapperService : CategorymapperService,
        private brandmapperService : BrandmapperService
    ) { }

    ngOnInit(): void {

        this.Categories$ = this.productCategoriesClient.getProductCategoriesByProductParentCategoryId(null).pipe(switchMap((res => ([this.categorymapperService.mapProductCategoriesToCategories(res ?? [])]))));
        this.Categories$.subscribe( {
            next : (res) => {
            let category1 : Category;
            let category2 : Category;
            let category3 : Category;
            category1 = res.slice(0, 1)[0];
            category2 = res.slice(1, 2)[0];
            category3 = res.slice(2, 3)[0];

            this.featuredProducts = this.makeCarouselData([
                {
                    label: 'All',
                    products$: this.productsClient.getProductsWithPagination(1,8,'1').pipe(switchMap((res => ([this.productmapperService.mapProducts(res.items ?? [])]))))
                },
                {
                    label: category1?.name?? "",
                    products$: this.productsClient.getProductsWithProductParentCategoryIdWithPagination(category1.id, 1, 8, '1').pipe(switchMap((res => ([this.productmapperService.mapProducts(res.items ?? [])]))))
                },
                {
                    label: category2?.name?? "",
                    products$: this.productsClient.getProductsWithProductParentCategoryIdWithPagination(category2.id, 1, 8, '1').pipe(switchMap((res => ([this.productmapperService.mapProducts(res.items ?? [])]))))
                },
                {
                    label: category3?.name?? "",
                    products$: this.productsClient.getProductsWithProductParentCategoryIdWithPagination(category3.id, 1, 8, '1').pipe(switchMap((res => ([this.productmapperService.mapProducts(res.items ?? [])]))))
                },
            ]);
        },
            error : (err) => {console.log(err);}
    });




        this.blockSale = this.makeDeferredData(this.productsClient.getSpecialOffersProductsWithPagination(1, 8, '1')
        .pipe(switchMap((res => ([this.productmapperService.mapProducts(res.items ?? [])])))));


        
        this.blockZones = [
            {
                image: 'assets/images/categories/category-overlay-1.jpg',
                mobileImage: 'assets/images/categories/category-overlay-1-mobile.jpg',
                category$: this.productCategoriesClient.getProductCategoryById(1).pipe(switchMap((res => [this.categorymapperService.mapProductCategoryByIdToCategory(res)])))
            },
            {
                image: 'assets/images/categories/category-overlay-2.jpg',
                mobileImage: 'assets/images/categories/category-overlay-2-mobile.jpg',
                category$: this.productCategoriesClient.getProductCategoryById(2).pipe(switchMap((res => [this.categorymapperService.mapProductCategoryByIdToCategory(res)])))
            },
            {
                image: 'assets/images/categories/category-overlay-3.jpg',
                mobileImage: 'assets/images/categories/category-overlay-3-mobile.jpg',
                category$: this.productCategoriesClient.getProductCategoryById(3).pipe(switchMap((res => [this.categorymapperService.mapProductCategoryByIdToCategory(res)])))
            },
        ];

        this.newArrivals = this.makeDeferredData(this.productsClient.getLatestProductsWithPagination(1, 8, '1')
        .pipe(switchMap((res => ([this.productmapperService.mapProducts(res.items ?? [])])))));

        this.latestPosts = this.makeDeferredData(this.blogApi.getLatestPosts(8));

        this.brands$ = this.brandsClient.getVehicleBrandsWithPagination(1,16)
        .pipe(switchMap((res => ([this.brandmapperService.mapBrands(res.items ?? [])]))));

        this.columnTopRated$ = this.productsClient.getTopRatedProductsWithPagination(1, 3, '1').pipe(switchMap((res => ([this.productmapperService.mapProducts(res.items ?? [])]))));
        this.columnSpecialOffers$ = this.productsClient.getSpecialOffersProductsWithPagination(1, 3, '1').pipe(switchMap((res => ([this.productmapperService.mapProducts(res.items ?? [])]))));
        this.columnBestsellers$ = this.productsClient.getBestSellersProductsWithPagination(1, 3, '1').pipe(switchMap((res => ([this.productmapperService.mapProducts(res.items ?? [])]))));
    }

    groupChange(carousel: ProductsCarouselData, group: SectionHeaderGroup): void {
        carousel.subject$.next(group as ProductsCarouselGroup);
    }

    private makeCarouselData(groups: ProductsCarouselGroup[]): ProductsCarouselData {
        const subject = new BehaviorSubject<ProductsCarouselGroup>(groups[0]);
        const carouselData: ProductsCarouselData = {
            subject$: subject,
            products$: subject.pipe(
                filter(x => x !== null),
                tap(() => carouselData.loading = true),
                switchMap(group => group.products$),
                tap(() => carouselData.loading = false),
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

        data.data$ = timer(0).pipe(mergeMap(() => dataSource.pipe(tap(() => data.loading = false))));

        return data;
    }
}
