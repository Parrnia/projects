import { MaterialFilter, ActiveFilterMaterial } from "projects/storefront/src/app/interfaces/filter";
import { FilterHandler } from "../filter.handler";


export class FilterMaterialHandler extends FilterHandler {
    type = 'material';

    serialize(value: string[]): string {
        return value.join(',');
    }

    deserialize(value: string): string[] {
        return value !== '' ? value.split(',') : [];
    }

    isDefaultValue(filter: MaterialFilter, value: string[]): boolean {
        return value.length === 0;
    }

    activeFilters(filter: MaterialFilter): ActiveFilterMaterial[] {
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

    getResetValue(activeFilters: ActiveFilterMaterial[]): string {
        const itemSlugs = activeFilters.map(x => x.item.slug);

        return this.serialize(activeFilters[0].original.value.filter(x => !itemSlugs.includes(x)));
    }
}
