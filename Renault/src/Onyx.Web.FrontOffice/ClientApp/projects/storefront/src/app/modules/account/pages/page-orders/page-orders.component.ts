import { FileResponse, OrderByCustomerIdWithPaginationDto, SelfDeleteOrderCommand, UpdateOrderPriceAndCountCommand } from './../../../../web-api-client';
import { Component, OnDestroy, OnInit, TemplateRef } from '@angular/core';
import { AccountApi, GetOrdersListOptions } from '../../../../api';
import { merge, Observable, of, Subject } from 'rxjs';
import { OrdersList } from '../../../../interfaces/list';
import { distinctUntilChanged, mergeMap, switchMap, takeUntil } from 'rxjs/operators';
import { FormControl } from '@angular/forms';
import { UrlService } from '../../../../services/url.service';
import { OrderByCustomerIdDto, OrdersClient } from 'projects/storefront/src/app/web-api-client';
import { OrdermapperService } from 'projects/storefront/src/app/mapServieces/ordersCluster/ordermapper.service';
import { Order } from 'projects/storefront/src/app/interfaces/order';
import { ChangeDetectorRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Router } from '@angular/router';

@Component({
    selector: 'app-page-orders',
    templateUrl: './page-orders.component.html',
    styleUrls: ['./page-orders.component.scss'],
})
export class PageOrdersComponent implements OnInit, OnDestroy {
    private destroy$: Subject<void> = new Subject<void>();
    currentPage: FormControl = new FormControl(1);
    list!: OrdersList;
    isDeleting: { [orderId: number]: boolean } = {}; // Track individual delete loading
    isUpdating: { [orderId: number]: boolean } = {}; // Track individual update loading
    deleteModalRef?: BsModalRef;
    orderid ?:number ;
 
        constructor(
        public url: UrlService,
        private ordersClient: OrdersClient,
        private ordermapperService: OrdermapperService,
        private modalService: BsModalService ,
        private router: Router,
        private cdRef: ChangeDetectorRef // Inject ChangeDetectorRef to manually trigger change detection
    ) { }

    ngOnInit(): void {
        this.loadOrders();
    }

    loadOrders() {
        merge(
            of(this.currentPage.value),
            this.currentPage.valueChanges,
        ).pipe(
            distinctUntilChanged(),
            mergeMap(page => this.getOrdersList({
                limit: 5,
                page,
            })),
            takeUntil(this.destroy$),
        ).subscribe(x => {
            this.list = x;
        });
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    openDeleteConfirmationModal(template: TemplateRef<any>, orderId: number): void {
        this.deleteModalRef = this.modalService!.show(template); // استفاده از !
        this.orderid = orderId;
      }
      

      confirmDeleteOrder(): void {
        this.deleteModalRef?.hide();
        this.deleteOrder(this.orderid ?? 0);
      
      }
    deleteOrder(orderId: number): void {
        console.log(this.orderid );
        this.isDeleting[orderId] = true; // Show loading state for delete button
        let command = new SelfDeleteOrderCommand();
        command.id = orderId;
        command.customerId = localStorage.getItem("userId") ?? '';

        this.ordersClient.delete(orderId, command).subscribe({
            next: () => {
                console.log('Order deleted successfully');
                // Update the list without full reload: Remove the deleted order from the list
                this.list.items = this.list.items.filter(order => order.id !== orderId);
                this.list.total -= 1; // Decrease total count
                
                // Manually trigger change detection to immediately update the UI
                this.cdRef.detectChanges();
                this.isDeleting[orderId] = false;
            },
            error: (err) => {
                console.error('Error deleting order', err);
                this.isDeleting[orderId] = false;
            }
        });
    }

    updateOrder(id: number): void {
        this.isUpdating[id] = true; 
        let command = new UpdateOrderPriceAndCountCommand();
        command.orderId = id;
        command.customerId = localStorage.getItem("userId") ?? '';
        this.ordersClient.update(id, command).subscribe({
            next: (response) => {
                console.log('Order updated successfully', response);
                const index = this.list.items.findIndex(order => order.id === id);
                this.isUpdating[id] = false; 
            },
            error: (err) => {
                console.error('Error updating order', err);
                this.isUpdating[id] = false; 
            }
        });
    }
    

    payOrder(id: number){
        this.router.navigateByUrl('/account/orderpayment/' + id);
    }

    getOrdersList(options?: GetOrdersListOptions): Observable<OrdersList> {
        options = options || {};

        return this.ordersClient.getOrdersByCustomerIdWithPagination(options.page, options.limit, undefined).pipe(
            switchMap(res => {
                const items: Order[] = this.ordermapperService.mapOrdersByCustomerIdWithPagination(res.items!);
                const page = options?.page || 1;
                const limit = options?.limit || 5;
                const sort = options?.sort || 'default';
                const total = res.totalCount ?? 0;
                const pages = Math.ceil(total / limit);
                const from = (page - 1) * limit + 1;
                const to = Math.min(page * limit, total);

                return of({
                    page,
                    limit,
                    sort,
                    total,
                    pages,
                    from,
                    to,
                    items,
                });
            })
        );
    }
}
