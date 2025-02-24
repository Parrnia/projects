import { Injectable, PipeTransform } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { DecimalPipe } from '@angular/common';
import { debounceTime, delay, switchMap, tap } from 'rxjs/operators';
import { ProductBadgesModel } from './product-badges.model';
import { BadgesClient } from 'src/app/web-api-client';
// import { SortColumn, SortDirection } from './product-images-sortable.directive';



interface SearchResult {
  productBadges: ProductBadgesModel[];
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


function matches(productBadges: ProductBadgesModel, term: string, pipe: PipeTransform) {

  return productBadges.value.toString().toLowerCase().includes(term.toLowerCase())

    ;

}

@Injectable({ providedIn: 'root' })
export class ProductBadgesGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _productBadges$ = new BehaviorSubject<ProductBadgesModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private productBadgesList: any[];
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
    , public client: BadgesClient
  ) {
    this.productBadgesList = [];
    // this.getAllproductBadges();
    this._search$.pipe(
      tap(() => this._loading$.next(true)),
      debounceTime(200),
      switchMap(() => this._search()),
      delay(200),
      tap(() => this._loading$.next(false))
    ).subscribe(result => {
      this._productBadges$.next(result.productBadges);
      this._total$.next(result.total);
    });

  }

  get productBadges$() { return this._productBadges$.asObservable(); }
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
    // let productBadges = sort(this.productBadgesList, sortColumn, sortDirection);
    let productBadges = this.productBadgesList;
    // 2. filter
    productBadges = productBadges.filter(productBadges => matches(productBadges, searchTerm, this.pipe));
    const total = productBadges.length;

    // 3. paginate
    this._state.totalRecords = productBadges.length;
    this._state.startIndex = (page - 1) * this.pageSize + 1;
    this._state.endIndex = (page - 1) * this.pageSize + this.pageSize;
    if (this.endIndex > this.totalRecords) {
      this._state.endIndex = this.totalRecords;
    }
    productBadges = productBadges.slice(this._state.startIndex - 1, this._state.endIndex);
    return of({ productBadges, total });
  }

  public getAllProductBadges(optionId: number) {
    this.client.getAllBadgesByOptionId(optionId).subscribe(result => {
      this.productBadgesList = result;
      this._search$.next();
    }, error => console.error(error));

  }


}
