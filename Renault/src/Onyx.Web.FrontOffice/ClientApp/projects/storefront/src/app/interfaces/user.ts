import { Address } from "./address";

export class User {
    email!: string;
    phone!: string;
    firstName!: string;
    lastName!: string;
    avatar!: string;
    company?: string;
}
export class Customer extends User {
    addresses! : Address[]
}
