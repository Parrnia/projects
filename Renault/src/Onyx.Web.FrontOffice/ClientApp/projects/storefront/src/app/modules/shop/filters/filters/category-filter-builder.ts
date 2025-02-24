
import { AbstractFilterBuilder } from './abstract-filter-builder';
import { ShopCategory } from 'projects/storefront/src/app/interfaces/category';
import { CategoryFilter } from 'projects/storefront/src/app/interfaces/filter';
import { CategorymapperService } from 'projects/storefront/src/app/mapServieces/categoriesCluster/categorymapper.service';
import { ProductCategoriesClient } from 'projects/storefront/src/app/web-api-client';
import { prepareCategory } from 'projects/storefront/src/fake-server/endpoints';
import { Observable, delay, of } from 'rxjs';

export class CategoryFilterBuilder extends AbstractFilterBuilder {
    public override value: string | null = null;

    public categoryItems: ShopCategory[] = [];

    constructor(
        slug: string,
        name: string,
        private productCategoriesClient: ProductCategoriesClient,
        private categorymapperService: CategorymapperService
    ) {
        super(slug, name);
    }




    makeItems(value: string, isLoaded: boolean): Observable<void> {
        let category = new ShopCategory();
        let shopCategoriesTree: ShopCategory[] = [];
        this.value = value;
        if (value == undefined) {
            this.productCategoriesClient.getAllFirstLayerProductCategories().subscribe({
                next: (res) => {
                    shopCategoriesTree = this.categorymapperService.mapAllFirstLayerProductCategories(res);
                },
                error: (err) => console.log(err),
                complete: () => this.categoryItems = shopCategoriesTree.map(x => prepareCategory(x))
            })
        }

        this.productCategoriesClient.getProductCategoryBySlug(value).subscribe({
            next: (res) => {
                category = this.categorymapperService.mapProductCategoryBySlug(res);
                if (category) {
                    this.categoryItems = [prepareCategory(category, 1)];
                } else {
                    this.productCategoriesClient.getAllFirstLayerProductCategories().subscribe({
                        next: (res) => {
                            shopCategoriesTree = this.categorymapperService.mapAllFirstLayerProductCategories(res);
                        },
                        error: (err) => console.log(err),
                        complete: () => {
                            this.categoryItems = shopCategoriesTree.map(x => prepareCategory(x));
                        }
                    })
                };
            },
            error: (err) => console.log(err)
        })
        return of(undefined);
    }

    build(): CategoryFilter {
        return {
            type: 'category',
            slug: this.slug,
            name: this.name,
            items: this.categoryItems,
            value: this.value,
        };
    }

}

