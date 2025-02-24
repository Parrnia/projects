import { OrderItemProductAttributeOptionValue } from "../../app/interfaces/order";
import { PaymentType } from "../../app/web-api-client";

export interface OrderItemOptionDef {
    name: string;
    value: string;
}

export interface OrderItemDef {
    product: string;
    options: OrderItemOptionDef[];
    quantity: number;
    optionValues: OrderItemProductAttributeOptionValue[];
}

export interface OrderDef {
    number: string;
    createdAt: string;
    payment: PaymentType;
    status: string;
    items: OrderItemDef[];
}
