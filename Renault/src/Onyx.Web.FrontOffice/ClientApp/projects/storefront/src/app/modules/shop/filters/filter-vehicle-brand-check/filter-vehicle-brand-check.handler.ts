import { ActiveFilterVehicleCheck, CheckVehicleFilter } from 'projects/storefront/src/app/interfaces/filter';
import { FilterHandler } from '../filter.handler';


export class FilterVehicleCheckHandler extends FilterHandler {
    type = 'vehicleBrand';

    serialize(value: string[]): string {
        return value.join(',');
    }

    deserialize(value: string): string[] {
        return value !== '' ? value.split(',') : [];
    }

    isDefaultValue(filter: CheckVehicleFilter, value: string[]): boolean {
        return value.length === 0;
    }

    activeFilters(filter: CheckVehicleFilter): ActiveFilterVehicleCheck[] {
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

    getResetValue(activeFilters: ActiveFilterVehicleCheck[]): string {
        const itemSlugs = activeFilters.map(x => x.item.slug);

        return this.serialize(activeFilters[0].original.value.filter(x => !itemSlugs.includes(x)));
    }
}
