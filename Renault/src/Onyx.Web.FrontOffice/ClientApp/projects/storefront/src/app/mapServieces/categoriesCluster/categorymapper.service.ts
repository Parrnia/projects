import { Injectable } from '@angular/core';
import { Category, ShopCategory, ShopCategoryLayout } from '../../interfaces/category';
import { AllFirstLayerProductCategoryDto, AllProductCategoryDto, AllProductCategoryWithPaginationDto, ProductCategoryByIdDto, ProductCategoryByProductParentCategoryIdDto, ProductCategoryBySlugDto, ProductCategoryForSearchSuggestionDto } from '../../web-api-client';
import { ImageService } from '../image.service';

@Injectable({
    providedIn: 'root'
})
export class CategorymapperService {

    constructor(private imageService: ImageService) { }

    //#region AllFirstLayerCategories
    mapAllFirstLayerProductCategories(productCategoryDtos: AllFirstLayerProductCategoryDto[], layout?: ShopCategoryLayout) {
        let categories: ShopCategory[] = [];
        productCategoryDtos.forEach(l => {
            let category = new ShopCategory();
            category = this.mapAllFirstLayerProductCategory(l, layout);
            categories.push(category);
        })
        return categories;
    }

    mapAllFirstLayerProductCategory(productCategory: AllFirstLayerProductCategoryDto, layout?: ShopCategoryLayout): ShopCategory {
        let shopCategory = new ShopCategory();
        shopCategory.id = productCategory?.id ?? 0;
        shopCategory.name = productCategory?.localizedName ?? "";
        shopCategory.slug = productCategory?.slug ?? "";
        shopCategory.image = this.imageService.makeImageUrl(productCategory?.image) ?? '';
        shopCategory.type = 'shop';
        shopCategory.layout = layout ?? 'products';
        if (productCategory?.productChildrenCategories) {
            shopCategory.children = this.mapAllFirstLayerProductCategories(productCategory.productChildrenCategories);
        }
        return shopCategory;
    }
    //#endregion

    //#region AllProductCategories
    mapAllProductCategory(productCategory: AllProductCategoryDto, layout?: ShopCategoryLayout): ShopCategory {
        let shopCategory = new ShopCategory();
        shopCategory.id = productCategory?.id ?? 0;
        shopCategory.name = productCategory?.localizedName ?? "";
        shopCategory.slug = productCategory?.slug ?? "";
        shopCategory.image = this.imageService.makeImageUrl(productCategory?.image) ?? '';
        shopCategory.type = 'shop';
        shopCategory.layout = layout ?? 'products';
        if (productCategory?.productChildrenCategories) {
            shopCategory.children = this.mapAllProductCategories(productCategory.productChildrenCategories);
        }
        if (productCategory?.productParentCategory) {
            shopCategory.parent = this.mapAllProductCategory(productCategory.productParentCategory);
        }
        return shopCategory;
    }

    mapAllProductCategories(productCategoryDtos: AllProductCategoryDto[], layout?: ShopCategoryLayout) {
        let categories: ShopCategory[] = [];
        productCategoryDtos.forEach(l => {
            let category = new ShopCategory();
            category = this.mapAllProductCategory(l, layout);
            categories.push(category);
        })
        return categories;
    }
    //#endregion

    //#region ProductCategoriesByProductParentCategoryId
    mapProductCategoryByProductParentCategoryId(productCategory: ProductCategoryByProductParentCategoryIdDto, layout?: ShopCategoryLayout): ShopCategory {
        let shopCategory = new ShopCategory();
        shopCategory.id = productCategory?.id ?? 0;
        shopCategory.name = productCategory?.localizedName ?? "";
        shopCategory.slug = productCategory?.slug ?? "";
        shopCategory.image = this.imageService.makeImageUrl(productCategory?.image) ?? '';
        shopCategory.type = 'shop';
        shopCategory.layout = layout ?? 'products';
        if (productCategory?.productChildrenCategories) {
            shopCategory.children = this.mapProductCategoriesByProductParentCategoryId(productCategory.productChildrenCategories);
        }

        return shopCategory;
    }

    mapProductCategoriesByProductParentCategoryId(productCategoryDtos: ProductCategoryByProductParentCategoryIdDto[], layout?: ShopCategoryLayout) {
        let categories: ShopCategory[] = [];
        productCategoryDtos.forEach(l => {
            let category = new ShopCategory();
            category = this.mapProductCategoryByProductParentCategoryId(l, layout);
            categories.push(category);
        })
        return categories;
    }

    //#endregion

