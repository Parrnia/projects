import { CancelOrderCommand, CompleteOrderCommand, ConfirmOrderCommand, ConfirmOrderPaymentCommand, OrdersClient, OrderStateBaseDto, OrderStatus, PrepareOrderCommand, RefundOrderCostCommand, ShipOrderCommand, UnConfirmOrderCommand, UnConfirmOrderPaymentCommand } from 'src/app/web-api-client';
import { ChangeDetectorRef, Component, Input, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import Swal from 'sweetalert2';
import { NgbAlertConfig, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { MatStepper } from '@angular/material/stepper';
import { ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-order-workflow',
  templateUrl: './order-workflow.component.html',
  styleUrls: ['./order-workflow.component.scss'],
})
export class OrderWorkflowComponent {
  @ViewChild('confirmOperation') confirmOperation: any = [];

  breadCrumbItems!: Array<{}>;
  orderData = {
    name: 'سفارش تستی',
    address: 'آدرس مشتری',
  };
  orderId: number = 0;
  orderstates: OrderStateBaseDto[] = [];
  currentStep: number = 0;
  steps: number[] = [];
  paymentConfirmed = false;
  currentOperationInfo = new OperatioInfo();
  inProgress = false;
  isLoaded = false;
  constructor(
    private _formBuilder: FormBuilder,
    private ordersClient: OrdersClient,
    private modalService: NgbModal,
    private route: ActivatedRoute
  ) {
    this.route.paramMap.subscribe(params => {
      this.orderId = parseFloat(params.get('id') ?? '0');
      this.ordersClient.getOrderStatesByOrderId(this.orderId).subscribe({
        next: (response) => {
          this.orderstates = response;
          this.orderstates.forEach(c => {
            this.updateSteps(c);
          });
          this.isLoaded = true;
        }
      })
    });
    

  }

  pendingForRegisterStateFormGroup!: FormGroup;
  pendingForConfirmPaymentStateFormGroup!: FormGroup;
  pendingForConfirmStateFormGroup!: FormGroup;
  pendingForPrepareStateFormGroup!: FormGroup;
  pendingForShipStateFormGroup!: FormGroup;
  pendingForRefundStateFormGroup!: FormGroup;
  pendingForCompletedStateFormGroup!: FormGroup;
  completedStateFormGroup!: FormGroup;

  pendingForRegisterState: string = '';
  pendingForConfirmPaymentState: string = '';
  pendingForConfirmState: string = '';
  pendingForPrepareState: string = '';
  pendingForShipState: string = '';
  pendingForRefundState: string = '';
  pendingForCompletedState: string = '';
  completedState: string = '';

  ngOnInit(): void {
    this.breadCrumbItems = [
      { label: " سفارش" },
      { label: "مسیر فرایند سفارش", active: true },
    ];
    this.pendingForRegisterStateFormGroup = this._formBuilder.group({
      details: ['', [Validators.required, Validators.maxLength(250), Validators.minLength(2)]],
    });

    this.pendingForConfirmPaymentStateFormGroup = this._formBuilder.group({
      details: ['', [Validators.required, Validators.maxLength(250), Validators.minLength(2)]],
    });

    this.pendingForConfirmStateFormGroup = this._formBuilder.group({
      details: ['', [Validators.required, Validators.maxLength(250), Validators.minLength(2)]],
    });

    this.pendingForPrepareStateFormGroup = this._formBuilder.group({
      details: ['', [Validators.required, Validators.maxLength(250), Validators.minLength(2)]],
    });

    this.pendingForShipStateFormGroup = this._formBuilder.group({
      details: ['', [Validators.required, Validators.maxLength(250), Validators.minLength(2)]],
    });

    this.pendingForRefundStateFormGroup = this._formBuilder.group({
      details: ['', [Validators.required, Validators.maxLength(250), Validators.minLength(2)]],
    });

    this.pendingForCompletedStateFormGroup = this._formBuilder.group({
      details: ['', [Validators.required, Validators.maxLength(250), Validators.minLength(2)]],
    });

    this.completedStateFormGroup = this._formBuilder.group({
      details: ['', [Validators.required, Validators.maxLength(250), Validators.minLength(2)]],
    });
  }

  get pendingForRegisterStateFormGroupControl() {
    return this.pendingForRegisterStateFormGroup.controls;
  }

  get pendingForConfirmPaymentStateFormGroupControl() {
    return this.pendingForConfirmPaymentStateFormGroup.controls;
  }

  get pendingForConfirmStateFormGroupControl() {
    return this.pendingForConfirmStateFormGroup.controls;
  }

  get pendingForPrepareStateFormGroupControl() {
    return this.pendingForPrepareStateFormGroup.controls;
  }

  get pendingForShipStateFormGroupControl() {
    return this.pendingForShipStateFormGroup.controls;
  }

  get pendingForRefundStateFormGroupControl() {
    return this.pendingForRefundStateFormGroup.controls;
  }

  get pendingForCompletedStateFormGroupControl() {
    return this.pendingForCompletedStateFormGroup.controls;
  }

  get completedStateFormGroupControl() {
    return this.completedStateFormGroup.controls;
  }

  updateSteps(orderState: OrderStateBaseDto) {
    if (orderState.orderStatus == 0) {
      this.pendingForRegisterState = ((orderState.orderStatusName ?? '') + ' در تاریخ ' + orderState.created);
      debugger;
      this.currentStep = 0;
    }

    if (orderState.orderStatus == 1) {
      this.pendingForConfirmPaymentState = (orderState.orderStatusName ?? '') + ' در تاریخ ' + orderState.created;

      this.pendingForRegisterStateFormGroup.controls["details"].setValue(orderState.details);
      this.pendingForRegisterStateFormGroup.controls["details"].disable();

      this.currentStep = 1;
    }

    if (orderState.orderStatus == 2) {
      this.paymentConfirmed = true;
      this.pendingForConfirmState = (orderState.orderStatusName ?? '') + ' در تاریخ ' + orderState.created;
      debugger;
      this.pendingForConfirmPaymentStateFormGroup.controls["details"].setValue(orderState.details);
      this.pendingForConfirmPaymentStateFormGroup.controls["details"].disable();

      this.currentStep = 2;
    }

    if (orderState.orderStatus == 3) {
      this.pendingForPrepareState = (orderState.orderStatusName ?? '') + ' در تاریخ ' + orderState.created;

      this.pendingForConfirmStateFormGroup.controls["details"].setValue(orderState.details);
      this.pendingForConfirmStateFormGroup.controls["details"].disable();

      this.currentStep = 3;
    }

    if (orderState.orderStatus == 4) {
      this.pendingForShipState = (orderState.orderStatusName ?? '') + ' در تاریخ ' + orderState.created;

      this.pendingForPrepareStateFormGroup.controls["details"].setValue(orderState.details);
      this.pendingForPrepareStateFormGroup.controls["details"].disable();

      this.currentStep = 4;
    }

    if (orderState.orderStatus == 5) {
      this.pendingForCompletedState = (orderState.orderStatusName ?? '') + ' در تاریخ ' + orderState.created;

      this.pendingForShipStateFormGroup.controls["details"].setValue(orderState.details);
      this.pendingForShipStateFormGroup.controls["details"].disable();

      this.currentStep = 6;
    }

    if (orderState.orderStatus == 6) {
      this.pendingForCompletedState = (orderState.orderStatusName ?? '') + ' در تاریخ ' + orderState.created;

      this.pendingForConfirmPaymentStateFormGroup.controls["details"].setValue(orderState.details);
      this.pendingForConfirmPaymentStateFormGroup.controls["details"].disable();

      this.currentStep = 6;
    }

    if (orderState.orderStatus == 7) {
      this.pendingForRefundState = (orderState.orderStatusName ?? '') + ' در تاریخ ' + orderState.created;

      this.pendingForConfirmStateFormGroup.controls["details"].setValue(orderState.details);
      this.pendingForConfirmStateFormGroup.controls["details"].disable();

      this.currentStep = 5;
    }

    if (orderState.orderStatus == 8) {
      if (this.paymentConfirmed) {
        this.pendingForRefundState = (orderState.orderStatusName ?? '') + ' در تاریخ ' + orderState.created;

        this.currentStep = 5;
      } else {
        this.pendingForCompletedState = (orderState.orderStatusName ?? '') + ' در تاریخ ' + orderState.created;

        this.currentStep = 6;
      }
      switch (this.orderstates[this.orderstates.indexOf(orderState) - 1].orderStatus) {
        case 0:
          this.pendingForRegisterStateFormGroup.controls["details"].setValue(orderState.details);
          this.pendingForRegisterStateFormGroup.controls["details"].disable();
          break;
        case 1:
          this.pendingForConfirmPaymentStateFormGroup.controls["details"].setValue(orderState.details);
          this.pendingForConfirmPaymentStateFormGroup.controls["details"].disable();
          break;
        case 2:
          this.pendingForConfirmStateFormGroup.controls["details"].setValue(orderState.details);
          this.pendingForConfirmStateFormGroup.controls["details"].disable();
          break;
        case 3:
          this.pendingForPrepareStateFormGroup.controls["details"].setValue(orderState.details);
          this.pendingForPrepareStateFormGroup.controls["details"].disable();
          break;
        case 4:
          this.pendingForShipStateFormGroup.controls["details"].setValue(orderState.details);
          this.pendingForShipStateFormGroup.controls["details"].disable();
          break;
      }
    }

    if (orderState.orderStatus == 9) {
      this.pendingForCompletedState = (orderState.orderStatusName ?? '') + ' در تاریخ ' + orderState.created;

      this.pendingForRefundStateFormGroup.controls["details"].setValue(orderState.details);
      this.pendingForRefundStateFormGroup.controls["details"].disable();

      this.currentStep = 6;
    }

    if (orderState.orderStatus == 10) {

      this.pendingForCompletedStateFormGroup.controls["details"].setValue(orderState.details);
      this.pendingForCompletedStateFormGroup.controls["details"].disable();
      this.currentStep = 7;
    }

    this.steps.push(this.currentStep);
  }
  confirmPayment(details: string): void {
    this.inProgress = true;
    let cmd = new ConfirmOrderPaymentCommand();
    cmd.id = this.orderId;
    cmd.details = details;
    this.ordersClient.confirmPayment(this.orderId, cmd).subscribe({
      next: (response) => {
        this.inProgress = false;
        this.modalService.dismissAll();
        if (response == null) {
          Swal.fire({
            title: 'تایید پرداخت سفارش با موفقیت انجام شد',
            icon: 'success',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });

          this.ordersClient.getCurrentOrderStateByOrderId(this.orderId).subscribe({
            next: (response) => {
              if (response != null) {
                this.updateSteps(response);
              }
            }
          })
        } else {
          Swal.fire({
            title: ' تایید پرداخت سفارش با خطا مواجه شد',
            icon: 'error',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });
        }
      },
      error: (error) => {
        this.inProgress = false;
      }
    }
    );
  }

  unConfirmPayment(details: string): void {
    this.inProgress = true;
    let cmd = new UnConfirmOrderPaymentCommand();
    cmd.id = this.orderId;
    cmd.details = details;
    this.ordersClient.unConfirmPayment(this.orderId, cmd).subscribe({
      next: (response) => {
        this.inProgress = false;
        this.modalService.dismissAll();
        if (response == null) {
          Swal.fire({
            title: 'رد پرداخت سفارش با موفقیت انجام شد',
            icon: 'success',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });

          this.ordersClient.getCurrentOrderStateByOrderId(this.orderId).subscribe({
            next: (response) => {
              if (response != null) {
                this.updateSteps(response);
              }
            }
          })
        } else {
          Swal.fire({
            title: ' رد پرداخت سفارش با خطا مواجه شد',
            icon: 'error',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });
        }
      },
      error: (error) => {
        this.inProgress = false;
      }
    }
    );
  }

  cancel(details: string): void {
    this.inProgress = true;
    let cmd = new CancelOrderCommand();
    cmd.id = this.orderId;
    cmd.details = details;
    this.ordersClient.cancel(this.orderId, cmd).subscribe({
      next: (response) => {
        this.inProgress = false;
        this.modalService.dismissAll();
        if (response == null) {
          Swal.fire({
            title: 'لغو سفارش با موفقیت انجام شد',
            icon: 'success',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });

          this.ordersClient.getCurrentOrderStateByOrderId(this.orderId).subscribe({
            next: (response) => {
              if (response != null) {
                this.updateSteps(response);
              }
            }
          })
        } else {
          Swal.fire({
            title: ' لغو سفارش با خطا مواجه شد',
            icon: 'error',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });
        }
      },
      error: (error) => {
        this.inProgress = false;
      }
    }
    );
  }
  confirm(details: string): void {
    this.inProgress = true;
    let cmd = new ConfirmOrderCommand();
    cmd.id = this.orderId;
    cmd.details = details;
    this.ordersClient.confirm(this.orderId, cmd).subscribe({
      next: (response) => {
        this.inProgress = false;
        this.modalService.dismissAll();
        if (response == null) {
          Swal.fire({
            title: 'تایید سفارش با موفقیت انجام شد',
            icon: 'success',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });

          this.ordersClient.getCurrentOrderStateByOrderId(this.orderId).subscribe({
            next: (response) => {
              if (response != null) {
                this.updateSteps(response);
              }
            }
          })
        } else {
          Swal.fire({
            title: ' تایید سفارش با خطا مواجه شد',
            icon: 'error',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });
        }
      },
      error: (error) => {
        this.inProgress = false;
      }
    }
    );
  }

  unConfirm(details: string): void {
    this.inProgress = true;
    let cmd = new UnConfirmOrderCommand();
    cmd.id = this.orderId;
    cmd.details = details;
    this.ordersClient.unConfirm(this.orderId, cmd).subscribe({
      next: (response) => {
        this.inProgress = false;
        this.modalService.dismissAll();
        if (response == null) {
          Swal.fire({
            title: 'رد سفارش با موفقیت انجام شد',
            icon: 'success',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });

          this.ordersClient.getCurrentOrderStateByOrderId(this.orderId).subscribe({
            next: (response) => {
              if (response != null) {
                this.updateSteps(response);
              }
            }
          })
        } else {
          Swal.fire({
            title: ' رد سفارش با خطا مواجه شد',
            icon: 'error',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });
        }
      },
      error: (error) => {
        this.inProgress = false;
      }
    }
    );
  }

  prepare(details: string): void {
    this.inProgress = true;
    let cmd = new PrepareOrderCommand();
    cmd.id = this.orderId;
    cmd.details = details;
    this.ordersClient.prepare(this.orderId, cmd).subscribe({
      next: (response) => {
        this.inProgress = false;
        this.modalService.dismissAll();
        if (response == null) {
          Swal.fire({
            title: 'تایید سفارش با موفقیت انجام شد',
            icon: 'success',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });

          this.ordersClient.getCurrentOrderStateByOrderId(this.orderId).subscribe({
            next: (response) => {
              if (response != null) {
                this.updateSteps(response);
              }
            }
          })
        } else {
          Swal.fire({
            title: ' تایید سفارش با خطا مواجه شد',
            icon: 'error',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });
        }
      },
      error: (error) => {
        this.inProgress = false;
      }
    }
    );
  }

  ship(details: string): void {
    this.inProgress = true;
    let cmd = new ShipOrderCommand();
    cmd.id = this.orderId;
    cmd.details = details;
    this.ordersClient.ship(this.orderId, cmd).subscribe({
      next: (response) => {
        this.inProgress = false;
        this.modalService.dismissAll();
        if (response == null) {
          Swal.fire({
            title: 'ارسال سفارش با موفقیت انجام شد',
            icon: 'success',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });

          this.ordersClient.getCurrentOrderStateByOrderId(this.orderId).subscribe({
            next: (response) => {
              if (response != null) {
                this.updateSteps(response);
              }
            }
          })
        } else {
          Swal.fire({
            title: ' ارسال سفارش با خطا مواجه شد',
            icon: 'error',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });
        }
      },
      error: (error) => {
        this.inProgress = false;
      }
    }
    );
  }

  refundCost(details: string): void {
    this.inProgress = true;
    let cmd = new RefundOrderCostCommand();
    cmd.id = this.orderId;
    cmd.details = details;
    this.ordersClient.refundCost(this.orderId, cmd).subscribe({
      next: (response) => {
        this.inProgress = false;
        this.modalService.dismissAll();
        if (response == null) {
          Swal.fire({
            title: 'بازگشت وجه سفارش با موفقیت انجام شد',
            icon: 'success',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });

          this.ordersClient.getCurrentOrderStateByOrderId(this.orderId).subscribe({
            next: (response) => {
              if (response != null) {
                this.updateSteps(response);
              }
            }
          })
        } else {
          Swal.fire({
            title: ' بازگشت وجه سفارش با خطا مواجه شد',
            icon: 'error',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });
        }
      },
      error: (error) => {
        this.inProgress = false;
      }
    }
    );
  }


  complete(details: string): void {
    this.inProgress = true;
    let cmd = new CompleteOrderCommand();
    cmd.id = this.orderId;
    cmd.details = details;
    this.ordersClient.complete(this.orderId, cmd).subscribe({
      next: (response) => {
        this.inProgress = false;
        this.modalService.dismissAll();
        if (response == null) {
          Swal.fire({
            title: 'تکمیل فرایند سفارش با موفقیت انجام شد',
            icon: 'success',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });

          this.ordersClient.getCurrentOrderStateByOrderId(this.orderId).subscribe({
            next: (response) => {
              if (response != null) {
                this.updateSteps(response);
              }
            }
          })
        } else {
          Swal.fire({
            title: 'تکمیل فرایند سفارش با خطا مواجه شد',
            icon: 'error',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });
        }
      },
      error: (error) => {
        this.inProgress = false;
      }
    }
    );
  }

  openConfirmOperationModal(operationId: number, formGroup: FormGroup) {
    this.inProgress = true;
    this.markAllControlsAsTouched(formGroup);
    debugger;
    if (formGroup.valid) {
      this.currentOperationInfo.operationId = operationId;
      this.currentOperationInfo.details = formGroup.controls["details"].value;
      this.modalService.open(this.confirmOperation, { size: 'md', backdrop: 'static' });
      this.inProgress = false;
    } else {
      this.inProgress = false;
    }

  }

  operate() {
    switch (this.currentOperationInfo.operationId) {
      case 1:
        this.confirmPayment(this.currentOperationInfo.details);
        break;
      case 2:
        this.unConfirmPayment(this.currentOperationInfo.details);
        break;
      case 3:
        this.cancel(this.currentOperationInfo.details);
        break;
      case 4:
        this.confirm(this.currentOperationInfo.details);
        break;
      case 5:
        this.unConfirm(this.currentOperationInfo.details);
        break;
      case 6:
        this.prepare(this.currentOperationInfo.details);
        break;
      case 7:
        this.ship(this.currentOperationInfo.details);
        break;
      case 8:
        this.refundCost(this.currentOperationInfo.details);
        break;
      case 9:
        this.complete(this.currentOperationInfo.details);
        break;
    }
  }
  markAllControlsAsTouched(formGroup: FormGroup): void {
    Object.values(formGroup.controls).forEach(control => {
      if (control instanceof FormGroup) {
        this.markAllControlsAsTouched(control);
      } else {
        control.markAsTouched();
      }
    });
  }
}
export class OperatioInfo {
  operationId!: number;
  details!: string;
}