import { Injectable, PipeTransform } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { DecimalPipe } from '@angular/common';
import { debounceTime, delay, switchMap, tap } from 'rxjs/operators';
import { KindsClient, ProductKindsClient } from 'src/app/web-api-client';
import { KindsModel } from './kinds.model';
// import { SortColumn, SortDirection } from './product-images-sortable.directive';



interface SearchResult {
  kinds: KindsModel[];
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


function matches(kinds: KindsModel, term: string, pipe: PipeTransform) {
  return kinds.localizedName.toString().toLowerCase().includes(term.toLowerCase())
  ;

}

@Injectable({ providedIn: 'root' })
export class KindsGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _kinds$ = new BehaviorSubject<KindsModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private kindsList: any[];
  createResult: Subject<any> = new Subject<any>();

  private _state: State = {
    page: 1,
    pageSize: 5,
    searchTerm: '',
    startIndex: 0,
    endIndex: 9,
    totalRecords: 0
  };

  constructor(private pipe: DecimalPipe
    , public client: KindsClient
  ) {
    this.kindsList=[];
    // this.getAllkinds();
    this._search$.pipe(
      tap(() => this._loading$.next(true)),
      debounceTime(200),
      switchMap(() => this._search()),
      delay(200),
      tap(() => this._loading$.next(false))
    ).subscribe(result => {
      this._kinds$.next(result.kinds);
      this._total$.next(result.total);
    });

    this._search$.next();
  }

  get kinds$() { return this._kinds$.asObservable(); }
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
    const {  pageSize, page, searchTerm } = this._state;
    debugger;
    // 1. sort
    //TODO : Fill data instead []
    // let kinds = sort(this.kindsList, sortColumn, sortDirection);
    let kinds= this.kindsList;
    // 2. filter
    kinds = kinds.filter(kinds => matches(kinds, searchTerm, this.pipe));
    const total = kinds.length;

    // 3. paginate
    this._state.totalRecords = kinds.length;
    this._state.startIndex = (page - 1) * this.pageSize + 1;
    this._state.endIndex = (page - 1) * this.pageSize + this.pageSize;
    if (this.endIndex > this.totalRecords) {
      this._state.endIndex = this.totalRecords;
    }
    kinds = kinds.slice(this._state.startIndex - 1, this._state.endIndex);
    return of({ kinds, total });
  }

  public  getAllKinds(){
    this.client.getAllKindsDropDown().subscribe(result => {
        this.kindsList = result ;
    }, error => console.error(error));
   
  }

  
}
