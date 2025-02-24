import { Injectable, PipeTransform } from "@angular/core";

import { BehaviorSubject, Observable, of, Subject } from "rxjs";

import { DecimalPipe } from "@angular/common";
import { debounceTime, delay, switchMap, tap } from "rxjs/operators";

import { CorporationInfoModel } from "./corporation-info.model";
import {
  SortColumn,
  SortDirection,
} from "./corporation-info-sortable.directive";
import {
  CorporationInfoDto,
  CorporationInfosClient,
} from "src/app/web-api-client";
import { br } from "@fullcalendar/core/internal-common";
import { ImageService } from "src/app/core/services/image.service";

interface SearchResult {
  corporationInfo: CorporationInfoModel[];
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
  corporationInfos: CorporationInfoModel[],
  column: SortColumn,
  direction: string
): CorporationInfoModel[] {
  if (direction === "" || column === "") {
    return corporationInfos;
  } else {
    return [...corporationInfos].sort((a, b) => {
      const res = compare(a[column], b[column]);
      return direction === "asc" ? res : -res;
    });
  }
}

function matches(
  corporationInfo: CorporationInfoModel,
  term: string,
  pipe: PipeTransform
) {
  return (
    corporationInfo.contactUsMessage
      ?.toLowerCase()
      .includes(term.toLowerCase()) ||
    corporationInfo.poweredBy?.toLowerCase().includes(term.toLowerCase()) ||
    corporationInfo.callUs?.toLowerCase().includes(term.toLowerCase()) ||
    corporationInfo.isDefault
      ?.toString()
      .toLowerCase()
      .includes(term.toLowerCase())
  );
}

@Injectable({ providedIn: "root" })
export class CorporationInfoGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _corporationInfo$ = new BehaviorSubject<CorporationInfoModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private corporationInfoList: any[];
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
    public client: CorporationInfosClient,
    public imageService: ImageService
  ) {
    this.corporationInfoList = [];
    this.getAllCorporationInfo();
    this._search$
      .pipe(
        tap(() => this._loading$.next(true)),
        debounceTime(200),
        switchMap(() => this._search()),
        delay(200),
        tap(() => this._loading$.next(false))
      )
      .subscribe((result) => {
        this._corporationInfo$.next(result.corporationInfo);
        this._total$.next(result.total);
      });
  }

  get corporationInfos$() {
    return this._corporationInfo$.asObservable();
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
    let corporationInfo = sort(
      this.corporationInfoList,
      sortColumn,
      sortDirection
    );

    // 2. filter
    corporationInfo = corporationInfo.filter((country) =>
      matches(country, searchTerm, this.pipe)
    );
    const total = corporationInfo.length;

    // 3. paginate
    this._state.totalRecords = corporationInfo.length;
    this._state.startIndex = (page - 1) * this.pageSize + 1;
    this._state.endIndex = (page - 1) * this.pageSize + this.pageSize;
    if (this.endIndex > this.totalRecords) {
      this._state.endIndex = this.totalRecords;
    }
    corporationInfo = corporationInfo.slice(
      this._state.startIndex - 1,
      this._state.endIndex
    );
    return of({ corporationInfo, total });
  }

  public getAllCorporationInfo() {
    this.client.getAllCorporationInfos().subscribe(
      (result) => {
        this.corporationInfoList = result;
        this.corporationInfoList.forEach((c) => {
          c.mobileLogoSrc = this.imageService.getImageSrcById(c.mobileLogo);
          c.desktopLogoSrc = this.imageService.getImageSrcById(c.desktopLogo);
          c.footerLogoSrc = this.imageService.getImageSrcById(c.footerLogo);
          c.sliderBackGroundImageSrc = this.imageService.getImageSrcById(c.sliderBackGroundImage);
        });

        this._search$.next();
      },
      (error) => console.error(error)
    );
  }
}