    //#region AllProductCategoriesWithPagination
    mapAllProductCategoryWithPagination(productCategory: AllProductCategoryWithPaginationDto, layout?: ShopCategoryLayout): ShopCategory {
        let shopCategory = new ShopCategory();
        shopCategory.id = productCategory?.id ?? 0;
        shopCategory.name = productCategory?.localizedName ?? "";
        shopCategory.slug = productCategory?.slug ?? "";
        shopCategory.image = this.imageService.makeImageUrl(productCategory?.image) ?? '';
        shopCategory.type = 'shop';
        shopCategory.layout = layout ?? 'products';
        if (productCategory?.productChildrenCategories) {
            shopCategory.children = this.mapAllProductCategoriesWithPagination(productCategory.productChildrenCategories);
        }
        if (productCategory?.productParentCategory) {
            shopCategory.parent = this.mapAllProductCategoryWithPagination(productCategory.productParentCategory);
        }
        return shopCategory;
    }

    mapAllProductCategoriesWithPagination(productCategoryDtos: AllProductCategoryWithPaginationDto[], layout?: ShopCategoryLayout) {
        let categories: ShopCategory[] = [];
        productCategoryDtos.forEach(l => {
            let category = new ShopCategory();
            category = this.mapAllProductCategoryWithPagination(l, layout);
            categories.push(category);
        })
        return categories;
    }
    //#endregion

    //#region ProductCategoryById
    mapProductCategoryById(productCategory: ProductCategoryByIdDto, layout?: ShopCategoryLayout): ShopCategory {
        let shopCategory = new ShopCategory();
        shopCategory.id = productCategory?.id ?? 0;
        shopCategory.name = productCategory?.localizedName ?? "";
        shopCategory.slug = productCategory?.slug ?? "";
        shopCategory.image = this.imageService.makeImageUrl(productCategory?.image) ?? '';
        shopCategory.type = 'shop';
        shopCategory.layout = layout ?? 'products';
        if (productCategory?.productChildrenCategories) {
            shopCategory.children = this.mapProductCategoriesById(productCategory.productChildrenCategories);
        }
        if (productCategory?.productParentCategory) {
            shopCategory.parent = this.mapProductCategoryById(productCategory.productParentCategory);
        }
        return shopCategory;
    }

    mapProductCategoriesById(productCategoryDtos: ProductCategoryByIdDto[], layout?: ShopCategoryLayout) {
        let categories: ShopCategory[] = [];
        productCategoryDtos.forEach(l => {
            let category = new ShopCategory();
            category = this.mapProductCategoryById(l, layout);
            categories.push(category);
        })
        return categories;
    }
    //#endregion

    //#region ProductCategoryBySlug
    mapProductCategoryBySlug(productCategory: ProductCategoryBySlugDto, layout?: ShopCategoryLayout): ShopCategory {
        let shopCategory = new ShopCategory();
        shopCategory.id = productCategory?.id ?? 0;
        shopCategory.name = productCategory?.localizedName ?? "";
        shopCategory.slug = productCategory?.slug ?? "";
        shopCategory.image = this.imageService.makeImageUrl(productCategory?.image) ?? '';
        shopCategory.type = 'shop';
        shopCategory.layout = layout ?? 'products';
        if (productCategory?.productChildrenCategories) {
            shopCategory.children = this.mapProductCategoriesBySlug(productCategory.productChildrenCategories);
        }
        if (productCategory?.productParentCategory) {
            shopCategory.parent = this.mapProductCategoryBySlug(productCategory.productParentCategory);
        }
        return shopCategory;
    }

    mapProductCategoriesBySlug(productCategoryDtos: ProductCategoryBySlugDto[], layout?: ShopCategoryLayout) {
        let categories: ShopCategory[] = [];
        productCategoryDtos.forEach(l => {
            let category = new ShopCategory();
            category = this.mapProductCategoryBySlug(l, layout);
            categories.push(category);
        })
        return categories;
    }
    //#endregion

    //#region ProductCategoryForSearchSuggestion
    mapProductCategoryForSearchSuggestion(productCategory: ProductCategoryForSearchSuggestionDto, layout?: ShopCategoryLayout): ShopCategory {
        let shopCategory = new ShopCategory();
        shopCategory.id = productCategory?.id ?? 0;
        shopCategory.name = productCategory?.localizedName ?? "";
        shopCategory.slug = productCategory?.slug ?? "";
        shopCategory.type = 'shop';
        shopCategory.layout = layout ?? 'products';
        return shopCategory;
    }

    mapProductCategoriesForSearchSuggestion(productCategoryDtos: ProductCategoryForSearchSuggestionDto[], layout?: ShopCategoryLayout) {
        let categories: ShopCategory[] = [];
        productCategoryDtos.forEach(l => {
            let category = new ShopCategory();
            category = this.mapProductCategoryForSearchSuggestion(l, layout);
            categories.push(category);
        })
        return categories;
    }
    //#endregion

    flatTree<T extends Category>(categories: T[]): T[] {
        let result: T[] = [];

        categories.forEach(category => result = [...result, category, ...this.flatTree(category.children as T[])]);

        return result;
    }

}
