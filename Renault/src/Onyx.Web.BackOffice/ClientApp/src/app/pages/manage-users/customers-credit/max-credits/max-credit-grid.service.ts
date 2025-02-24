import { Injectable, PipeTransform } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { DecimalPipe } from '@angular/common';
import { debounceTime, delay, distinctUntilChanged, map, switchMap, takeUntil, tap } from 'rxjs/operators';
import { FilteredCreditDto, CreditsClient, FilteredMaxCreditDto } from 'src/app/web-api-client';
import { MaxCreditModel } from './max-credit.model';



class SearchResult {
  maxCredits!: MaxCreditModel[];
  total!: number;
}

interface State {
  page: number;
  pageSize: number;
  searchTerm: string;
  startIndex: number;
  endIndex: number;
  totalRecords: number;
}


@Injectable({ providedIn: 'root' })
export class MaxCreditGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _maxCredits$ = new BehaviorSubject<MaxCreditModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private maxCreditsList!: any[];
  createResult: Subject<any> = new Subject<any>();
  private _customerId = '';
  private _state: State = {
    page: 1,
    pageSize: 5,
    searchTerm: '',
    startIndex: 0,
    endIndex: 9,
    totalRecords: 0
  };

  constructor(private pipe: DecimalPipe, public client: CreditsClient) {
    debugger;
    this._search$.pipe(
      tap(() => this._loading$.next(true)),
      debounceTime(200),
      switchMap(() => this._search()),
      tap(() => this._loading$.next(false)),
    ).pipe().subscribe(result => {
      debugger;
      this._maxCredits$.next(result.maxCredits);
      this._total$.next(result.total);
    });

    //this._search$.next();
  }

  get maxCredits$() { return this._maxCredits$.asObservable(); }
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
  set startIndex(startIndex: number) { this._state.startIndex = startIndex }
  set endIndex(endIndex: number) { this._state.endIndex = endIndex }
  set totalRecords(totalRecords: number) {  this._state.totalRecords = totalRecords }

  private _set(patch: Partial<State>) {
    Object.assign(this._state, patch);
    this._search$.next();
  }

  private _search(): Observable<SearchResult> {
    const { pageSize, page, searchTerm } = this._state;
    debugger;
    return this.client.getMaxCreditsByCustomerIdWithPagination(page, pageSize, searchTerm, this._customerId)
      .pipe(
        map((result: FilteredMaxCreditDto) => {
          if (!result) {
            return { maxCredits: [], total: 0 };
          }
  
          const maxCredits = result.maxCredits || [];
          const total = result.count || 0;
          debugger;
          this.totalRecords = total;
          this.startIndex = ((page - 1) * pageSize) + 1;
          let end = result.maxCredits!.length + (page-1) * pageSize;
          this.endIndex = Math.min(end, page * pageSize);
          this.pageSize = pageSize;
          let searchResult = new SearchResult();
          searchResult.maxCredits = maxCredits as any;
          searchResult.total = total;
          return  searchResult;
        }),
      );
  }

  

  public refreshMaxCredits(customerId: string){
    this._customerId = customerId;
    this._search$.next();
  }
  
}
