import { CustomFields } from './custom-fields';

export class Link {
    title!: string;
    url?: string;
    external?: boolean;
    customFields?: CustomFields;
}

export class NestedLink extends Link {
    links?: NestedLink[];
}
