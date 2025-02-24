import { Injectable } from "@angular/core";
import { Brand } from "../../interfaces/brand";
import { ShopCategoryLayout, ShopCategory } from "../../interfaces/category";
import { Product, ProductType, ProductAttribute, ProductTypeAttributeGroup, ProductOption, ProductOptionColor, ProductOptionMaterial, ProductOptionValueColor, ProductOptionValueBase, ProductAttributeOption, ProductAttributeOptionValue, ProductAttributeValue } from "../../interfaces/product";
import { CustomerTypeEnum, ProductByIdDto, ProductCategoryDto, ProductOptionColorByIdDto, ProductOptionMaterialByIdDto, BrandByIdDto, ProductTypeAttributeGroupByIdDto, ProductOptionValueColorByIdDto, ProductOptionValueMaterialByIdDto, ProductAttributeByIdDto, ProductAttributeOptionByIdDto, AvailabilityEnum, ProductAttributeOptionValueByIdDto, ProductBySlugDto, ProductOptionColorBySlugDto, ProductOptionMaterialBySlugDto, BrandBySlugDto, ProductTypeAttributeGroupBySlugDto, ProductOptionValueColorBySlugDto, ProductOptionValueMaterialBySlugDto, ProductAttributeBySlugDto, ProductAttributeOptionBySlugDto, ProductAttributeOptionValueBySlugDto, AllProductDto, MainProductOptionColorDto, MainProductOptionMaterialDto, ProductByBrandIdDto, ProductByKindIdDto, ProductByProductCategoryIdDto, ProductByProductCategorySlugDto, BestSellerProductWithPaginationDto, FeaturedProductWithPaginationDto, LatestProductWithPaginationDto, NewProductWithPaginationDto, PopularProductWithPaginationDto, ProductByBrandIdWithPaginationDto, ProductByKindIdWithPaginationDto, ProductByProductCategoryIdWithPaginationDto, ProductWithPaginationDto, RelatedProductWithPaginationDto, SalesProductWithPaginationDto, SpecialOffersProductWithPaginationDto, TopRatedProductWithPaginationDto, ProductForFilterResultDto, ProductForSearchSuggestionDto, ProductAttributeOptionForSearchSuggestionDto, ProductAttributeOptionValueForSearchSuggestionDto, MainBrandDto, MainProductTypeAttributeGroupDto, MainProductOptionValueColorDto, MainProductOptionValueMaterialDto, MainProductAttributeDto, MainProductAttributeOptionDto, MainProductAttributeOptionValueDto, CompatibilityEnum, ProductOptionTypeEnum } from "../../web-api-client";
import { ImageService } from "../image.service";

@Injectable({
    providedIn: 'root',
})
export class ProductmapperService {
    private readonly currentCustomerTypeEnum: CustomerTypeEnum;
    constructor(private imageService: ImageService) {
        this.currentCustomerTypeEnum = parseInt(
            localStorage.getItem('currentCustomerCustomerTypeEnum') ??
            CustomerTypeEnum.Personal.toString(),
            10
        ) as CustomerTypeEnum;
    }

    //#region ProductById
    mapProductById(productDto: ProductByIdDto) {
        let product = new Product();
        product.type = new ProductType();
        product.attributes = [new ProductAttribute()];
        product.id = productDto.id ?? 0;
        product.sevenId = productDto.related7SoftProductId ?? '';
        product.name = productDto.localizedName ?? '';
        product.excerpt = productDto.excerpt ?? '';
        product.description = productDto.description ?? '';
        product.slug = productDto.slug ?? '';
        product.sku = productDto.sku ?? productDto.productNo;
        product.partNumber = productDto.productNo ?? '';
        product.images = productDto.images?.map(
            (e) => this.imageService.makeImageUrl(e.image) ?? ''
        );
        product.reviews = productDto.reviewsLength;
        product.compatibility = this.mapProductCompatibility(
            productDto.kindIds ?? [],
            productDto.compatibility ?? 2
        );
        product.brand = this.mapBrandById(productDto.productBrand!);
        product.tags = productDto.tags?.map((l) => l.faTitle!);
        product.type.name = productDto.productAttributeType?.name ?? '';
        product.type.slug = productDto.productAttributeType?.slug ?? '';
        product.type.attributeGroups = this.mapProductAttributeGroupsById(
            productDto.productAttributeType?.attributeGroups ?? []
        );
        product.categories = this.mapProductCategories([
            productDto.productCategory ?? new ProductCategoryDto(),
        ]);
        product.attributes = this.mapProductAttributesById(
            productDto.attributes ?? []
        );
        product.options = this.mapProductOptionsById(
            productDto.colorOption ?? new ProductOptionColorByIdDto(),
            productDto.materialOption ?? new ProductOptionMaterialByIdDto()
        );
        product.rating = productDto.rating;
        product.attributeOptions = this.mapProductAttributeOptionsById(
            productDto.attributeOptions ?? []
        );
        product.selectedAttributeOption = this.mapProductAttributeOptionById(
            productDto.selectedProductAttributeOption!
        );
        product.countryName = productDto.countryName;
        return product;
    }

    mapProductsById(productDtos: ProductByIdDto[]) {
        let products: Product[] = [];
        productDtos?.forEach(async (l) => {
            let product = new Product();
            product = this.mapProductById(l);
            products.push(product);
        });
        return products;
    }
    //#region Brand
    mapBrandById(brandDto: BrandByIdDto) {
        let brand = new Brand();
        brand.name = brandDto.localizedName ?? '';
        brand.slug = brandDto.slug ?? '';
        brand.country = brandDto.countryName ?? '';
        brand.image = brandDto.brandLogo ?? '';
        return brand;
    }
    //#endregion

    //#region ProductTypeAttributeGroup
    mapProductAttributeGroupsById(
        productAttributeGroups: ProductTypeAttributeGroupByIdDto[]
    ) {
        let res: ProductTypeAttributeGroup[] = [];
        productAttributeGroups.forEach((l) => {
            let child: ProductTypeAttributeGroup = {
                name: '',
                slug: '',
                attributes: [],
            };
            child.name = l.name ?? '';
            child.slug = l.slug ?? '';
            child.attributes = l.attributes?.map((x) => x.value ?? '') ?? [];
            res.push(child);
        });
        return res;
    }
    //#endregion

    //#region ProductOption
    mapProductOptionsById(
        productOptionColorDto: ProductOptionColorByIdDto,
        productOptionMaterialDto: ProductOptionMaterialByIdDto
    ) {
        let res: ProductOption[] = [];

        let child1 = new ProductOptionColor();
        child1.name = productOptionColorDto.name ?? '';
        child1.slug = productOptionColorDto.slug ?? '';
        child1.type = 'color';
        child1.values = this.mapProductOptionValueColorsById(
            productOptionColorDto.values ?? []
        );
        res.push(child1);

        let child2 = new ProductOptionMaterial();
        child2.name = productOptionMaterialDto.name ?? '';
        child2.slug = productOptionMaterialDto.slug ?? '';
        child2.type = 'material';
        child2.values = this.mapProductOptionValueMaterialsById(
            productOptionMaterialDto.values ?? []
        );
        res.push(child2);

        return res;
    }
    mapProductOptionValueColorsById(
        productOptionValueColorDtos: ProductOptionValueColorByIdDto[]
    ) {
        let res: ProductOptionValueColor[] = [];
        productOptionValueColorDtos.forEach((l) => {
            let child = new ProductOptionValueColor();
            child.name = l.name ?? '';
            child.slug = l.slug ?? '';
            child.color = l.color ?? '';
            res.push(child);
        });
        return res;
    }
    mapProductOptionValueMaterialsById(
        productOptionValueMaterialDtos: ProductOptionValueMaterialByIdDto[]
    ) {
        let res: ProductOptionValueBase[] = [];
        productOptionValueMaterialDtos.forEach((l) => {
            let child = new ProductOptionValueBase();
            child.name = l.name ?? '';
            child.slug = l.slug ?? '';
            res.push(child);
        });
        return res;
    }
    //#endregion

    //#region ProductAttribute
    mapProductAttributesById(productAttributeDtos: ProductAttributeByIdDto[]) {
        let res: ProductAttribute[] = [];
        productAttributeDtos.forEach((l) => {
            let child: ProductAttribute = {
                name: '',
                slug: '',
                featured: false,
                values: [],
                customFields: [],
            };
            child.name = l.name ?? '';
            child.slug = l.slug ?? '';
            child.featured = l.featured ?? false;
            child.values = this.mapProductAttributeValue(
                l.valueSlug!,
                l.valueName!
            );
            res.push(child);
        });
        return res;
    }

    //#endregion

    //#region ProductAttributeOption
    mapProductAttributeOptionById(
        productAttributeOptionDto: ProductAttributeOptionByIdDto
    ) {
        let child: ProductAttributeOption = {
            id: 0,
            availability: 'out-of-stock',
            isDefault: false,
            optionValues: [],
            price: 0,
            maxOrderQuantityPerOrder: 1,
            minOrderQuantityPerOrder: 1,
            badges: [],
            compareAtPrice: 0,
        };
        let price = productAttributeOptionDto?.prices
            ? productAttributeOptionDto.prices.sort(
                (a, b) => b.date!.getTime() - a.date!.getTime()
            )[0]
            : null;
        let selectedRole =
            productAttributeOptionDto?.productAttributeOptionRoles
                ? productAttributeOptionDto.productAttributeOptionRoles[0]
                : null;
        child.id = productAttributeOptionDto?.id ?? 0;
        child.availability =
            selectedRole?.availability == AvailabilityEnum.InStock
                ? 'in-stock'
                : 'out-of-stock';
        child.isDefault = productAttributeOptionDto?.isDefault ?? false;
        child.price =
            selectedRole?.discountPercent! > 0
                ? price?.mainPrice! *
                ((100 - selectedRole?.discountPercent!) / 100)
                : price?.mainPrice!;
        child.compareAtPrice =
            selectedRole?.discountPercent! > 0 ? price?.mainPrice : undefined;
        child.discountPercent = selectedRole?.discountPercent ?? 0;
        child.maxOrderQuantityPerOrder = selectedRole?.currentMaxOrderQty ?? 0;
        child.minOrderQuantityPerOrder = selectedRole?.currentMinOrderQty ?? 0;
        child.badges =
            productAttributeOptionDto?.badges?.map((l) => l.value ?? '') ?? [];
        child.optionValues = this.mapProductAttributeOptionValuesById(
            productAttributeOptionDto?.optionValues ?? []
        );
        child.price = this.AddTax(child.price)!;
        child.compareAtPrice = this.AddTax(child.compareAtPrice);
        child.price = this.ConvertToTooman(child.price)!;
        child.compareAtPrice = this.ConvertToTooman(child.compareAtPrice);
        return child;
    }
    mapProductAttributeOptionsById(
        productAttributeOptionDtos: ProductAttributeOptionByIdDto[]
    ) {
        let res: ProductAttributeOption[] = [];
        productAttributeOptionDtos.forEach((l) => {
            res.push(this.mapProductAttributeOptionById(l));
        });
        return res;
    }

