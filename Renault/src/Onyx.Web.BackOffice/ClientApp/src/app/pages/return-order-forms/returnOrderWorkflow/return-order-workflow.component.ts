import { ChangeDetectorRef, Component, Input, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import Swal from 'sweetalert2';
import { NgbAlertConfig, NgbModal, NgbModalRef } from "@ng-bootstrap/ng-bootstrap";
import { MatStepper } from '@angular/material/stepper';
import { ActivatedRoute } from '@angular/router';
import { ReturnOrderStateBaseDto, ReturnOrdersClient, CancelReturnOrderCommand, RefundReturnOrderCostCommand, CompleteReturnOrderCommand, AcceptReturnOrderCommand, RejectReturnOrderCommand, SendReturnOrderCommand, ReceiveReturnOrderCommand, ConfirmAllReturnOrderCommand, ConfirmSomeReturnOrderCommand, CostRefundType, FileUploadMetadataDto, ReturnOrderTransportationType, ReturnOrderDto, DocumentCommandForTotal, ReturnOrderTotalDto, ReturnOrderTotalType, SendReturnOrderTotalDto } from 'src/app/web-api-client';
import { ReturnOrderTotalDocumentModel } from '../acceptedReturnOrders/return-order-total-document.model';
import { ImageService } from 'src/app/core/services/image.service';
import { forkJoin } from 'rxjs';
export type CostRefundTypeKey = keyof typeof CostRefundType;
export type ReturnOrderTransportationTypeKey = keyof typeof ReturnOrderTransportationType;

@Component({
  selector: 'app-return-order-workflow',
  templateUrl: './return-order-workflow.component.html',
  styleUrls: ['./return-order-workflow.component.scss'],
})
export class ReturnOrderWorkflowComponent {
  costRefundTypes = CostRefundType;
  documentForm!: FormGroup;

  @ViewChild('confirmOperation') confirmOperation: any = [];
  @ViewChild('documentFormModal') documentFormModal: any = [];
  @ViewChild("confirmationModal") confirmationModal: any;
  confirmationModalRef: NgbModalRef | undefined;

  breadCrumbItems!: Array<{}>;
  returnOrderId: number = 0;
  returnOrder!: ReturnOrderDto;
  returnOrderTotalForSend?: SendReturnOrderTotalDto;
  returnOrderstates: ReturnOrderStateBaseDto[] = [];
  currentStep: number = 0;
  steps: number[] = [];
  paymentConfirmed = false;
  currentOperationInfo = new OperatioInfo();
  inProgress = false;
  isLoaded = false;
  documentList: ReturnOrderTotalDocumentModel[] = [];
  selectedFileUrl: string | undefined = undefined;
  checkedItems: Set<string> = new Set<string>();
  submit!: boolean;
  selectedId: any = 0;
  returnOrderTransportationTypes = ReturnOrderTransportationType;
  isInSendStep = true;
  constructor(
    private _formBuilder: FormBuilder,
    private returnOrdersClient: ReturnOrdersClient,
    private modalService: NgbModal,
    private route: ActivatedRoute,
    private imageService: ImageService
  ) {
    this.route.paramMap.subscribe(params => {
      this.returnOrderId = parseFloat(params.get('id') ?? '0');
      let res = forkJoin(
        this.returnOrdersClient.getReturnOrderById(this.returnOrderId),
        this.returnOrdersClient.getSendReturnOrderTotalByReturnOrderId(this.returnOrderId)
      );
      res.subscribe({
        next: (response) => {
          this.returnOrder = response[0];
          this.returnOrderstates = response[0].returnOrderStateHistory ?? [];

          this.returnOrderTotalForSend = response[1];
          this.returnOrderstates.forEach(c => {
            this.updateSteps(c);
          });
          this.isLoaded = true;
        }
      });
    });

  }

  getTransNumericKeys(): ReturnOrderTransportationTypeKey[] {
    return Object.entries(this.returnOrderTransportationTypes)
      .filter(([key, value]) => typeof value === "number")
      .map(([key, value]) => key as ReturnOrderTransportationTypeKey);
  }

  mapTransNumericKeyToString(key: ReturnOrderTransportationTypeKey): string {
    return this.mapTransNemberKeyToString(
      this.returnOrderTransportationTypes[key].toString()
    );
  }
  mapTransNemberKeyToString(key: string): string {
    switch (parseInt(key)) {
      case ReturnOrderTransportationType.NotDetermined:
        return "مشخص نشده";
      case ReturnOrderTransportationType.CustomerReturn:
        return "ارسال بر عهده مشتری";
      case ReturnOrderTransportationType.OrganizationReturn:
        return "ارسال بر عهده سازمان";
      case ReturnOrderTransportationType.OnLocation:
        return "سازمان کالا را در محل تحویل گرفته";
      default:
        return "";
    }
  }

  getNumericKeys(): CostRefundTypeKey[] {
    return Object.entries(this.costRefundTypes)
      .filter(([key, value]) => typeof value === 'number')
      .map(([key, value]) => key as CostRefundTypeKey);
  }

  mapNumericKeyToString(key: CostRefundTypeKey): string {
    return this.mapNemberKeyToString(this.costRefundTypes[key].toString());
  }
  mapNemberKeyToString(key: string): string {
    switch (parseInt(key)) {
      case CostRefundType.NotDetermined:
        return 'مشخص نشده';
      case CostRefundType.Cash:
        return 'نقد';
      case CostRefundType.Credit:
        return 'اعتباری';
      case CostRefundType.Online:
        return 'اینترنتی';
      default:
        return '';
    }
  }

  pendingForRegisterStateFormGroup!: FormGroup;
  pendingForExpertRequestConfirmStateFormGroup!: FormGroup;
  pendingForSendStateFormGroup!: FormGroup;
  pendingForReceiveStateFormGroup!: FormGroup;
  pendingForExpertProductConfirmStateFormGroup!: FormGroup;
  pendingForRefundCostStateFormGroup!: FormGroup;
  pendingForCompletedStateFormGroup!: FormGroup;
  completedStateFormGroup!: FormGroup;

  pendingForRegisterState: string = '';
  pendingForExpertRequestConfirmState: string = '';
  pendingForSendState: string = '';
  pendingForReceiveState: string = '';
  pendingForExpertProductConfirmState: string = '';
  pendingForRefundCostState: string = '';
  pendingForCompletedState: string = '';
  completedState: string = '';

  ngOnInit(): void {
    this.breadCrumbItems = [
      { label: " سفارش بازگشت" },
      { label: "مسیر فرایند سفارش بازگشت بازگشت ", active: true },
    ];
    this.pendingForRegisterStateFormGroup = this._formBuilder.group({
      details: ['', [Validators.required, Validators.maxLength(250), Validators.minLength(2)]],
    });

    this.pendingForExpertRequestConfirmStateFormGroup = this._formBuilder.group({
      details: ['', [Validators.required, Validators.maxLength(250), Validators.minLength(2)]],
    });

    this.pendingForSendStateFormGroup = this._formBuilder.group({
      details: ['', [Validators.required, Validators.maxLength(250), Validators.minLength(2)]],
      isReturnShippingPriceEntered: false,
      returnShippingPrice: [null, [this.returnShippingPriceValidator()]],
      returnOrderTransportationType: null
    });
    this.pendingForSendStateFormGroup.get('isReturnShippingPriceEntered')?.valueChanges.subscribe((isEntered) => {
      const returnShippingPriceControl = this.pendingForSendStateFormGroup.get('returnShippingPrice');

      if (isEntered) {
        returnShippingPriceControl?.setValidators([this.returnShippingPriceValidator()]);
      } else {
        returnShippingPriceControl?.clearValidators();
        returnShippingPriceControl?.markAsUntouched();
      }
      debugger;
      returnShippingPriceControl?.updateValueAndValidity();
    });

    this.pendingForReceiveStateFormGroup = this._formBuilder.group({
      details: ['', [Validators.required, Validators.maxLength(250), Validators.minLength(2)]],
    });

    this.pendingForExpertProductConfirmStateFormGroup = this._formBuilder.group({
      details: ['', [Validators.required, Validators.maxLength(250), Validators.minLength(2)]],
    });

    this.pendingForRefundCostStateFormGroup = this._formBuilder.group({
      details: ['', [Validators.required, Validators.maxLength(250), Validators.minLength(2)]],
      costRefundType: [null, [Validators.required]],
    });

    this.pendingForCompletedStateFormGroup = this._formBuilder.group({
      details: ['', [Validators.required, Validators.maxLength(250), Validators.minLength(2)]],
    });

    this.completedStateFormGroup = this._formBuilder.group({
      details: ['', [Validators.required, Validators.maxLength(250), Validators.minLength(2)]],
    });
    this.documentForm = this._formBuilder.group({
      id: 0,
      image: ["", [Validators.required]],
      description: ["", [Validators.required]],
    });
  }

  get pendingForRegisterStateFormGroupControl() {
    return this.pendingForRegisterStateFormGroup.controls;
  }

  get pendingForExpertRequestConfirmStateFormGroupControl() {
    return this.pendingForExpertRequestConfirmStateFormGroup.controls;
  }

  get pendingForSendStateFormGroupControl() {
    return this.pendingForSendStateFormGroup.controls;
  }

  get pendingForReceiveStateFormGroupControl() {
    return this.pendingForReceiveStateFormGroup.controls;
  }

  get pendingForExpertProductConfirmStateFormGroupControl() {
    return this.pendingForExpertProductConfirmStateFormGroup.controls;
  }

  get pendingForRefundCostStateFormGroupControl() {
    return this.pendingForRefundCostStateFormGroup.controls;
  }

  get pendingForCompletedStateFormGroupControl() {
    return this.pendingForCompletedStateFormGroup.controls;
  }

  get completedStateFormGroupControl() {
    return this.completedStateFormGroup.controls;
  }

  get documentFormControl() {
    return this.documentForm.controls;
  }


  updateSteps(returnOrderState: ReturnOrderStateBaseDto) {
    if (returnOrderState.returnOrderStatus == 0) {
      this.pendingForRegisterState = ((returnOrderState.returnOrderStatusName ?? '') + ' در تاریخ ' + returnOrderState.created);
      debugger;
      this.currentStep = 0;
    }

    if (returnOrderState.returnOrderStatus == 1) {
      this.pendingForExpertRequestConfirmState = (returnOrderState.returnOrderStatusName ?? '') + ' در تاریخ ' + returnOrderState.created;

      this.pendingForRegisterStateFormGroup.controls["details"].setValue(returnOrderState.details);
      this.pendingForRegisterStateFormGroup.controls["details"].disable();

      this.currentStep = 1;
    }

    if (returnOrderState.returnOrderStatus == 2) {
      this.pendingForCompletedState = (returnOrderState.returnOrderStatusName ?? '') + ' در تاریخ ' + returnOrderState.created;
      debugger;
      this.pendingForExpertRequestConfirmStateFormGroup.controls["details"].setValue(returnOrderState.details);
      this.pendingForExpertRequestConfirmStateFormGroup.controls["details"].disable();

      this.currentStep = 6;
    }

    if (returnOrderState.returnOrderStatus == 3) {
      this.pendingForSendState = (returnOrderState.returnOrderStatusName ?? '') + ' در تاریخ ' + returnOrderState.created;

      this.pendingForExpertRequestConfirmStateFormGroup.controls["details"].setValue(returnOrderState.details);
      this.pendingForExpertRequestConfirmStateFormGroup.controls["details"].disable();

      this.currentStep = 2;
    }

    if (returnOrderState.returnOrderStatus == 4) {
      this.pendingForReceiveState = (returnOrderState.returnOrderStatusName ?? '') + ' در تاریخ ' + returnOrderState.created;

      this.pendingForSendStateFormGroup.controls["details"].setValue(returnOrderState.details);
      this.pendingForSendStateFormGroup.controls["details"].disable();
      this.pendingForSendStateFormGroup.controls["returnOrderTransportationType"].setValue(this.returnOrder.returnOrderTransportationType);
      this.pendingForSendStateFormGroup.controls["returnOrderTransportationType"].disable();
      if(this.returnOrderTotalForSend){
        this.pendingForSendStateFormGroup.controls["isReturnShippingPriceEntered"].setValue(true);
      }
      this.pendingForSendStateFormGroup.controls["returnShippingPrice"].setValue(this.returnOrderTotalForSend?.price);
      this.pendingForSendStateFormGroup.controls["returnShippingPrice"].disable();
      this.documentList = this.returnOrderTotalForSend?.returnOrderTotalDocuments?.map(c => {
        let doc = new ReturnOrderTotalDocumentModel();
        doc.image = c.image ?? '';
        doc.imageSrc = this.imageService.getImageSrcById(c.image ?? '');
        doc.description = c.description ?? '';
        return doc;
      }) ?? [];
      this.isInSendStep = false;

      this.currentStep = 3;
    }

    if (returnOrderState.returnOrderStatus == 5) {
      this.pendingForExpertProductConfirmState = (returnOrderState.returnOrderStatusName ?? '') + ' در تاریخ ' + returnOrderState.created;

      this.pendingForReceiveStateFormGroup.controls["details"].setValue(returnOrderState.details);
      this.pendingForReceiveStateFormGroup.controls["details"].disable();

      this.currentStep = 4;
    }

    if (returnOrderState.returnOrderStatus == 6) {
      this.pendingForRefundCostState = (returnOrderState.returnOrderStatusName ?? '') + ' در تاریخ ' + returnOrderState.created;

      this.pendingForExpertProductConfirmStateFormGroup.controls["details"].setValue(returnOrderState.details);
      this.pendingForExpertProductConfirmStateFormGroup.controls["details"].disable();
      this.currentStep = 5;
    }

    if (returnOrderState.returnOrderStatus == 7) {
      this.pendingForCompletedState = (returnOrderState.returnOrderStatusName ?? '') + ' در تاریخ ' + returnOrderState.created;

      this.pendingForRefundCostStateFormGroup.controls["details"].setValue(returnOrderState.details);
      this.pendingForRefundCostStateFormGroup.controls["details"].disable();
      this.pendingForRefundCostStateFormGroup.controls["costRefundType"].setValue(this.returnOrder.costRefundType);
      this.pendingForRefundCostStateFormGroup.controls["costRefundType"].disable();
      this.currentStep = 6;
    }

    if (returnOrderState.returnOrderStatus == 8) {
      this.pendingForRefundCostState = (returnOrderState.returnOrderStatusName ?? '') + ' در تاریخ ' + returnOrderState.created;

      this.pendingForExpertProductConfirmStateFormGroup.controls["details"].setValue(returnOrderState.details);
      this.pendingForExpertProductConfirmStateFormGroup.controls["details"].disable();

      this.currentStep = 5;
    }

    if (returnOrderState.returnOrderStatus == 9) {
      this.pendingForCompletedState = (returnOrderState.returnOrderStatusName ?? '') + ' در تاریخ ' + returnOrderState.created;

      this.currentStep = 6;
      console.log(this.returnOrderstates);
      switch (this.returnOrderstates[this.returnOrderstates.indexOf(returnOrderState) - 1].returnOrderStatus) {
        case 3:
          this.pendingForSendStateFormGroup.controls["details"].setValue(returnOrderState.details);
          this.pendingForSendStateFormGroup.controls["details"].disable();
          break;
        case 4:
          this.pendingForReceiveStateFormGroup.controls["details"].setValue(returnOrderState.details);
          this.pendingForReceiveStateFormGroup.controls["details"].disable();
          break;
      }
    }



    if (returnOrderState.returnOrderStatus == 10) {
      debugger;
      console.log(returnOrderState);
      this.pendingForCompletedStateFormGroup.controls["details"].setValue(returnOrderState.details);
      this.pendingForCompletedStateFormGroup.controls["details"].disable();
      this.currentStep = 7;
    }

    this.steps.push(this.currentStep);
  }

  accept(details: string): void {
    this.inProgress = true;
    let cmd = new AcceptReturnOrderCommand();
    cmd.id = this.returnOrderId;
    cmd.details = details;
    this.returnOrdersClient.accept(this.returnOrderId, cmd).subscribe({
      next: (response) => {
        this.inProgress = false;
        this.modalService.dismissAll();
        if (response == null) {
          Swal.fire({
            title: 'پذیرفتن سفارش بازگشت با موفقیت انجام شد',
            icon: 'success',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });

          this.returnOrdersClient.getCurrentReturnOrderStateByReturnOrderId(this.returnOrderId).subscribe({
            next: (response) => {
              if (response != null) {
                this.updateSteps(response);
              }
            }
          })
        } else {
          Swal.fire({
            title: ' پذیرفتن سفارش بازگشت با خطا مواجه شد',
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

  reject(details: string): void {
    this.inProgress = true;
    let cmd = new RejectReturnOrderCommand();
    cmd.id = this.returnOrderId;
    cmd.details = details;
    this.returnOrdersClient.reject(this.returnOrderId, cmd).subscribe({
      next: (response) => {
        this.inProgress = false;
        this.modalService.dismissAll();
        if (response == null) {
          Swal.fire({
            title: 'رد سفارش بازگشت با موفقیت انجام شد',
            icon: 'success',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });

          this.returnOrdersClient.getCurrentReturnOrderStateByReturnOrderId(this.returnOrderId).subscribe({
            next: (response) => {
              if (response != null) {
                this.updateSteps(response);
              }
            }
          })
        } else {
          Swal.fire({
            title: ' رد سفارش بازگشت با خطا مواجه شد',
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

  send(details: string): void {
    debugger;
    this.inProgress = true;
    let cmd = new SendReturnOrderCommand();
    cmd.id = this.returnOrderId;
    cmd.details = details;
    cmd.returnShippingPrice = this.pendingForSendStateFormGroupControl.returnShippingPrice.value;
    cmd.returnOrderTransportationType = this.pendingForSendStateFormGroupControl.returnOrderTransportationType.value;
    cmd.documentCommandForTotals = this.documentList.map(c => {
      let docCmd = new DocumentCommandForTotal();
      docCmd.image = c.image;
      docCmd.description = c.description;
      return docCmd;
    })
    this.returnOrdersClient.send(this.returnOrderId, cmd).subscribe({
      next: (response) => {
        this.inProgress = false;
        this.modalService.dismissAll();
        if (response == null) {
          Swal.fire({
            title: 'تایید ارسال سفارش بازگشت با موفقیت انجام شد',
            icon: 'success',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });

          this.returnOrdersClient.getCurrentReturnOrderStateByReturnOrderId(this.returnOrderId).subscribe({
            next: (response) => {
              if (response != null) {
                this.updateSteps(response);
              }
            }
          })
        } else {
          Swal.fire({
            title: ' تایید ارسال سفارش بازگشت با خطا مواجه شد',
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

  receive(details: string): void {
    this.inProgress = true;
    let cmd = new ReceiveReturnOrderCommand();
    cmd.id = this.returnOrderId;
    cmd.details = details;
    this.returnOrdersClient.receive(this.returnOrderId, cmd).subscribe({
      next: (response) => {
        this.inProgress = false;
        this.modalService.dismissAll();
        if (response == null) {
          Swal.fire({
            title: 'تایید دیافت سفارش بازگشت با موفقیت انجام شد',
            icon: 'success',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });

          this.returnOrdersClient.getCurrentReturnOrderStateByReturnOrderId(this.returnOrderId).subscribe({
            next: (response) => {
              if (response != null) {
                this.updateSteps(response);
              }
            }
          })
        } else {
          Swal.fire({
            title: ' تایید دیافت سفارش بازگشت با خطا مواجه شد',
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

  confirmAll(details: string): void {
    this.inProgress = true;
    let cmd = new ConfirmAllReturnOrderCommand();
    cmd.id = this.returnOrderId;
    cmd.details = details;
    this.returnOrdersClient.confirmAll(this.returnOrderId, cmd).subscribe({
      next: (response) => {
        this.inProgress = false;
        this.modalService.dismissAll();
        if (response == null) {
          Swal.fire({
            title: 'تایید همه کالاهای سفارش بازگشت با موفقیت انجام شد',
            icon: 'success',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });

          this.returnOrdersClient.getCurrentReturnOrderStateByReturnOrderId(this.returnOrderId).subscribe({
            next: (response) => {
              if (response != null) {
                this.updateSteps(response);
              }
            }
          })
        } else {
          Swal.fire({
            title: ' تایید همه کالاهای سفارش بازگشت با خطا مواجه شد',
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

  confirmSome(details: string): void {
    this.inProgress = true;
    let cmd = new ConfirmSomeReturnOrderCommand();
    cmd.id = this.returnOrderId;
    cmd.details = details;
    this.returnOrdersClient.confirmSome(this.returnOrderId, cmd).subscribe({
      next: (response) => {
        this.inProgress = false;
        this.modalService.dismissAll();
        if (response == null) {
          Swal.fire({
            title: 'تایید بعضی کالاهای سفارش بازگشت با موفقیت انجام شد',
            icon: 'success',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });

          this.returnOrdersClient.getCurrentReturnOrderStateByReturnOrderId(this.returnOrderId).subscribe({
            next: (response) => {
              if (response != null) {
                this.updateSteps(response);
              }
            }
          })
        } else {
          Swal.fire({
            title: ' تایید بعضی کالاهای سفارش بازگشت با خطا مواجه شد',
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
    let cmd = new RefundReturnOrderCostCommand();
    cmd.id = this.returnOrderId;
    cmd.details = details;
    cmd.costRefundType = this.pendingForRefundCostStateFormGroupControl.costRefundType.value;
    this.returnOrdersClient.refundCost(this.returnOrderId, cmd).subscribe({
      next: (response) => {
        this.inProgress = false;
        this.modalService.dismissAll();
        if (response == null) {
          Swal.fire({
            title: 'بازگشت وجه سفارش بازگشت با موفقیت انجام شد',
            icon: 'success',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });

          this.returnOrdersClient.getCurrentReturnOrderStateByReturnOrderId(this.returnOrderId).subscribe({
            next: (response) => {
              if (response != null) {
                this.updateSteps(response);
              }
            }
          })
        } else {
          Swal.fire({
            title: ' بازگشت وجه سفارش بازگشت با خطا مواجه شد',
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
    let cmd = new CancelReturnOrderCommand();
    cmd.id = this.returnOrderId;
    cmd.details = details;
    this.returnOrdersClient.cancel(this.returnOrderId, cmd).subscribe({
      next: (response) => {
        this.inProgress = false;
        this.modalService.dismissAll();
        if (response == null) {
          Swal.fire({
            title: 'لغو سفارش بازگشت با موفقیت انجام شد',
            icon: 'success',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });

          this.returnOrdersClient.getCurrentReturnOrderStateByReturnOrderId(this.returnOrderId).subscribe({
            next: (response) => {
              if (response != null) {
                this.updateSteps(response);
              }
            }
          })
        } else {
          Swal.fire({
            title: ' لغو سفارش بازگشت با خطا مواجه شد',
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
    let cmd = new CompleteReturnOrderCommand();
    cmd.id = this.returnOrderId;
    cmd.details = details;
    this.returnOrdersClient.complete(this.returnOrderId, cmd).subscribe({
      next: (response) => {
        this.inProgress = false;
        this.modalService.dismissAll();
        if (response == null) {
          Swal.fire({
            title: 'تکمیل فرایند سفارش بازگشت با موفقیت انجام شد',
            icon: 'success',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });

          this.returnOrdersClient.getCurrentReturnOrderStateByReturnOrderId(this.returnOrderId).subscribe({
            next: (response) => {
              if (response != null) {
                this.updateSteps(response);
              }
            }
          })
        } else {
          Swal.fire({
            title: 'تکمیل فرایند سفارش بازگشت با خطا مواجه شد',
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
        this.accept(this.currentOperationInfo.details);
        break;
      case 2:
        this.reject(this.currentOperationInfo.details);
        break;
      case 3:
        this.send(this.currentOperationInfo.details);
        break;
      case 4:
        this.receive(this.currentOperationInfo.details);
        break;
      case 5:
        this.confirmAll(this.currentOperationInfo.details);
        break;
      case 6:
        this.confirmSome(this.currentOperationInfo.details);
        break;
      case 7:
        this.refundCost(this.currentOperationInfo.details);
        break;
      case 8:
        this.cancel(this.currentOperationInfo.details);
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

  onFileSelected(event: FileUploadMetadataDto | null): void {
    if (event) {
      this.documentForm.controls["image"].setValue(event.fileId);
    } else {
      this.documentForm.controls["image"].setValue(null);
    }
  }

  toggleCheckbox(itemId: string) {
    if (this.checkedItems.has(itemId)) {
      this.checkedItems.delete(itemId);
    } else {
      this.checkedItems.add(itemId);
    }
  }

  isChecked(itemId: string): boolean {
    return this.checkedItems.has(itemId);
  }

  deleteMultiple(content: any) {
    if ([...this.checkedItems].length > 0) {
      this.confirmationModalRef = this.modalService.open(content, { centered: true });
    } else {
      Swal.fire({
        title: "حداقل یک مورد را انتخاب کنید!",
        icon: "question",
        iconHtml: "!",
        confirmButtonText: "تایید",
      });
    }
  }


  onSubmit(): void {
    debugger;
    this.inProgress = true;
    this.markAllControlsAsTouched(this.documentForm);
    this.submit = true;
    if (this.documentForm.valid) {
      let doc = new ReturnOrderTotalDocumentModel();
      doc.description = this.documentForm.controls["description"].value;
      doc.image = this.documentForm.controls["image"].value;
      doc.imageSrc = this.imageService.getImageSrcByIdTemp(doc.image);
      this.documentList.push(doc);
      this.inProgress = false;
    }
    else {
      this.inProgress = false;
    }
  }

  resetAddForm(form: FormGroup): void {
    Object.keys(form.controls).forEach((controlName) => {
      const control = form.controls[controlName];
      if (control.enabled) {
        control.markAsPristine();
        control.markAsUntouched();
        control.reset();
      }
    });
  }


  openInsertModal() {
    this.modalService.open(this.documentFormModal, { size: "lg", backdrop: "static" });
  }

  openDeleteConfirmationModal(id: any) {
    this.selectedId = id;
    this.modalService.open(this.confirmationModal);
  }

  deleteItems() {
    this.inProgress = true;
    let ids = [...this.checkedItems];
    this.documentList = this.documentList.filter(record => !ids.includes(record.image));
    this.checkedItems = new Set<string>();
    this.inProgress = false;
    this.confirmationModalRef?.dismiss();
  }

  public handleCloseFormModal(form: FormGroup) {
    form.reset();
    form.markAsUntouched();
    form.setErrors(null);
    form.markAsPristine();
    this.selectedId = 0;
    this.selectedFileUrl = undefined;
  }

  returnShippingPriceValidator(): ValidatorFn {
    return (control: AbstractControl) => {
      const formGroup = control.parent;
      if (!formGroup) return null;
      debugger;
      const isReturnShippingPriceEntered = formGroup.get('isReturnShippingPriceEntered')?.value;
      const returnShippingPrice = +control.value;

      if (isReturnShippingPriceEntered && (returnShippingPrice === null || returnShippingPrice === undefined || returnShippingPrice <= 0)) {
        return { required: true };
      }

      if (isReturnShippingPriceEntered && typeof returnShippingPrice !== 'number') {
        return { invalidNumber: true };
      }

      return null; // Valid
    };
  }

}
export class OperatioInfo {
  operationId!: number;
  details!: string;
}