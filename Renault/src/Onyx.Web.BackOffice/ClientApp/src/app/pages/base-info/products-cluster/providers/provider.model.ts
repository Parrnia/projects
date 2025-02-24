import { AddressDto, PaymentType } from "src/app/web-api-client";

export interface ProviderModel {
    id: number;
    code: number;
    name: string;
    localizedName: string;
    localizedCode: string;
    description: string;
    state: number;
}
