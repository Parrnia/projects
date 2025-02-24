import { AddressDto, PaymentType } from "src/app/web-api-client";

export class WorkflowOrderModel {
    id!: number;
    number!: number;
    quantity!: number;
    subtotal!: number;
    discountPercentForRole!: number;
    total!: number;
    createdAt!: string;
    paymentTypeName!: string;
    orderAddress!: string;
    currentOrderStateName!:string;
    customerTypeEnumName!: string;
    phoneNumber!: string;
    fullCustomerName!: string;
}
