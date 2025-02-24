import { Injectable, PipeTransform } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { DecimalPipe } from '@angular/common';
import { debounceTime, delay, distinctUntilChanged, map, switchMap, takeUntil, tap } from 'rxjs/operators';
import { FilteredCreditDto, CreditsClient } from 'src/app/web-api-client';
import { CreditModel } from './credit.model';



class SearchResult {
  credits!: CreditModel[];
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
export class CreditGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _credits$ = new BehaviorSubject<CreditModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private creditsList!: any[];
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
      this._credits$.next(result.credits);
      this._total$.next(result.total);
    });

    //this._search$.next();
  }

  get credits$() { return this._credits$.asObservable(); }
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
    return this.client.getCreditsByCustomerIdWithPagination(page, pageSize, searchTerm, this._customerId)
      .pipe(
        map((result: FilteredCreditDto) => {
          if (!result) {
            return { credits: [], total: 0 };
          }
  
          const credits = result.credits || [];
          const total = result.count || 0;
          debugger;
          this.totalRecords = total;
          this.startIndex = ((page - 1) * pageSize) + 1;
          let end = result.credits!.length + (page-1) * pageSize;
          this.endIndex = Math.min(end, page * pageSize);
          this.pageSize = pageSize;
          let searchResult = new SearchResult();
          searchResult.credits = credits as any;
          searchResult.total = total;
          return  searchResult;
        }),
      );
  }

  

  public refreshCredits(customerId: string){
    this._customerId = customerId;
    this._search$.next();
  }
  
}
