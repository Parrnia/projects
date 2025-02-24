import { Injectable, PipeTransform } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { DecimalPipe } from '@angular/common';
import { debounceTime, delay, switchMap, tap } from 'rxjs/operators';
import { SortColumn, SortDirection } from './return-order-total-document-sortable.directive';
import { ReturnOrdersClient , ReturnOrderTotalDocumentDto, ReturnOrderTotalDocumentsClient } from 'src/app/web-api-client';
import { ImageService } from "src/app/core/services/image.service";
import { br } from '@fullcalendar/core/internal-common';
import { ReturnOrderTotalDocumentModel } from './return-order-total-document.model';


interface SearchResult {
  returnOrderTotalDocument: ReturnOrderTotalDocumentModel[];
  total: number;
}

interface State {
  page: number;
  pageSize: number;
  searchTerm: string;
  //sortColumn: SortColumn;
  //sortDirection: SortDirection;
  startIndex: number;
  endIndex: number;
  totalRecords: number;
}

const compare = (v1: string | number | boolean, v2: string | number | boolean) => v1 < v2 ? -1 : v1 > v2 ? 1 : 0;

function sort(returnOrderTotalDocuments: ReturnOrderTotalDocumentModel[], column: SortColumn, direction: string): ReturnOrderTotalDocumentModel[] {
  if (direction === '' || column === '') {
    return returnOrderTotalDocuments;
  } else {
    return [...returnOrderTotalDocuments].sort((a, b) => {
      const res = compare(a[column], b[column]);
      return direction === 'asc' ? res : -res;
    });
  }
}

function matches(returnOrderTotalDocument: ReturnOrderTotalDocumentModel, term: string, pipe: PipeTransform) {
  return returnOrderTotalDocument.description?.toLowerCase().includes(term.toLowerCase())
  ;
}

@Injectable({ providedIn: 'root' })
export class ReturnOrderTotalDocumentGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _returnOrderTotalDocument$ = new BehaviorSubject<ReturnOrderTotalDocumentModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private returnOrderTotalDocumentList: any[];
  createResult: Subject<any> = new Subject<any>();

  private _state: State = {
    page: 1,
    pageSize: 5,
    searchTerm: '',
    //sortColumn: '',
    //sortDirection: '',
    startIndex: 0,
    endIndex: 9,
    totalRecords: 0
  };

  constructor(private pipe: DecimalPipe,
    public client: ReturnOrderTotalDocumentsClient,
    public imageService: ImageService
  ) {
    this.returnOrderTotalDocumentList = [];
    this._search$.pipe(
      tap(() => this._loading$.next(true)),
      debounceTime(200),
      switchMap(() => this._search()),
      delay(200),
      tap(() => this._loading$.next(false))
    ).subscribe(result => {
      this._returnOrderTotalDocument$.next(result.returnOrderTotalDocument);
      this._total$.next(result.total);
    });

  }

  get returnOrderTotalDocuments$() { return this._returnOrderTotalDocument$.asObservable(); }
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
  //set sortColumn(sortColumn: SortColumn) { this._set({ sortColumn }); }
  //set sortDirection(sortDirection: SortDirection) { this._set({ sortDirection }); }
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
    //let returnOrderTotalDocuments = sort(this.returnOrderTotalDocumentList, sortColumn, sortDirection);
    let returnOrderTotalDocuments = this.returnOrderTotalDocumentList;

    // 2. filter
    returnOrderTotalDocuments = returnOrderTotalDocuments.filter(d => matches(d, searchTerm, this.pipe));
    const total = returnOrderTotalDocuments.length;

    // 3. paginate
    this._state.totalRecords = returnOrderTotalDocuments.length;
    this._state.startIndex = (page - 1) * this.pageSize + 1;
    this._state.endIndex = +((page - 1) * this.pageSize + this.pageSize);
    if (this.endIndex > this.totalRecords) {
      this._state.endIndex = this.totalRecords;
    }
    returnOrderTotalDocuments = returnOrderTotalDocuments.slice(this._state.startIndex - 1, this._state.endIndex);
    return of({ returnOrderTotalDocument: returnOrderTotalDocuments, total });
  }

  public getAllReturnOrderTotalDocumentsByReturnOrderTotalId(id: number) {
    this.client.getReturnOrderTotalDocumentsByReturnOrderTotalId(id).subscribe(result => {
      this.returnOrderTotalDocumentList = result;
      this.returnOrderTotalDocumentList.forEach((c) => {
        c.imageSrc = this.imageService.getImageSrcById(c.image);
      });
      this._search$.next();
      debugger;
    }, error => console.error(error));
  }
}
