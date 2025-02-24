import { Injectable, PipeTransform } from "@angular/core";

import { BehaviorSubject, Observable, of, Subject } from "rxjs";

import { DecimalPipe } from "@angular/common";
import { debounceTime, delay, switchMap, tap } from "rxjs/operators";

import { BlockBannerModel } from "./block-banner.model";
import { SortColumn, SortDirection } from "./block-banner-sortable.directive";
import { BlockBannerDto, BlockBannersClient } from "src/app/web-api-client";
import { br } from "@fullcalendar/core/internal-common";
import { ImageService } from "src/app/core/services/image.service";

interface SearchResult {
  blockBanner: BlockBannerModel[];
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
  blockBanners: BlockBannerModel[],
  column: SortColumn,
  direction: string
): BlockBannerModel[] {
  if (direction === "" || column === "") {
    return blockBanners;
  } else {
    return [...blockBanners].sort((a, b) => {
      const res = compare(a[column], b[column]);
      return direction === "asc" ? res : -res;
    });
  }
}

function matches(
  blockBanner: BlockBannerModel,
  term: string,
  pipe: PipeTransform
) {
  return (
    blockBanner.blockBannerPositionName
      ?.toLowerCase()
      .includes(term.toLowerCase()) ||
    blockBanner.buttonText?.toLowerCase().includes(term.toLowerCase()) ||
    blockBanner.subtitle?.toLowerCase().includes(term.toLowerCase()) ||
    blockBanner.title?.toLowerCase().includes(term.toLowerCase())
  );
}

@Injectable({ providedIn: "root" })
export class BlockBannerGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _blockBanner$ = new BehaviorSubject<BlockBannerModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private blockBannerList: any[];
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
    public client: BlockBannersClient,
    public imageService: ImageService
  ) {
    this.blockBannerList = [];
    this.getAllBlockBanner();
    this._search$
      .pipe(
        tap(() => this._loading$.next(true)),
        debounceTime(200),
        switchMap(() => this._search()),
        delay(200),
        tap(() => this._loading$.next(false))
      )
      .subscribe((result) => {
        this._blockBanner$.next(result.blockBanner);
        this._total$.next(result.total);
      });
  }

  get blockBanners$() {
    return this._blockBanner$.asObservable();
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
    let blockBanner = sort(this.blockBannerList, sortColumn, sortDirection);

    // 2. filter
    blockBanner = blockBanner.filter((country) =>
      matches(country, searchTerm, this.pipe)
    );
    const total = blockBanner.length;

    // 3. paginate
    this._state.totalRecords = blockBanner.length;
    this._state.startIndex = (page - 1) * this.pageSize + 1;
    this._state.endIndex = (page - 1) * this.pageSize + this.pageSize;
    if (this.endIndex > this.totalRecords) {
      this._state.endIndex = this.totalRecords;
    }
    blockBanner = blockBanner.slice(
      this._state.startIndex - 1,
      this._state.endIndex
    );
    return of({ blockBanner, total });
  }

  public getAllBlockBanner() {
    this.client.getAllBlockBanners().subscribe(
      (result) => {
        this.blockBannerList = result;
        this.blockBannerList.forEach((c) => {
          c.imageSrc = this.imageService.getImageSrcById(c.image);
        });

        this._search$.next();
      },
      (error) => console.error(error)
    );
  }
}
