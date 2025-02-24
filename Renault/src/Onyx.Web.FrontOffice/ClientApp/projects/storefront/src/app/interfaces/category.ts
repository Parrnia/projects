import { CustomFields } from './custom-fields';

export class BaseCategory {
    id!: number;
    type!: string;
    name!: string;
    slug!: string;
    image!: string | null;
    items?: number;
    parent?: this;
    children?: this[];
    customFields!: CustomFields;
}

export type ShopCategoryLayout = 'categories' | 'products';

export class ShopCategory extends BaseCategory {
    override type!: 'shop';
    layout!: ShopCategoryLayout;
}

export class BlogCategory extends BaseCategory {
    override type!: 'blog';
}

export type Category = ShopCategory | BlogCategory;
