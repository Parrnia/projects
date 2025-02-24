import { Injectable, PipeTransform } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { DecimalPipe } from '@angular/common';
import { debounceTime, delay, switchMap, tap } from 'rxjs/operators';
// import { SortColumn, SortDirection } from './addresses-sortable.directive';
import { ImageService } from 'src/app/core/services/image.service';
import { LocationAddressModel } from './location-address.model';
import { CorporationInfosClient } from 'src/app/web-api-client';



interface SearchResult {
  locationAddresses: LocationAddressModel[];
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
export class LocationAddressGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _locationAddresses$ = new BehaviorSubject<LocationAddressModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private locationAddressesList: any[];
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
    this.locationAddressesList = [];
    // this.getAlllocationAddresses();
    this._search$.pipe(
      tap(() => this._loading$.next(true)),
      debounceTime(200),
      switchMap(() => this._search()),
      delay(200),
      tap(() => this._loading$.next(false))
    ).subscribe(result => {
      this._locationAddresses$.next(result.locationAddresses);
      this._total$.next(result.total);
    });
  }

  get locationAddresses$() { return this._locationAddresses$.asObservable(); }
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
    // let locationAddresses = sort(this.locationAddressesList, sortColumn, sortDirection);
    let locationAddresses = this.locationAddressesList;
    // 2. filter
    //locationAddresses = locationAddresses.filter(locationAddresses => matches(locationAddresses, searchTerm, this.pipe));
    const total = locationAddresses.length;

    // 3. paginate
    this._state.totalRecords = locationAddresses.length;
    this._state.startIndex = (page - 1) * this.pageSize + 1;
    this._state.endIndex = (page - 1) * this.pageSize + this.pageSize;
    if (this.endIndex > this.totalRecords) {
      this._state.endIndex = this.totalRecords;
    }
    locationAddresses = locationAddresses.slice(this._state.startIndex - 1, this._state.endIndex);
    debugger;
    return of({ locationAddresses, total });
  }

  public setLocationAddresses(locationAddresses: string[]) {
    debugger;
      this.locationAddressesList = locationAddresses.map(location => ({ locationAddress: location }));;
      this._search$.next();
  }


}