    async findSelectedProductAttributeOptionById(
        productAttributeOptionDtos: ProductAttributeOptionByIdDto[]
    ) {
        let selectedOption!: ProductAttributeOptionByIdDto;

        for (const option of productAttributeOptionDtos) {
            const availability = option.productAttributeOptionRoles?.find(
                (c) => c.customerTypeEnum == this.currentCustomerTypeEnum
            )?.availability;

            if (option.isDefault && availability === AvailabilityEnum.InStock) {
                selectedOption = option;
                break;
            }
        }

        if (!selectedOption) {
            for (const option of productAttributeOptionDtos) {
                const availability = option.productAttributeOptionRoles?.find(
                    (c) => c.customerTypeEnum == this.currentCustomerTypeEnum
                )?.availability;
                if (availability === AvailabilityEnum.InStock) {
                    selectedOption = option;
                    break;
                }
            }
        }

        if (!selectedOption && productAttributeOptionDtos.length > 0) {
            selectedOption = productAttributeOptionDtos[0];
        }

        return this.mapProductAttributeOptionById(selectedOption);
    }
    mapProductAttributeOptionValuesById(
        productAttributeOptionValueDtos: ProductAttributeOptionValueByIdDto[]
    ) {
        let res: ProductAttributeOptionValue[] = [];
        productAttributeOptionValueDtos.forEach((l) => {
            let child: ProductAttributeOptionValue = {
                id: 0,
                name: '',
                value: '',
            };
            child.id = l.id ?? 0;
            child.name = l.name ?? '';
            child.value = l.value ?? '';
            res.push(child);
        });
        return res;
    }
    //#endregion
    //#endregion

    //#region ProductBySlug
    mapProductBySlug(productDto: ProductBySlugDto) {
        let product = new Product();
        product.type = new ProductType();
        product.attributes = [new ProductAttribute()];
        product.id = productDto.id ?? 0;
        product.sevenId = productDto.related7SoftProductId ?? '';
        product.name = productDto.localizedName ?? '';
        product.excerpt = productDto.excerpt ?? '';
        product.description = productDto.description ?? '';
        product.slug = productDto.slug ?? '';
        product.sku = productDto.sku ?? productDto.productNo;
        product.partNumber = productDto.productNo ?? '';
        product.images = productDto.images?.map(
            (e) => this.imageService.makeImageUrl(e.image) ?? ''
        );
        product.reviews = productDto.reviewsLength;
        product.compatibility = this.mapProductCompatibility(
            productDto.kindIds ?? [],
            productDto.compatibility ?? 2
        );
        product.brand = this.mapBrandBySlug(productDto.productBrand!);
        product.tags = productDto.tags?.map((l) => l.faTitle!);
        product.type.name = productDto.productAttributeType?.name ?? '';
        product.type.slug = productDto.productAttributeType?.slug ?? '';
        product.type.attributeGroups = this.mapProductAttributeGroupsBySlug(
            productDto.productAttributeType?.attributeGroups ?? []
        );
        product.categories = this.mapProductCategories([
            productDto.productCategory ?? new ProductCategoryDto(),
        ]);
        product.attributes = this.mapProductAttributesBySlug(
            productDto.attributes ?? []
        );
        product.options = this.mapProductOptionsBySlug(
            productDto.colorOption ?? new ProductOptionColorBySlugDto(),
            productDto.materialOption ?? new ProductOptionMaterialBySlugDto()
        );
        product.rating = productDto.rating;
        product.attributeOptions = this.mapProductAttributeOptionsBySlug(
            productDto.attributeOptions ?? []
        );
        product.selectedAttributeOption = this.mapProductAttributeOptionBySlug(
            productDto.selectedProductAttributeOption!
        );
        product.countryName = productDto.countryName;
        return product;
    }

    mapProductsBySlug(productDtos: ProductBySlugDto[]) {
        let products: Product[] = [];
        productDtos?.forEach(async (l) => {
            let product = new Product();
            product = this.mapProductBySlug(l);
            products.push(product);
        });
        return products;
    }
    //#region Brand
    mapBrandBySlug(brandDto: BrandBySlugDto) {
        let brand = new Brand();
        brand.name = brandDto.localizedName ?? '';
        brand.slug = brandDto.slug ?? '';
        brand.country = brandDto.countryName ?? '';
        brand.image = brandDto.brandLogo ?? '';
        return brand;
    }
    //#endregion

    //#region ProductTypeAttributeGroup
    mapProductAttributeGroupsBySlug(
        productAttributeGroups: ProductTypeAttributeGroupBySlugDto[]
    ) {
        let res: ProductTypeAttributeGroup[] = [];
        productAttributeGroups.forEach((l) => {
            let child: ProductTypeAttributeGroup = {
                name: '',
                slug: '',
                attributes: [],
            };
            child.name = l.name ?? '';
            child.slug = l.slug ?? '';
            child.attributes = l.attributes?.map((x) => x.value ?? '') ?? [];
            res.push(child);
        });
        return res;
    }
    //#endregion

    //#region ProductOption
    mapProductOptionsBySlug(
        productOptionColorDto: ProductOptionColorBySlugDto,
        productOptionMaterialDto: ProductOptionMaterialBySlugDto
    ) {
        let res: ProductOption[] = [];

        let child1 = new ProductOptionColor();
        child1.name = productOptionColorDto.name ?? '';
        child1.slug = productOptionColorDto.slug ?? '';
        child1.type = 'color';
        child1.values = this.mapProductOptionValueColorsBySlug(
            productOptionColorDto.values ?? []
        );
        res.push(child1);

        let child2 = new ProductOptionMaterial();
        child2.name = productOptionMaterialDto.name ?? '';
        child2.slug = productOptionMaterialDto.slug ?? '';
        child2.type = 'material';
        child2.values = this.mapProductOptionValueMaterialsBySlug(
            productOptionMaterialDto.values ?? []
        );
        res.push(child2);

        return res;
    }
    mapProductOptionValueColorsBySlug(
        productOptionValueColorDtos: ProductOptionValueColorBySlugDto[]
    ) {
        let res: ProductOptionValueColor[] = [];
        productOptionValueColorDtos.forEach((l) => {
            let child = new ProductOptionValueColor();
            child.name = l.name ?? '';
            child.slug = l.slug ?? '';
            child.color = l.color ?? '';
            res.push(child);
        });
        return res;
    }
    mapProductOptionValueMaterialsBySlug(
        productOptionValueMaterialDtos: ProductOptionValueMaterialBySlugDto[]
    ) {
        let res: ProductOptionValueBase[] = [];
        productOptionValueMaterialDtos.forEach((l) => {
            let child = new ProductOptionValueBase();
            child.name = l.name ?? '';
            child.slug = l.slug ?? '';
            res.push(child);
        });
        return res;
    }
    //#endregion

    //#region ProductAttribute
    mapProductAttributesBySlug(
        productAttributeDtos: ProductAttributeBySlugDto[]
    ) {
        let res: ProductAttribute[] = [];
        productAttributeDtos.forEach((l) => {
            let child: ProductAttribute = {
                name: '',
                slug: '',
                featured: false,
                values: [],
                customFields: [],
            };
            child.name = l.name ?? '';
            child.slug = l.slug ?? '';
            child.featured = l.featured ?? false;
            child.values = this.mapProductAttributeValue(
                l.valueSlug!,
                l.valueName!
            );
            res.push(child);
        });
        return res;
    }

    //#endregion

    //#region ProductAttributeOption
    mapProductAttributeOptionBySlug(
        productAttributeOptionDto: ProductAttributeOptionBySlugDto
    ) {
        let child: ProductAttributeOption = {
            id: 0,
            availability: 'out-of-stock',
            isDefault: false,
            optionValues: [],
            price: 0,
            maxOrderQuantityPerOrder: 1,
            minOrderQuantityPerOrder: 1,
            badges: [],
            compareAtPrice: 0,
        };
        let price = productAttributeOptionDto?.prices
            ? productAttributeOptionDto.prices.sort(
                (a, b) => b.date!.getTime() - a.date!.getTime()
            )[0]
            : null;
        let selectedRole =
            productAttributeOptionDto?.productAttributeOptionRoles
                ? productAttributeOptionDto.productAttributeOptionRoles[0]
                : null;
        child.id = productAttributeOptionDto?.id ?? 0;
        child.availability =
            selectedRole?.availability == AvailabilityEnum.InStock
                ? 'in-stock'
                : 'out-of-stock';
        child.isDefault = productAttributeOptionDto?.isDefault ?? false;
        child.price =
            selectedRole?.discountPercent! > 0
                ? price?.mainPrice! *
                ((100 - selectedRole?.discountPercent!) / 100)
                : price?.mainPrice!;
        child.compareAtPrice =
            selectedRole?.discountPercent! > 0 ? price?.mainPrice : undefined;
        child.discountPercent = selectedRole?.discountPercent ?? 0;
        child.maxOrderQuantityPerOrder = selectedRole?.currentMaxOrderQty ?? 0;
        child.minOrderQuantityPerOrder = selectedRole?.currentMinOrderQty ?? 0;
        child.badges =
            productAttributeOptionDto?.badges?.map((l) => l.value ?? '') ?? [];
        child.optionValues = this.mapProductAttributeOptionValuesBySlug(
            productAttributeOptionDto?.optionValues ?? []
        );
        child.price = this.AddTax(child.price)!;
        child.compareAtPrice = this.AddTax(child.compareAtPrice);
        child.price = this.ConvertToTooman(child.price)!;
        child.compareAtPrice = this.ConvertToTooman(child.compareAtPrice);
        return child;
    }
    mapProductAttributeOptionsBySlug(
        productAttributeOptionDtos: ProductAttributeOptionBySlugDto[]
    ) {
        let res: ProductAttributeOption[] = [];
        productAttributeOptionDtos.forEach((l) => {
            res.push(this.mapProductAttributeOptionBySlug(l));
        });
        return res;
    }

