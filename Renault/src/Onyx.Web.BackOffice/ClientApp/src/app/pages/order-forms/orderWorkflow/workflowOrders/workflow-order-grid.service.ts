import { Injectable, PipeTransform } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { DecimalPipe } from '@angular/common';
import { debounceTime, delay, distinctUntilChanged, map, switchMap, takeUntil, tap } from 'rxjs/operators';
import { FilteredOrderDto, OrderDto, OrdersClient } from 'src/app/web-api-client';
import { WorkflowOrderModel } from './workflow-order.model';



class SearchResult {
  order!: WorkflowOrderModel;
}

@Injectable({ providedIn: 'root' })
export class WorkflowOrderGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _orders$ = new BehaviorSubject<WorkflowOrderModel>(new WorkflowOrderModel());
  createResult: Subject<any> = new Subject<any>();
  orderId: number = 0;


  constructor(private pipe: DecimalPipe, public client: OrdersClient) {
    debugger;
    this._search$.pipe(
      tap(() => this._loading$.next(true)),
      debounceTime(200),
      switchMap(() => this._search()),
      tap(() => this._loading$.next(false)),
    ).pipe().subscribe(result => {
      debugger;
      this._orders$.next(result.order);
    });

    this._search$.next();
  }

  get orders$() { return this._orders$.asObservable(); }
  get loading$() { return this._loading$.asObservable(); }



  private _search(): Observable<SearchResult> {
    debugger;
    return this.client.getOrderById(this.orderId)
      .pipe(
        map((result: OrderDto) => {
          if (!result) {
            return { order: new WorkflowOrderModel(), total: 0 };
          }
          const order = result;
          debugger;
          let searchResult = new SearchResult();
          searchResult = order as any;
          return  searchResult;
        }),
      );
  }

  

  public refreshOrders(orderId : number){
    this.orderId = orderId;
    this._search$.next();
  }
  
}
