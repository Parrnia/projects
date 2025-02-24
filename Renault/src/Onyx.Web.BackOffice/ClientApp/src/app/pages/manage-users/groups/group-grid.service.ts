

import { Injectable, PipeTransform } from '@angular/core';

import { BehaviorSubject, Observable, of, Subject } from 'rxjs';


import { DecimalPipe } from '@angular/common';
import { debounceTime, delay, switchMap, tap } from 'rxjs/operators';

import { GroupModel } from './group.model';
import { SortColumn, SortDirection } from './group-sortable.directive';
import { ImageService } from 'src/app/core/services/image.service';
import { AuthenticationService } from 'src/app/core/services/authService/auth.service';



interface SearchResult {
  group: GroupModel[];
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

const compare = (v1: string | number, v2: string | number) => v1 < v2 ? -1 : v1 > v2 ? 1 : 0;

function sort(groups: GroupModel[], column: SortColumn, direction: string): GroupModel[] {
  if (direction === '' || column === '') {
    return groups;
  } else {
    return [...groups].sort((a, b) => {
      const res = compare(a[column], b[column]);
      return direction === 'asc' ? res : -res;
    });
  }
}

function matches(group: GroupModel, term: string, pipe: PipeTransform) {
  return group.name?.toLowerCase().includes(term.toLowerCase())
    || group.creationDate?.toString().toLowerCase().includes(term.toLowerCase())
    ;

}

@Injectable({ providedIn: 'root' })
export class GroupGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _group$ = new BehaviorSubject<GroupModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private groupList: any[];
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
    public client: AuthenticationService,
    public imageService: ImageService
  ) {
    this.groupList = [];
    this.getAllGroup();
    this._search$.pipe(
      tap(() => this._loading$.next(true)),
      debounceTime(200),
      switchMap(() => this._search()),
      delay(200),
      tap(() => this._loading$.next(false))
    ).subscribe(result => {
      this._group$.next(result.group);
      this._total$.next(result.total);
    });

  }

  get groups$() { return this._group$.asObservable(); }
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
    let group = sort(this.groupList, sortColumn, sortDirection);

    // 2. filter
    group = group.filter(country => matches(country, searchTerm, this.pipe));
    const total = group.length;

    // 3. paginate
    this._state.totalRecords = group.length;
    this._state.startIndex = (page - 1) * this.pageSize + 1;
    this._state.endIndex = +((page - 1) * this.pageSize + this.pageSize);
    if (this.endIndex > this.totalRecords) {
      this._state.endIndex = this.totalRecords;
    }
    group = group.slice(this._state.startIndex - 1, this._state.endIndex);
    return of({ group, total });
  }

  public getAllGroup() {
    this.client.getGroups().subscribe(result => {
      debugger;
      this.groupList = result;
      this._search$.next();
    }, error => console.error(error));
  }
}