    async findSelectedProductAttributeOptionBySlug(
        productAttributeOptionDtos: ProductAttributeOptionBySlugDto[]
    ) {
        let selectedOption!: ProductAttributeOptionBySlugDto;

        for (const option of productAttributeOptionDtos) {
            const availability = option.productAttributeOptionRoles?.find(
                (c) => c.customerTypeEnum == this.currentCustomerTypeEnum
            )?.availability;

            if (option.isDefault && availability === AvailabilityEnum.InStock) {
                selectedOption = option;
                break;
            }
        }

        if (!selectedOption) {
            for (const option of productAttributeOptionDtos) {
                const availability = option.productAttributeOptionRoles?.find(
                    (c) => c.customerTypeEnum == this.currentCustomerTypeEnum
                )?.availability;
                if (availability === AvailabilityEnum.InStock) {
                    selectedOption = option;
                    break;
                }
            }
        }

        if (!selectedOption && productAttributeOptionDtos.length > 0) {
            selectedOption = productAttributeOptionDtos[0];
        }

        return this.mapProductAttributeOptionBySlug(selectedOption);
    }
    mapProductAttributeOptionValuesBySlug(
        productAttributeOptionValueDtos: ProductAttributeOptionValueBySlugDto[]
    ) {
        let res: ProductAttributeOptionValue[] = [];
        productAttributeOptionValueDtos.forEach((l) => {
            let child: ProductAttributeOptionValue = {
                id: 0,
                name: '',
                value: '',
            };
            child.id = l.id ?? 0;
            child.name = l.name ?? '';
            child.value = l.value ?? '';
            res.push(child);
        });
        return res;
    }
    //#endregion
    //#endregion

    //#region AllProduct
    mapAllProduct(productDto: AllProductDto) {
        let product = new Product();
        product.type = new ProductType();
        product.attributes = [new ProductAttribute()];
        product.id = productDto.id ?? 0;
        product.sevenId = productDto.related7SoftProductId ?? '';
        product.name = productDto.localizedName ?? '';
        product.excerpt = productDto.excerpt ?? '';
        product.description = productDto.description ?? '';
        product.slug = productDto.slug ?? '';
        product.sku = productDto.sku ?? productDto.productNo;
        product.partNumber = productDto.productNo ?? '';
        product.images = productDto.images?.map(
            (e) => this.imageService.makeImageUrl(e.image) ?? ''
        );
        product.reviews = productDto.reviewsLength;
        product.compatibility = this.mapProductCompatibility(
            productDto.kindIds ?? [],
            productDto.compatibility ?? 2
        );
        product.brand = this.mapMainBrand(productDto.productBrand!);
        product.tags = productDto.tags?.map((l) => l.faTitle!);
        product.type.name = productDto.productAttributeType?.name ?? '';
        product.type.slug = productDto.productAttributeType?.slug ?? '';
        product.type.attributeGroups = this.mapMainProductAttributeGroups(
            productDto.productAttributeType?.attributeGroups ?? []
        );
        product.categories = this.mapProductCategories([
            productDto.productCategory ?? new ProductCategoryDto(),
        ]);
        product.attributes = this.mapMainProductAttributes(
            productDto.attributes ?? []
        );
        product.options = this.mapMainProductOptions(
            productDto.colorOption ?? new MainProductOptionColorDto(),
            productDto.materialOption ?? new MainProductOptionMaterialDto()
        );
        product.rating = productDto.rating;
        product.attributeOptions = this.mapMainProductAttributeOptions(
            productDto.attributeOptions ?? []
        );
        product.selectedAttributeOption = this.mapMainProductAttributeOption(
            productDto.selectedProductAttributeOption!
        );
        product.countryName = productDto.countryName;
        return product;
    }

    mapAllProducts(productDtos: AllProductDto[]) {
        let products: Product[] = [];
        productDtos?.forEach(async (l) => {
            let product = new Product();
            product = this.mapAllProduct(l);
            products.push(product);
        });
        return products;
    }
    
    //#endregion

    //#region ProductByBrandId
    mapProductByBrandId(productDto: ProductByBrandIdDto) {
        let product = new Product();
        product.type = new ProductType();
        product.attributes = [new ProductAttribute()];
        product.id = productDto.id ?? 0;
        product.sevenId = productDto.related7SoftProductId ?? '';
        product.name = productDto.localizedName ?? '';
        product.excerpt = productDto.excerpt ?? '';
        product.description = productDto.description ?? '';
        product.slug = productDto.slug ?? '';
        product.sku = productDto.sku ?? productDto.productNo;
        product.partNumber = productDto.productNo ?? '';
        product.images = productDto.images?.map(
            (e) => this.imageService.makeImageUrl(e.image) ?? ''
        );
        product.reviews = productDto.reviewsLength;
        product.compatibility = this.mapProductCompatibility(
            productDto.kindIds ?? [],
            productDto.compatibility ?? 2
        );
        product.brand = this.mapMainBrand(productDto.productBrand!);
        product.tags = productDto.tags?.map((l) => l.faTitle!);
        product.type.name = productDto.productAttributeType?.name ?? '';
        product.type.slug = productDto.productAttributeType?.slug ?? '';
        product.type.attributeGroups = this.mapMainProductAttributeGroups(
            productDto.productAttributeType?.attributeGroups ?? []
        );
        product.categories = this.mapProductCategories([
            productDto.productCategory ?? new ProductCategoryDto(),
        ]);
        product.attributes = this.mapMainProductAttributes(
            productDto.attributes ?? []
        );
        product.options = this.mapMainProductOptions(
            productDto.colorOption ?? new MainProductOptionColorDto(),
            productDto.materialOption ?? new MainProductOptionMaterialDto()
        );
        product.rating = productDto.rating;
        product.attributeOptions = this.mapMainProductAttributeOptions(
            productDto.attributeOptions ?? []
        );
        product.selectedAttributeOption =
            this.mapMainProductAttributeOption(
                productDto.selectedProductAttributeOption!
            );
        product.countryName = productDto.countryName;
        return product;
    }

    mapProductsByBrandId(productDtos: ProductByBrandIdDto[]) {
        let products: Product[] = [];
        productDtos?.forEach(async (l) => {
            let product = new Product();
            product = this.mapProductByBrandId(l);
            products.push(product);
        });
        return products;
    }
    //#endregion

    //#region ProductByKindId
    mapProductByKindId(productDto: ProductByKindIdDto) {
        let product = new Product();
        product.type = new ProductType();
        product.attributes = [new ProductAttribute()];
        product.id = productDto.id ?? 0;
        product.sevenId = productDto.related7SoftProductId ?? '';
        product.name = productDto.localizedName ?? '';
        product.excerpt = productDto.excerpt ?? '';
        product.description = productDto.description ?? '';
        product.slug = productDto.slug ?? '';
        product.sku = productDto.sku ?? productDto.productNo;
        product.partNumber = productDto.productNo ?? '';
        product.images = productDto.images?.map(
            (e) => this.imageService.makeImageUrl(e.image) ?? ''
        );
        product.reviews = productDto.reviewsLength;
        product.compatibility = this.mapProductCompatibility(
            productDto.kindIds ?? [],
            productDto.compatibility ?? 2
        );
        product.brand = this.mapMainBrand(productDto.productBrand!);
        product.tags = productDto.tags?.map((l) => l.faTitle!);
        product.type.name = productDto.productAttributeType?.name ?? '';
        product.type.slug = productDto.productAttributeType?.slug ?? '';
        product.type.attributeGroups = this.mapMainProductAttributeGroups(
            productDto.productAttributeType?.attributeGroups ?? []
        );
        product.categories = this.mapProductCategories([
            productDto.productCategory ?? new ProductCategoryDto(),
        ]);
        product.attributes = this.mapMainProductAttributes(
            productDto.attributes ?? []
        );
        product.options = this.mapMainProductOptions(
            productDto.colorOption ?? new MainProductOptionColorDto(),
            productDto.materialOption ?? new MainProductOptionMaterialDto()
        );
        product.rating = productDto.rating;
        product.attributeOptions = this.mapMainProductAttributeOptions(
            productDto.attributeOptions ?? []
        );
        product.selectedAttributeOption =
            this.mapMainProductAttributeOption(
                productDto.selectedProductAttributeOption!
            );
        product.countryName = productDto.countryName;
        return product;
    }

    mapProductsByKindId(productDtos: ProductByKindIdDto[]) {
        let products: Product[] = [];
        productDtos?.forEach(async (l) => {
            let product = new Product();
            product = this.mapProductByKindId(l);
            products.push(product);
        });
        return products;
    }
    //#endregion

    //#region ProductByProductCategoryId
    mapProductByProductCategoryId(productDto: ProductByProductCategoryIdDto) {
        let product = new Product();
        product.type = new ProductType();
        product.attributes = [new ProductAttribute()];
        product.id = productDto.id ?? 0;
        product.sevenId = productDto.related7SoftProductId ?? '';
        product.name = productDto.localizedName ?? '';
        product.excerpt = productDto.excerpt ?? '';
        product.description = productDto.description ?? '';
        product.slug = productDto.slug ?? '';
        product.sku = productDto.sku ?? productDto.productNo;
        product.partNumber = productDto.productNo ?? '';
        product.images = productDto.images?.map(
            (e) => this.imageService.makeImageUrl(e.image) ?? ''
        );
        product.reviews = productDto.reviewsLength;
        product.compatibility = this.mapProductCompatibility(
            productDto.kindIds ?? [],
            productDto.compatibility ?? 2
        );
        product.brand = this.mapMainBrand(
            productDto.productBrand!
        );
        product.tags = productDto.tags?.map((l) => l.faTitle!);
        product.type.name = productDto.productAttributeType?.name ?? '';
        product.type.slug = productDto.productAttributeType?.slug ?? '';
        product.type.attributeGroups =
            this.mapMainProductAttributeGroups(
                productDto.productAttributeType?.attributeGroups ?? []
            );
        product.categories = this.mapProductCategories([
            productDto.productCategory ?? new ProductCategoryDto(),
        ]);
        product.attributes = this.mapMainProductAttributes(
            productDto.attributes ?? []
        );
        product.options = this.mapMainProductOptions(
            productDto.colorOption ??
            new MainProductOptionColorDto(),
            productDto.materialOption ??
            new MainProductOptionMaterialDto()
        );
        product.rating = productDto.rating;
        product.attributeOptions =
            this.mapMainProductAttributeOptions(
                productDto.attributeOptions ?? []
            );
        product.selectedAttributeOption =
            this.mapMainProductAttributeOption(
                productDto.selectedProductAttributeOption!
            );
        product.countryName = productDto.countryName;
        return product;
    }

