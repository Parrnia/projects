import { Injectable, PipeTransform } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { DecimalPipe } from '@angular/common';
import { debounceTime, delay, distinctUntilChanged, map, switchMap, takeUntil, tap } from 'rxjs/operators';
import { FilteredReviewDto, ReviewDto, ReviewsClient } from 'src/app/web-api-client';
import { SortColumn, SortDirection } from './review-sortable.directive';
import { ReviewModel } from './review.model';
import { forEach } from 'lodash';
import * as momentJalaali from "moment-jalaali";



class SearchResult {
  reviews!: ReviewModel[];
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


@Injectable({ providedIn: 'root' })
export class ReviewGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _reviews$ = new BehaviorSubject<ReviewModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private reviewList!: any[];
  createResult: Subject<any> = new Subject<any>();

  private _state: State = {
    page: 1,
    pageSize: 5,
    searchTerm: '',
    sortColumn: '',
    sortDirection: '',
    startIndex: 0,
    endIndex: 9,
    totalRecords: 0
  };

  constructor(private pipe: DecimalPipe, public client: ReviewsClient) {
    debugger;
    this._search$.pipe(
      tap(() => this._loading$.next(true)),
      debounceTime(200),
      switchMap(() => this._search()),
      tap(() => this._loading$.next(false)),
    ).pipe().subscribe(result => {
      debugger;
      this._reviews$.next(result.reviews);
      this._total$.next(result.total);
    });

    this._search$.next();
  }

  get reviews$() { return this._reviews$.asObservable(); }
  get total$() { return this._total$.asObservable(); }
  get loading$() { return this._loading$.asObservable(); }
  get page() { return this._state.page; }
  get pageSize() { return this._state.pageSize; }
  get searchTerm() { return this._state.searchTerm; }
  get startIndex() { return this._state.startIndex; }
  get endIndex() { return this._state.endIndex; }
  get totalRecords() { return this._state.totalRecords; }

  set page(page: number) { this._set({ page });  }
  set pageSize(pageSize: number) { this._state.pageSize = pageSize }
  set searchTerm(searchTerm: string) { this._set({ searchTerm }); }
  set sortColumn(sortColumn: SortColumn) { this._set({ sortColumn }); }
  set sortDirection(sortDirection: SortDirection) { this._set({ sortDirection }); }
  set startIndex(startIndex: number) { this._state.startIndex = startIndex }
  set endIndex(endIndex: number) { this._state.endIndex = endIndex }
  set totalRecords(totalRecords: number) {  this._state.totalRecords = totalRecords }

  private _set(patch: Partial<State>) {
    Object.assign(this._state, patch);
    this._search$.next();
  }

  private _search(): Observable<SearchResult> {
    const { sortColumn, sortDirection, pageSize, page, searchTerm } = this._state;
    debugger;
    return this.client.getReviewsWithPagination(page, pageSize, sortColumn, sortDirection, searchTerm)
      .pipe(
        map((result: FilteredReviewDto) => {
          if (!result) {
            return { reviews: [], total: 0 };
          }
          const reviews = result.reviews || [];
          const total = result.count || 0;
          debugger;
          // Update your state properties as needed
          this.totalRecords = total;
          this.startIndex = ((page - 1) * pageSize) + 1;
          let end = result.reviews!.length + (page-1) * pageSize;
          this.endIndex = Math.min(end, page * pageSize);
          this.pageSize = pageSize;
          let searchResult = new SearchResult();
          searchResult.reviews = reviews.map(c => this.mapReviewDtoToReviewModel(c));
          searchResult.total = total;
          return  searchResult;
        }),
      );
  }

  

  public refreshReviews(){
    this._search$.next();
  }
  
  private mapReviewDtoToReviewModel(dto: ReviewDto): ReviewModel {
    momentJalaali.loadPersian(/*{ usePersianDigits: true }*/);
    return {
        id: dto.id || 0,
        date: momentJalaali(dto.date).format("jD jMMMM jYYYY"), // Customize date format here
        rating: dto.rating ? dto.rating.toString() : '', // Convert to string if needed
        content: dto.content || '',
        authorName: dto.authorName || '',
        productName: dto.productName || '',
        isActive: dto.isActive || false
    };
}
}
