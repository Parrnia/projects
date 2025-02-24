import { Observable } from 'rxjs';
import { ShopCategory } from './category';
import { Vehicle } from './vehicle';


export interface BaseFilter<T extends string, V> {
    type: T;
    name: string;
    slug: string;
    value: V;
}
export interface BaseFilterItem {
    slug: string;
    name: string;
    count: number;
}


export interface ColorFilterItem extends BaseFilterItem {
    color: string;
}

export interface RatingFilterItem {
    rating: number;
    count: number;
}


export type CategoryFilterValue = string | null;
export type VehicleFilterValue = number | null;
export type RangeFilterValue = [number, number];
export type CheckProductFilterValue = string[];
export type CheckVehicleFilterValue = string[];
export type RadioFilterValue = string | null;
export type RatingFilterValue = number[];
export type ColorFilterValue = string[];
export type MaterialFilterValue = string[];
export type SearchFilterValue = string;


export type CategoryFilter = BaseFilter<'category', CategoryFilterValue> & {items:ShopCategory[]};
export type VehicleFilter = BaseFilter<'vehicle', VehicleFilterValue> & {vehicle: Vehicle|null};
export type RangeFilter = BaseFilter<'range', RangeFilterValue> & {min: number; max: number};
export type CheckProductFilter = BaseFilter<'productBrand', CheckProductFilterValue> & {items: BaseFilterItem[]};
export type CheckVehicleFilter = BaseFilter<'vehicleBrand', CheckVehicleFilterValue> & {items: BaseFilterItem[]};
export type RadioFilter = BaseFilter<'radio', RadioFilterValue> & {items: BaseFilterItem[]};
export type RatingFilter = BaseFilter<'rating', RatingFilterValue> & {items: RatingFilterItem[]};
export type ColorFilter = BaseFilter<'color', ColorFilterValue> & {items: ColorFilterItem[]};
export type MaterialFilter = BaseFilter<'material', MaterialFilterValue> & {items: BaseFilterItem[]};
export type SearchFilter = BaseFilter<'search', SearchFilterValue> & {placeHolder : string};

export type Filter =
    CategoryFilter |
    VehicleFilter |
    RangeFilter |
    CheckVehicleFilter |
    CheckProductFilter |
    RadioFilter |
    RatingFilter |
    ColorFilter|
    MaterialFilter|
    SearchFilter;


export interface ActiveFilterBase<T extends Filter> {
    id: string;
    type: T['type'];
    original: T;
}

export type ActiveFilterVehicle = ActiveFilterBase<VehicleFilter> & {original: VehicleFilter};
export type ActiveFilterRange = ActiveFilterBase<RangeFilter>;
export type ActiveFilterProductCheck = ActiveFilterBase<CheckProductFilter> & {item: BaseFilterItem};
export type ActiveFilterVehicleCheck = ActiveFilterBase<CheckVehicleFilter> & {item: BaseFilterItem};
export type ActiveFilterRadio = ActiveFilterBase<RadioFilter> & {item: BaseFilterItem};
export type ActiveFilterRating = ActiveFilterBase<RatingFilter> & {item: RatingFilterItem};
export type ActiveFilterColor = ActiveFilterBase<ColorFilter> & {item: ColorFilterItem};
export type ActiveFilterMaterial = ActiveFilterBase<MaterialFilter> & {item: BaseFilterItem};
export type ActiveFilterSearch = ActiveFilterBase<SearchFilter>;


export type ActiveFilter =
    ActiveFilterVehicle |
    ActiveFilterRange |
    ActiveFilterProductCheck |
    ActiveFilterVehicleCheck |
    ActiveFilterRadio |
    ActiveFilterRating |
    ActiveFilterColor|
    ActiveFilterSearch|
    ActiveFilterMaterial;
