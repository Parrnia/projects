import { Injectable, PipeTransform } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { DecimalPipe } from '@angular/common';
import { debounceTime, delay, switchMap, tap } from 'rxjs/operators';
import { ProductAttributesClient, OrdersClient, } from 'src/app/web-api-client';
import { OptionModel } from './option.model';



interface SearchResult {
  option: OptionModel[];
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



function matches(option: OptionModel, term: string, pipe: PipeTransform) {

  return option.name.toLowerCase().includes(term.toLowerCase())

    ;

}

@Injectable({ providedIn: 'root' })
export class OptionGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _option$ = new BehaviorSubject<OptionModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private optionColorList: any[];
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
    , public client: OrdersClient
  ) {
    this.optionColorList = [];
    // this.getAllproductKinds();
    this._search$.pipe(
      tap(() => this._loading$.next(true)),
      debounceTime(200),
      switchMap(() => this._search()),
      delay(200),
      tap(() => this._loading$.next(false))
    ).subscribe(result => {
      this._option$.next(result.option);
      this._total$.next(result.total);
    });

  }

  get option$() { return this._option$.asObservable(); }
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
    // let option = sort(this.optionColorList, sortColumn, sortDirection);
    let option = this.optionColorList;
    // 2. filter
    option = option.filter(option => matches(option, searchTerm, this.pipe));
    const total = option.length;

    // 3. paginate
    this._state.totalRecords = option.length;
    this._state.startIndex = (page - 1) * this.pageSize + 1;
    this._state.endIndex = (page - 1) * this.pageSize + this.pageSize;
    if (this.endIndex > this.totalRecords) {
      this._state.endIndex = this.totalRecords;
    }
    option = option.slice(this._state.startIndex - 1, this._state.endIndex);
    return of({ option, total });
  }

  public getAllOptionByOrderItemId(id: number) {
    this.client.getOrderItemOptionsByOrderItemId(id).subscribe(result => {
      console.log('this.client.getOptionByOrderId', result)
      this.optionColorList = result;
      this._search$.next();
    }, error => console.error(error));

  }


}
