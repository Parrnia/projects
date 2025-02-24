import { Injectable, PipeTransform } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { DecimalPipe } from '@angular/common';
import { debounceTime, delay, distinctUntilChanged, map, switchMap, takeUntil, tap } from 'rxjs/operators';
import { CustomersClient } from 'src/app/web-api-client';
import { SortColumn, SortDirection } from './customer-sortable.directive';
import { CustomerModel } from './customer.model';
import { AuthenticationService } from 'src/app/core/services/authService/auth.service';
import { FilteredUser } from 'src/app/core/services/authService/models/entities/FilteredUser';



class SearchResult {
  customers!: CustomerModel[];
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
export class CustomerGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _customers$ = new BehaviorSubject<CustomerModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private customersList!: any[];
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

  constructor(private pipe: DecimalPipe, public client: AuthenticationService) {
    debugger;
    this._search$.pipe(
      tap(() => this._loading$.next(true)),
      debounceTime(200),
      switchMap(() => this._search()),
      tap(() => this._loading$.next(false)),
    ).pipe().subscribe(result => {
      debugger;
      this._customers$.next(result.customers);
      this._total$.next(result.total);
    });

    this._search$.next();
  }

  get customers$() { return this._customers$.asObservable(); }
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
    return this.client.getCustomersWithPagination(page, pageSize, sortColumn, sortDirection, searchTerm)
      .pipe(
        map((result: FilteredUser) => {
          if (!result) {
            return { customers: [], total: 0 };
          }
  
          const customers = result.users || [];
          const total = result.count || 0;
          debugger;
          this.totalRecords = total;
          this.startIndex = ((page - 1) * pageSize) + 1;
          let end = result.users!.length + (page-1) * pageSize;
          this.endIndex = Math.min(end, page * pageSize);
          this.pageSize = pageSize;
          let searchResult = new SearchResult();
          searchResult.customers = customers as any;
          searchResult.total = total;
          return  searchResult;
        }),
      );
  }

  

  public refreshCustomers(){
    this._search$.next();
  }
  
}
