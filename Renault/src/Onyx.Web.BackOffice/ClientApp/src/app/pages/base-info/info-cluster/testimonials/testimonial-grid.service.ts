import { Injectable, PipeTransform } from "@angular/core";

import { BehaviorSubject, Observable, of, Subject } from "rxjs";

import { DecimalPipe } from "@angular/common";
import { debounceTime, delay, switchMap, tap } from "rxjs/operators";

import { SortColumn, SortDirection } from "./testimonial-sortable.directive";
import { TestimonialDto, TestimonialsClient } from "src/app/web-api-client";
import { br } from "@fullcalendar/core/internal-common";
import { ImageService } from "src/app/core/services/image.service";
import { TestimonialModel } from "./testimonial.model";

interface SearchResult {
  testimonial: TestimonialModel[];
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
  testimonials: TestimonialModel[],
  column: SortColumn,
  direction: string
): TestimonialModel[] {
  if (direction === "" || column === "") {
    return testimonials;
  } else {
    return [...testimonials].sort((a, b) => {
      const res = compare(a[column], b[column]);
      return direction === "asc" ? res : -res;
    });
  }
}

function matches(
  testimonial: TestimonialModel,
  term: string,
  pipe: PipeTransform
) {
  return (
    testimonial.name?.toLowerCase().includes(term.toLowerCase()) ||
    testimonial.position?.toLowerCase().includes(term.toLowerCase()) ||
    testimonial.aboutUsTitle?.toLowerCase().includes(term.toLowerCase())
  );
}

@Injectable({ providedIn: "root" })
export class TestimonialGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _testimonial$ = new BehaviorSubject<TestimonialModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private testimonialList: any[];
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
    public client: TestimonialsClient,
    public imageService: ImageService
  ) {
    this.testimonialList = [];
    this.getAllTestimonial();
    this._search$
      .pipe(
        tap(() => this._loading$.next(true)),
        debounceTime(200),
        switchMap(() => this._search()),
        delay(200),
        tap(() => this._loading$.next(false))
      )
      .subscribe((result) => {
        this._testimonial$.next(result.testimonial);
        this._total$.next(result.total);
      });
  }

  get testimonials$() {
    return this._testimonial$.asObservable();
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
    let testimonial = sort(this.testimonialList, sortColumn, sortDirection);

    // 2. filter
    testimonial = testimonial.filter((country) =>
      matches(country, searchTerm, this.pipe)
    );
    const total = testimonial.length;

    // 3. paginate
    this._state.totalRecords = testimonial.length;
    this._state.startIndex = (page - 1) * this.pageSize + 1;
    this._state.endIndex = (page - 1) * this.pageSize + this.pageSize;
    if (this.endIndex > this.totalRecords) {
      this._state.endIndex = this.totalRecords;
    }
    testimonial = testimonial.slice(
      this._state.startIndex - 1,
      this._state.endIndex
    );
    return of({ testimonial, total });
  }

  public getAllTestimonial() {
    this.client.getAllTestimonials().subscribe(
      (result) => {
        this.testimonialList = result;
        this.testimonialList.forEach((c) => {
          c.avatarSrc = this.imageService.getImageSrcById(c.avatar);
        });

        this._search$.next();
      },
      (error) => console.error(error)
    );
  }
}
