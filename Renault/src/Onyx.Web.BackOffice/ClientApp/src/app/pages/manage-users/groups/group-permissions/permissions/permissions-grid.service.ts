import { Injectable, PipeTransform } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { DecimalPipe } from '@angular/common';
import { debounceTime, delay, switchMap, tap } from 'rxjs/operators';
import { PermissionsModel } from './permissions.model';
import { AuthenticationService } from 'src/app/core/services/authService/auth.service';
// import { SortColumn, SortDirection } from './product-images-sortable.directive';



interface SearchResult {
  permissions: PermissionsModel[];
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


function matches(permissions: PermissionsModel, term: string, pipe: PipeTransform) {
  return permissions.slug.toString().toLowerCase().includes(term.toLowerCase())
  || permissions.name.toString().toLowerCase().includes(term.toLowerCase())
  || permissions.category.toString().toLowerCase().includes(term.toLowerCase())
  ;

}

@Injectable({ providedIn: 'root' })
export class PermissionsGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _permissions$ = new BehaviorSubject<PermissionsModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private permissionsList: any[];
  createResult: Subject<any> = new Subject<any>();

  private _state: State = {
    page: 1,
    pageSize: 5,
    searchTerm: '',
    startIndex: 0,
    endIndex: 9,
    totalRecords: 0
  };

  constructor(private pipe: DecimalPipe
    , public client: AuthenticationService
  ) {
    this.permissionsList=[];
    // this.getAllpermissions();
    this._search$.pipe(
      tap(() => this._loading$.next(true)),
      debounceTime(200),
      switchMap(() => this._search()),
      delay(200),
      tap(() => this._loading$.next(false))
    ).subscribe(result => {
      this._permissions$.next(result.permissions);
      this._total$.next(result.total);
    });

    this._search$.next();
  }

  get permissions$() { return this._permissions$.asObservable(); }
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
//   set sortColumn(sortColumn: SortColumn) { this._set({ sortColumn }); }
//   set sortDirection(sortDirection: SortDirection) { this._set({ sortDirection }); }
  set startIndex(startIndex: number) { this._set({ startIndex }); }
  set endIndex(endIndex: number) { this._set({ endIndex }); }
  set totalRecords(totalRecords: number) { this._set({ totalRecords }); }

  private _set(patch: Partial<State>) {
   
    Object.assign(this._state, patch);
    this._search$.next();
  }

  private _search(): Observable<SearchResult> {
    const {  pageSize, page, searchTerm } = this._state;
    debugger;
    // 1. sort
    //TODO : Fill data instead []
    // let permissions = sort(this.permissionsList, sortColumn, sortDirection);
    let permissions= this.permissionsList;
    // 2. filter
    permissions = permissions.filter(permissions => matches(permissions, searchTerm, this.pipe));
    const total = permissions.length;

    // 3. paginate
    this._state.totalRecords = permissions.length;
    this._state.startIndex = (page - 1) * this.pageSize + 1;
    this._state.endIndex = (page - 1) * this.pageSize + this.pageSize;
    if (this.endIndex > this.totalRecords) {
      this._state.endIndex = this.totalRecords;
    }
    permissions = permissions.slice(this._state.startIndex - 1, this._state.endIndex);
    return of({ permissions, total });
  }

  public  getAllPermissions(){
    this.client.getPermissions().subscribe(result => {
      debugger;
        this.permissionsList = result ;
    }, error => console.error(error));
   
  }

  
}
