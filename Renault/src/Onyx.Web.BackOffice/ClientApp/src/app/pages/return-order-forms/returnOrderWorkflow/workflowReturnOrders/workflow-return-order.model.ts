import { ReturnOrderStatus } from "src/app/web-api-client";

export class WorkflowReturnOrderModel {
    id!: number;
    number!: number;
    quantity!: number;
    subtotal!: number;
    total!: number;
    createdAt!: string;
    costRefundTypeName!: string;
    currentReturnOrderState!: ReturnOrderStatus;
    currentReturnOrderStateName!: string;
    returnOrderTransportationTypeName!: string;
    orderNumber!: string;
    phoneNumber!: string;
    fullCustomerName!: string;
    orderId!: number;
}
