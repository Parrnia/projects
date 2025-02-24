import { Injectable, PipeTransform } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { DecimalPipe } from '@angular/common';
import { debounceTime, delay, switchMap, tap } from 'rxjs/operators';

import { ProductAttributeOptionDto, ProductAttributeOptionsClient } from 'src/app/web-api-client';
import { ProductAttributeOptionModel } from './product-attribute-option.model';



interface SearchResult {
  productAttributeOption: ProductAttributeOptionModel[];
  total: number;
}

interface State {
  page: number;
  pageSize: number;
  searchTerm: string;
  startIndex: number;
  endIndex: number;
  totalRecords: number;
}

function matches(productAttributeOption: ProductAttributeOptionModel, term: string, pipe: PipeTransform) {

  return productAttributeOption.totalCount.toString().toLowerCase().includes(term.toLowerCase())
    || productAttributeOption.safetyStockQty.toString().toLowerCase().includes(term.toLowerCase())
    ;

}

@Injectable({ providedIn: 'root' })
export class ProductAttributeOptionGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _productAttributeOption$ = new BehaviorSubject<ProductAttributeOptionModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private productAttributeOptionList: ProductAttributeOptionModel[];
  createResult: Subject<any> = new Subject<any>();

  private _state: State = {
    page: 1,
    pageSize: 5,
    searchTerm: '',
    startIndex: 0,
    endIndex: 9,
    totalRecords: 0
  };

  constructor(private pipe: DecimalPipe
    , public client: ProductAttributeOptionsClient
  ) {
    this.productAttributeOptionList = [];
    // this.getAllproductAttributeOption();
    this._search$.pipe(
      tap(() => this._loading$.next(true)),
      debounceTime(200),
      switchMap(() => this._search()),
      delay(200),
      tap(() => this._loading$.next(false))
    ).subscribe(result => {
      this._productAttributeOption$.next(result.productAttributeOption);
      this._total$.next(result.total);
    });

  }

  get productAttributeOption$() { return this._productAttributeOption$.asObservable(); }
  get total$() { return this._total$.asObservable(); }
  get loading$() { return this._loading$.asObservable(); }
  get page() { return this._state.page; }
  get pageSize() { return this._state.pageSize; }
  get searchTerm() { return this._state.searchTerm; }
  get startIndex() { return this._state.startIndex; }
  get endIndex() { return this._state.endIndex; }
  get totalRecords() { return this._state.totalRecords; }

  set page(page: number) { this._set({ page }); }
  set pageSize(pageSize: number) { this._set({ pageSize }); }
  set searchTerm(searchTerm: string) { this._set({ searchTerm }); }
  set startIndex(startIndex: number) { this._set({ startIndex }); }
  set endIndex(endIndex: number) { this._set({ endIndex }); }
  set totalRecords(totalRecords: number) { this._set({ totalRecords }); }

  private _set(patch: Partial<State>) {

    Object.assign(this._state, patch);
    this._search$.next();
  }

  private _search(): Observable<SearchResult> {
    const { pageSize, page, searchTerm } = this._state;

    // 1. sort
    //TODO : Fill data instead []
    // let productAttributeOption = sort(this.productAttributeOptionList, sortColumn, sortDirection);
    let productAttributeOption = this.productAttributeOptionList;
    // 2. filter
    productAttributeOption = productAttributeOption.filter(productAttributeOption => matches(productAttributeOption, searchTerm, this.pipe));
    const total = productAttributeOption.length;

    // 3. paginate
    this._state.totalRecords = productAttributeOption.length;
    this._state.startIndex = (page - 1) * this.pageSize + 1;
    this._state.endIndex = (page - 1) * this.pageSize + this.pageSize;
    if (this.endIndex > this.totalRecords) {
      this._state.endIndex = this.totalRecords;
    }
    productAttributeOption = productAttributeOption.slice(this._state.startIndex - 1, this._state.endIndex);
    return of({ productAttributeOption, total });
  }

  public getAllProductAttributeOption(productId: number) {
    this.client.getAllProductAttributeOptionByProductId(productId).subscribe(result => {
      this.productAttributeOptionList = this.mapProductAttributeOptions(result);
      this._search$.next();
    }, error => console.error(error));

  }


  private mapProductAttributeOptions(productAttributeOptionDtos: ProductAttributeOptionDto[]) {
    let productAttributeOptionModels: ProductAttributeOptionModel[] = [];

    productAttributeOptionDtos.forEach(c => {
      productAttributeOptionModels.push(this.mapProductAttributeOption(c));
    })
    return productAttributeOptionModels;
  }

  private mapProductAttributeOption(productAttributeOptionDto: ProductAttributeOptionDto) {
    let productAttributeOptionModel = new ProductAttributeOptionModel();
    productAttributeOptionModel.id = productAttributeOptionDto.id!;
    productAttributeOptionModel.isDefault = productAttributeOptionDto.isDefault!;
    productAttributeOptionModel.maxSalePriceNonCompanyProductPercent = productAttributeOptionDto.maxSalePriceNonCompanyProductPercent;
    productAttributeOptionModel.maxStockQty = productAttributeOptionDto.maxStockQty!;
    productAttributeOptionModel.minStockQty = productAttributeOptionDto.minStockQty!;
    productAttributeOptionModel.safetyStockQty = productAttributeOptionDto.safetyStockQty!;
    productAttributeOptionModel.totalCount = productAttributeOptionDto.totalCount!;
    productAttributeOptionModel.optionValues = productAttributeOptionDto.optionValues?.map(item => item.name + ':' + (item.value != 'null' ? item.value : 'ندارد') ?? '').join('/') ?? '';
    debugger;
    return productAttributeOptionModel;
  }

}
