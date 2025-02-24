import { FilterHandler } from '../filter.handler';
import { ActiveFilterProductCheck, CheckProductFilter } from '../../../../interfaces/filter';


export class FilterProductCheckHandler extends FilterHandler {
    type = 'productBrand';

    serialize(value: string[]): string {
        return value.join(',');
    }

    deserialize(value: string): string[] {
        return value !== '' ? value.split(',') : [];
    }

    isDefaultValue(filter: CheckProductFilter, value: string[]): boolean {
        return value.length === 0;
    }

    activeFilters(filter: CheckProductFilter): ActiveFilterProductCheck[] {
        if (this.isDefaultValue(filter, filter.value)) {
            return [];
        }

        return filter.items.filter(x => filter.value.includes(x.slug)).map(item => ({
            id: `${filter.slug}/${item.slug}`,
            type: filter.type,
            original: filter,
            item,
        }));
    }

    getResetValue(activeFilters: ActiveFilterProductCheck[]): string {
        const itemSlugs = activeFilters.map(x => x.item.slug);

        return this.serialize(activeFilters[0].original.value.filter(x => !itemSlugs.includes(x)));
    }
}
