
import { BaseFilterItem, RadioFilter } from 'projects/storefront/src/app/interfaces/filter';
import { AbstractFilterBuilder } from './abstract-filter-builder';
import { Observable, of } from 'rxjs';

export class RadioFilterBuilder extends AbstractFilterBuilder {

    public override value: string|null = null;


    makeItems(value: string, isLoaded : boolean): Observable<void> {
         
    if(!isLoaded){
        this.items = this.extractItems();
    }
        
        this.value = value || 'any';
        return of(undefined);
    }


    build(): RadioFilter {
        return {
            type: 'radio',
            slug: this.slug,
            name: this.name,
            items: this.items,
            value: this.value,
        };
    }

    private extractItems(): BaseFilterItem[] {
        if (this.slug === 'discount') {
            const items: BaseFilterItem[] = [
                { slug: 'any', name: 'Any', count: 0 },
                { slug: 'yes', name: 'Yes', count: 0 },
                { slug: 'no', name: 'No', count: 0 }
            ];

            return items;
        }

        throw Error();
    }
}
