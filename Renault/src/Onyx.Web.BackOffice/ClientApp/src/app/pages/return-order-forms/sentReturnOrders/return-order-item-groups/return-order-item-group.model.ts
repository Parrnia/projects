import { ReturnOrderItemModel } from "./return-order-items/return-order-item.model";

export interface ReturnOrderItemGroupModel {
    id: number;
    productLocalizedName : string;
    productName : string;
    price: number;
    totalDiscountPercent: number;
    productNo:string;
  }
