import { Injectable, PipeTransform } from "@angular/core";

import { BehaviorSubject, Observable, of, Subject } from "rxjs";

import { DecimalPipe } from "@angular/common";
import { debounceTime, delay, switchMap, tap } from "rxjs/operators";

import { VehicleBrandModel } from "./vehicle-brand.model";
import { SortColumn, SortDirection } from "./vehicle-brand-sortable.directive";
import { VehicleBrandDto, VehicleBrandsClient } from "src/app/web-api-client";
import { br } from "@fullcalendar/core/internal-common";
import { ImageService } from "src/app/core/services/image.service";

interface SearchResult {
  vehicleBrand: VehicleBrandModel[];
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
  vehicleBrands: VehicleBrandModel[],
  column: SortColumn,
  direction: string
): VehicleBrandModel[] {
  if (direction === "" || column === "") {
    return vehicleBrands;
  } else {
    return [...vehicleBrands].sort((a, b) => {
      const res = compare(a[column], b[column]);
      return direction === "asc" ? res : -res;
    });
  }
}

function matches(
  vehicleBrand: VehicleBrandModel,
  term: string,
  pipe: PipeTransform
) {
  return (
    vehicleBrand.localizedName?.toLowerCase().includes(term.toLowerCase()) ||
    vehicleBrand.name?.toLowerCase().includes(term.toLowerCase()) ||
    vehicleBrand.code?.toString().toLowerCase().includes(term.toLowerCase()) ||
    vehicleBrand.slug?.toLowerCase().includes(term.toLowerCase()) ||
    vehicleBrand.countryName?.toLowerCase().includes(term.toLowerCase())
  );
}

@Injectable({ providedIn: "root" })
export class VehicleBrandGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _vehicleBrand$ = new BehaviorSubject<VehicleBrandModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private vehicleBrandList: any[];
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
    public client: VehicleBrandsClient,
    public imageService: ImageService
  ) {
    this.vehicleBrandList = [];
    this.getAllVehicleBrand();
    this._search$
      .pipe(
        tap(() => this._loading$.next(true)),
        debounceTime(200),
        switchMap(() => this._search()),
        delay(200),
        tap(() => this._loading$.next(false))
      )
      .subscribe((result) => {
        this._vehicleBrand$.next(result.vehicleBrand);
        this._total$.next(result.total);
      });
  }

  get vehicleBrands$() {
    return this._vehicleBrand$.asObservable();
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

    // 1. sort
    //TODO : Fill data instead []
    let vehicleBrand = sort(this.vehicleBrandList, sortColumn, sortDirection);

    // 2. filter
    vehicleBrand = vehicleBrand.filter((country) =>
      matches(country, searchTerm, this.pipe)
    );
    const total = vehicleBrand.length;

    // 3. paginate
    this._state.totalRecords = vehicleBrand.length;
    this._state.startIndex = (page - 1) * this.pageSize + 1;
    this._state.endIndex = +((page - 1) * this.pageSize + this.pageSize);
    if (this.endIndex > this.totalRecords) {
      this._state.endIndex = this.totalRecords;
    }
    vehicleBrand = vehicleBrand.slice(
      this._state.startIndex - 1,
      this._state.endIndex
    );
    return of({ vehicleBrand, total });
  }

  public getAllVehicleBrand() {
    this.client.getAllVehicleBrands().subscribe(
      (result) => {
        this.vehicleBrandList = result;
        this.vehicleBrandList.forEach((c) => {
          c.brandLogoSrc = this.imageService.getImageSrcById(c.brandLogo);
        });

        this._search$.next();
      },
      (error) => console.error(error)
    );
  }
}
