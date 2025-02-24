

import { Injectable, PipeTransform } from '@angular/core';

import { BehaviorSubject, Observable, of, Subject } from 'rxjs';


import { DecimalPipe } from '@angular/common';
import { debounceTime, delay, switchMap, tap } from 'rxjs/operators';

import { ModelModel } from './model.model';
import { SortColumn, SortDirection } from './model-sortable.directive';
import { ModelDto, ModelsClient,  } from 'src/app/web-api-client';
import { br } from '@fullcalendar/core/internal-common';
import { ImageService } from 'src/app/core/services/image.service';



interface SearchResult {
  model: ModelModel[];
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


function sort(models: ModelModel[], column: SortColumn, direction: string): ModelModel[] {
  if (direction === '' || column === '') {
    return models;
  } else {
    return [...models].sort((a, b) => {
      const res = compare(a[column], b[column]);
      return direction === 'asc' ? res : -res;
    });
  }
}

function matches(model: ModelModel, term: string, pipe: PipeTransform) {
  return model.localizedName?.toLowerCase().includes(term.toLowerCase())
    || model.name?.toLowerCase().includes(term.toLowerCase())
    || model.familyName?.toLowerCase().includes(term.toLowerCase())
    || model.familyId?.toString().toLowerCase().includes(term.toLowerCase());
}

@Injectable({ providedIn: 'root' })
export class ModelGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _model$ = new BehaviorSubject<ModelModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private modelList: any[];
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
    public client: ModelsClient
  ) {
    this.modelList = [];
    this.getAllModel();
    this._search$.pipe(
      tap(() => this._loading$.next(true)),
      debounceTime(200),
      switchMap(() => this._search()),
      delay(200),
      tap(() => this._loading$.next(false))
    ).subscribe(result => {
      this._model$.next(result.model);
      this._total$.next(result.total);
    });

  }

  get models$() { return this._model$.asObservable(); }
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
    let model = sort(this.modelList, sortColumn, sortDirection);

    // 2. filter
    model = model.filter(country => matches(country, searchTerm, this.pipe));
    const total = model.length;

    // 3. paginate
    this._state.totalRecords = model.length;
    this._state.startIndex = (page - 1) * this.pageSize + 1;
    this._state.endIndex = +((page - 1) * this.pageSize + this.pageSize);
    if (this.endIndex > this.totalRecords) {
      this._state.endIndex = this.totalRecords;
    }
    model = model.slice(this._state.startIndex - 1, this._state.endIndex);
    return of({ model, total });
  }

  public getAllModel() {
    this.client.getAllModels().subscribe(result => {
      this.modelList = result;
      this._search$.next();

      debugger;
    }, error => console.error(error));
  }
}
