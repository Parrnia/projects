import { Injectable, PipeTransform } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { DecimalPipe } from '@angular/common';
import { debounceTime, delay, switchMap, tap } from 'rxjs/operators';
import { ProductAttributesClient, ReturnOrdersClient } from 'src/app/web-api-client';
import { ReturnOrderStateModel } from './return-order-state.model';



interface SearchResult {
  returnOrderStatus: ReturnOrderStateModel[];
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



function matches(returnOrderStatus: ReturnOrderStateModel, term: string, pipe: PipeTransform) {
  return returnOrderStatus.returnOrderStatusName.toLowerCase().includes(term.toLowerCase())
    || returnOrderStatus.details.toLowerCase().includes(term.toLowerCase());

}

@Injectable({ providedIn: 'root' })
export class ReturnOrderStateGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _returnOrderStatus$ = new BehaviorSubject<ReturnOrderStateModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private returnOrderStateList: any[];
  createResult: Subject<any> = new Subject<any>();

  private _state: State = {
    page: 1,
    pageSize: 5,
    searchTerm: '',
    // sortColumn: '',
    // sortDirection: '',
    startIndex: 0,
    endIndex: 9,
    totalRecords: 0
  };

  constructor(private pipe: DecimalPipe
    , public client: ReturnOrdersClient
  ) {
    this.returnOrderStateList = [];
    // this.getAllproductKinds();
    this._search$.pipe(
      tap(() => this._loading$.next(true)),
      debounceTime(200),
      switchMap(() => this._search()),
      delay(200),
      tap(() => this._loading$.next(false))
    ).subscribe(result => {
      this._returnOrderStatus$.next(result.returnOrderStatus);
      this._total$.next(result.state);
    });

  }

  get returnOrderStatus$() { return this._returnOrderStatus$.asObservable(); }
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
  //   set sortColumn(sortColumn: SortColumn) { this._set({ sortColumn }); }
  //   set sortDirection(sortDirection: SortDirection) { this._set({ sortDirection }); }
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
    // let returnOrderStatus = sort(this.optionColorList, sortColumn, sortDirection);
    let returnOrderStatus = this.returnOrderStateList;
    // 2. filter
    returnOrderStatus = returnOrderStatus.filter(returnOrderStatus => matches(returnOrderStatus, searchTerm, this.pipe));
    const state = returnOrderStatus.length;

    // 3. paginate
    this._state.totalRecords = returnOrderStatus.length;
    this._state.startIndex = (page - 1) * this.pageSize + 1;
    this._state.endIndex = (page - 1) * this.pageSize + this.pageSize;
    if (this.endIndex > this.totalRecords) {
      this._state.endIndex = this.totalRecords;
    }
    returnOrderStatus = returnOrderStatus.slice(this._state.startIndex - 1, this._state.endIndex);
    return of({ returnOrderStatus, state });
  }

  public getReturnOrderStatesByReturnOrderId(id: number) {
    debugger;
    this.client.getReturnOrderStatesByReturnOrderId(id).subscribe(result => {
      this.returnOrderStateList = result;
      this._search$.next();
    }, error => console.error(error));

  }


}
