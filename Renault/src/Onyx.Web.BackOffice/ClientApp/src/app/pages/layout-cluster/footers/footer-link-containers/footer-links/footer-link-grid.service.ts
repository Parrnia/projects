

import { Injectable, PipeTransform } from '@angular/core';

import { BehaviorSubject, Observable, of, Subject } from 'rxjs';


import { DecimalPipe } from '@angular/common';
import { debounceTime, delay, switchMap, tap } from 'rxjs/operators';


import { FooterLinkDto, FooterLinksClient,  } from 'src/app/web-api-client';
import { br } from '@fullcalendar/core/internal-common';
import { ImageService } from 'src/app/core/services/image.service';
import { FooterLinkModel } from './footer-link.model';



interface SearchResult {
  footerLink: FooterLinkModel[];
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


function matches(footerLink: FooterLinkModel, term: string, pipe: PipeTransform) {
  return footerLink.title?.toLowerCase().includes(term.toLowerCase())
    ;
}

@Injectable({ providedIn: 'root' })
export class FooterLinkGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _footerLink$ = new BehaviorSubject<FooterLinkModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private footerLinkList: any[];
  createResult: Subject<any> = new Subject<any>();

  private _state: State = {
    page: 1,
    pageSize: 5,
    searchTerm: '',
    startIndex: 0,
    endIndex: 9,
    totalRecords: 0
  };

  constructor(private pipe: DecimalPipe,
    public client: FooterLinksClient
  ) {
    this.footerLinkList = [];
    this._search$.pipe(
      tap(() => this._loading$.next(true)),
      debounceTime(200),
      switchMap(() => this._search()),
      delay(200),
      tap(() => this._loading$.next(false))
    ).subscribe(result => {
      this._footerLink$.next(result.footerLink);
      this._total$.next(result.total);
    });

  }

  get footerLinks$() { return this._footerLink$.asObservable(); }
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
    //let footerLink = sort(this.footerLinkList, sortColumn, sortDirection);
    let footerLink = this.footerLinkList;

    // 2. filter
    footerLink = footerLink.filter(country => matches(country, searchTerm, this.pipe));
    const total = footerLink.length;

    // 3. paginate
    this._state.totalRecords = footerLink.length;
    this._state.startIndex = (page - 1) * this.pageSize + 1;
    this._state.endIndex = (page - 1) * this.pageSize + this.pageSize;
    if (this.endIndex > this.totalRecords) {
      this._state.endIndex = this.totalRecords;
    }
    footerLink = footerLink.slice(this._state.startIndex - 1, this._state.endIndex);
    return of({ footerLink, total });
  }

  public getFooterLinksByFooterLinkContainerId(id: number) {
    this.client.getFooterLinksByFooterLinkContainerId(id).subscribe(result => {
      this.footerLinkList = result;
      this._search$.next();
      debugger;
    }, error => console.error(error));
  }
}
