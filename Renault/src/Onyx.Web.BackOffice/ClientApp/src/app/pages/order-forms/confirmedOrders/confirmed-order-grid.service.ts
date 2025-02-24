import { Injectable, PipeTransform } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { DecimalPipe } from '@angular/common';
import { debounceTime, delay, distinctUntilChanged, map, switchMap, takeUntil, tap } from 'rxjs/operators';
import { FilteredOrderDto, OrdersClient } from 'src/app/web-api-client';

import { OrderModel } from '../order.model';
import { SortColumn, SortDirection } from './confirmed-order-sortable.directive';



class SearchResult {
  orders!: OrderModel[];
  total!: number;
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


@Injectable({ providedIn: 'root' })
export class ConfirmedOrderGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _orders$ = new BehaviorSubject<OrderModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private ordersList!: any[];
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

  constructor(private pipe: DecimalPipe, public client: OrdersClient) {
    debugger;
    this._search$.pipe(
      tap(() => this._loading$.next(true)),
      debounceTime(200),
      switchMap(() => this._search()),
      tap(() => this._loading$.next(false)),
    ).pipe().subscribe(result => {
      debugger;
      this._orders$.next(result.orders);
      this._total$.next(result.total);
    });

    this._search$.next();
  }

  get orders$() { return this._orders$.asObservable(); }
  get total$() { return this._total$.asObservable(); }
  get loading$() { return this._loading$.asObservable(); }
  get page() { return this._state.page; }
  get pageSize() { return this._state.pageSize; }
  get searchTerm() { return this._state.searchTerm; }
  get startIndex() { return this._state.startIndex; }
  get endIndex() { return this._state.endIndex; }
  get totalRecords() { return this._state.totalRecords; }

  set page(page: number) { this._set({ page });  }
  set pageSize(pageSize: number) { this._state.pageSize = pageSize }
  set searchTerm(searchTerm: string) { this._set({ searchTerm }); }
  set sortColumn(sortColumn: SortColumn) { this._set({ sortColumn }); }
  set sortDirection(sortDirection: SortDirection) { this._set({ sortDirection }); }
  set startIndex(startIndex: number) { this._state.startIndex = startIndex }
  set endIndex(endIndex: number) { this._state.endIndex = endIndex }
  set totalRecords(totalRecords: number) {  this._state.totalRecords = totalRecords }

  private _set(patch: Partial<State>) {
    Object.assign(this._state, patch);
    this._search$.next();
  }

  private _search(): Observable<SearchResult> {
    const { sortColumn, sortDirection, pageSize, page, searchTerm } = this._state;
    debugger;
    return this.client.getConfirmedOrders(page, pageSize, sortColumn, sortDirection, searchTerm)
      .pipe(
        map((result: FilteredOrderDto) => {
          if (!result) {
            // Handle the case when the result is undefined or null
            return { orders: [], total: 0 };
          }
  
          const orders = result.orders || [];
          const total = result.count || 0;
          debugger;
          // Update your state properties as needed
          this.totalRecords = total;
          this.startIndex = ((page - 1) * pageSize) + 1;
          let end = result.orders!.length + (page-1) * pageSize;
          this.endIndex = Math.min(end, page * pageSize);
          this.pageSize = pageSize;
          let searchResult = new SearchResult();
          searchResult.orders = orders as any;
          searchResult.total = total;
          return  searchResult;
        }),
      );
  }
  public refreshOrders(){
    this._search$.next();
  }
}