    mapProductsByProductCategoryId(
        productDtos: ProductByProductCategoryIdDto[]
    ) {
        let products: Product[] = [];
        productDtos?.forEach(async (l) => {
            let product = new Product();
            product = this.mapProductByProductCategoryId(l);
            products.push(product);
        });
        return products;
    }
    //#endregion

    //#region ProductByProductCategorySlug
    mapProductByProductCategorySlug(
        productDto: ProductByProductCategorySlugDto
    ) {
        let product = new Product();
        product.type = new ProductType();
        product.attributes = [new ProductAttribute()];
        product.id = productDto.id ?? 0;
        product.sevenId = productDto.related7SoftProductId ?? '';
        product.name = productDto.localizedName ?? '';
        product.excerpt = productDto.excerpt ?? '';
        product.description = productDto.description ?? '';
        product.slug = productDto.slug ?? '';
        product.sku = productDto.sku ?? productDto.productNo;
        product.partNumber = productDto.productNo ?? '';
        product.images = productDto.images?.map(
            (e) => this.imageService.makeImageUrl(e.image) ?? ''
        );
        product.reviews = productDto.reviewsLength;
        product.compatibility = this.mapProductCompatibility(
            productDto.kindIds ?? [],
            productDto.compatibility ?? 2
        );
        product.brand = this.mapMainBrand(
            productDto.productBrand!
        );
        product.tags = productDto.tags?.map((l) => l.faTitle!);
        product.type.name = productDto.productAttributeType?.name ?? '';
        product.type.slug = productDto.productAttributeType?.slug ?? '';
        product.type.attributeGroups =
            this.mapMainProductAttributeGroups(
                productDto.productAttributeType?.attributeGroups ?? []
            );
        product.categories = this.mapProductCategories([
            productDto.productCategory ?? new ProductCategoryDto(),
        ]);
        product.attributes = this.mapMainProductAttributes(
            productDto.attributes ?? []
        );
        product.options = this.mapMainProductOptions(
            productDto.colorOption ??
            new MainProductOptionColorDto(),
            productDto.materialOption ??
            new MainProductOptionMaterialDto()
        );
        product.rating = productDto.rating;
        product.attributeOptions =
            this.mapMainProductAttributeOptions(
                productDto.attributeOptions ?? []
            );
        product.selectedAttributeOption =
            this.mapMainProductAttributeOption(
                productDto.selectedProductAttributeOption!
            );
        product.countryName = productDto.countryName;
        return product;
    }

    mapProductsByProductCategorySlug(
        productDtos: ProductByProductCategorySlugDto[]
    ) {
        let products: Product[] = [];
        productDtos?.forEach(async (l) => {
            let product = new Product();
            product = this.mapProductByProductCategorySlug(l);
            products.push(product);
        });
        return products;
    }
    //#endregion

    //#region BestSellersProductWithPagination
    mapBestSellerProductWithPagination(
        productDto: BestSellerProductWithPaginationDto
    ) {
        let product = new Product();
        product.type = new ProductType();
        product.attributes = [new ProductAttribute()];
        product.id = productDto.id ?? 0;
        product.sevenId = productDto.related7SoftProductId ?? '';
        product.name = productDto.localizedName ?? '';
        product.excerpt = productDto.excerpt ?? '';
        product.description = productDto.description ?? '';
        product.slug = productDto.slug ?? '';
        product.sku = productDto.sku ?? productDto.productNo;
        product.partNumber = productDto.productNo ?? '';
        product.images = productDto.images?.map(
            (e) => this.imageService.makeImageUrl(e.image) ?? ''
        );
        product.reviews = productDto.reviewsLength;
        product.compatibility = this.mapProductCompatibility(
            productDto.kindIds ?? [],
            productDto.compatibility ?? 2
        );
        product.brand = this.mapMainBrand(
            productDto.productBrand!
        );
        product.tags = productDto.tags?.map((l) => l.faTitle!);
        product.type.name = productDto.productAttributeType?.name ?? '';
        product.type.slug = productDto.productAttributeType?.slug ?? '';
        product.type.attributeGroups =
            this.mapMainProductAttributeGroups(
                productDto.productAttributeType?.attributeGroups ?? []
            );
        product.categories = this.mapProductCategories([
            productDto.productCategory ?? new ProductCategoryDto(),
        ]);
        product.attributes = this.mapMainProductAttributes(
            productDto.attributes ?? []
        );
        product.options = this.mapMainProductOptions(
            productDto.colorOption ??
            new MainProductOptionColorDto(),
            productDto.materialOption ??
            new MainProductOptionMaterialDto()
        );
        product.rating = productDto.rating;
        product.attributeOptions =
            this.mapMainProductAttributeOptions(
                productDto.attributeOptions ?? []
            );
        product.selectedAttributeOption =
            this.mapMainProductAttributeOption(
                productDto.selectedProductAttributeOption!
            );
        product.countryName = productDto.countryName;
        return product;
    }

    mapBestSellerProductsWithPagination(
        productDtos: BestSellerProductWithPaginationDto[]
    ) {
        let products: Product[] = [];
        productDtos?.forEach(async (l) => {
            let product = new Product();
            product = this.mapBestSellerProductWithPagination(l);
            products.push(product);
        });
        return products;
    }
    //#endregion

    //#region FeaturedProductWithPagination
    mapFeaturedProductWithPagination(
        productDto: FeaturedProductWithPaginationDto
    ) {
        let product = new Product();
        product.type = new ProductType();
        product.attributes = [new ProductAttribute()];
        product.id = productDto.id ?? 0;
        product.sevenId = productDto.related7SoftProductId ?? '';
        product.name = productDto.localizedName ?? '';
        product.excerpt = productDto.excerpt ?? '';
        product.description = productDto.description ?? '';
        product.slug = productDto.slug ?? '';
        product.sku = productDto.sku ?? productDto.productNo;
        product.partNumber = productDto.productNo ?? '';
        product.images = productDto.images?.map(
            (e) => this.imageService.makeImageUrl(e.image) ?? ''
        );
        product.reviews = productDto.reviewsLength;
        product.compatibility = this.mapProductCompatibility(
            productDto.kindIds ?? [],
            productDto.compatibility ?? 2
        );
        product.brand = this.mapMainBrand(
            productDto.productBrand!
        );
        product.tags = productDto.tags?.map((l) => l.faTitle!);
        product.type.name = productDto.productAttributeType?.name ?? '';
        product.type.slug = productDto.productAttributeType?.slug ?? '';
        product.type.attributeGroups =
            this.mapMainProductAttributeGroups(
                productDto.productAttributeType?.attributeGroups ?? []
            );
        product.categories = this.mapProductCategories([
            productDto.productCategory ?? new ProductCategoryDto(),
        ]);
        product.attributes = this.mapMainProductAttributes(
            productDto.attributes ?? []
        );
        product.options = this.mapMainProductOptions(
            productDto.colorOption ??
            new MainProductOptionColorDto(),
            productDto.materialOption ??
            new MainProductOptionMaterialDto()
        );
        product.rating = productDto.rating;
        product.attributeOptions =
            this.mapMainProductAttributeOptions(
                productDto.attributeOptions ?? []
            );
        product.selectedAttributeOption =
            this.mapMainProductAttributeOption(
                productDto.selectedProductAttributeOption!
            );
        product.countryName = productDto.countryName;
        return product;
    }

    mapFeaturedProductsWithPagination(
        productDtos: FeaturedProductWithPaginationDto[]
    ) {
        let products: Product[] = [];
        productDtos?.forEach(async (l) => {
            let product = new Product();
            product = this.mapFeaturedProductWithPagination(l);
            products.push(product);
        });
        return products;
    }
    //#endregion

    //#region LatestProductWithPagination
    mapLatestProductWithPagination(productDto: LatestProductWithPaginationDto) {
        let product = new Product();
        product.type = new ProductType();
        product.attributes = [new ProductAttribute()];
        product.id = productDto.id ?? 0;
        product.sevenId = productDto.related7SoftProductId ?? '';
        product.name = productDto.localizedName ?? '';
        product.excerpt = productDto.excerpt ?? '';
        product.description = productDto.description ?? '';
        product.slug = productDto.slug ?? '';
        product.sku = productDto.sku ?? productDto.productNo;
        product.partNumber = productDto.productNo ?? '';
        product.images = productDto.images?.map(
            (e) => this.imageService.makeImageUrl(e.image) ?? ''
        );
        product.reviews = productDto.reviewsLength;
        product.compatibility = this.mapProductCompatibility(
            productDto.kindIds ?? [],
            productDto.compatibility ?? 2
        );
        product.brand = this.mapMainBrand(
            productDto.productBrand!
        );
        product.tags = productDto.tags?.map((l) => l.faTitle!);
        product.type.name = productDto.productAttributeType?.name ?? '';
        product.type.slug = productDto.productAttributeType?.slug ?? '';
        product.type.attributeGroups =
            this.mapMainProductAttributeGroups(
                productDto.productAttributeType?.attributeGroups ?? []
            );
        product.categories = this.mapProductCategories([
            productDto.productCategory ?? new ProductCategoryDto(),
        ]);
        product.attributes = this.mapMainProductAttributes(
            productDto.attributes ?? []
        );
        product.options = this.mapMainProductOptions(
            productDto.colorOption ??
            new MainProductOptionColorDto(),
            productDto.materialOption ??
            new MainProductOptionMaterialDto()
        );
        product.rating = productDto.rating;
        product.attributeOptions =
            this.mapMainProductAttributeOptions(
                productDto.attributeOptions ?? []
            );
        product.selectedAttributeOption =
            this.mapMainProductAttributeOption(
                productDto.selectedProductAttributeOption!
            );
        product.countryName = productDto.countryName;
        return product;
    }

    mapLatestProductsWithPagination(
        productDtos: LatestProductWithPaginationDto[]
    ) {
        let products: Product[] = [];
        productDtos?.forEach(async (l) => {
            let product = new Product();
            product = this.mapLatestProductWithPagination(l);
            products.push(product);
        });
        return products;
    }
    //#endregion

