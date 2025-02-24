import { Injectable, PipeTransform } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { DecimalPipe } from '@angular/common';
import { debounceTime, delay, switchMap, tap } from 'rxjs/operators';
import { ProductKindsModel } from './product-kinds.model';
import { ProductKindsClient } from 'src/app/web-api-client';
// import { SortColumn, SortDirection } from './product-images-sortable.directive';



interface SearchResult {
  productKinds: ProductKindsModel[];
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


function matches(productKinds: ProductKindsModel, term: string, pipe: PipeTransform) {
     
  return productKinds.kindName.toString().toLowerCase().includes(term.toLowerCase())
   
;

}

@Injectable({ providedIn: 'root' })
export class ProductKindsGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _productKinds$ = new BehaviorSubject<ProductKindsModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private productKindsList: any[];
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
    , public client: ProductKindsClient
  ) {
    this.productKindsList=[];
    // this.getAllproductKinds();
    this._search$.pipe(
      tap(() => this._loading$.next(true)),
      debounceTime(200),
      switchMap(() => this._search()),
      delay(200),
      tap(() => this._loading$.next(false))
    ).subscribe(result => {
      this._productKinds$.next(result.productKinds);
      this._total$.next(result.total);
    });

    this._search$.next();
  }

  get productKinds$() { return this._productKinds$.asObservable(); }
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
    debugger;
    this._search$.next();
  }

  private _search(): Observable<SearchResult> {
    const {  pageSize, page, searchTerm } = this._state;

    // 1. sort
    //TODO : Fill data instead []
    // let productKinds = sort(this.productKindsList, sortColumn, sortDirection);
    let productKinds= this.productKindsList;
    // 2. filter
    productKinds = productKinds.filter(productKinds => matches(productKinds, searchTerm, this.pipe));
    const total = productKinds.length;

    // 3. paginate
    this._state.totalRecords = productKinds.length;
    this._state.startIndex = (page - 1) * this.pageSize + 1;
    this._state.endIndex = (page - 1) * this.pageSize + this.pageSize;
    if (this.endIndex > this.totalRecords) {
      this._state.endIndex = this.totalRecords;
    }
    productKinds = productKinds.slice(this._state.startIndex - 1, this._state.endIndex);
    return of({ productKinds, total });
  }

  public  getAllProductKinds(productId: number){
    this.client.getAllProductKindsByProductId(productId).subscribe(result => {
        this.productKindsList = result ;
    }, error => console.error(error));
   
  }

  public refresh(productId: number){
    this.client.getAllProductKindsByProductId(productId).subscribe(result => {
      this.productKindsList = result ;
      this._search$.next();
  }, error => console.error(error));
    
  }
  
}
