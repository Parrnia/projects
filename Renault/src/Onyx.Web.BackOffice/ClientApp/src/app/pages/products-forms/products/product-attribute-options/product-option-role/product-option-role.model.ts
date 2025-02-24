export interface ProductOptionRoleModel {
    id: number;
    minimumStockToDisplayProductForThisCustomerTypeEnum:number;
    availability:number;
    availabilityName:string;
    mainMaxOrderQty:number;
    currentMaxOrderQty:number;
    mainMinOrderQty:number;
    currentMinOrderQty:number;
    customerTypeEnumName:string;
    discountPercent:number;
    state: number;
  }