    //#region NewProductWithPagination
    mapNewProductWithPagination(productDto: NewProductWithPaginationDto) {
        let product = new Product();
        product.type = new ProductType();
        product.attributes = [new ProductAttribute()];
        product.id = productDto.id ?? 0;
        product.sevenId = productDto.related7SoftProductId ?? '';
        product.name = productDto.localizedName ?? '';
        product.excerpt = productDto.excerpt ?? '';
        product.description = productDto.description ?? '';
        product.slug = productDto.slug ?? '';
        product.sku = productDto.sku ?? productDto.productNo;
        product.partNumber = productDto.productNo ?? '';
        product.images = productDto.images?.map(
            (e) => this.imageService.makeImageUrl(e.image) ?? ''
        );
        product.reviews = productDto.reviewsLength;
        product.compatibility = this.mapProductCompatibility(
            productDto.kindIds ?? [],
            productDto.compatibility ?? 2
        );
        product.brand = this.mapMainBrand(
            productDto.productBrand!
        );
        product.tags = productDto.tags?.map((l) => l.faTitle!);
        product.type.name = productDto.productAttributeType?.name ?? '';
        product.type.slug = productDto.productAttributeType?.slug ?? '';
        product.type.attributeGroups =
            this.mapMainProductAttributeGroups(
                productDto.productAttributeType?.attributeGroups ?? []
            );
        product.categories = this.mapProductCategories([
            productDto.productCategory ?? new ProductCategoryDto(),
        ]);
        product.attributes = this.mapMainProductAttributes(
            productDto.attributes ?? []
        );
        product.options = this.mapMainProductOptions(
            productDto.colorOption ??
            new MainProductOptionColorDto(),
            productDto.materialOption ??
            new MainProductOptionMaterialDto()
        );
        product.rating = productDto.rating;
        product.attributeOptions =
            this.mapMainProductAttributeOptions(
                productDto.attributeOptions ?? []
            );
        product.selectedAttributeOption =
            this.mapMainProductAttributeOption(
                productDto.selectedProductAttributeOption!
            );
        product.countryName = productDto.countryName;
        return product;
    }

    mapNewProductsWithPagination(productDtos: NewProductWithPaginationDto[]) {
        let products: Product[] = [];
        productDtos?.forEach(async (l) => {
            let product = new Product();
            product = this.mapNewProductWithPagination(l);
            products.push(product);
        });
        return products;
    }
    //#endregion

    //#region PopularProductWithPagination
    mapPopularProductWithPagination(
        productDto: PopularProductWithPaginationDto
    ) {
        let product = new Product();
        product.type = new ProductType();
        product.attributes = [new ProductAttribute()];
        product.id = productDto.id ?? 0;
        product.sevenId = productDto.related7SoftProductId ?? '';
        product.name = productDto.localizedName ?? '';
        product.excerpt = productDto.excerpt ?? '';
        product.description = productDto.description ?? '';
        product.slug = productDto.slug ?? '';
        product.sku = productDto.sku ?? productDto.productNo;
        product.partNumber = productDto.productNo ?? '';
        product.images = productDto.images?.map(
            (e) => this.imageService.makeImageUrl(e.image) ?? ''
        );
        product.reviews = productDto.reviewsLength;
        product.compatibility = this.mapProductCompatibility(
            productDto.kindIds ?? [],
            productDto.compatibility ?? 2
        );
        product.brand = this.mapMainBrand(
            productDto.productBrand!
        );
        product.tags = productDto.tags?.map((l) => l.faTitle!);
        product.type.name = productDto.productAttributeType?.name ?? '';
        product.type.slug = productDto.productAttributeType?.slug ?? '';
        product.type.attributeGroups =
            this.mapMainProductAttributeGroups(
                productDto.productAttributeType?.attributeGroups ?? []
            );
        product.categories = this.mapProductCategories([
            productDto.productCategory ?? new ProductCategoryDto(),
        ]);
        product.attributes = this.mapMainProductAttributes(
            productDto.attributes ?? []
        );
        product.options = this.mapMainProductOptions(
            productDto.colorOption ??
            new MainProductOptionColorDto(),
            productDto.materialOption ??
            new MainProductOptionMaterialDto()
        );
        product.rating = productDto.rating;
        product.attributeOptions =
            this.mapMainProductAttributeOptions(
                productDto.attributeOptions ?? []
            );
        product.selectedAttributeOption =
            this.mapMainProductAttributeOption(
                productDto.selectedProductAttributeOption!
            );
        product.countryName = productDto.countryName;
        return product;
    }

    mapPopularProductsWithPagination(
        productDtos: PopularProductWithPaginationDto[]
    ) {
        let products: Product[] = [];
        productDtos?.forEach(async (l) => {
            let product = new Product();
            product = this.mapPopularProductWithPagination(l);
            products.push(product);
        });
        return products;
    }
    //#endregion

    //#region ProductByBrandIdWithPagination
    mapProductByBrandIdWithPagination(
        productDto: ProductByBrandIdWithPaginationDto
    ) {
        let product = new Product();
        product.type = new ProductType();
        product.attributes = [new ProductAttribute()];
        product.id = productDto.id ?? 0;
        product.sevenId = productDto.related7SoftProductId ?? '';
        product.name = productDto.localizedName ?? '';
        product.excerpt = productDto.excerpt ?? '';
        product.description = productDto.description ?? '';
        product.slug = productDto.slug ?? '';
        product.sku = productDto.sku ?? productDto.productNo;
        product.partNumber = productDto.productNo ?? '';
        product.images = productDto.images?.map(
            (e) => this.imageService.makeImageUrl(e.image) ?? ''
        );
        product.reviews = productDto.reviewsLength;
        product.compatibility = this.mapProductCompatibility(
            productDto.kindIds ?? [],
            productDto.compatibility ?? 2
        );
        product.brand = this.mapMainBrand(
            productDto.productBrand!
        );
        product.tags = productDto.tags?.map((l) => l.faTitle!);
        product.type.name = productDto.productAttributeType?.name ?? '';
        product.type.slug = productDto.productAttributeType?.slug ?? '';
        product.type.attributeGroups =
            this.mapMainProductAttributeGroups(
                productDto.productAttributeType?.attributeGroups ?? []
            );
        product.categories = this.mapProductCategories([
            productDto.productCategory ?? new ProductCategoryDto(),
        ]);
        product.attributes = this.mapMainProductAttributes(
            productDto.attributes ?? []
        );
        product.options = this.mapMainProductOptions(
            productDto.colorOption ??
            new MainProductOptionColorDto(),
            productDto.materialOption ??
            new MainProductOptionMaterialDto()
        );
        product.rating = productDto.rating;
        product.attributeOptions =
            this.mapMainProductAttributeOptions(
                productDto.attributeOptions ?? []
            );
        product.selectedAttributeOption =
            this.mapMainProductAttributeOption(
                productDto.selectedProductAttributeOption!
            );
        product.countryName = productDto.countryName;
        return product;
    }

    mapProductsByBrandIdWithPagination(
        productDtos: ProductByBrandIdWithPaginationDto[]
    ) {
        let products: Product[] = [];
        productDtos?.forEach(async (l) => {
            let product = new Product();
            product = this.mapProductByBrandIdWithPagination(l);
            products.push(product);
        });
        return products;
    }
    //#endregion

    //#region ProductByKindIdWithPagination
    mapProductByKindIdWithPagination(
        productDto: ProductByKindIdWithPaginationDto
    ) {
        let product = new Product();
        product.type = new ProductType();
        product.attributes = [new ProductAttribute()];
        product.id = productDto.id ?? 0;
        product.sevenId = productDto.related7SoftProductId ?? '';
        product.name = productDto.localizedName ?? '';
        product.excerpt = productDto.excerpt ?? '';
        product.description = productDto.description ?? '';
        product.slug = productDto.slug ?? '';
        product.sku = productDto.sku ?? productDto.productNo;
        product.partNumber = productDto.productNo ?? '';
        product.images = productDto.images?.map(
            (e) => this.imageService.makeImageUrl(e.image) ?? ''
        );
        product.reviews = productDto.reviewsLength;
        product.compatibility = this.mapProductCompatibility(
            productDto.kindIds ?? [],
            productDto.compatibility ?? 2
        );
        product.brand = this.mapMainBrand(
            productDto.productBrand!
        );
        product.tags = productDto.tags?.map((l) => l.faTitle!);
        product.type.name = productDto.productAttributeType?.name ?? '';
        product.type.slug = productDto.productAttributeType?.slug ?? '';
        product.type.attributeGroups =
            this.mapMainProductAttributeGroups(
                productDto.productAttributeType?.attributeGroups ?? []
            );
        product.categories = this.mapProductCategories([
            productDto.productCategory ?? new ProductCategoryDto(),
        ]);
        product.attributes = this.mapMainProductAttributes(
            productDto.attributes ?? []
        );
        product.options = this.mapMainProductOptions(
            productDto.colorOption ??
            new MainProductOptionColorDto(),
            productDto.materialOption ??
            new MainProductOptionMaterialDto()
        );
        product.rating = productDto.rating;
        product.attributeOptions =
            this.mapMainProductAttributeOptions(
                productDto.attributeOptions ?? []
            );
        product.selectedAttributeOption =
            this.mapMainProductAttributeOption(
                productDto.selectedProductAttributeOption!
            );
        product.countryName = productDto.countryName;
        return product;
    }

    mapProductsByKindIdWithPagination(
        productDtos: ProductByKindIdWithPaginationDto[]
    ) {
        let products: Product[] = [];
        productDtos?.forEach(async (l) => {
            let product = new Product();
            product = this.mapProductByKindIdWithPagination(l);
            products.push(product);
        });
        return products;
    }
    //#endregion

