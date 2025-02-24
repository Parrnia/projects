import { Injectable, PipeTransform } from "@angular/core";
import { BehaviorSubject, Observable, of, Subject } from "rxjs";
import { DecimalPipe } from "@angular/common";
import {
  debounceTime,
  delay,
  distinctUntilChanged,
  map,
  switchMap,
  takeUntil,
  tap,
} from "rxjs/operators";
import {
  FilteredRequestLogDto,
  HttpStatusCode,
  RequestLogDto,
  RequestLogsClient,
} from "src/app/web-api-client";
import { SortColumn, SortDirection } from "./request-log-sortable.directive";
import { RequestLogModel } from "./request-log.model";
import { forEach } from "lodash";
import * as momentJalaali from "moment-jalaali";

class SearchResult {
  requestLogs!: RequestLogModel[];
  total!: number;
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

@Injectable({ providedIn: "root" })
export class RequsetLogGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _requestLogs$ = new BehaviorSubject<RequestLogModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  // private reviewList!: any[];
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

  constructor(private pipe: DecimalPipe, public client: RequestLogsClient) {
    debugger;
    this._search$
      .pipe(
        tap(() => this._loading$.next(true)),
        debounceTime(200),
        switchMap(() => this._search()),
        tap(() => this._loading$.next(false))
      )
      .pipe()
      .subscribe((result) => {
        debugger;
        this._requestLogs$.next(result.requestLogs);
        this._total$.next(result.total);
      });

    this._search$.next();
  }

  get requestLogs$() {
    return this._requestLogs$.asObservable();
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
    this._state.pageSize = pageSize;
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
    this._state.startIndex = startIndex;
  }
  set endIndex(endIndex: number) {
    this._state.endIndex = endIndex;
  }
  set totalRecords(totalRecords: number) {
    this._state.totalRecords = totalRecords;
  }

  private _set(patch: Partial<State>) {
    Object.assign(this._state, patch);
    this._search$.next();
  }

  private _search(): Observable<SearchResult> {
    const { sortColumn, sortDirection, pageSize, page, searchTerm } =
      this._state;
    debugger;
    return this.client
      .getRequestLogsWithPagination(
        page,
        pageSize,
        sortColumn,
        sortDirection,
        searchTerm
      )
      .pipe(
        map((result: FilteredRequestLogDto) => {
          if (!result) {
            return { requestLogs: [], total: 0 };
          }
          const requestLogs = result.requestLogs || [];
          const total = result.count || 0;
          debugger;
          // Update your state properties as needed
          this.totalRecords = total;
          this.startIndex = (page - 1) * pageSize + 1;
          let end = result.requestLogs!.length + (page - 1) * pageSize;
          this.endIndex = Math.min(end, page * pageSize);
          this.pageSize = pageSize;
          let searchResult = new SearchResult();
          searchResult.requestLogs = requestLogs.map((c) =>
            this.mapRequestLogDtoToRequestLogModel(c)
          );
          searchResult.total = total;
          return searchResult;
        })
      );
  }

  public refreshRequestLogs() {
    this._search$.next();
  }

  private mapRequestLogDtoToRequestLogModel(
    dto: RequestLogDto
  ): RequestLogModel {
    return {
      id: dto.id || 0,
      apiAddress: dto.apiAddress || "",
      requestBody: dto.requestBody || "",
      errorMessage: dto.errorMessage || undefined,
      httpStatusCode: dto.responseStatus ?? HttpStatusCode.BadRequest,
      httpStatusCodeName: dto.httpStatusCodeName || "",
      created: dto.created || "",
      requestTypeName: dto.requestTypeName || "",
      requestType: dto.requestType ?? 0,
      apiTypeName: dto.apiTypeName || "",
    };
  }
}
