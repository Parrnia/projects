import { Product, ProductAttributeOption, ProductAttributeOptionValue, ProductStock } from './product';
import { Address, AddressData } from './address';
import { OnlinePaymentStatus, OrderPaymentType, OrderStatus, PaymentServiceType, PaymentType } from '../web-api-client';


export class OrderItemOption {
    name!: string;
    value!: string;
}

export class OrderItemProductAttributeOptionValue {
    id!: number;
    name!: string;
    value!: string;
}

export class OrderItem {
    product!: Product;
    selectedProductAttributeOption!: ProductAttributeOption;
    options!: OrderItemOption[];
    price!: number;
    quantity!: number;
    total!: number;
    discountPercentForProduct!: number;
    optionValues!: OrderItemProductAttributeOptionValue[];
}

export class OrderTotal {
    title!: string;
    price!: number;
    type!: 'shipping' | 'fee' | 'tax' | 'other' | 'freeShipping' | 'discountOnProduct' | 'discountOnCustomerType';
}

export class OrderStateBase {
    orderStatus!: string;
    details!: string;
    created!: string;
}

export class Order {
    id!: number;
    token!: string;
    number!: string;
    createdAt!: string;
    payment!: OrderPaymentType;
    paymentHistory!: OrderPayment[];
    status!: string;
    statusDetails!: string;
    CurrentOrderStatus!: OrderStatus
    items!: OrderItem[];
    quantity!: number;
    subtotal!: number;
    totals!: OrderTotal[];
    total!: number;
    orderAddress!: Address;
    orderStateHistory?: OrderStateBase[];
    phoneNumber?: string;
    
  }
  
  export class OrderPayment {
    id?: number;
    paymentType?: PaymentType;
    paymentTypeName?: string;
    amount?: number | undefined;
    paymentServiceType?: PaymentServiceType | undefined;
    paymentServiceTypeName?: string | undefined;
    status?: OnlinePaymentStatus | undefined;
    statusName?: string | undefined;
    authority?: string | undefined;
    cardNumber?: string | undefined;
    rrn?: string | undefined;
    refId?: string | undefined;
    payGateTranId?: string | undefined;
    salesOrderId?: number | undefined;
    serviceTypeId?: number | undefined;
    error?: string | undefined;
}

export class OrderForReturn {
    id!: number;
    token!: string;
    number!: string;
    createdAt!: string;
    payment!: OrderPaymentType;
    status!: string;
    statusDetails!: string;
    items!: OrderItemForReturn[];
    quantity!: number;
    subtotal!: number;
    totals!: OrderTotal[];
    total!: number;
    orderAddress!: Address;
    orderStateHistory?: OrderStateBase[]
    phoneNumber?: string;
}

export class OrderItemForReturn {
    id?: number;
    price!: number;
    totalDiscountPercent!: number;
    taxPercent!: number;
    quantity!: number;
    total!: number;
    productLocalizedName!: string;
    productNo: string | undefined;
    optionValues?: OrderItemProductAttributeOptionValue[];
    selectedProductAttributeOption!: ProductAttributeOptionForReturn;
    selectedProductAttributeOptionId?: number;
    options?: OrderItemOption[];
}

export class ProductAttributeOptionForReturn {
    id!: number;
    productName!: string;
    productNo!: string;
    productBrandName!: string;
    productCategory!: string;
    productImage?: string;
}