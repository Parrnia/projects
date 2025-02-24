
export interface ReturnOrderModel {
    id: number;
    number: number;
    quantity: number;
    subtotal: number;
    total: number;
    createdAt: string;
    costRefundTypeName: string;
    currentReturnOrderStateName: string;
    returnOrderTransportationTypeName: string;
    orderNumber: string;
    phoneNumber: string;
    fullCustomerName: string;
    customerAccountInfo: string;
    orderId: number;
}
