import { TranslateService } from '@ngx-translate/core';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
    Product,
    ProductAttribute,
    ProductAttributeGroup,
    ProductAttributeOption,
    ProductCompatibilityResult,
    ProductStock,
} from '../../../../interfaces/product';
import { Vehicle } from '../../../../interfaces/vehicle';
import { ProductGalleryLayout } from '../../../shared/components/product-gallery/product-gallery.component';
import { UrlService } from '../../../../services/url.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CartService } from '../../../../services/cart.service';
import { getCategoryPath } from '../../../../functions/utils';
import { LanguageService } from '../../../language/services/language.service';
import { BreadcrumbItem } from '../../../shared/components/breadcrumb/breadcrumb.component';
import { ProductmapperService } from 'projects/storefront/src/app/mapServieces/productsCluster/productmapper.service';
import {
    CustomerTypeEnum,
    ProductsClient,
} from 'projects/storefront/src/app/web-api-client';
import { CurrentVehicleService } from 'projects/storefront/src/app/services/current-vehicle.service';
import {
    Subject,
    Observable,
    map,
    switchMap,
    takeUntil,
    finalize,
    forkJoin,
} from 'rxjs';

export type PageProductLayout = 'sidebar' | 'full';

export type PageProductSidebarPosition = 'start' | 'end';

export interface PageProductData {
    layout: PageProductLayout;
    sidebarPosition: PageProductSidebarPosition;
    product: Product;
}

@Component({
    selector: 'app-page-product',
    templateUrl: './page-product.component.html',
    styleUrls: ['./page-product.component.scss'],
})
export class PageProductComponent implements OnInit {
    private destroy$: Subject<void> = new Subject<void>();

    layout: PageProductLayout = 'sidebar';

    sidebarPosition: PageProductSidebarPosition = 'start';

    breadcrumb$!: Observable<BreadcrumbItem[]>;

    vehicle: Vehicle | null = null;

    product!: Product;

    featuredAttributes: ProductAttribute[] = [];

    spec: ProductAttributeGroup[] = [];

    relatedProducts: Product[] = [];

    form!: FormGroup;

    addToCartInProgress = false;

    selectedColor: string | undefined = '';
    selectedMaterail: string | undefined = '';
    selectedAttributeOption!: ProductAttributeOption;
    selectedPrice: number = -1;
    selectedCompareAtPrice: number | undefined;
    selectedMaxOrderQty: number = -1;
    selectedAvailibility: ProductStock = 'out-of-stock';

    @ViewChild('tabs', { read: ElementRef }) tabsElementRef!: ElementRef;

    get tabsElement(): HTMLElement {
        return this.tabsElementRef.nativeElement;
    }

    get galleryLayout(): ProductGalleryLayout {
        return `product-${this.layout}` as ProductGalleryLayout;
    }

    constructor(
        private fb: FormBuilder,
        private router: Router,
        private route: ActivatedRoute,
        private translate: TranslateService,
        private language: LanguageService,
        private cart: CartService,
        private productsClient: ProductsClient,
        private productmapperService: ProductmapperService,
        public vehicleService: CurrentVehicleService,
        public url: UrlService
    ) {}

    ngOnInit(): void {
        const data$ = this.route.data as Observable<PageProductData>;
        const product$ = data$.pipe(
            map((data: PageProductData) => data.product)
        );
        data$.subscribe((data: PageProductData) => {
            this.layout = data.layout;
            this.sidebarPosition = data.sidebarPosition;
            debugger;
            this.product = data.product;
            this.featuredAttributes = this.product.attributes.filter(
                (x) => x.featured
            );
            this.spec = this.product.type.attributeGroups
                .map((group) => ({
                    ...group,
                    attributes: group.attributes
                        .map(
                            (attribute) =>
                                this.product.attributes.find(
                                    (x) => x.name === attribute
                                ) || null
                        )
                        .filter((x): x is ProductAttribute => x !== null),
                }))
                .filter((x) => x.attributes.length > 0);

            this.product.selectedAttributeOption =
                this.product.attributeOptions.find((e) => e.isDefault == true)!;
            let color = this.product.selectedAttributeOption.optionValues.find(
                (e) => e.name == 'Color'
            )?.value;
            let materail =
                this.product.selectedAttributeOption.optionValues.find(
                    (e) => e.name == 'Material'
                )?.value;
            this.setSelectedOptions([color, materail]);
        });

        this.breadcrumb$ = this.language.current$.pipe(
            switchMap(() =>
                product$.pipe(
                    map((product) => {
                        const categoryPath = product.categories
                            ? getCategoryPath(product.categories[0])
                            : [];

                        return [
                            {
                                label: this.translate.instant('LINK_HOME'),
                                url: '/',
                            },
                            {
                                label: this.translate.instant('LINK_SHOP'),
                                url: this.url.shop(),
                            },
                            ...categoryPath.map((x) => ({
                                label: x.name,
                                url: this.url.category(x),
                            })),
                            { label: product.name, url: '/' },
                        ];
                    })
                )
            )
        );

        data$
            .pipe(
                map((data: PageProductData) => data.product),
                switchMap((product) => {
                    return this.productsClient.getRelatedProductsWithPagination(
                        1,
                        8,
                        CustomerTypeEnum.Personal
                    );
                }),
                takeUntil(this.destroy$)
            )
            .subscribe((response) => {
                this.relatedProducts =
                    this.productmapperService.mapRelatedProductsWithPagination(
                        response.items ?? []
                    );
            });

        this.vehicleService.value$
            .pipe(takeUntil(this.destroy$))
            .subscribe((vehicle) => (this.vehicle = vehicle));

        this.form = this.fb.group({
            options: [{}],
            quantity: [
                this.product.selectedAttributeOption
                    ?.minOrderQuantityPerOrder ?? 1,
                [Validators.required],
            ],
        });
    }

