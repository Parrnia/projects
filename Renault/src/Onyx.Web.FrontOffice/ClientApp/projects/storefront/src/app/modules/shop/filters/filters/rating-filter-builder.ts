
import { RatingFilter, RatingFilterItem } from 'projects/storefront/src/app/interfaces/filter';
import { AbstractFilterBuilder } from './abstract-filter-builder';
import { Observable, of } from 'rxjs';

export class RatingFilterBuilder extends AbstractFilterBuilder {
    private ratingItems : RatingFilterItem[] = [];

    public override value: number[] = [];


    makeItems(value: string, isLoaded : boolean): Observable<void> {
        let ratings = [1,2,3,4,5]
        ratings.forEach(rating => {
            const item = this.extractItem(rating);

            if (!this.ratingItems.find(x => x.rating === item.rating)) {
                this.ratingItems.push(item);
            }
        });
        
        

        this.value = this.parseValue(value);
        this.ratingItems.sort((a, b) => b.rating - a.rating);
        return of(undefined);
    }


    build(): RatingFilter {
        return {
            type: 'rating',
            slug: this.slug,
            name: this.name,
            items: this.ratingItems.sort((a, b) => b.rating - a.rating),
            value: this.value,
        };
    }

    private parseValue(value: string): number[] {
        return value ? value.split(',').map(x => parseFloat(x)) : [];
    }

    // noinspection JSMethodCanBeStatic
    private extractItem(rating : number): RatingFilterItem {
        return {
            rating: Math.round(rating || 0),
            count: 0,
        };
    }
}
