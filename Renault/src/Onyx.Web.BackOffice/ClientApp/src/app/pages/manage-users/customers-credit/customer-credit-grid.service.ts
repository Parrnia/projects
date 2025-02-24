import { Injectable, PipeTransform } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { DecimalPipe } from '@angular/common';
import { debounceTime, delay, distinctUntilChanged, map, switchMap, takeUntil, tap, first } from 'rxjs/operators';
import { CustomersClient } from 'src/app/web-api-client';
import { SortColumn, SortDirection } from './customer-credit-sortable.directive';
import { CustomerCreditModel } from './customer-credit.model';
import { AuthenticationService } from 'src/app/core/services/authService/auth.service';
import { FilteredUser } from 'src/app/core/services/authService/models/entities/FilteredUser';



class SearchResult {
  customerCredits!: CustomerCreditModel[];
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
export class CustomerCreditGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _customerCredits$ = new BehaviorSubject<CustomerCreditModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private customerCreditsList!: any[];
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

  constructor(
    private pipe: DecimalPipe,
    public client: AuthenticationService,
    private customersClient: CustomersClient) {
    debugger;
    this._search$.pipe(
      tap(() => this._loading$.next(true)),
      debounceTime(200),
      switchMap(() => this._search()),
      tap(() => this._loading$.next(false)),
    ).pipe().subscribe(result => {
      debugger;
      this._customerCredits$.next(result.customerCredits);
      this._total$.next(result.total);
    });

    this._search$.next();
  }

  get customerCredits$() { return this._customerCredits$.asObservable(); }
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
  
    return this.client.getStoreCustomersWithPagination(page, pageSize, sortColumn, sortDirection, searchTerm)
      .pipe(
        switchMap((result: FilteredUser) => {
          if (!result) {
            return of({ customerCredits: [], total: 0 } as SearchResult); // Return an empty SearchResult
          }
  
          // Pass the result to the second API call
          return this.customersClient.getCustomersByIds(result.users?.map(c => c.id)).pipe(
            map(customerDtos => {
              const customerCredits = result.users || [];
              const total = result.count || 0;
              
              

              this.totalRecords = total;
              this.startIndex = ((page - 1) * pageSize) + 1;
              let end = result.users!.length + (page - 1) * pageSize;
              this.endIndex = Math.min(end, page * pageSize);
              this.pageSize = pageSize;
  
              let searchResult = new SearchResult();
              searchResult.customerCredits = customerCredits as any;
              searchResult.customerCredits.forEach(c => {
                c.credit = 0;
                c.maxCredit = 0;
                customerDtos.forEach(e => {
                  if(e.id === c.id){
                    c.credit = e.credits == undefined || e.credits.length == 0 ? 0 : e.credits[e.credits.length - 1].value ?? 0;
                    c.maxCredit = e.maxCredits == undefined || e.maxCredits.length == 0 ? 0 : e.maxCredits[e.maxCredits.length - 1].value ?? 0;
                  }
                })
              });
              searchResult.total = total;
  
              return searchResult;
            })
          );
        })
      );
  }

  

  public refreshCustomerCredits(){
    this._search$.next();
  }
  
}
