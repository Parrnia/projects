import { CostRefundType, ReturnOrderCustomerReasonType, ReturnOrderItemGroupDto, ReturnOrderOrganizationReasonType, ReturnOrderStateBaseDto, ReturnOrderTotalDto, ReturnOrderTransportationType } from "projects/storefront/src/app/web-api-client";

export class ReturnOrderItemGroupModel {
    groupCommand! : CreateItemGroupCommandForReturnOrderModel;
    productName!: string;
    productNo?: string;
    productBrandName!: string;
    productCategory!: string;
    productImage!: string;
    price!: number;
    totalDiscountPercent!: number;
    taxPercent!: number;
    options!: OptionValue[];
}

export class OptionValue {
    name!: string;
    value!: string;
}
export class CreateItemGroupCommandForReturnOrderModel {
    productAttributeOptionId?: number;
    orderItems?: CreateItemCommandForItemGroupModel[];
}

export class CreateItemCommandForItemGroupModel {
    id!: number;
    quantity?: number;
    returnOrderReason?: CreateReasonCommandForItemModel;
    returnOrderItemDocuments?: DocumentCommandForItemModel[];
}

export class CreateReasonCommandForItemModel {
    id!: number;
    details?: string;
    customerType?: ReturnOrderCustomerReasonType | undefined;
    organizationType?: ReturnOrderOrganizationReasonType | undefined;
}

export class DocumentCommandForItemModel {
    id!: number;
    image?: string;
    description?: string;
}

export class ReturnOrderModel {
    id?: number;
    token?: string | undefined;
    number?: string;
    quantity?: number;
    subtotal?: number;
    total?: number;
    createdAt?: Date;
    createdAtText?: string;
    costRefundType?: CostRefundType;
    costRefundTypeName?: string;
    returnOrderStateHistory?: ReturnOrderStateBaseDto[];
    currentReturnOrderState?: ReturnOrderStateBaseDto;
    returnOrderTransportationType?: ReturnOrderTransportationType;
    returnOrderTransportationTypeName?: string;
    itemGroups?: ReturnOrderItemGroupDto[];
    totals?: ReturnOrderTotalDto[];
    orderNumber?: string;
    customerAccountInfo?: string | undefined;
}