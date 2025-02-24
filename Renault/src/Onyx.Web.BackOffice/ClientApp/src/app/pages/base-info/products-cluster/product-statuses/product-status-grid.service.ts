

import { Injectable, PipeTransform } from '@angular/core';

import { BehaviorSubject, Observable, of, Subject } from 'rxjs';


import { DecimalPipe } from '@angular/common';
import { debounceTime, delay, switchMap, tap } from 'rxjs/operators';

import { ProductStatusModel } from './product-status.model';
import { SortColumn, SortDirection } from './product-status-sortable.directive';
import { ProductStatusDto, ProductStatusesClient,  } from 'src/app/web-api-client';
import { br } from '@fullcalendar/core/internal-common';
import { ImageService } from 'src/app/core/services/image.service';



interface SearchResult {
  productStatus: ProductStatusModel[];
  total: number;
}

interface State {
  page: number;
  pageSize: number;
  searchTerm: string;
  sortColumn: SortColumn;
  sortDirection: SortDirection;
  startIndex: number;
  endIndex: number;
  totalRecords: number;
}

const compare = (v1: string | number, v2: string | number) => v1 < v2 ? -1 : v1 > v2 ? 1 : 0;

function sort(productStatuss: ProductStatusModel[], column: SortColumn, direction: string): ProductStatusModel[] {
  if (direction === '' || column === '') {
    return productStatuss;
  } else {
    return [...productStatuss].sort((a, b) => {
      const res = compare(a[column], b[column]);
      return direction === 'asc' ? res : -res;
    });
  }
}

function matches(productStatus: ProductStatusModel, term: string, pipe: PipeTransform) {
  return productStatus.code?.toString().toLowerCase().includes(term.toLowerCase())
    || productStatus.name?.toLowerCase().includes(term.toLowerCase())
    || productStatus.localizedName?.toLowerCase().includes(term.toLowerCase())
    ;
}

@Injectable({ providedIn: 'root' })
export class ProductStatusGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _productStatus$ = new BehaviorSubject<ProductStatusModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private productStatusList: any[];
  createResult: Subject<any> = new Subject<any>();

  private _state: State = {
    page: 1,
    pageSize: 5,
    searchTerm: '',
    sortColumn: '',
    sortDirection: '',
    startIndex: 0,
    endIndex: 9,
    totalRecords: 0
  };

  constructor(private pipe: DecimalPipe,
    public client: ProductStatusesClient
  ) {
    this.productStatusList = [];
    this.getAllProductStatus();
    this._search$.pipe(
      tap(() => this._loading$.next(true)),
      debounceTime(200),
      switchMap(() => this._search()),
      delay(200),
      tap(() => this._loading$.next(false))
    ).subscribe(result => {
      this._productStatus$.next(result.productStatus);
      this._total$.next(result.total);
    });

  }

  get productStatuss$() { return this._productStatus$.asObservable(); }
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
  set sortColumn(sortColumn: SortColumn) { this._set({ sortColumn }); }
  set sortDirection(sortDirection: SortDirection) { this._set({ sortDirection }); }
  set startIndex(startIndex: number) { this._set({ startIndex }); }
  set endIndex(endIndex: number) { this._set({ endIndex }); }
  set totalRecords(totalRecords: number) { this._set({ totalRecords }); }

  private _set(patch: Partial<State>) {
    Object.assign(this._state, patch);
    this._search$.next();
  }

  private _search(): Observable<SearchResult> {
    const { sortColumn, sortDirection, pageSize, page, searchTerm } = this._state;

    // 1. sort
    //TODO : Fill data instead []
    let productStatus = sort(this.productStatusList, sortColumn, sortDirection);

    // 2. filter
    productStatus = productStatus.filter(country => matches(country, searchTerm, this.pipe));
    const total = productStatus.length;

    // 3. paginate
    this._state.totalRecords = productStatus.length;
    this._state.startIndex = (page - 1) * this.pageSize + 1;
    this._state.endIndex = +((page - 1) * this.pageSize + this.pageSize);
    if (this.endIndex > this.totalRecords) {
      this._state.endIndex = this.totalRecords;
    }
    productStatus = productStatus.slice(this._state.startIndex - 1, this._state.endIndex);
    return of({ productStatus, total });
  }

  public getAllProductStatus() {
    this.client.getAllProductStatuses().subscribe(result => {
      debugger;
      this.productStatusList = result;
      this._search$.next();
    }, error => console.error(error));
  }
}
