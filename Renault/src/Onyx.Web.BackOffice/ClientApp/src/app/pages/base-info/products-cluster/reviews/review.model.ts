import { AddressDto, PaymentType } from "src/app/web-api-client";

export interface ReviewModel {
    id: number;
    date: string;
    rating: string;
    content: string;
    authorName: string;
    productName: string;
    isActive: boolean;
}
