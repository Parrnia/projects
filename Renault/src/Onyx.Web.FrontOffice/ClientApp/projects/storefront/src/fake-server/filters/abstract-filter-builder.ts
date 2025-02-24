import { Product } from '../../app/interfaces/product';
import { Filter } from '../../app/interfaces/filter';

export abstract class AbstractFilterBuilder {
    public slug: string = '';
    public name: string = '';
    

    abstract test(product: Product): boolean;

    abstract makeItems(products: Product[], value: string): void;

    abstract calc(filters: AbstractFilterBuilder[]): void;

    abstract build(): Filter;

    setNameAndSlug(slug : string ,name : string){
        this.slug = slug;
        this.name = name;
    }
}

