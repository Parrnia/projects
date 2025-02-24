import { FilterHandler } from './filter.handler';
import { FilterVehicleCheckHandler } from './filter-vehicle-brand-check/filter-vehicle-brand-check.handler';
import { FilterColorHandler } from './filter-color/filter-color.handler';
import { FilterRadioHandler } from './filter-radio/filter-radio.handler';
import { FilterRangeHandler } from './filter-range/filter-range.handler';
import { FilterRatingHandler } from './filter-rating/filter-rating.handler';
import { FilterVehicleHandler } from './filter-vehicle/filter-vehicle.handler';
import { FilterSearchHandler } from './filter-search/filter-search.handler';
import { FilterMaterialHandler } from './filter-material/filter-material.handler';
import { FilterProductCheckHandler } from './filter-product-brand-check/filter-product-brand-check.handler';


export const filterHandlers: FilterHandler[] = [
    new FilterVehicleCheckHandler(),
    new FilterProductCheckHandler(),
    new FilterColorHandler(),
    new FilterMaterialHandler(),
    new FilterRadioHandler(),
    new FilterRangeHandler(),
    new FilterRatingHandler(),
    new FilterVehicleHandler(),
    new FilterSearchHandler()
];
