

import { Injectable, PipeTransform } from '@angular/core';

import { BehaviorSubject, forkJoin, Observable, of, Subject } from 'rxjs';


import { DecimalPipe } from '@angular/common';
import { debounceTime, delay, switchMap, tap } from 'rxjs/operators';


import { SortColumn, SortDirection } from './vehicle-sortable.directive';
import { VehicleDto, VehiclesClient,  } from 'src/app/web-api-client';
import { br } from '@fullcalendar/core/internal-common';
import { ImageService } from 'src/app/core/services/image.service';
import { VehicleModel } from './vehicle.model';
import { AuthenticationService } from 'src/app/core/services/authService/auth.service';



interface SearchResult {
  vehicle: VehicleModel[];
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

const compare = (v1: string | number, v2: string | number) => v1 < v2 ? -1 : v1 > v2 ? 1 : 0;

function sort(vehicles: VehicleModel[], column: SortColumn, direction: string): VehicleModel[] {
  if (direction === '' || column === '') {
    return vehicles;
  } else {
    return [...vehicles].sort((a, b) => {
      const res = compare(a[column], b[column]);
      return direction === 'asc' ? res : -res;
    });
  }
}

function matches(vehicle: VehicleModel, term: string, pipe: PipeTransform) {
  return vehicle.vinNumber?.toLowerCase().includes(term.toLowerCase())
    || vehicle.kindName?.toLowerCase().includes(term.toLowerCase())
    || vehicle.kindId?.toString().toLowerCase().includes(term.toLowerCase())
    || vehicle.customerId?.toString().toLowerCase().includes(term.toLowerCase());
}

@Injectable({ providedIn: 'root' })
export class VehicleGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _vehicle$ = new BehaviorSubject<VehicleModel[]>([]);
  private _total$ = new BehaviorSubject<number>(0);
  private vehicleList: any[];
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

  constructor(private pipe: DecimalPipe,
    public client: VehiclesClient,
    public authenticationService: AuthenticationService
  ) {
    this.vehicleList = [];
    this.getAllVehicle();
    debugger;
    this._search$.pipe(
      tap(() => this._loading$.next(true)),
      debounceTime(200),
      switchMap(() => this._search()),
      delay(200),
      tap(() => this._loading$.next(false))
    ).subscribe(result => {
      this._vehicle$.next(result.vehicle);
      this._total$.next(result.total);
    });

  }

  get vehicles$() { return this._vehicle$.asObservable(); }
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
  set sortColumn(sortColumn: SortColumn) { this._set({ sortColumn }); }
  set sortDirection(sortDirection: SortDirection) { this._set({ sortDirection }); }
  set startIndex(startIndex: number) { this._set({ startIndex }); }
  set endIndex(endIndex: number) { this._set({ endIndex }); }
  set totalRecords(totalRecords: number) { this._set({ totalRecords }); }

  private _set(patch: Partial<State>) {
    Object.assign(this._state, patch);
    this._search$.next();
  }

  private _search(): Observable<SearchResult> {
    const { sortColumn, sortDirection, pageSize, page, searchTerm } = this._state;

    // 1. sort
    //TODO : Fill data instead []
    let vehicle = sort(this.vehicleList, sortColumn, sortDirection);

    // 2. filter
    vehicle = vehicle.filter(country => matches(country, searchTerm, this.pipe));
    const total = vehicle.length;

    // 3. paginate
    this._state.totalRecords = vehicle.length;
    this._state.startIndex = (page - 1) * this.pageSize + 1;
    this._state.endIndex = +((page - 1) * this.pageSize + this.pageSize);
    if (this.endIndex > this.totalRecords) {
      this._state.endIndex = this.totalRecords;
    }
    vehicle = vehicle.slice(this._state.startIndex - 1, this._state.endIndex);
    return of({ vehicle, total });
  }

  public getAllVehicle() {
    this.client.getAllVehicles().pipe(
      switchMap(vehicles => {
        return forkJoin([
          this.authenticationService.getCustomers(),
          of(vehicles)  // Use 'of' to emit the existing 'vehicles' array
        ]);
      })
    ).subscribe(
      ([customers, vehicles]) => {
        vehicles.forEach(vehicle => {
          const matchingItem = customers.find(customer => customer.id === vehicle.customerId);
          if (matchingItem) {
            vehicle.customerName = matchingItem.firstName + ' ' + matchingItem.lastName;
          }
        });
        debugger;
        this.vehicleList = vehicles;
        this._search$.next();
      },
      error => {
        console.error('Error:', error);
      }
    );
  }
}
