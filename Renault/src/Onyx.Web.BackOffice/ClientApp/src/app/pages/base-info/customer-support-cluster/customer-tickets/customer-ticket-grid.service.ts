import { Injectable, PipeTransform } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { DecimalPipe } from '@angular/common';
import { debounceTime, delay, distinctUntilChanged, map, switchMap, takeUntil, tap } from 'rxjs/operators';
import { FilteredCustomerTicketDto, CustomerTicketDto, CustomerTicketsClient } from 'src/app/web-api-client';
import { SortColumn, SortDirection } from './customer-ticket-sortable.directive';
import { CustomerTicketModel } from './customer-ticket.model';
import { forEach } from 'lodash';
import * as momentJalaali from "moment-jalaali";



class SearchResult {
  customerTickets!: CustomerTicketModel[];
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
export class CustomerTicketGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _customerTickets$ = new BehaviorSubject<CustomerTicketModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private customerTicketList!: any[];
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

  constructor(private pipe: DecimalPipe, public client: CustomerTicketsClient) {
    debugger;
    this._search$.pipe(
      tap(() => this._loading$.next(true)),
      debounceTime(200),
      switchMap(() => this._search()),
      tap(() => this._loading$.next(false)),
    ).pipe().subscribe(result => {
      debugger;
      this._customerTickets$.next(result.customerTickets);
      this._total$.next(result.total);
    });

    this._search$.next();
  }

  get customerTickets$() { return this._customerTickets$.asObservable(); }
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
    return this.client.getCustomerTicketsWithPagination(page, pageSize, sortColumn, sortDirection, searchTerm)
      .pipe(
        map((result: FilteredCustomerTicketDto) => {
          if (!result) {
            return { customerTickets: [], total: 0 };
          }
          const customerTickets = result.customerTickets || [];
          const total = result.count || 0;
          debugger;
          // Update your state properties as needed
          this.totalRecords = total;
          this.startIndex = ((page - 1) * pageSize) + 1;
          let end = result.customerTickets!.length + (page-1) * pageSize;
          this.endIndex = Math.min(end, page * pageSize);
          this.pageSize = pageSize;
          let searchResult = new SearchResult();
          searchResult.customerTickets = customerTickets.map(c => this.mapCustomerTicketDtoToCustomerTicketModel(c));
          searchResult.total = total;
          return  searchResult;
        }),
      );
  }

  

  public refreshCustomerTickets(){
    this._search$.next();
  }
  
  private mapCustomerTicketDtoToCustomerTicketModel(dto: CustomerTicketDto): CustomerTicketModel {
    momentJalaali.loadPersian(/*{ usePersianDigits: true }*/);
    return {
        id: dto.id || 0,
        subject: dto.subject ?? '',
        message: dto.message || '',
        date: momentJalaali(dto.date).format("jD jMMMM jYYYY"),
        customerPhoneNumber: dto.customerPhoneNumber || '',
        customerName: dto.customerName || '',
        isActive: dto.isActive || false
    };
}
}