    //#region ProductByProductCategoryIdWithPagination
    mapProductByProductCategoryIdWithPagination(
        productDto: ProductByProductCategoryIdWithPaginationDto
    ) {
        let product = new Product();
        product.type = new ProductType();
        product.attributes = [new ProductAttribute()];
        product.id = productDto.id ?? 0;
        product.sevenId = productDto.related7SoftProductId ?? '';
        product.name = productDto.localizedName ?? '';
        product.excerpt = productDto.excerpt ?? '';
        product.description = productDto.description ?? '';
        product.slug = productDto.slug ?? '';
        product.sku = productDto.sku ?? productDto.productNo;
        product.partNumber = productDto.productNo ?? '';
        product.images = productDto.images?.map(
            (e) => this.imageService.makeImageUrl(e.image) ?? ''
        );
        product.reviews = productDto.reviewsLength;
        product.compatibility = this.mapProductCompatibility(
            productDto.kindIds ?? [],
            productDto.compatibility ?? 2
        );
        product.brand = this.mapMainBrand(
            productDto.productBrand!
        );
        product.tags = productDto.tags?.map((l) => l.faTitle!);
        product.type.name = productDto.productAttributeType?.name ?? '';
        product.type.slug = productDto.productAttributeType?.slug ?? '';
        product.type.attributeGroups =
            this.mapMainProductAttributeGroups(
                productDto.productAttributeType?.attributeGroups ?? []
            );
        product.categories = this.mapProductCategories([
            productDto.productCategory ?? new ProductCategoryDto(),
        ]);
        product.attributes =
            this.mapMainProductAttributes(
                productDto.attributes ?? []
            );
        product.options =
            this.mapMainProductOptions(
                productDto.colorOption ??
                new MainProductOptionColorDto(),
                productDto.materialOption ??
                new MainProductOptionMaterialDto()
            );
        product.rating = productDto.rating;
        product.attributeOptions =
            this.mapMainProductAttributeOptions(
                productDto.attributeOptions ?? []
            );
        product.selectedAttributeOption =
            this.mapMainProductAttributeOption(
                productDto.selectedProductAttributeOption!
            );
        product.countryName = productDto.countryName;
        return product;
    }

    mapProductsByProductCategoryIdWithPagination(
        productDtos: ProductByProductCategoryIdWithPaginationDto[]
    ) {
        let products: Product[] = [];
        productDtos?.forEach(async (l) => {
            let product = new Product();
            product = this.mapProductByProductCategoryIdWithPagination(l);
            products.push(product);
        });
        return products;
    }
    //#endregion

    //#region ProductWithPagination
    mapProductWithPagination(productDto: ProductWithPaginationDto) {
        let product = new Product();
        product.type = new ProductType();
        product.attributes = [new ProductAttribute()];
        product.id = productDto.id ?? 0;
        product.sevenId = productDto.related7SoftProductId ?? '';
        product.name = productDto.localizedName ?? '';
        product.excerpt = productDto.excerpt ?? '';
        product.description = productDto.description ?? '';
        product.slug = productDto.slug ?? '';
        product.sku = productDto.sku ?? productDto.productNo;
        product.partNumber = productDto.productNo ?? '';

        product.images = productDto.images?.map(
            (e) => this.imageService.makeImageUrl(e.image) ?? ''
        );

        product.reviews = productDto.reviewsLength;
        product.compatibility = this.mapProductCompatibility(
            productDto.kindIds ?? [],
            productDto.compatibility ?? 2
        );
        product.brand = this.mapMainBrand(productDto.productBrand!);
        product.tags = productDto.tags?.map((l) => l.faTitle!);
        product.type.name = productDto.productAttributeType?.name ?? '';
        product.type.slug = productDto.productAttributeType?.slug ?? '';
        product.type.attributeGroups =
            this.mapMainProductAttributeGroups(
                productDto.productAttributeType?.attributeGroups ?? []
            );
        product.categories = this.mapProductCategories([
            productDto.productCategory ?? new ProductCategoryDto(),
        ]);
        product.attributes = this.mapMainProductAttributes(
            productDto.attributes ?? []
        );
        product.options = this.mapMainProductOptions(
            productDto.colorOption ?? new MainProductOptionColorDto(),
            productDto.materialOption ??
            new MainProductOptionMaterialDto()
        );
        product.rating = productDto.rating;
        product.attributeOptions =
            this.mapMainProductAttributeOptions(
                productDto.attributeOptions ?? []
            );
        product.selectedAttributeOption =
            this.mapMainProductAttributeOption(
                productDto.selectedProductAttributeOption!
            );
        product.countryName = productDto.countryName;
        return product;
    }

    mapProductsWithPagination(productDtos: ProductWithPaginationDto[]) {
        let products: Product[] = [];
        productDtos?.forEach(async (l) => {
            let product = new Product();
            product = this.mapProductWithPagination(l);
            products.push(product);
        });
        return products;
    }
    //#endregion

    //#region RelatedProductWithPagination
    mapRelatedProductWithPagination(
        productDto: RelatedProductWithPaginationDto
    ) {
        let product = new Product();
        product.type = new ProductType();
        product.attributes = [new ProductAttribute()];
        product.id = productDto.id ?? 0;
        product.sevenId = productDto.related7SoftProductId ?? '';
        product.name = productDto.localizedName ?? '';
        product.excerpt = productDto.excerpt ?? '';
        product.description = productDto.description ?? '';
        product.slug = productDto.slug ?? '';
        product.sku = productDto.sku ?? productDto.productNo;
        product.partNumber = productDto.productNo ?? '';
        product.images = productDto.images?.map(
            (e) => this.imageService.makeImageUrl(e.image) ?? ''
        );
        product.reviews = productDto.reviewsLength;
        product.compatibility = this.mapProductCompatibility(
            productDto.kindIds ?? [],
            productDto.compatibility ?? 2
        );
        product.brand = this.mapMainBrand(
            productDto.productBrand!
        );
        product.tags = productDto.tags?.map((l) => l.faTitle!);
        product.type.name = productDto.productAttributeType?.name ?? '';
        product.type.slug = productDto.productAttributeType?.slug ?? '';
        product.type.attributeGroups =
            this.mapMainProductAttributeGroups(
                productDto.productAttributeType?.attributeGroups ?? []
            );
        product.categories = this.mapProductCategories([
            productDto.productCategory ?? new ProductCategoryDto(),
        ]);
        product.attributes = this.mapMainProductAttributes(
            productDto.attributes ?? []
        );
        product.options = this.mapMainProductOptions(
            productDto.colorOption ??
            new MainProductOptionColorDto(),
            productDto.materialOption ??
            new MainProductOptionMaterialDto()
        );
        product.rating = productDto.rating;
        product.attributeOptions =
            this.mapMainProductAttributeOptions(
                productDto.attributeOptions ?? []
            );
        product.selectedAttributeOption =
            this.mapMainProductAttributeOption(
                productDto.selectedProductAttributeOption!
            );
        product.countryName = productDto.countryName;
        return product;
    }

    mapRelatedProductsWithPagination(
        productDtos: RelatedProductWithPaginationDto[]
    ) {
        let products: Product[] = [];
        productDtos?.forEach(async (l) => {
            let product = new Product();
            product = this.mapRelatedProductWithPagination(l);
            products.push(product);
        });
        return products;
    }
    //#endregion

    //#region SalesProductWithPagination
    mapSalesProductWithPagination(productDto: SalesProductWithPaginationDto) {
        let product = new Product();
        product.type = new ProductType();
        product.attributes = [new ProductAttribute()];
        product.id = productDto.id ?? 0;
        product.sevenId = productDto.related7SoftProductId ?? '';
        product.name = productDto.localizedName ?? '';
        product.excerpt = productDto.excerpt ?? '';
        product.description = productDto.description ?? '';
        product.slug = productDto.slug ?? '';
        product.sku = productDto.sku ?? productDto.productNo;
        product.partNumber = productDto.productNo ?? '';

        product.images = productDto.images?.map(
            (e) => this.imageService.makeImageUrl(e.image) ?? ''
        );
        product.reviews = productDto.reviewsLength;
        product.compatibility = this.mapProductCompatibility(
            productDto.kindIds ?? [],
            productDto.compatibility ?? 2
        );
        product.brand = this.mapMainBrand(
            productDto.productBrand!
        );
        product.tags = productDto.tags?.map((l) => l.faTitle!);
        product.type.name = productDto.productAttributeType?.name ?? '';
        product.type.slug = productDto.productAttributeType?.slug ?? '';
        product.type.attributeGroups =
            this.mapMainProductAttributeGroups(
                productDto.productAttributeType?.attributeGroups ?? []
            );
        product.categories = this.mapProductCategories([
            productDto.productCategory ?? new ProductCategoryDto(),
        ]);
        product.attributes = this.mapMainProductAttributes(
            productDto.attributes ?? []
        );
        product.options = this.mapMainProductOptions(
            productDto.colorOption ??
            new MainProductOptionColorDto(),
            productDto.materialOption ??
            new MainProductOptionMaterialDto()
        );
        product.rating = productDto.rating;
        product.attributeOptions =
            this.mapMainProductAttributeOptions(
                productDto.attributeOptions ?? []
            );
        product.selectedAttributeOption =
            this.mapMainProductAttributeOption(
                productDto.selectedProductAttributeOption!
            );
        product.countryName = productDto.countryName;
        return product;
    }

    mapSalesProductsWithPagination(
        productDtos: SalesProductWithPaginationDto[]
    ) {
        let products: Product[] = [];
        productDtos?.forEach(async (l) => {
            let product = new Product();
            product = this.mapSalesProductWithPagination(l);
            products.push(product);
        });
        return products;
    }
    //#endregion

    //#region SpecialOffersProductWithPagination
    mapSpecialOffersProductWithPagination(
        productDto: SpecialOffersProductWithPaginationDto
    ) {
        let product = new Product();
        product.type = new ProductType();
        product.attributes = [new ProductAttribute()];
        product.id = productDto.id ?? 0;
        product.sevenId = productDto.related7SoftProductId ?? '';
        product.name = productDto.localizedName ?? '';
        product.excerpt = productDto.excerpt ?? '';
        product.description = productDto.description ?? '';
        product.slug = productDto.slug ?? '';
        product.sku = productDto.sku ?? productDto.productNo;
        product.partNumber = productDto.productNo ?? '';
        product.images = productDto.images?.map(
            (e) => this.imageService.makeImageUrl(e.image) ?? ''
        );
        product.reviews = productDto.reviewsLength;
        product.compatibility = this.mapProductCompatibility(
            productDto.kindIds ?? [],
            productDto.compatibility ?? 2
        );
        product.brand = this.mapMainBrand(
            productDto.productBrand!
        );
        product.tags = productDto.tags?.map((l) => l.faTitle!);
        product.type.name = productDto.productAttributeType?.name ?? '';
        product.type.slug = productDto.productAttributeType?.slug ?? '';
        product.type.attributeGroups =
            this.mapMainProductAttributeGroups(
                productDto.productAttributeType?.attributeGroups ?? []
            );
        product.categories = this.mapProductCategories([
            productDto.productCategory ?? new ProductCategoryDto(),
        ]);
        product.attributes =
            this.mapMainProductAttributes(
                productDto.attributes ?? []
            );
        product.options = this.mapMainProductOptions(
            productDto.colorOption ??
            new MainProductOptionColorDto(),
            productDto.materialOption ??
            new MainProductOptionMaterialDto()
        );
        product.rating = productDto.rating;
        product.attributeOptions =
            this.mapMainProductAttributeOptions(
                productDto.attributeOptions ?? []
            );
        product.selectedAttributeOption =
            this.mapMainProductAttributeOption(
                productDto.selectedProductAttributeOption!
            );
        product.countryName = productDto.countryName;
        return product;
    }

