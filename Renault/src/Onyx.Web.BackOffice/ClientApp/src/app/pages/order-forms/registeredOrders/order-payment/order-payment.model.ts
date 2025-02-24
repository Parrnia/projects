import { OnlinePaymentStatus, PaymentType } from "src/app/web-api-client";

export interface OrderPaymentModel {
  id: number;
  paymentType: PaymentType;
  amount: number ;
  status?: OnlinePaymentStatus;
  authority?: string | undefined;
  cardNumber?: string | undefined;
  rrn?: string | undefined;
  refId?: string | undefined;
  payGateTranId?: string | undefined;
  salesOrderId?: number | undefined;
  serviceTypeId?: number | undefined;
  error?: string | undefined;
}
