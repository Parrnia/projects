

import { Injectable, PipeTransform } from '@angular/core';

import { BehaviorSubject, Observable, of, Subject } from 'rxjs';


import { DecimalPipe } from '@angular/common';
import { debounceTime, delay, switchMap, tap } from 'rxjs/operators';

import { CustomerTypeModel } from './customer-type.model';
import { SortColumn, SortDirection } from './customer-type-sortable.directive';
import { CustomerTypeDto, CustomerTypesClient,  } from 'src/app/web-api-client';
import { br } from '@fullcalendar/core/internal-common';
import { ImageService } from 'src/app/core/services/image.service';



interface SearchResult {
  customerType: CustomerTypeModel[];
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

function sort(customerTypes: CustomerTypeModel[], column: SortColumn, direction: string): CustomerTypeModel[] {
  if (direction === '' || column === '') {
    return customerTypes;
  } else {
    return [...customerTypes].sort((a, b) => {
      const res = compare(a[column], b[column]);
      return direction === 'asc' ? res : -res;
    });
  }
}

function matches(customerType: CustomerTypeModel, term: string, pipe: PipeTransform) {
  return customerType.customerTypeEnumName?.toLowerCase().includes(term.toLowerCase())
    || customerType.discountPercent?.toString().toLowerCase().includes(term.toLowerCase())
;
}

@Injectable({ providedIn: 'root' })
export class CustomerTypeGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _customerType$ = new BehaviorSubject<CustomerTypeModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private customerTypeList: any[];
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
    public client: CustomerTypesClient,
    public imageService: ImageService
  ) {
    this.customerTypeList = [];
    this.getAllCustomerType();
    this._search$.pipe(
      tap(() => this._loading$.next(true)),
      debounceTime(200),
      switchMap(() => this._search()),
      delay(200),
      tap(() => this._loading$.next(false))
    ).subscribe(result => {
      this._customerType$.next(result.customerType);
      this._total$.next(result.total);
    });

  }

  get customerTypes$() { return this._customerType$.asObservable(); }
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
    debugger;
    this._search$.next();
  }

  private _search(): Observable<SearchResult> {
    const { sortColumn, sortDirection, pageSize, page, searchTerm } = this._state;

    // 1. sort
    //TODO : Fill data instead []
    let customerType = sort(this.customerTypeList, sortColumn, sortDirection);

    // 2. filter
    customerType = customerType.filter(country => matches(country, searchTerm, this.pipe));
    const total = customerType.length;

    // 3. paginate
    this._state.totalRecords = customerType.length;
    this._state.startIndex = (page - 1) * this.pageSize + 1;
    this._state.endIndex = +((page - 1) * this.pageSize + this.pageSize);
    if (this.endIndex > this.totalRecords) {
      this._state.endIndex = this.totalRecords;
    }
    customerType = customerType.slice(this._state.startIndex - 1, this._state.endIndex);
    return of({ customerType, total });
  }

  public getAllCustomerType() {
    this.client.getAllCustomerTypes().subscribe(result => {
      this.customerTypeList = result;
      this._search$.next();
    }, error => console.error(error));
  }
}
