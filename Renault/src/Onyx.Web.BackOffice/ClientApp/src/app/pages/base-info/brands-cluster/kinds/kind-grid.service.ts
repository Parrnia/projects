

import { Injectable, PipeTransform } from '@angular/core';

import { BehaviorSubject, Observable, of, Subject } from 'rxjs';


import { DecimalPipe } from '@angular/common';
import { debounceTime, delay, switchMap, tap } from 'rxjs/operators';


import { SortColumn, SortDirection } from './kind-sortable.directive';
import { KindDto, KindsClient,  } from 'src/app/web-api-client';
import { br } from '@fullcalendar/core/internal-common';
import { ImageService } from 'src/app/core/services/image.service';
import { KindModel } from './kind.model';



interface SearchResult {
  kind: KindModel[];
  total: number;
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

const compare = (v1: string | number | boolean, v2: string | number | boolean) => v1 < v2 ? -1 : v1 > v2 ? 1 : 0;

function sort(kinds: KindModel[], column: SortColumn, direction: string): KindModel[] {
  if (direction === '' || column === '') {
    return kinds;
  } else {
    return [...kinds].sort((a, b) => {
      const res = compare(a[column], b[column]);
      return direction === 'asc' ? res : -res;
    });
  }
}

function matches(kind: KindModel, term: string, pipe: PipeTransform) {
  return kind.localizedName?.toLowerCase().includes(term.toLowerCase())
    || kind.name?.toLowerCase().includes(term.toLowerCase())
    || kind.modelName?.toLowerCase().includes(term.toLowerCase())
    || kind.modelId?.toString().toLowerCase().includes(term.toLowerCase());
}

@Injectable({ providedIn: 'root' })
export class KindGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _kind$ = new BehaviorSubject<KindModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private kindList: any[];
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

  constructor(private pipe: DecimalPipe,
    public client: KindsClient
  ) {
    this.kindList = [];
    this.getAllKind();
    this._search$.pipe(
      tap(() => this._loading$.next(true)),
      debounceTime(200),
      switchMap(() => this._search()),
      delay(200),
      tap(() => this._loading$.next(false))
    ).subscribe(result => {
      this._kind$.next(result.kind);
      this._total$.next(result.total);
    });

  }

  get kinds$() { return this._kind$.asObservable(); }
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
  set sortColumn(sortColumn: SortColumn) { this._set({ sortColumn }); }
  set sortDirection(sortDirection: SortDirection) { this._set({ sortDirection }); }
  set startIndex(startIndex: number) { this._set({ startIndex }); }
  set endIndex(endIndex: number) { this._set({ endIndex }); }
  set totalRecords(totalRecords: number) { this._set({ totalRecords }); }

  private _set(patch: Partial<State>) {
    Object.assign(this._state, patch);
    this._search$.next();
  }

  private _search(): Observable<SearchResult> {
    const { sortColumn, sortDirection, pageSize, page, searchTerm } = this._state;

    // 1. sort
    //TODO : Fill data instead []
    let kind = sort(this.kindList, sortColumn, sortDirection);

    // 2. filter
    kind = kind.filter(country => matches(country, searchTerm, this.pipe));
    const total = kind.length;

    // 3. paginate
    this._state.totalRecords = kind.length;
    this._state.startIndex = (page - 1) * this.pageSize + 1;
    this._state.endIndex = +((page - 1) * this.pageSize + this.pageSize);
    if (this.endIndex > this.totalRecords) {
      this._state.endIndex = this.totalRecords;
    }
    kind = kind.slice(this._state.startIndex - 1, this._state.endIndex);
    return of({ kind, total });
  }

  public getAllKind() {
    this.client.getAllKinds().subscribe(result => {
      this.kindList = result;
      this._search$.next();
      debugger;
    }, error => console.error(error));
  }
}
