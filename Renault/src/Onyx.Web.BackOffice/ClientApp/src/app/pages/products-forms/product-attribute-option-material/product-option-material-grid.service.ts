import { Injectable, PipeTransform } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { DecimalPipe } from '@angular/common';
import { debounceTime, delay, switchMap, tap } from 'rxjs/operators';
import { ProductOptionMaterialsClient } from 'src/app/web-api-client';
import { ProductOptionMaterialModel } from './product-option-material.model';
import { SortColumn, SortDirection } from './product-option-material-sortable.directive';



interface SearchResult {
  optionMaterial: ProductOptionMaterialModel[];
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

function sort(materials: ProductOptionMaterialModel[], column: SortColumn, direction: string): ProductOptionMaterialModel[] {
  if (direction === '' || column === '') {
    return materials;
  } else {
    return [...materials].sort((a, b) => {
      const res = compare(a[column], b[column]);
      return direction === 'asc' ? res : -res;
    });
  }
}

function matches(optionMaterial: ProductOptionMaterialModel, term: string, pipe: PipeTransform) {
  return optionMaterial.name.toLowerCase().includes(term.toLowerCase())
}

@Injectable({ providedIn: 'root' })
export class ProductOptionMaterialsGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _brandType$ = new BehaviorSubject<ProductOptionMaterialModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private optionMaterialsList: any[];
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

  constructor(private pipe: DecimalPipe
    , public client: ProductOptionMaterialsClient
  ) {
    this.optionMaterialsList = [];
    this.getAllOptionMaterials();
    this._search$.pipe(
      tap(() => this._loading$.next(true)),
      debounceTime(200),
      switchMap(() => this._search()),
      delay(200),
      tap(() => this._loading$.next(false))
    ).subscribe(result => {
      this._brandType$.next(result.optionMaterial);
      this._total$.next(result.total);
    });

  }

  get optionMaterial$() { return this._brandType$.asObservable(); }
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
    let optionMaterial = sort(this.optionMaterialsList, sortColumn, sortDirection);

    // 2. filter
    optionMaterial = optionMaterial.filter(optionMaterial => matches(optionMaterial, searchTerm, this.pipe));
    const total = optionMaterial.length;

    // 3. paginate
    this._state.totalRecords = optionMaterial.length;
    this._state.startIndex = (page - 1) * this.pageSize + 1;
    this._state.endIndex = +((page - 1) * this.pageSize + this.pageSize);
    if (this.endIndex > this.totalRecords) {
      this._state.endIndex = this.totalRecords;
    }
    optionMaterial = optionMaterial.slice(this._state.startIndex - 1, this._state.endIndex);
    return of({ optionMaterial, total });
  }

  public getAllOptionMaterials() {
    this.client.getAllProductOptionMaterials().subscribe(result => {
      this.optionMaterialsList = result;
      this._search$.next();
    }, error => console.error(error));

  }


}
