import { Injectable, PipeTransform } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { DecimalPipe } from '@angular/common';
import { debounceTime, delay, switchMap, tap } from 'rxjs/operators';
import { ProductAttributesClient, ProductOptionColorsClient, ProductOptionValueColorsClient, } from 'src/app/web-api-client';
import { OptionColorsModel } from './option-colors.model';



interface SearchResult {
  optionColors: OptionColorsModel[];
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




function matches(optionColors: OptionColorsModel, term: string, pipe: PipeTransform) {
  return optionColors.name.toString().toLowerCase().includes(term.toLowerCase())
    ;

}

@Injectable({ providedIn: 'root' })
export class OptionColorsGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _optionColors$ = new BehaviorSubject<OptionColorsModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private optionColorList: any[];
  createResult: Subject<any> = new Subject<any>();

  private _state: State = {
    page: 1,
    pageSize: 5,
    searchTerm: '',
    // sortColumn: '',
    // sortDirection: '',
    startIndex: 0,
    endIndex: 9,
    totalRecords: 0
  };

  constructor(private pipe: DecimalPipe
    , public client: ProductOptionColorsClient,
    public clientValue: ProductOptionValueColorsClient,
  ) {
    this.optionColorList = [];
    // this.getAllproductKinds();
    this._search$.pipe(
      tap(() => this._loading$.next(true)),
      debounceTime(200),
      switchMap(() => this._search()),
      delay(200),
      tap(() => this._loading$.next(false))
    ).subscribe(result => {
      this._optionColors$.next(result.optionColors);
      this._total$.next(result.total);
    });

  }

  get optionColors$() { return this._optionColors$.asObservable(); }
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
    // let optionColors = sort(this.optionColorList, sortColumn, sortDirection);
    let optionColors = this.optionColorList;
    // 2. filter
    optionColors = optionColors.filter(optionColors => matches(optionColors, searchTerm, this.pipe));
    const total = optionColors.length;

    // 3. paginate
    this._state.totalRecords = optionColors.length;
    this._state.startIndex = (page - 1) * this.pageSize + 1;
    this._state.endIndex = (page - 1) * this.pageSize + this.pageSize;
    if (this.endIndex > this.totalRecords) {
      this._state.endIndex = this.totalRecords;
    }
    optionColors = optionColors.slice(this._state.startIndex - 1, this._state.endIndex);
    return of({ optionColors, total });
  }

  public getAllProductOptionColorsByColorId(id: number) {
    this.clientValue.getAllProductOptionValueByColorId(id).subscribe(result => {
      this.optionColorList = result;
      this._search$.next();
    }, error => console.error(error));

  }


}
