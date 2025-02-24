import { AddressDto, PaymentType } from "src/app/web-api-client";

export interface TagModel {
    id: number;
    enTitle: string;
    faTitle: string;
    isActive : boolean;
    state: number;
    
}
