import { BaseFilterItem, Filter } from "projects/storefront/src/app/interfaces/filter";
import { Observable } from "rxjs";


export abstract class AbstractFilterBuilder {
    constructor(
        public slug: string,
        public name: string,
    ) { }
    public value : any;
    public items: BaseFilterItem[] = [];
    abstract makeItems(value: string, isLoaded : boolean): Observable<void>;

    abstract build(): Filter;
}