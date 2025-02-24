import { Vehicle } from 'projects/storefront/src/app/interfaces/vehicle';
import { vehicles } from './../../../../fake-server/database/vehicles';
import { Injectable } from '@angular/core';
import { CategoryFilterBuilder } from './filters/category-filter-builder';
import { GetProductsListOptions } from '../../../api';
import { Observable, of, forkJoin } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { ProductsList } from '../../../interfaces/list';
import { CustomerTypeEnum, ProductBrandsClient, ProductCategoriesClient, ProductOptionColorsClient, ProductOptionMaterialsClient, ProductsClient, SearchClient, VehicleBrandsClient, VehiclesClient } from '../../../web-api-client';
import { AbstractFilterBuilder } from './filters/abstract-filter-builder';
import { CheckVehicleBrandFilterBuilder } from './filters/check-vehicle-brand-filter-builder';
import { ColorFilterBuilder } from './filters/color-filter-builder';
import { RadioFilterBuilder } from './filters/radio-filter-builder';
import { RangeFilterBuilder } from './filters/range-filter-builder';
import { RatingFilterBuilder } from './filters/rating-filter-builder';
import { VehicleFilterBuilder } from './filters/vehicle-filter-builder';
import { CategorymapperService } from '../../../mapServieces/categoriesCluster/categorymapper.service';
import { VehiclemapperService } from '../../../mapServieces/brandsCluster/vehiclemapper.service';
import { Product } from '../../../interfaces/product';
import { ProductmapperService } from '../../../mapServieces/productsCluster/productmapper.service';
import { BaseFilterItem } from '../../../interfaces/filter';
import { SearchFilterBuilder } from './filters/search-filter-builder';
import { MaterialFilterBuilder } from './filters/material-filter-builder';
import { AuthService } from '../../../services/authService/auth.service';
import { UserVehicleAccountService } from '../../../services/user-vehicle-account.service';
import { UserVehicleService } from '../../../services/user-vehicle.service';
import { CurrentVehicleService } from '../../../services/current-vehicle.service';
import { CheckProductBrandFilterBuilder } from './filters/check-product-brand-filter-builder';
import { ProductBrandmapperService } from '../../../mapServieces/brandsCluster/product-brand-mapper.service';
import { VehicleBrandmapperService } from '../../../mapServieces/brandsCluster/vehicle-brand-mapper.service';


@Injectable({
  providedIn: 'root'
})
export class FilterService {
  private items: Product[] = [];
  private lastFilterValues: { [slug: string]: string } = {};
  private itemsLoaded: boolean = false;
  private lastFilterItems: { [slug: string]: BaseFilterItem[] } = {};
  private lastVehicle!: Vehicle|null;
  constructor(
    private productCategoriesClient: ProductCategoriesClient,
    private categorymapperService: CategorymapperService,
    private productBrandsClient: ProductBrandsClient,
    private productBrandmapperService: ProductBrandmapperService,
    private vehicleBrandsClient: VehicleBrandsClient,
    private vehicleBrandmapperService: VehicleBrandmapperService,
    private productOptionColorsClient: ProductOptionColorsClient,
    private productOptionMaterialsClient: ProductOptionMaterialsClient,
    private userVehicleService: UserVehicleService,
    private userVehicleAccountService: UserVehicleAccountService,
    private authService: AuthService,
    private productmapperService: ProductmapperService,
    private searchClient: SearchClient,
    private currentVehicleService: CurrentVehicleService

  ) { }

