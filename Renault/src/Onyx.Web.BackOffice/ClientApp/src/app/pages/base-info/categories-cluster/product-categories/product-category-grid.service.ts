import { Injectable, PipeTransform } from "@angular/core";

import { BehaviorSubject, Observable, of, Subject } from "rxjs";

import { DecimalPipe } from "@angular/common";
import { debounceTime, delay, switchMap, tap } from "rxjs/operators";

import {
  SortColumn,
  SortDirection,
} from "./product-category-sortable.directive";
import {
  ProductCategoryDto,
  ProductCategoriesClient,
} from "src/app/web-api-client";
import { br } from "@fullcalendar/core/internal-common";
import { ImageService } from "src/app/core/services/image.service";
import { ProductCategoryModel } from "./product-category.model";

interface SearchResult {
  productCategory: ProductCategoryModel[];
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
  productCategories: ProductCategoryModel[],
  column: SortColumn,
  direction: string
): ProductCategoryModel[] {
  if (direction === "" || column === "") {
    return productCategories;
  } else {
    return [...productCategories].sort((a, b) => {
      const res = compare(a[column], b[column]);
      return direction === "asc" ? res : -res;
    });
  }
}

function matches(
  productCategory: ProductCategoryModel,
  term: string,
  pipe: PipeTransform
) {
  return (
    productCategory.code
      ?.toString()
      .toLowerCase()
      .includes(term.toLowerCase()) ||
    productCategory.localizedName?.toLowerCase().includes(term.toLowerCase()) ||
    productCategory.name?.toLowerCase().includes(term.toLowerCase()) ||
    productCategory.slug?.toLowerCase().includes(term.toLowerCase()) ||
    productCategory.productCategoryNo
      ?.toString()
      .toLowerCase()
      .includes(term.toLowerCase()) ||
    productCategory.image
      ?.toString()
      .toLowerCase()
      .includes(term.toLowerCase()) ||
    productCategory.productParentCategoryName
      ?.toString()
      .toLowerCase()
      .includes(term.toLowerCase())
  );
}

@Injectable({ providedIn: "root" })
export class ProductCategoryGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _productCategory$ = new BehaviorSubject<ProductCategoryModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private productCategoryList: any[];
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
    public client: ProductCategoriesClient,
    public imageService: ImageService
  ) {
    this.productCategoryList = [];
    this.getAllProductCategory();
    this._search$
      .pipe(
        tap(() => this._loading$.next(true)),
        debounceTime(200),
        switchMap(() => this._search()),
        delay(200),
        tap(() => this._loading$.next(false))
      )
      .subscribe((result) => {
        this._productCategory$.next(result.productCategory);
        this._total$.next(result.total);
      });
  }

  get productCategories$() {
    return this._productCategory$.asObservable();
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
    this._search$.next();
  }

  private _search(): Observable<SearchResult> {
    const { sortColumn, sortDirection, pageSize, page, searchTerm } =
      this._state;

    // 1. sort
    //TODO : Fill data instead []
    let productCategory = sort(
      this.productCategoryList,
      sortColumn,
      sortDirection
    );

    // 2. filter
    productCategory = productCategory.filter((country) =>
      matches(country, searchTerm, this.pipe)
    );
    const total = productCategory.length;

    // 3. paginate
    this._state.totalRecords = productCategory.length;
    this._state.startIndex = (page - 1) * this.pageSize + 1;
    this._state.endIndex = +((page - 1) * this.pageSize + this.pageSize);
    if (this.endIndex > this.totalRecords) {
      this._state.endIndex = this.totalRecords;
    }
    productCategory = productCategory.slice(
      this._state.startIndex - 1,
      this._state.endIndex
    );
    return of({ productCategory, total });
  }

  public getAllProductCategory() {
    this.client.getAllProductCategories().subscribe(
      (result) => {
        this.productCategoryList = result;
        this.productCategoryList.forEach((c) => {
          c.imageSrc = this.imageService.getImageSrcById(c.image);
          c.menuImageSrc = this.imageService.getImageSrcById(c.menuImage);
        });

        this._search$.next();
      },
      (error) => console.error(error)
    );
  }
}