    scrollToTabs(): void {
        this.tabsElement.scrollIntoView({ behavior: 'smooth' });
    }

    addToCart(): void {
        if (this.addToCartInProgress) {
            return;
        }
        if (this.form.get('quantity')!.invalid) {
            alert(this.translate.instant('ERROR_ADD_TO_CART_QUANTITY'));
            return;
        }
        if (this.form.get('options')!.invalid) {
            alert(this.translate.instant('ERROR_ADD_TO_CART_OPTIONS'));
            return;
        }

        const options: { name: string; value: string }[] = [];
        const formOptions = this.form.get('options')!.value;

        Object.keys(formOptions).forEach((optionSlug) => {
            const option = this.product.options.find(
                (x) => x.slug === optionSlug
            );

            if (!option) {
                return;
            }

            const value = option.values.find(
                (x) => x.slug === formOptions[optionSlug]
            );

            if (!value) {
                return;
            }

            options.push({ name: option.name, value: value.name });
        });

        this.addToCartInProgress = true;

        this.cart
            .add(this.product, this.form.get('quantity')!.value, options)
            .pipe(finalize(() => (this.addToCartInProgress = false)))
            .subscribe();
    }

    compatibility(): ProductCompatibilityResult {
        if (this.product.compatibility === 'all') {
            return 'all';
        }
        if (this.product.compatibility === 'unknown') {
            return 'unknown';
        }
        if (
            this.vehicle &&
            this.product.compatibility.includes(this.vehicle.kindId)
        ) {
            return 'fit';
        } else {
            return 'not-fit';
        }
    }
    setProductSelectedOptions(values: (string | undefined)[]) {
        this.product.selectedAttributeOption =
            this.product.attributeOptions.find(
                (e) =>
                    (values[0] == undefined ||
                        e.optionValues
                            .find((r) => r.name == 'Color')
                            ?.value.toLowerCase() ==
                            values[0]?.toLowerCase()) &&
                    (values[1] == undefined ||
                        e.optionValues
                            .find((r) => r.name == 'Material')
                            ?.value.toLowerCase() == values[1]?.toLowerCase())!
            )!;
        this.setSelectedOptions(values);
    }
    setSelectedOptions(values: (string | undefined)[]) {
        this.selectedColor = values[0];
        this.selectedMaterail = values[1];
        this.selectedPrice = this.product.selectedAttributeOption?.price;
        this.selectedCompareAtPrice =
            this.product.selectedAttributeOption?.compareAtPrice;
        if (this.selectedColor) {
            this.product.attributes
                .find((e) => e.name == 'Color')
                ?.values.splice(0);
            this.product.attributes
                .find((e) => e.name == 'Color')
                ?.values.push({
                    name: this.selectedColor,
                    slug: this.selectedColor,
                });
        }
        if (this.selectedMaterail) {
            this.product.attributes
                .find((e) => e.name == 'Material')
                ?.values.splice(0);
            this.product.attributes
                .find((e) => e.name == 'Material')
                ?.values.push({
                    name: this.selectedMaterail,
                    slug: this.selectedMaterail,
                });
        }
        this.selectedAvailibility =
            this.product.selectedAttributeOption?.availability ??
            'out-of-stock';
    }
}