  getProductsList(options?: GetProductsListOptions): Observable<ProductsList> {

    let filters: AbstractFilterBuilder[] = [
      new SearchFilterBuilder('search', 'جستجو'),
      new CategoryFilterBuilder('category', 'دسته بندی ها', this.productCategoriesClient, this.categorymapperService),
      new VehicleFilterBuilder('vehicle', 'خودرو', this.userVehicleService,this.userVehicleAccountService,this.authService,this.currentVehicleService),
      new RangeFilterBuilder('price', 'محدوده قیمت'),
      new CheckVehicleBrandFilterBuilder('vehicleBrand', 'برند خودرو', this.vehicleBrandsClient, this.vehicleBrandmapperService),
      new CheckProductBrandFilterBuilder('productBrand', 'برند محصول', this.productBrandsClient, this.productBrandmapperService),
      new RadioFilterBuilder('discount', 'با تخفیف'),
      new RatingFilterBuilder('rating', 'امتیاز'),
      new ColorFilterBuilder('color', 'رنگ', this.productOptionColorsClient),
      new MaterialFilterBuilder('material', 'جنس', this.productOptionMaterialsClient)
    ];
     
    let filterValues = options?.filters || {};
    if (filterValues['category'] != this.lastFilterValues['category']) {
      this.lastFilterValues['category'] = filterValues['category'];
      filterValues = this.lastFilterValues;
    }
    
    const page = options?.page || 1;
    const limit = options?.limit || 16;
    const sort = options?.sort || 'default';

    let categoryFilter = filters.find(c => c.slug == 'category');
    let vehicleFilter = filters.find(c => c.slug == 'vehicle');
    let priceFilter = filters.find(c => c.slug == 'price');
    let vehicleBrandFilter = filters.find(c => c.slug == 'vehicleBrand');
    let productBrandFilter = filters.find(c => c.slug == 'productBrand');
    let discountFilter = filters.find(c => c.slug == 'discount');
    let ratingFilter = filters.find(c => c.slug == 'rating');
    let colorFilter = filters.find(c => c.slug == 'color');
    let materialFilter = filters.find(c => c.slug == 'material');
    let searchFilter = filters.find(c => c.slug == 'search');


    if(vehicleFilter!.items){
      this.lastFilterItems[vehicleFilter!.slug] = vehicleFilter!.items;
    } else{
      vehicleFilter!.items = this.lastFilterItems[vehicleFilter!.slug];
    }


    let makeItemsObservables: Observable<void>[] = [];
    if (this.itemsLoaded) {
       
      makeItemsObservables = filters.map(filter =>
        filter.makeItems(filterValues[filter.slug], this.itemsLoaded));
      vehicleBrandFilter!.items = this.lastFilterItems[vehicleBrandFilter!.slug];
      productBrandFilter!.items = this.lastFilterItems[productBrandFilter!.slug];
      vehicleFilter!.items = this.lastFilterItems[vehicleFilter!.slug];
      discountFilter!.items = this.lastFilterItems[discountFilter!.slug];
    } else {
       
      makeItemsObservables = filters.map(filter =>
        filter.makeItems(filterValues[filter.slug], this.itemsLoaded));
      filters.forEach(filter => this.lastFilterItems[filter.slug] = filter.items);
      this.itemsLoaded = true;
       
    };


    return forkJoin(makeItemsObservables).pipe(
      switchMap(() => {
        this.lastFilterValues = filterValues;
    
        return this.searchClient.getProductsByFilter(
          CustomerTypeEnum.Personal,
          categoryFilter?.value ?? categoryFilter?.value,
          vehicleFilter?.value != undefined && !Number.isNaN(vehicleFilter?.value) ? parseFloat(vehicleFilter?.value) : undefined,
          priceFilter?.value[0] ?? this.parseValuePrice(priceFilter?.value[0]),
          priceFilter?.value[1] ?? this.parseValuePrice(priceFilter?.value[1]),
          productBrandFilter?.value ?? this.parseValueBrandName(productBrandFilter?.value),
          vehicleBrandFilter?.value ?? this.parseValueBrandName(vehicleBrandFilter?.value),
          discountFilter?.value ?? discountFilter?.value,
          ratingFilter?.value ?? this.parseValueRating(ratingFilter?.value),
          colorFilter?.value ?? colorFilter?.value,
          materialFilter?.value ?? materialFilter?.value,
          searchFilter?.value ?? searchFilter?.value,
          page,
          limit,
          sort
        ).pipe(
          switchMap((res) => {
             
            this.items = this.productmapperService.mapProductsForFilterResult(res.products ?? []);
            const total = res.productsCount ?? 0;
            const pages = Math.ceil(total / limit);
            const from = (page - 1) * limit + 1;
            const to = Math.min(page * limit, total);
            return of({
              items: this.items,
              page,
              limit,
              sort,
              total,
              pages,
              from,
              to,
              filters: filters.map(x => x.build()),
            });
          }
          )
        );
    })
  );
}

  // ... Your other methods ...

  private parseValueBrandName(value: string): string[] {
    return value ? value.split(',') : [];
  }

  private parseValueRating(value: string): number[] {
    return value ? value.split(',').map(x => parseFloat(x)) : [];
  }

  parseValuePrice(value: string): [number, number] {
    return value.split('-').map(x => parseFloat(x)) as [number, number];
  }
}
