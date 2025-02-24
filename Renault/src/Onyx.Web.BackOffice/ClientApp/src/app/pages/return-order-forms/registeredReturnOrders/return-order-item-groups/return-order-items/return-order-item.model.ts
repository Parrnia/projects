import { ReturnOrderItemReasonModel } from "./return-order-item-reason.model";

export interface ReturnOrderItemModel {
    id: number;
    quantity: number;
    total: number;
    isAccepted:boolean;
    returnOrderReason: ReturnOrderItemReasonModel;
  }
