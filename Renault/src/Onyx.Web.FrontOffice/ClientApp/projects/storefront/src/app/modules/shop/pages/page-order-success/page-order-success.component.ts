import { OrdermapperService } from 'projects/storefront/src/app/mapServieces/ordersCluster/ordermapper.service';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Observable, Subject, timer } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { map, switchMap, takeUntil } from 'rxjs/operators';
import { AccountApi } from '../../../../api';
import { Order } from '../../../../interfaces/order';
import { UrlService } from '../../../../services/url.service';
import { OrdersClient } from 'projects/storefront/src/app/web-api-client';

export interface PageOrderSuccessParams {
    orderToken: string;
}

@Component({
    selector: 'app-page-order-success',
    templateUrl: './page-order-success.component.html',
    styleUrls: ['./page-order-success.component.scss'],
})
export class PageOrderSuccessComponent implements OnInit, OnDestroy {
    private destroy$: Subject<void> = new Subject<void>();
    countdown: number = 20; 
    order!: Order;
    orderDetailsLink: string = "/";
    orderPaymentLink: string = "/";

    constructor(
        private route: ActivatedRoute,
        private accountApi: AccountApi,
        public url: UrlService,
        private ordersClient: OrdersClient,
        private ordermapperService: OrdermapperService,
        private router: Router
    ) {}

    ngOnInit(): void {
      
        const params$ = this.route.params as Observable<PageOrderSuccessParams>;

        params$.pipe(
            map((params: PageOrderSuccessParams) => params.orderToken || (this.route.snapshot.data['orderId'] as string)),
            switchMap(orderId => this.ordersClient.getOrderById(parseInt(orderId)).pipe(
                switchMap((res) => [this.ordermapperService.mapOrderById(res ?? [])])
            )),
            takeUntil(this.destroy$)
        ).subscribe(order => {
            this.order = order;
            this.orderDetailsLink = "/account/orders/" + order.id;
            this.orderPaymentLink = "/account/orderpayment/" + order.id;
        });

       
        timer(0, 1000).pipe(
            takeUntil(this.destroy$)
        ).subscribe((elapsedSeconds) => {
            this.countdown = 20 - elapsedSeconds;

            if (this.countdown <= 0) {
                this.navigateToOrderDetails(); 
            }
        });
    }

    private navigateToOrderDetails(): void {
        this.router.navigate([this.orderDetailsLink]); 
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    getStringPaymentType(value: number): string {
        switch (value) {
            case 0:
                return 'نامشخص';
            case 1:
                return 'نقدی';
            case 2:
                return 'اعتباری';
            case 3:
                return 'آنلاین';
            case 4:
                return 'اعتباری-آنلاین';
            default:
                return 'نامشخص';
        }
    }
}
