
import { Product } from 'projects/storefront/src/app/interfaces/product';
import { AbstractFilterBuilder } from './abstract-filter-builder';
import { RangeFilter } from 'projects/storefront/src/app/interfaces/filter';
import { Observable, of } from 'rxjs';

export class RangeFilterBuilder extends AbstractFilterBuilder {
    public min: number|null = null;
    public max: number|null = null;
    public override value: [number, number]|null = null;


    parseValue(value: string): [number, number] {
        return value.split('-').map(x => parseFloat(x)) as [number, number];
    }

    makeItems(value: string, isLoaded : boolean): Observable<void> {
        this.max = 20000000;
        this.min = 0;

        /** Calculates the number of digits for rounding. */
        let digit = Math.max(Math.ceil(this.max).toString().length - 2, 1);

        digit = Math.pow(10, digit);

        this.max = Math.ceil(this.max / digit) * digit;
        this.min = Math.floor(this.min / digit) * digit;
        this.value = [this.min, this.max];

        if (value) {
            this.value = this.parseValue(value);
        }
        return of(undefined);
    }


    extractValue(product: Product): number {
        if (this.slug === 'price') {
            return product.price;
        }

        throw Error();
    }

    build(): RangeFilter {
        if (this.value === null) {
            throw new Error('RangeFilterBuilder.value is null');
        }
        if (this.min === null) {
            throw new Error('RangeFilterBuilder.min is null');
        }
        if (this.max === null) {
            throw new Error('RangeFilterBuilder.max is null');
        }

        return {
            type: 'range',
            slug: this.slug,
            name: this.name,
            min: this.min,
            max: this.max,
            value: this.value,
        };
    }
}
