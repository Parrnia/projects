import { AddressDto, PaymentType } from "src/app/web-api-client";

export interface PendingForRegisterOrdersModel {
  id: number;
  number: number;
  quantity: number;
  subtotal: number;
  discountPercentForRole: number;
  total: number;
  createdAt: string;
  orderPaymentTypeName: string;
  orderAddress: string;
  currentOrderStateName: string;
  customerTypeEnumName: string;
  phoneNumber: string;
  fullCustomerName: string;
  isPayed: boolean;
}
