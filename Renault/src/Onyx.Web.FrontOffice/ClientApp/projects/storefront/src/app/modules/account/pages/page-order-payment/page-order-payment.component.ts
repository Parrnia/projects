import { CreateCreditOrderPayCommand } from './../../../../web-api-client';
import { ICreateOnlineOrderPayCommand, PaymentServiceType } from 'projects/storefront/src/app/web-api-client';  // اینترفیس‌ها را وارد کنید
import { Order } from './../../../../interfaces/order';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { OrdermapperService } from 'projects/storefront/src/app/mapServieces/ordersCluster/ordermapper.service';
import { CreateOnlineOrderPayCommand, CreditsClient, OrdersClient } from 'projects/storefront/src/app/web-api-client';
@Component({
  selector: 'app-page-order-payment',
  templateUrl: './page-order-payment.component.html',
  styleUrls: ['./page-order-payment.component.scss']
})
export class PageOrderPaymentComponent implements OnInit {
  banks = [
    { id: 1, name: 'آسان پرداخت', logo: 'assets/images/banks/up.png', paymentService: PaymentServiceType.AsanPardakht },
    { id: 2, name: 'پارسیان', logo: 'assets/images/banks/parsian.png', paymentService: PaymentServiceType.Parsian }
  ];
  availableCredit = 0;
  selectedPaymentMethod: string = '';
  selectedPaymentService: PaymentServiceType = PaymentServiceType.AsanPardakht;
  orderId: number = 0;
  userId = "";
  order!: Order;
  payableCost!: number;


  useWalletCredit = false;
  initialPayableCost: number = 0;
  initialcredit: number = 0;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private ordersClient: OrdersClient,
    private toastr: ToastrService,
    private creditsClient: CreditsClient,
    private ordermapperService: OrdermapperService
  ) { }

  ngOnInit(): void {
    this.userId = localStorage.getItem('userId') ?? "";
    this.route.params.subscribe(params => {
      this.orderId = params['orderId'];
    });
    this.creditsClient.getCreditByCustomerId(this.userId).subscribe({
      next: (res) => {
        this.availableCredit = res == null ? 0 : res / 10;
        this.initialcredit = this.availableCredit;
      }
    });
    this.ordersClient.getOrderById(this.orderId).subscribe({
      next: (res) => {
        this.order = this.ordermapperService.mapOrderById(res);
        this.initialPayableCost = this.order.total;
        this.payableCost = this.initialPayableCost;
      }
    });
  }

  onPaymentServiceChange(service: PaymentServiceType): void {
    this.selectedPaymentService = service;
  }

  onWalletPaymentChange(): void {
    if (this.useWalletCredit) {
      if (this.availableCredit >= this.payableCost) {
        this.payableCost = 0;
        this.availableCredit -= this.initialPayableCost;
      } else {
        this.payableCost -= this.availableCredit;
        this.availableCredit = 0;
      }
    } else {
      this.payableCost = this.initialPayableCost;
      this.availableCredit = this.initialcredit
    }
  }

  onSubmit(): void {
    debugger;
    if (this.payableCost === 0 && this.useWalletCredit && this.initialcredit > 0) {
      this.payOrderCredit();
    } 
    else if (this.payableCost !== 0 && this.useWalletCredit && this.initialcredit > 0) {
      if (this.selectedPaymentService) {
        this.payOrderCreditOnline(this.selectedPaymentService);
      } else {
        this.toastr.error('لطفاً یک سرویس پرداخت انتخاب کنید');
      }
    }
    else {
      if (this.selectedPaymentService) {
        this.payOrderOnline(this.selectedPaymentService);
      } else {
        this.toastr.error('لطفاً یک سرویس پرداخت انتخاب کنید');
      }
    }
  }

  registerOrder(): void {
    this.router.navigate([`/account/orders/${this.orderId}`]);
    this.toastr.success('سفارش با موفقیت ثبت شد');
  }

  deductCredit(): void {
    const command = new CreateCreditOrderPayCommand();
    command.orderId = this.orderId;
    command.customerId = localStorage.getItem("userId") ?? '';
    this.ordersClient.createCreditOrderPay(command).subscribe({
      next: (res) => {
        if (res === 1) {
          this.toastr.success('پرداخت با موفقیت انجام شد');
          this.router.navigate([`/orderconfirmation/${this.orderId}`]);
        } else {
          this.toastr.error('خطا در انجام پرداخت');
        }
      },
      error: (err) => {
        this.toastr.error('مشکلی در پردازش پرداخت پیش آمده است');
      }
    });
  }

  payOrderOnline(paymentService: PaymentServiceType): void {
    const cmd = new CreateOnlineOrderPayCommand();
    cmd.orderId = this.orderId;
    cmd.customerId = localStorage.getItem('userId') ?? "";
    cmd.paymentService = paymentService;

    this.ordersClient.createOnlineOrderPay(cmd).subscribe({
      next: (res) => {
        if (res.isSuccess && res.paymentUrl) {
          window.location.href = res.paymentUrl;
        } else {
          this.toastr.error(res.errorMessage);
        }
      },
      error: (err) => {
        this.toastr.error('خطا در پردازش پرداخت');
      }
    });
  }

  payOrderCreditOnline(paymentService: PaymentServiceType): void {
    const cmd = new CreateOnlineOrderPayCommand();
    cmd.orderId = this.orderId;
    cmd.customerId = localStorage.getItem('userId') ?? "";
    cmd.paymentService = paymentService;

    this.ordersClient.createCreditOnlineOrderPay(cmd).subscribe({
      next: (res) => {
        if (res.isSuccess && res.paymentUrl) {
          window.location.href = res.paymentUrl;
        } else {
          this.toastr.error(res.errorMessage);
        }
      },
      error: (err) => {
        this.toastr.error('خطا در پردازش پرداخت');
      }
    });
  }
  payOrderCredit(): void {
    const cmd = new CreateCreditOrderPayCommand();
    cmd.orderId = this.orderId;
    cmd.customerId = localStorage.getItem('userId') ?? "";

    this.ordersClient.createCreditOrderPay(cmd).subscribe({
      next: () => {
        this.registerOrder();
      },
      error: () => {
        this.toastr.error('خطا در پردازش پرداخت');
      }
    });
  }
}
