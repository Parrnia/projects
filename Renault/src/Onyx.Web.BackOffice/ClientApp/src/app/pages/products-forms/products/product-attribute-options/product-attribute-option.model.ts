export class ProductAttributeOptionModel {
    id!: number;
    totalCount!: number;
    safetyStockQty!: number;
    minStockQty!: number;
    maxStockQty!: number;
    maxSalePriceNonCompanyProductPercent?:number;
    isDefault!: boolean;
    optionValues!: string;
    state!: number;
  }
  
  