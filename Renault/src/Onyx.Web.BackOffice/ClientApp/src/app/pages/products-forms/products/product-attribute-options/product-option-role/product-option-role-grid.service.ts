import { Injectable, PipeTransform } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { DecimalPipe } from '@angular/common';
import { debounceTime, delay, switchMap, tap } from 'rxjs/operators';

import { ProductAttributeOptionRolesClient, ProductAttributeOptionValuesClient, ProductAttributeOptionsClient } from 'src/app/web-api-client';
import { ProductOptionRoleModel } from './product-option-role.model';

interface SearchResult {
  ProductOptionRole: ProductOptionRoleModel[];
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

const compare = (v1: string | number, v2: string | number) => v1 < v2 ? -1 : v1 > v2 ? 1 : 0;


function matches(ProductOptionRole: ProductOptionRoleModel, term: string, pipe: PipeTransform) {
  debugger;
  return ProductOptionRole.minimumStockToDisplayProductForThisCustomerTypeEnum.toString().toLowerCase().includes(term.toLowerCase())
    || ProductOptionRole.availability.toString().toLowerCase().includes(term.toLowerCase())
    || ProductOptionRole.availabilityName.toString().toLowerCase().includes(term.toLowerCase())
    || ProductOptionRole.mainMaxOrderQty.toString().toLowerCase().includes(term.toLowerCase())
    || ProductOptionRole.currentMaxOrderQty.toString().toLowerCase().includes(term.toLowerCase())
    || ProductOptionRole.mainMinOrderQty.toString().toLowerCase().includes(term.toLowerCase())
    || ProductOptionRole.currentMinOrderQty.toString().toLowerCase().includes(term.toLowerCase())
    || ProductOptionRole.customerTypeEnumName.toString().toLowerCase().includes(term.toLowerCase())
    || ProductOptionRole.discountPercent.toString().toLowerCase().includes(term.toLowerCase())
    ;

}

@Injectable({ providedIn: 'root' })
export class ProductOptionRoleGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _productPrices$ = new BehaviorSubject<ProductOptionRoleModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private ProductOptionRoleList: any[];
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
    , public client: ProductAttributeOptionRolesClient
  ) {
    this.ProductOptionRoleList = [];
    this._search$.pipe(
      tap(() => this._loading$.next(true)),
      debounceTime(200),
      switchMap(() => this._search()),
      delay(200),
      tap(() => this._loading$.next(false))
    ).subscribe(result => {
      this._productPrices$.next(result.ProductOptionRole);
      this._total$.next(result.total);
    });

  }

  get ProductOptionRole$() { return this._productPrices$.asObservable(); }
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
    // let ProductOptionRole = sort(this.ProductOptionRoleList, sortColumn, sortDirection);
    let ProductOptionRole = this.ProductOptionRoleList;
    // 2. filter
    ProductOptionRole = ProductOptionRole.filter(ProductOptionRole => matches(ProductOptionRole, searchTerm, this.pipe));
    const total = ProductOptionRole.length;

    // 3. paginate
    this._state.totalRecords = ProductOptionRole.length;
    this._state.startIndex = (page - 1) * this.pageSize + 1;
    this._state.endIndex = (page - 1) * this.pageSize + this.pageSize;
    if (this.endIndex > this.totalRecords) {
      this._state.endIndex = this.totalRecords;
    }
    ProductOptionRole = ProductOptionRole.slice(this._state.startIndex - 1, this._state.endIndex);
    return of({ ProductOptionRole, total });
  }

  public getAllProductAttributeOptionRoles(optionId: number) {
    this.client.getAllProductAttributeOptionRoleByOptionId(optionId).subscribe(result => {
      this.ProductOptionRoleList = result;
      this._search$.next();
    }, error => console.error(error));

  }


}
