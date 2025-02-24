import { Injectable, PipeTransform } from "@angular/core";

import { BehaviorSubject, Observable, of, Subject } from "rxjs";

import { DecimalPipe } from "@angular/common";
import { debounceTime, delay, switchMap, tap } from "rxjs/operators";

import { ProductBrandModel } from "./product-brand.model";
import { SortColumn, SortDirection } from "./product-brand-sortable.directive";
import { ProductBrandDto, ProductBrandsClient } from "src/app/web-api-client";
import { br } from "@fullcalendar/core/internal-common";
import { ImageService } from "src/app/core/services/image.service";

interface SearchResult {
  productBrand: ProductBrandModel[];
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

const compare = (
  v1: string | number | boolean,
  v2: string | number | boolean
) => (v1 < v2 ? -1 : v1 > v2 ? 1 : 0);

function sort(
  productBrands: ProductBrandModel[],
  column: SortColumn,
  direction: string
): ProductBrandModel[] {
  if (direction === "" || column === "") {
    return productBrands;
  } else {
    return [...productBrands].sort((a, b) => {
      const res = compare(a[column], b[column]);
      return direction === "asc" ? res : -res;
    });
  }
}

function matches(
  productBrand: ProductBrandModel,
  term: string,
  pipe: PipeTransform
) {
  return (
    productBrand.localizedName?.toLowerCase().includes(term.toLowerCase()) ||
    productBrand.name?.toLowerCase().includes(term.toLowerCase()) ||
    productBrand.code?.toString().toLowerCase().includes(term.toLowerCase()) ||
    productBrand.slug?.toLowerCase().includes(term.toLowerCase()) ||
    productBrand.countryName?.toLowerCase().includes(term.toLowerCase())
  );
}

@Injectable({ providedIn: "root" })
export class ProductBrandGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _productBrand$ = new BehaviorSubject<ProductBrandModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private productBrandList: any[];
  createResult: Subject<any> = new Subject<any>();

  private _state: State = {
    page: 1,
    pageSize: 5,
    searchTerm: "",
    sortColumn: "",
    sortDirection: "",
    startIndex: 0,
    endIndex: 9,
    totalRecords: 0,
  };

  constructor(
    private pipe: DecimalPipe,
    public client: ProductBrandsClient,
    public imageService: ImageService
  ) {
    this.productBrandList = [];
    this.getAllProductBrand();
    this._search$
      .pipe(
        tap(() => this._loading$.next(true)),
        debounceTime(200),
        switchMap(() => this._search()),
        delay(200),
        tap(() => this._loading$.next(false))
      )
      .subscribe((result) => {
        this._productBrand$.next(result.productBrand);
        this._total$.next(result.total);
      });
  }

  get productBrands$() {
    return this._productBrand$.asObservable();
  }
  get total$() {
    return this._total$.asObservable();
  }
  get loading$() {
    return this._loading$.asObservable();
  }
  get page() {
    return this._state.page;
  }
  get pageSize() {
    return this._state.pageSize;
  }
  get searchTerm() {
    return this._state.searchTerm;
  }
  get startIndex() {
    return this._state.startIndex;
  }
  get endIndex() {
    return this._state.endIndex;
  }
  get totalRecords() {
    return this._state.totalRecords;
  }

  set page(page: number) {
    this._set({ page });
  }
  set pageSize(pageSize: number) {
    this._set({ pageSize });
  }
  set searchTerm(searchTerm: string) {
    this._set({ searchTerm });
  }
  set sortColumn(sortColumn: SortColumn) {
    this._set({ sortColumn });
  }
  set sortDirection(sortDirection: SortDirection) {
    this._set({ sortDirection });
  }
  set startIndex(startIndex: number) {
    this._set({ startIndex });
  }
  set endIndex(endIndex: number) {
    this._set({ endIndex });
  }
  set totalRecords(totalRecords: number) {
    this._set({ totalRecords });
  }

  private _set(patch: Partial<State>) {
    Object.assign(this._state, patch);
    debugger;
    this._search$.next();
  }

  private _search(): Observable<SearchResult> {
    const { sortColumn, sortDirection, pageSize, page, searchTerm } =
      this._state;
    debugger;
    // 1. sort
    //TODO : Fill data instead []
    let productBrand = sort(this.productBrandList, sortColumn, sortDirection);

    // 2. filter
    productBrand = productBrand.filter((country) =>
      matches(country, searchTerm, this.pipe)
    );
    const total = productBrand.length;

    // 3. paginate
    this._state.totalRecords = productBrand.length;
    this._state.startIndex = (page - 1) * this.pageSize + 1;
    this._state.endIndex = +((page - 1) * this.pageSize + this.pageSize);
    if (this.endIndex > this.totalRecords) {
      this._state.endIndex = this.totalRecords;
    }
    productBrand = productBrand.slice(
      this._state.startIndex - 1,
      this._state.endIndex
    );
    return of({ productBrand, total });
  }

  public getAllProductBrand() {
    this.client.getAllProductBrands().subscribe(
      (result) => {
        this.productBrandList = result;
        this.productBrandList.forEach((c) => {
          c.brandLogoSrc = this.imageService.getImageSrcById(c.brandLogo);
        });

        this._search$.next();
      },
      (error) => console.error(error)
    );
  }
}
