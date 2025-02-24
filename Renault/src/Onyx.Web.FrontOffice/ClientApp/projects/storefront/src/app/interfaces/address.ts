export class AddressData {
    title!: string;
    company!: string;
    country!: string;
    countryId!: number;
    address1!: string;
    address2!: string;
    city!: string;
    state!: string;
    postcode!: string;
}

export class Address extends AddressData {
    id!: number;
    default!: boolean;
}
