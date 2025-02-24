import {
  Component,
  Input,
  QueryList,
  ViewChild,
  ViewChildren,
} from "@angular/core";
import { DecimalPipe } from "@angular/common";
import { NgbAlertConfig, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import {
  FormBuilder,
  FormGroup,
  UntypedFormBuilder,
  Validators,
} from "@angular/forms";
import { Observable } from "rxjs";
import Swal from "sweetalert2";

import { OrderPaymentModel } from "./order-payment.model";
import { OnlinePaymentStatus, PaymentType, ProductOptionColorsClient } from "src/app/web-api-client";
import { OrderPaymentGridService } from "./order-payment-grid.service";

@Component({
  selector: "app-order-payment",
  templateUrl: "./order-payment.component.html",
  styleUrls: ["./order-payment.component.scss"],
  providers: [OrderPaymentGridService, DecimalPipe, NgbAlertConfig],
})
export class OrderPaymentComponent {
  // bread crumb items
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: OrderPaymentModel;
  gridjsList$!: Observable<OrderPaymentModel[]>;
  total$: Observable<number>;
  nano!: string;
  // @ViewChildren(NgbdProductImagesSortableHeader) productImages!: QueryList<NgbdProductImagesSortableHeader>;
  @ViewChild("confirmationModal") confirmationModal: any;
  @ViewChild("formModal") formModal: any = [];
  @Input() order?: any;

  checkedItems: Set<number> = new Set<number>();
  constructor(
    alertConfig: NgbAlertConfig,
    private fb: FormBuilder,
    private modalService: NgbModal,
    public service: OrderPaymentGridService
  ) {
    this.gridjsList$ = service.orderStatus$;
    this.total$ = service.total$;
  }

  getStatusString(status?: OnlinePaymentStatus): string | null {
    switch (status) {
      case OnlinePaymentStatus.Waiting:
        return "Waiting";
      case OnlinePaymentStatus.Completed:
        return "Completed";
      case OnlinePaymentStatus.Failed:
        return "Failed";
      default:
        return null;
    }
  }

  getPaymentTypeString(paymentType?: PaymentType): string | null {
    switch (paymentType) {
      case PaymentType.Cash:
        return "Cash";
      case PaymentType.Credit:
        return "Credit";
      case PaymentType.Online:
        return "Online";
      default:
        return null; // or an empty string if that's preferred
    }
  }

  ngOnInit(): void {
    this.service.getOrderPaymentsByOrderId(this.order.id);
  }
}
