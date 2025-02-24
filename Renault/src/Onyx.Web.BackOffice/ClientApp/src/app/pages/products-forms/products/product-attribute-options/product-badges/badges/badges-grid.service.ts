import { Injectable, PipeTransform } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { DecimalPipe } from '@angular/common';
import { debounceTime, delay, switchMap, tap } from 'rxjs/operators';
import { BadgesClient } from 'src/app/web-api-client';
import { BadgesModel } from './badges.model';
// import { SortColumn, SortDirection } from './product-images-sortable.directive';



interface SearchResult {
  badges: BadgesModel[];
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


function matches(badges: BadgesModel, term: string, pipe: PipeTransform) {
  return badges.value.toString().toLowerCase().includes(term.toLowerCase())
    ;

}

@Injectable({ providedIn: 'root' })
export class BadgesGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _badges$ = new BehaviorSubject<BadgesModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private badgesList: any[];
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
    , public client: BadgesClient
  ) {
    this.badgesList = [];
    // this.getAllbadges();
    this._search$.pipe(
      tap(() => this._loading$.next(true)),
      debounceTime(200),
      switchMap(() => this._search()),
      delay(200),
      tap(() => this._loading$.next(false))
    ).subscribe(result => {
      this._badges$.next(result.badges);
      this._total$.next(result.total);
    });

  }

  get badges$() { return this._badges$.asObservable(); }
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
    // let badges = sort(this.badgesList, sortColumn, sortDirection);
    let badges = this.badgesList;
    // 2. filter
    badges = badges.filter(badges => matches(badges, searchTerm, this.pipe));
    const total = badges.length;

    // 3. paginate
    this._state.totalRecords = badges.length;
    this._state.startIndex = (page - 1) * this.pageSize + 1;
    this._state.endIndex = (page - 1) * this.pageSize + this.pageSize;
    if (this.endIndex > this.totalRecords) {
      this._state.endIndex = this.totalRecords;
    }
    badges = badges.slice(this._state.startIndex - 1, this._state.endIndex);
    return of({ badges, total });
  }

  public getAllBadges() {
    this.client.getAllBadges().subscribe(result => {
      this.badgesList = result;
      this._search$.next();
    }, error => console.error(error));

  }


}
