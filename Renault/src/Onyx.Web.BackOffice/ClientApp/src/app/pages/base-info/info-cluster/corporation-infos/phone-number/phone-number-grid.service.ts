import { Injectable, PipeTransform } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { DecimalPipe } from '@angular/common';
import { debounceTime, delay, switchMap, tap } from 'rxjs/operators';
// import { SortColumn, SortDirection } from './addresses-sortable.directive';
import { ImageService } from 'src/app/core/services/image.service';
import { PhoneNumberModel } from './phone-number.model';
import { CorporationInfosClient } from 'src/app/web-api-client';



interface SearchResult {
  phoneNumbers: PhoneNumberModel[];
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
export class PhoneNumberGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _phoneNumbers$ = new BehaviorSubject<PhoneNumberModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private phoneNumbersList: any[];
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
    public client: CorporationInfosClient
  ) {
    this.phoneNumbersList = [];
    // this.getAllphoneNumbers();
    this._search$.pipe(
      tap(() => this._loading$.next(true)),
      debounceTime(200),
      switchMap(() => this._search()),
      delay(200),
      tap(() => this._loading$.next(false))
    ).subscribe(result => {
      this._phoneNumbers$.next(result.phoneNumbers);
      this._total$.next(result.total);
    });
  }

  get phoneNumbers$() { return this._phoneNumbers$.asObservable(); }
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
    // let phoneNumbers = sort(this.phoneNumbersList, sortColumn, sortDirection);
    let phoneNumbers = this.phoneNumbersList;
    // 2. filter
    //phoneNumbers = phoneNumbers.filter(phoneNumbers => matches(phoneNumbers, searchTerm, this.pipe));
    const total = phoneNumbers.length;

    // 3. paginate
    this._state.totalRecords = phoneNumbers.length;
    this._state.startIndex = (page - 1) * this.pageSize + 1;
    this._state.endIndex = (page - 1) * this.pageSize + this.pageSize;
    if (this.endIndex > this.totalRecords) {
      this._state.endIndex = this.totalRecords;
    }
    phoneNumbers = phoneNumbers.slice(this._state.startIndex - 1, this._state.endIndex);
    debugger;
    return of({ phoneNumbers, total });
  }

  public setPhoneNumbers(phoneNumbers: string[]) {
    debugger;
      this.phoneNumbersList = phoneNumbers.map(phoneNumber => ({ phoneNumber: phoneNumber }));;
      this._search$.next();
  }


}
