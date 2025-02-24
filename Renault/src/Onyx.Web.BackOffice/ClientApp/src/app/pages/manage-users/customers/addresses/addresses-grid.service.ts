import { Injectable, PipeTransform } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { DecimalPipe } from '@angular/common';
import { debounceTime, delay, switchMap, tap } from 'rxjs/operators';
import { AddressesModel } from './addresses.model';
// import { SortColumn, SortDirection } from './addresses-sortable.directive';
import { AddressesClient, ProductTypesClient } from 'src/app/web-api-client';
import { ImageService } from 'src/app/core/services/image.service';



interface SearchResult {
  addresses: AddressesModel[];
  total: number;
}

interface State {
  page: number;
  pageSize: number;
  searchTerm: string;
  //   sortColumn: SortColumn;
  //   sortDirection: SortDirection;
  startIndex: number;
  endIndex: number;
  totalRecords: number;
}

@Injectable({ providedIn: 'root' })
export class AddressesGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _addresses$ = new BehaviorSubject<AddressesModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private addressesList: any[];
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

  constructor(
    private pipe: DecimalPipe, 
    public client: AddressesClient
  ) {
    this.addressesList = [];
    // this.getAlladdresses();
    this._search$.pipe(
      tap(() => this._loading$.next(true)),
      debounceTime(200),
      switchMap(() => this._search()),
      delay(200),
      tap(() => this._loading$.next(false))
    ).subscribe(result => {
      this._addresses$.next(result.addresses);
      this._total$.next(result.total);
    });
  }

  get addresses$() { return this._addresses$.asObservable(); }
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
    // let addresses = sort(this.addressesList, sortColumn, sortDirection);
    let addresses = this.addressesList;
    // 2. filter
    //addresses = addresses.filter(addresses => matches(addresses, searchTerm, this.pipe));
    const total = addresses.length;

    // 3. paginate
    this._state.totalRecords = addresses.length;
    this._state.startIndex = (page - 1) * this.pageSize + 1;
    this._state.endIndex = (page - 1) * this.pageSize + this.pageSize;
    if (this.endIndex > this.totalRecords) {
      this._state.endIndex = this.totalRecords;
    }
    addresses = addresses.slice(this._state.startIndex - 1, this._state.endIndex);
    return of({ addresses, total });
  }

  public getAlladdresses(customerId: string) {
    this.client.getAddressesByCustomerId(customerId).subscribe(result => {
      this.addressesList = result;
      this._search$.next();
    }, error => console.error(error));
  }


}
