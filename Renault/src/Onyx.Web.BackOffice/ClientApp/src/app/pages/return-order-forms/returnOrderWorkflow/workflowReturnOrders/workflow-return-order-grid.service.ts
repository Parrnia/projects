import { Injectable, Input, PipeTransform } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { DecimalPipe } from '@angular/common';
import { debounceTime, delay, distinctUntilChanged, map, switchMap, takeUntil, tap } from 'rxjs/operators';
import { FilteredReturnOrderDto , ReturnOrderDto, ReturnOrdersClient } from 'src/app/web-api-client';
import { WorkflowReturnOrderModel } from './workflow-return-order.model';




class SearchResult {
  returnOrder!: WorkflowReturnOrderModel;
}


@Injectable({ providedIn: 'root' })
export class WorkflowReturnOrderGridService {
  private _loading$ = new BehaviorSubject<boolean>(true);
  private _search$ = new Subject<void>();
  private _returnOrder$ = new BehaviorSubject<WorkflowReturnOrderModel>(new WorkflowReturnOrderModel());
  createResult: Subject<any> = new Subject<any>();
  returnOrderId: number = 0;
  

  constructor(private pipe: DecimalPipe, public client: ReturnOrdersClient) {
    debugger;
    this._search$.pipe(
      tap(() => this._loading$.next(true)),
      debounceTime(200),
      switchMap(() => this._search()),
      tap(() => this._loading$.next(false)),
    ).pipe().subscribe(result => {
      debugger;
      this._returnOrder$.next(result.returnOrder);
    });
  }

  get returnOrder$() { return this._returnOrder$.asObservable(); }
  get loading$() { return this._loading$.asObservable(); }


  private _search(): Observable<SearchResult> {
    debugger;
    return this.client.getReturnOrderById(this.returnOrderId)
      .pipe(
        map((result: ReturnOrderDto) => {
          if (!result) {
            return { returnOrder: new WorkflowReturnOrderModel(), total: 0 };
          }
          const returnOrder = result;
          debugger;
          let searchResult = new SearchResult();
          searchResult.returnOrder = returnOrder as any;
          return  searchResult;
        }),
      );
  }

  

  public refreshReturnOrders(returnOrderId : number){
    this.returnOrderId = returnOrderId;
    this._search$.next();
  }
  
}
