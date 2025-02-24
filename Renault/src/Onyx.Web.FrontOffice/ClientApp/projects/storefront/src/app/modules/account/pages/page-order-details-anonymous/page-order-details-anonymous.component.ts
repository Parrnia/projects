import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { map, mergeMap, takeUntil } from 'rxjs/operators';
import { Order, OrderStateBase } from '../../../../interfaces/order';
import { TranslateService } from '@ngx-translate/core';
import { OrdersClient } from 'projects/storefront/src/app/web-api-client';
import { OrdermapperService } from 'projects/storefront/src/app/mapServieces/ordersCluster/ordermapper.service';

@Component({
    selector: 'app-page-order-details-anonymous',
    templateUrl: './page-order-details-anonymous.component.html',
    styleUrls: ['./page-order-details-anonymous.component.scss'],
})
export class PageOrderDetailsAnonymousComponent implements OnInit, OnDestroy {
    private destroy$: Subject<void> = new Subject<void>();

    order!: Order;
    orderStateHistory!: OrderStateBase[];
    currentOrderState!: OrderStateBase;
    get orderNumber(): string {
        return this.translate.instant('FORMAT_ORDER_NUMBER', { number: this.order?.number });
    }

    constructor(
        private route: ActivatedRoute,
        private translate: TranslateService,
        private ordersClient : OrdersClient,
        private ordermapperService : OrdermapperService
    ) { }

    ngOnInit(): void {
        this.route.params.pipe(
            map(params => {
                debugger;
                const number = params['number'] || this.route.snapshot.data['number'];
                const phoneNumber = params['phoneNumber'] || this.route.snapshot.data['phoneNumber'];
                return { number, phoneNumber };
            }),
            mergeMap(({ number, phoneNumber }) => this.ordersClient.getOrderByNumber(number, phoneNumber)),
            takeUntil(this.destroy$)
        ).subscribe(order => {
            this.order = this.ordermapperService.mapOrderById(order);
            debugger;
            this.currentOrderState = this.order.orderStateHistory?.pop()!;
            this.orderStateHistory = this.order.orderStateHistory!;
        });
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }
}
