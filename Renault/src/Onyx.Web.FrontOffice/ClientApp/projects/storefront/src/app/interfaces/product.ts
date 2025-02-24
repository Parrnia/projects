import { Order } from './order';
import { CustomFields } from './custom-fields';
import { Brand } from './brand';
import { ShopCategory } from './category';
import { AvailabilityEnum } from '../web-api-client';

export class BaseAttributeGroup {
    name!: string;
    slug!: string;
    customFields?: CustomFields;
}

export type ProductAttributeGroup = BaseAttributeGroup & {
    attributes: ProductAttribute[];
};
export type ProductTypeAttributeGroup = BaseAttributeGroup & {
    attributes: string[];
};

export class ProductType {
    name!: string;
    slug!: string;
    attributeGroups!: ProductTypeAttributeGroup[];
    customFields?: CustomFields;
}

export class ProductAttributeValue {
    name!: string;
    slug!: string;
    customFields?: CustomFields;
}

export class ProductAttribute {
    name!: string;
    slug!: string;
    featured!: boolean;
    values!: ProductAttributeValue[];
    customFields?: CustomFields;
}

export class ProductOptionValueBase {
    name!: string;
    slug!: string;
    customFields?: CustomFields;
}

export class ProductOptionValueColor extends ProductOptionValueBase {
    color!: string;
}

export class ProductOptionBase {
    type!: string;
    name!: string;
    slug!: string;
    values!: ProductOptionValueBase[];
    customFields?: CustomFields;
}

export class ProductOptionDefault extends ProductOptionBase {
    override type!: 'default';
}
export class ProductOptionMaterial extends ProductOptionBase {
    override type!: 'material';
}

export class ProductOptionColor extends ProductOptionBase {
    override type!: 'color';
    override values!: ProductOptionValueColor[];
}

export type ProductOption =
    | ProductOptionDefault
    | ProductOptionColor
    | ProductOptionMaterial;

export class ProductAttributeOptionValue {
    id!: number;
    name!: string;
    value!: string;
}

export class ProductAttributeOption {
    id!: number;
    availability!: ProductStock;
    isDefault!: boolean;
    price!: number;
    compareAtPrice?: number;
    discountPercent?: number;
    maxOrderQuantityPerOrder!: number;
    minOrderQuantityPerOrder!: number;
    optionValues!: ProductAttributeOptionValue[];
    badges!: string[];
}

export type ProductStock = 'in-stock' | 'out-of-stock' | 'on-backorder';

export type ProductCompatibilityResult = 'all' | 'fit' | 'not-fit' | 'unknown';

export class Product {
    id!: number;
    sevenId!: string;
    name!: string;
    /**
     * A short product description without HTML tags.
     */
    excerpt!: string;
    description!: string;
    slug!: string;
    sku?: string;
    partNumber!: string;
    stock!: ProductStock;
    price!: number;
    compareAtPrice?: number | null;
    images?: string[];
    badges?: string[];
    rating?: number;
    reviews?: number;
    availability?: string;
    countryName?: string;
    /**
     * 'all'     - Compatible with all vehicles.
     * 'unknown' - No compatibility information. Part may not fit the specified vehicle.
     * number[]  - An array of vehicle identifiers with which this part is compatible.
     */
    compatibility!: 'all' | 'unknown' | number[];
    brand?: Brand | null;
    tags?: string[];
    type!: ProductType;
    categories?: ShopCategory[];
    attributes!: ProductAttribute[];
    options!: ProductOption[];
    attributeOptions!: ProductAttributeOption[];
    selectedAttributeOption!: ProductAttributeOption;
    displayNames?: string[];
    customFields?: CustomFields;
}
