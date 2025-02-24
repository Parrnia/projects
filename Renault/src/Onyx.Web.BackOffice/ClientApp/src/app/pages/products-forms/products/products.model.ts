import { CompatibilityEnum } from "src/app/web-api-client";

export interface ProductModel {
    id: number;
    state: number;
    localizedName: string;
    name: string;
    code: string;
    productNo: string;
    oldProductNo?: string;
    productCatalog?: string;
    orderRate: number;
    mileage?: number;
    duration?: number;
    excerpt: string;
    description: string;
    slug: string;
    sku: string;

    providerId?: number;
    providerName: string;
    countryId?: number;
    countryName: string;
    productTypeId?: number;
    productTypeName:string,
    productStatusId?: number;
    productStatusName:string;
    mainCountingUnitId?: number;
    mainCountingUnitName:string;
    commonCountingUnitId?: number;
    commonCountingUnitName:string;
    brandId: number;
    brandName:string;
    productCategoryId: number;
    productCategoryName:string;
    productAttributeTypeId?: number;
    productAttributeTypeName:string;
    colorOptionId?: number;
    colorOptionName:string;
    materialOptionId?: number;
    materialOptionName:string;

    compatibility : number
    compatibilityName: string;
    isActive : boolean;
}