    mapSpecialOffersProductsWithPagination(
        productDtos: SpecialOffersProductWithPaginationDto[]
    ) {
        let products: Product[] = [];
        productDtos?.forEach(async (l) => {
            let product = new Product();
            product = this.mapSpecialOffersProductWithPagination(l);
            products.push(product);
        });
        return products;
    }
    //#endregion

    //#region TopRatedProductWithPagination
    mapTopRatedProductWithPagination(
        productDto: TopRatedProductWithPaginationDto
    ) {
        let product = new Product();
        product.type = new ProductType();
        product.attributes = [new ProductAttribute()];
        product.id = productDto.id ?? 0;
        product.sevenId = productDto.related7SoftProductId ?? '';
        product.name = productDto.localizedName ?? '';
        product.excerpt = productDto.excerpt ?? '';
        product.description = productDto.description ?? '';
        product.slug = productDto.slug ?? '';
        product.sku = productDto.sku ?? productDto.productNo;
        product.partNumber = productDto.productNo ?? '';
        product.images = productDto.images?.map(
            (e) => this.imageService.makeImageUrl(e.image) ?? ''
        );
        product.reviews = productDto.reviewsLength;
        product.compatibility = this.mapProductCompatibility(
            productDto.kindIds ?? [],
            productDto.compatibility ?? 2
        );
        product.brand = this.mapMainBrand(
            productDto.productBrand!
        );
        product.tags = productDto.tags?.map((l) => l.faTitle!);
        product.type.name = productDto.productAttributeType?.name ?? '';
        product.type.slug = productDto.productAttributeType?.slug ?? '';
        product.type.attributeGroups =
            this.mapMainProductAttributeGroups(
                productDto.productAttributeType?.attributeGroups ?? []
            );
        product.categories = this.mapProductCategories([
            productDto.productCategory ?? new ProductCategoryDto(),
        ]);
        product.attributes = this.mapMainProductAttributes(
            productDto.attributes ?? []
        );
        product.options = this.mapMainProductOptions(
            productDto.colorOption ??
            new MainProductOptionColorDto(),
            productDto.materialOption ??
            new MainProductOptionMaterialDto()
        );
        product.rating = productDto.rating;
        product.attributeOptions =
            this.mapMainProductAttributeOptions(
                productDto.attributeOptions ?? []
            );
        product.selectedAttributeOption =
            this.mapMainProductAttributeOption(
                productDto.selectedProductAttributeOption!
            );
        product.countryName = productDto.countryName;
        return product;
    }

    mapTopRatedProductsWithPagination(
        productDtos: TopRatedProductWithPaginationDto[]
    ) {
        let products: Product[] = [];
        productDtos?.forEach(async (l) => {
            let product = new Product();
            product = this.mapTopRatedProductWithPagination(l);
            products.push(product);
        });
        return products;
    }
    //#endregion

    //#region ProductForFilterResult
    mapProductForFilterResult(productDto: ProductForFilterResultDto) {
        let product = new Product();
        product.type = new ProductType();
        product.attributes = [new ProductAttribute()];
        product.id = productDto.id ?? 0;
        product.sevenId = productDto.related7SoftProductId ?? '';
        product.name = productDto.localizedName ?? '';
        product.excerpt = productDto.excerpt ?? '';
        product.description = productDto.description ?? '';
        product.slug = productDto.slug ?? '';
        product.sku = productDto.sku ?? productDto.productNo;
        product.partNumber = productDto.productNo ?? '';
        product.images = productDto.images?.map(
            (e) => this.imageService.makeImageUrl(e.image) ?? ''
        );
        product.reviews = productDto.reviewsLength;
        product.compatibility = this.mapProductCompatibility(
            productDto.kindIds ?? [],
            productDto.compatibility ?? 2
        );
        product.brand = this.mapMainBrand(productDto.productBrand!);
        product.tags = productDto.tags?.map((l) => l.faTitle!);
        product.type.name = productDto.productAttributeType?.name ?? '';
        product.type.slug = productDto.productAttributeType?.slug ?? '';
        product.type.attributeGroups =
            this.mapMainProductAttributeGroups(
                productDto.productAttributeType?.attributeGroups ?? []
            );
        product.categories = this.mapProductCategories([
            productDto.productCategory ?? new ProductCategoryDto(),
        ]);
        product.attributes = this.mapMainProductAttributes(
            productDto.attributes ?? []
        );
        product.options = this.mapMainProductOptions(
            productDto.colorOption ??
            new MainProductOptionColorDto(),
            productDto.materialOption ??
            new MainProductOptionMaterialDto()
        );
        product.rating = productDto.rating;
        product.attributeOptions =
            this.mapMainProductAttributeOptions(
                productDto.attributeOptions ?? []
            );
        product.selectedAttributeOption =
            this.mapMainProductAttributeOption(
                productDto.selectedProductAttributeOption!
            );
        product.countryName = productDto.countryName;
        return product;
    }

    mapProductsForFilterResult(productDtos: ProductForFilterResultDto[]) {
        let products: Product[] = [];
        productDtos?.forEach(async (l) => {
            let product = new Product();
            product = this.mapProductForFilterResult(l);
            products.push(product);
        });
        return products;
    }

    //#endregion

    //#region ProductForSearchSuggestion
    mapProductForSearchSuggestion(productDto: ProductForSearchSuggestionDto) {
        let product = new Product();
        product.type = new ProductType();
        product.attributes = [new ProductAttribute()];
        product.id = productDto.id ?? 0;
        product.name = productDto.localizedName ?? '';
        product.slug = productDto.slug ?? '';
        product.images = productDto.images?.map(
            (e) => this.imageService.makeImageUrl(e.image) ?? ''
        );
        product.reviews = productDto.reviewsLength;
        product.compatibility = this.mapProductCompatibility(
            productDto.kindIds ?? [],
            productDto.compatibility ?? 2
        );
        product.rating = productDto.rating;
        product.attributeOptions =
            this.mapProductAttributeOptionsForSearchSuggestion(
                productDto.attributeOptions ?? []
            );
        product.selectedAttributeOption =
            this.mapProductAttributeOptionForSearchSuggestion(
                productDto.selectedProductAttributeOption!
            );
        return product;
    }

    mapProductsForSearchSuggestion(
        productDtos: ProductForSearchSuggestionDto[]
    ) {
        let products: Product[] = [];
        productDtos?.forEach(async (l) => {
            let product = new Product();
            product = this.mapProductForSearchSuggestion(l);
            products.push(product);
        });
        return products;
    }

    //#region ProductAttributeOption
    mapProductAttributeOptionForSearchSuggestion(
        productAttributeOptionDto: ProductAttributeOptionForSearchSuggestionDto
    ) {
        let child: ProductAttributeOption = {
            id: 0,
            availability: 'out-of-stock',
            isDefault: false,
            optionValues: [],
            price: 0,
            maxOrderQuantityPerOrder: 1,
            minOrderQuantityPerOrder: 1,
            badges: [],
            compareAtPrice: 0,
        };
        let price = productAttributeOptionDto?.prices
            ? productAttributeOptionDto.prices.sort(
                  (a, b) => b.date!.getTime() - a.date!.getTime()
              )[0]
            : null;
        let selectedRole =
            productAttributeOptionDto?.productAttributeOptionRoles
                ? productAttributeOptionDto.productAttributeOptionRoles[0]
                : null;
        child.id = productAttributeOptionDto?.id ?? 0;
        child.availability =
            selectedRole?.availability == AvailabilityEnum.InStock
                ? 'in-stock'
                : 'out-of-stock';
        child.isDefault = productAttributeOptionDto?.isDefault ?? false;
        child.price =
            selectedRole?.discountPercent! > 0
                ? price?.mainPrice! *
                  ((100 - selectedRole?.discountPercent!) / 100)
                : price?.mainPrice!;
        child.compareAtPrice =
            selectedRole?.discountPercent! > 0 ? price?.mainPrice : undefined;
        child.discountPercent = selectedRole?.discountPercent ?? 0;
        child.maxOrderQuantityPerOrder = selectedRole?.currentMaxOrderQty ?? 0;
        child.minOrderQuantityPerOrder = selectedRole?.currentMinOrderQty ?? 0;
        child.badges =
            productAttributeOptionDto?.badges?.map((l) => l.value ?? '') ?? [];
        child.optionValues =
            this.mapProductAttributeOptionValuesForSearchSuggestion(
                productAttributeOptionDto?.optionValues ?? []
            );
        child.price = this.AddTax(child.price)!;
        child.compareAtPrice = this.AddTax(child.compareAtPrice);
        return child;
    }
    mapProductAttributeOptionsForSearchSuggestion(
        productAttributeOptionDtos: ProductAttributeOptionForSearchSuggestionDto[]
    ) {
        let res: ProductAttributeOption[] = [];
        productAttributeOptionDtos.forEach((l) => {
            res.push(this.mapProductAttributeOptionForSearchSuggestion(l));
        });
        return res;
    }

    async findSelectedProductAttributeOptionForSearchSuggestion(
        productAttributeOptionDtos: ProductAttributeOptionForSearchSuggestionDto[]
    ) {
        let selectedOption!: ProductAttributeOptionForSearchSuggestionDto;

        for (const option of productAttributeOptionDtos) {
            const availability = option.productAttributeOptionRoles?.find(
                (c) => c.customerTypeEnum == this.currentCustomerTypeEnum
            )?.availability;

            if (option.isDefault && availability === AvailabilityEnum.InStock) {
                selectedOption = option;
                break;
            }
        }

        if (!selectedOption) {
            for (const option of productAttributeOptionDtos) {
                const availability = option.productAttributeOptionRoles?.find(
                    (c) => c.customerTypeEnum == this.currentCustomerTypeEnum
                )?.availability;
                if (availability === AvailabilityEnum.InStock) {
                    selectedOption = option;
                    break;
                }
            }
        }

        if (!selectedOption && productAttributeOptionDtos.length > 0) {
            selectedOption = productAttributeOptionDtos[0];
        }

        return this.mapProductAttributeOptionForSearchSuggestion(
            selectedOption
        );
    }
    mapProductAttributeOptionValuesForSearchSuggestion(
        productAttributeOptionValueDtos: ProductAttributeOptionValueForSearchSuggestionDto[]
    ) {
        let res: ProductAttributeOptionValue[] = [];
        productAttributeOptionValueDtos.forEach((l) => {
            let child: ProductAttributeOptionValue = {
                id: 0,
                name: '',
                value: '',
            };
            child.id = l.id ?? 0;
            child.name = l.name ?? '';
            child.value = l.value ?? '';
            res.push(child);
        });
        return res;
    }
    //#endregion
    //#endregion
    
