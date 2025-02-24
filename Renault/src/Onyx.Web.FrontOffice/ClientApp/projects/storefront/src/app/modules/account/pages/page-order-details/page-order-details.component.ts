import { Component, OnDestroy, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { Subject } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { map, mergeMap, takeUntil } from 'rxjs/operators';
import { Order, OrderPayment, OrderStateBase } from '../../../../interfaces/order';
import { TranslateService } from '@ngx-translate/core';
import { OrdersClient, OrderStatus, PaymentType, SelfDeleteOrderCommand, UpdateOrderPriceAndCountCommand } from 'projects/storefront/src/app/web-api-client';
import { OrdermapperService } from 'projects/storefront/src/app/mapServieces/ordersCluster/ordermapper.service';
import { ToastrService } from 'ngx-toastr';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';


@Component({
  selector: 'app-page-order-details',
  templateUrl: './page-order-details.component.html',
  styleUrls: ['./page-order-details.component.scss'],
})
export class PageOrderDetailsComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();

  order!: Order;
  orderStateHistory!: OrderStateBase[];
  currentOrderStateBase!: OrderStateBase;
  currentOrderState!: OrderStatus;
  isDeleting = false;
  isUpdating = false;

  deleteModalRef?: BsModalRef;
  paymentsModalRef?: BsModalRef;
  paymentsModalRef1?: BsModalRef;
  payments: OrderPayment[] = [];
  selectedPayment: OrderPayment | null = null;
  
  showPopup: boolean = false;  // وضعیت نمایش پاپ‌آپ
  popupMessage: string = ''; // پیام پاپ‌آپ


  constructor(
    private route: ActivatedRoute,
    private translate: TranslateService,
    private ordersClient: OrdersClient,
    private ordermapperService: OrdermapperService,
    private toastr: ToastrService,
    private router: Router,
    private modalService: BsModalService
  ) {}
  @ViewChild('paymentDetailsTemplate') paymentDetailsTemplate!: TemplateRef<any>;

  ngOnInit(): void {
    this.route.params
      .pipe(
        map((params) => +params['id'] || this.route.snapshot.data['orderId']),
        mergeMap((orderId) => this.ordersClient.getOrderById(orderId)),
        takeUntil(this.destroy$)
      )
      .subscribe((order) => {
        this.order = this.ordermapperService.mapOrderById(order);
        this.payments = this.order.paymentHistory || [];
        this.currentOrderState = this.order.CurrentOrderStatus;
        this.currentOrderStateBase = this.order.orderStateHistory?.pop()!;
        this.orderStateHistory = this.order.orderStateHistory!;
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }


// Open the payments modal
openPaymentsModal(template: any): void {
  this.paymentsModalRef = this.modalService.show(template, { class: 'modal-lg' });
}


// Close the payments modal
closePaymentsModal(): void {
  this.paymentsModalRef1?.hide();
 
}
closePaymentsModal1(): void {
  this.paymentsModalRef?.hide();
 
}
  // Opens delete confirmation modal
  openDeleteConfirmationModal(template: TemplateRef<any>): void {
    this.deleteModalRef = this.modalService.show(template);
  }

   
  
  confirmDeleteOrder(): void {
    this.deleteModalRef?.hide();
    this.deleteOrder(this.order.id);
  }
  showPaymentDetailsModal(payment: OrderPayment): void {
    this.selectedPayment = payment;
    this.paymentsModalRef1 = this.modalService.show(this.paymentDetailsTemplate, { class: 'modal-md' });
  }
   // متد برای به روز رسانی سفارش
   updateOrder(orderId: number): void {
    this.isUpdating = true;
    let command = new UpdateOrderPriceAndCountCommand();
    command.orderId = orderId;
    command.customerId = localStorage.getItem("userId") ?? '';

    this.ordersClient.update(orderId, command).subscribe({
      next: (response) => {
        if (!response) {
          this.toastr.success('سفارش با موفقیت به روز شد');
          
          
          if (this.order.quantity === 0) {
            this.deleteOrder(orderId);
          }
        } else {
          this.toastr.error('به روزرسانی سفارش ناموفق بود');
        }
      },
      error: () => this.toastr.error('به روزرسانی سفارش ناموفق بود'),
      complete: () => (this.isUpdating = false),
    });
  }

  // متد برای حذف سفارش
  deleteOrder(orderId: number): void {
    this.isDeleting = true;
    let command = new SelfDeleteOrderCommand();
    command.id = orderId;
    command.customerId = localStorage.getItem("userId") ?? '';

    this.ordersClient.delete(orderId, command).subscribe({
      next: (response) => {
        if (!response) {
          this.toastr.success('سفارش با موفقیت حذف شد');
          this.router.navigateByUrl('/account/orders');
          this.popupMessage = 'به دلیل اینکه محصول مورد نظر موجود نیست، سفارش حذف شد.';
          this.showPopup = true; // نمایش پاپ‌آپ
        } else {
          this.toastr.error('حذف سفارش ناموفق بود');
        }
      },
      error: () => {
        this.toastr.error('حذف سفارش ناموفق بود');
        this.showPopup = false; // در صورت خطا پاپ‌آپ بسته می‌شود
      },
      complete: () => (this.isDeleting = false),
    });
  }

  // متد برای بستن پاپ‌آپ
  closePopup(): void {
    this.showPopup = false;
  }

  hasOnlinePayment(): boolean {
    return this.payments.some((payment) => payment.paymentType === PaymentType.Online);
  }

  getPaymentTypeName(type: PaymentType): string {
    switch (type) {
      case PaymentType.Cash:
        return 'نقدی';
      case PaymentType.Credit:
        return 'اعتباری';
      case PaymentType.Online:
        return 'آنلاین';
      default:
        return 'نامشخص';
    }
  }

  get orderNumber(): string {
    return this.translate.instant('FORMAT_ORDER_NUMBER', { number: this.order?.number });
  }
}