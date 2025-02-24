import { OrderPaymentType } from './../../../../web-api-client';
import { Injectable, PipeTransform } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { DecimalPipe } from '@angular/common';
import { debounceTime, delay, switchMap, tap } from 'rxjs/operators';
import { ProductAttributesClient, OrdersClient } from 'src/app/web-api-client';
import { OrderPaymentModel } from './order-payment.model';



interface SearchResult {
  orderStatus: OrderPaymentModel[];
  state: number;
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



function matches(orderPayment: OrderPaymentModel, term: string, pipe: PipeTransform) {
  return (
    (orderPayment.amount && orderPayment.amount.toString().toLowerCase().includes(term.toLowerCase())) ||
    (orderPayment.authority && orderPayment.authority.toLowerCase().includes(term.toLowerCase()))
  );

}

@Injectable({ providedIn: "root" })
export class OrderPaymentGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _orderStatus$ = new BehaviorSubject<OrderPaymentModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private orderStateList: any[];
  createResult: Subject<any> = new Subject<any>();

  private _state: State = {
    page: 1,
    pageSize: 5,
    searchTerm: "",
    // sortColumn: '',
    // sortDirection: '',
    startIndex: 0,
    endIndex: 9,
    totalRecords: 0,
  };

  constructor(private pipe: DecimalPipe, public client: OrdersClient) {
    this.orderStateList = [];
    // this.getAllproductKinds();
    this._search$
      .pipe(
        tap(() => this._loading$.next(true)),
        debounceTime(200),
        switchMap(() => this._search()),
        delay(200),
        tap(() => this._loading$.next(false))
      )
      .subscribe((result) => {
        this._orderStatus$.next(result.orderStatus);
        this._total$.next(result.state);
      });
  }

  get orderStatus$() {
    return this._orderStatus$.asObservable();
  }
  get total$() {
    return this._total$.asObservable();
  }
  get loading$() {
    return this._loading$.asObservable();
  }
  get page() {
    return this._state.page;
  }
  get pageSize() {
    return this._state.pageSize;
  }
  get searchTerm() {
    return this._state.searchTerm;
  }
  get startIndex() {
    return this._state.startIndex;
  }
  get endIndex() {
    return this._state.endIndex;
  }
  get totalRecords() {
    return this._state.totalRecords;
  }

  set page(page: number) {
    this._set({ page });
  }
  set pageSize(pageSize: number) {
    this._set({ pageSize });
  }
  set searchTerm(searchTerm: string) {
    this._set({ searchTerm });
  }
  //   set sortColumn(sortColumn: SortColumn) { this._set({ sortColumn }); }
  //   set sortDirection(sortDirection: SortDirection) { this._set({ sortDirection }); }
  set startIndex(startIndex: number) {
    this._set({ startIndex });
  }
  set endIndex(endIndex: number) {
    this._set({ endIndex });
  }
  set totalRecords(totalRecords: number) {
    this._set({ totalRecords });
  }

  private _set(patch: Partial<State>) {
    Object.assign(this._state, patch);
    this._search$.next();
  }

  private _search(): Observable<SearchResult> {
    const { pageSize, page, searchTerm } = this._state;

    // 1. sort
    //TODO : Fill data instead []
    // let orderStatus = sort(this.optionColorList, sortColumn, sortDirection);
    let orderStatus = this.orderStateList;
    // 2. filter
    orderStatus = orderStatus.filter((orderStatus) =>
      matches(orderStatus, searchTerm, this.pipe)
    );
    const state = orderStatus.length;

    // 3. paginate
    this._state.totalRecords = orderStatus.length;
    this._state.startIndex = (page - 1) * this.pageSize + 1;
    this._state.endIndex = (page - 1) * this.pageSize + this.pageSize;
    if (this.endIndex > this.totalRecords) {
      this._state.endIndex = this.totalRecords;
    }
    orderStatus = orderStatus.slice(
      this._state.startIndex - 1,
      this._state.endIndex
    );
    return of({ orderStatus, state });
  }

  public getOrderPaymentsByOrderId(id: number) {
    debugger;
    this.client.getOrderStatesByOrderId(id).subscribe(
      (result) => {
        this.orderStateList = result;
        this._search$.next();
      },
      (error) => console.error(error)
    );
  }
}
