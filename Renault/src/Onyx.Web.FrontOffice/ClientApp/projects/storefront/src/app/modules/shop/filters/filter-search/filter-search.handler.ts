import { FilterHandler } from '../filter.handler';
import { ActiveFilterSearch, SearchFilter } from '../../../../interfaces/filter';


export class FilterSearchHandler extends FilterHandler {
    type = 'search';

    serialize(value: string): string {
        return value;
    }

    deserialize(value: string): string {
        return value;
    }

    isDefaultValue(filter: SearchFilter, value: string): boolean {
        return value === '';
    }

    activeFilters(filter: SearchFilter): ActiveFilterSearch[] {
      
        return filter.value ? [{ id: filter.slug, type: 'search', original: filter }] : [];
    }

    getResetValue(activeFilters: ActiveFilterSearch[]): string {
        return '';
    }
}
