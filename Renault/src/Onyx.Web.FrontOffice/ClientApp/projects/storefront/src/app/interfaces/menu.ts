import { NestedLink } from './link';

export class MenuBase {
    type!: string;
}

export type MegamenuSize = 'xl' | 'lg' | 'md' | 'nl' | 'sm';

export type MegamenuColumnSize =
    | 1
    | 2
    | 3
    | 4
    | 5
    | 6
    | 7
    | 8
    | 9
    | 10
    | 11
    | 12
    | '1of1'
    | '1of2'
    | '1of3'
    | '1of4'
    | '1of5';

export class MegamenuColumn {
    size!: MegamenuColumnSize;
    links!: NestedLink[];
}

export class Megamenu extends MenuBase {
    override type!: 'megamenu';
    size!: MegamenuSize;
    image?: string;
    imageSrc?: string;
    columns!: MegamenuColumn[];
}

export interface Menu extends MenuBase {
    type: 'menu';
    links: NestedLink[];
}