    //#region Shared

    //#region Brand
    mapMainBrand(brandDto: MainBrandDto) {
        let brand = new Brand();
        brand.name = brandDto.localizedName ?? '';
        brand.slug = brandDto.slug ?? '';
        brand.country = brandDto.countryName ?? '';
        brand.image = brandDto.brandLogo ?? '';
        return brand;
    }
    //#endregion

    //#region ProductTypeAttributeGroup
    mapMainProductAttributeGroups(
        productAttributeGroups: MainProductTypeAttributeGroupDto[]
    ) {
        let res: ProductTypeAttributeGroup[] = [];
        productAttributeGroups.forEach((l) => {
            let child: ProductTypeAttributeGroup = {
                name: '',
                slug: '',
                attributes: [],
            };
            child.name = l.name ?? '';
            child.slug = l.slug ?? '';
            child.attributes = l.attributes?.map((x) => x.value ?? '') ?? [];
            res.push(child);
        });
        return res;
    }
    //#endregion

    //#region ProductOption
    mapMainProductOptions(
        productOptionColorDto: MainProductOptionColorDto,
        productOptionMaterialDto: MainProductOptionMaterialDto
    ) {
        let res: ProductOption[] = [];

        let child1 = new ProductOptionColor();
        child1.name = productOptionColorDto.name ?? '';
        child1.slug = productOptionColorDto.slug ?? '';
        child1.type = 'color';
        child1.values = this.mapMainProductOptionValueColors(
            productOptionColorDto.values ?? []
        );
        res.push(child1);

        let child2 = new ProductOptionMaterial();
        child2.name = productOptionMaterialDto.name ?? '';
        child2.slug = productOptionMaterialDto.slug ?? '';
        child2.type = 'material';
        child2.values = this.mapMainProductOptionValueMaterials(
            productOptionMaterialDto.values ?? []
        );
        res.push(child2);

        return res;
    }
    mapMainProductOptionValueColors(
        productOptionValueColorDtos: MainProductOptionValueColorDto[]
    ) {
        let res: ProductOptionValueColor[] = [];
        productOptionValueColorDtos.forEach((l) => {
            let child = new ProductOptionValueColor();
            child.name = l.name ?? '';
            child.slug = l.slug ?? '';
            child.color = l.color ?? '';
            res.push(child);
        });
        return res;
    }
    mapMainProductOptionValueMaterials(
        productOptionValueMaterialDtos: MainProductOptionValueMaterialDto[]
    ) {
        let res: ProductOptionValueBase[] = [];
        productOptionValueMaterialDtos.forEach((l) => {
            let child = new ProductOptionValueBase();
            child.name = l.name ?? '';
            child.slug = l.slug ?? '';
            res.push(child);
        });
        return res;
    }
    //#endregion

    //#region ProductAttribute
    mapMainProductAttributes(productAttributeDtos: MainProductAttributeDto[]) {
        let res: ProductAttribute[] = [];
        productAttributeDtos.forEach((l) => {
            let child: ProductAttribute = {
                name: '',
                slug: '',
                featured: false,
                values: [],
                customFields: [],
            };
            child.name = l.name ?? '';
            child.slug = l.slug ?? '';
            child.featured = l.featured ?? false;
            child.values = this.mapProductAttributeValue(
                l.valueSlug!,
                l.valueName!
            );
            res.push(child);
        });
        return res;
    }

    //#endregion

    //#region ProductAttributeOption
    mapMainProductAttributeOption(
        productAttributeOptionDto: MainProductAttributeOptionDto
    ) {
        let child: ProductAttributeOption = {
            id: 0,
            availability: 'out-of-stock',
            isDefault: false,
            optionValues: [],
            price: 0,
            maxOrderQuantityPerOrder: 1,
            minOrderQuantityPerOrder: 1,
            badges: [],
            compareAtPrice: 0,
        };
        let price = productAttributeOptionDto?.prices
            ? productAttributeOptionDto.prices.sort(
                (a, b) => b.date!.getTime() - a.date!.getTime()
            )[0]
            : null;
        let selectedRole =
            productAttributeOptionDto?.productAttributeOptionRoles
                ? productAttributeOptionDto.productAttributeOptionRoles[0]
                : null;
        child.id = productAttributeOptionDto?.id ?? 0;
        child.availability =
            selectedRole?.availability == AvailabilityEnum.InStock
                ? 'in-stock'
                : 'out-of-stock';
        child.isDefault = productAttributeOptionDto?.isDefault ?? false;
        child.price =
            selectedRole?.discountPercent! > 0
                ? price?.mainPrice! *
                ((100 - selectedRole?.discountPercent!) / 100)
                : price?.mainPrice!;
        child.compareAtPrice =
            selectedRole?.discountPercent! > 0 ? price?.mainPrice : undefined;
        child.discountPercent = selectedRole?.discountPercent ?? 0;
        child.maxOrderQuantityPerOrder = selectedRole?.currentMaxOrderQty ?? 0;
        child.minOrderQuantityPerOrder = selectedRole?.currentMinOrderQty ?? 0;
        child.badges =
            productAttributeOptionDto?.badges?.map((l) => l.value ?? '') ?? [];
        child.optionValues = this.mapMainProductAttributeOptionValues(
            productAttributeOptionDto?.optionValues ?? []
        );
        child.price = this.AddTax(child.price)!;
        child.compareAtPrice = this.AddTax(child.compareAtPrice);
        child.price = this.ConvertToTooman(child.price)!;
        child.compareAtPrice = this.ConvertToTooman(child.compareAtPrice);
        return child;
    }
    mapMainProductAttributeOptions(
        productAttributeOptionDtos: MainProductAttributeOptionDto[]
    ) {
        let res: ProductAttributeOption[] = [];
        productAttributeOptionDtos.forEach((l) => {
            res.push(this.mapMainProductAttributeOption(l));
        });
        return res;
    }

    async findSelectedMainProductAttributeOption(
        productAttributeOptionDtos: MainProductAttributeOptionDto[]
    ) {
        let selectedOption!: MainProductAttributeOptionDto;

        for (const option of productAttributeOptionDtos) {
            const availability = option.productAttributeOptionRoles?.find(
                (c) => c.customerTypeEnum == this.currentCustomerTypeEnum
            )?.availability;

            if (option.isDefault && availability === AvailabilityEnum.InStock) {
                selectedOption = option;
                break;
            }
        }

        if (!selectedOption) {
            for (const option of productAttributeOptionDtos) {
                const availability = option.productAttributeOptionRoles?.find(
                    (c) => c.customerTypeEnum == this.currentCustomerTypeEnum
                )?.availability;
                if (availability === AvailabilityEnum.InStock) {
                    selectedOption = option;
                    break;
                }
            }
        }

        if (!selectedOption && productAttributeOptionDtos.length > 0) {
            selectedOption = productAttributeOptionDtos[0];
        }

        return this.mapMainProductAttributeOption(selectedOption);
    }
    mapMainProductAttributeOptionValues(
        productAttributeOptionValueDtos: MainProductAttributeOptionValueDto[]
    ) {
        let res: ProductAttributeOptionValue[] = [];
        productAttributeOptionValueDtos.forEach((l) => {
            let child: ProductAttributeOptionValue = {
                id: 0,
                name: '',
                value: '',
            };
            child.id = l.id ?? 0;
            child.name = l.name ?? '';
            child.value = l.value ?? '';
            res.push(child);
        });
        return res;
    }
    //#endregion

    //#region ProductCompatibility
    mapProductCompatibility(
        kindIds: number[],
        compatibilityEnum: CompatibilityEnum
    ) {
        //
        switch (compatibilityEnum) {
            case 0: {
                return 'all';
            }
            case 1: {
                return 'unknown';
            }
            case 2: {
                return kindIds;
            }
            default: {
                return 'unknown';
            }
        }
    }
    //#endregion 

    //#region ProductOptionType
    mapProductOptionTypeEnum(productOptionTypeEnum: ProductOptionTypeEnum) {
        switch (productOptionTypeEnum) {
            case 1: {
                return 'color';
            }
            case 2: {
                return 'material';
            }
            default: {
                return 'color';
            }
        }
    }
    //#endregion

    //#region ProductCategory
    mapProductCategory(
        productCategory: ProductCategoryDto,
        layout?: ShopCategoryLayout
    ): ShopCategory {
        let shopCategory = new ShopCategory();
        shopCategory.id = productCategory?.id ?? 0;
        shopCategory.name = productCategory?.localizedName ?? '';
        shopCategory.slug = productCategory?.slug ?? '';
        shopCategory.type = 'shop';
        shopCategory.layout = layout ?? 'products';
        return shopCategory;
    }
    mapProductCategories(
        productCategoryDtos: ProductCategoryDto[],
        layout?: ShopCategoryLayout
    ) {
        let categories: ShopCategory[] = [];
        productCategoryDtos.forEach((l) => {
            let category = new ShopCategory();
            category = this.mapProductCategory(l, layout);
            categories.push(category);
        });
        return categories;
    }
    //#endregion

    //#region ProductAttributeValue
    mapProductAttributeValue(slug: string, name: string) {
        let res: ProductAttributeValue[] = [];
        let child: ProductAttributeValue = {
            name: name,
            slug: slug,
            customFields: [],
        };
        res.push(child);
        return res;
    }
    //#endregion

    //#endregion

    //#region Shared
    AddTax(value: number | undefined) {
        if (value == undefined) {
            return undefined;
        }
        return value * 1.1;
    }
    ConvertToTooman(value: number | undefined) {
        if (value == undefined) {
            return undefined;
        }
        return value * 0.1;
    }
    //#endregion
}
