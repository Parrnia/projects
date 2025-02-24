export interface ITab {
  id: number;
  title: string;
}

import { Component, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { Observable } from "rxjs";
import { ActivatedRoute, Router } from "@angular/router";
import { DecimalPipe } from "@angular/common";
import { FormGroup, FormBuilder } from "@angular/forms";
import { NgbAlertConfig, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { ExportFileService } from "src/app/shared/services/export-file/export-file.service";
import { OrdersClient } from "src/app/web-api-client";
import { AllOrderGridService } from "./pending-for-register-order-grid.service";
import {
  NgbdpendingForRegisterOrderSortableHeader,
  SortEvent,
} from "./pending-for-register-order-sortable.directive";
import { PendingForRegisterOrdersModel } from "./pending-for-register-order.model";

@Component({
  selector: "app-pending-for-register-order",
  templateUrl: "./pending-for-register-order.component.html",
  styleUrls: ["./pending-for-register-order.component.scss"],
  providers: [AllOrderGridService, DecimalPipe, NgbAlertConfig],
})
export class PendingForRegisterOrderComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  cancelForm!: FormGroup;
  processForm!: FormGroup;
  unConfirmForm!: FormGroup;
  refundCostForm!: FormGroup;
  unSuccessfulPaymentForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: PendingForRegisterOrdersModel;
  gridjsList$!: Observable<PendingForRegisterOrdersModel[]>;
  total$: Observable<number>;
  selectedOrder?: any;
  selectedOrderInfo?: any;
  inProgressAllExportbtn = false;
  @ViewChildren(NgbdpendingForRegisterOrderSortableHeader)
  orders!: QueryList<NgbdpendingForRegisterOrderSortableHeader>;
  @ViewChild("productTabModel") productTabModel: any = [];
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("cancelModel") cancelModel: any = [];
  @ViewChild("processModel") processModel: any = [];
  @ViewChild("unConfirmModel") unConfirmModal: any = [];
  @ViewChild("refundCostModel") refundCostModel: any = [];
  @ViewChild("unSuccessfulPaymentModel") unSuccessfulPaymentModel: any = [];

  @ViewChild("cancelFormModal") cancelFormModal: any = [];
  @ViewChild("processFormModal") processFormModal: any = [];
  @ViewChild("unConfirmFormModal") unConfirmFormModal: any = [];
  @ViewChild("refundCostFormModal") refundCostFormModal: any = [];
  @ViewChild("unSuccessfulPaymentFormModal") unSuccessfulPaymentFormModal: any =
    [];

  providerId?: any;
  countryId?: any;
  productTypeId?: any;
  productStatusId?: any;
  brandId?: any;
  productCategoryId?: any;
  availabilityId?: any;
  productAttributeTypeId?: null;
  formData = new FormData();
  activeTab = 1;
  checkedItems: Set<number> = new Set<number>();
  constructor(
    alertConfig: NgbAlertConfig,
    private fb: FormBuilder,
    private modalService: NgbModal,
    public service: AllOrderGridService,
    private exportService: ExportFileService,
    public ordersClient: OrdersClient,
    private router: Router
  ) {
    this.gridjsList$ = service.orders$;
    this.total$ = service.total$;
  }

  ngOnInit(): void {
    this.breadCrumbItems = [
      { label: " سفارش" },
      { label: "لیست سفارشات ", active: true },
    ];
  }

  exportExcel() {
    this.inProgressAllExportbtn = true;
    this.ordersClient
      .exportExcelQuery(null, 1, null, null, null, null, null)
      .subscribe({
        next: (response) => {
          this.inProgressAllExportbtn = false;
          this.exportService.exportFile(response);
        },
        error: (error) => {
          this.inProgressAllExportbtn = false;
          console.error(error);
        },
      });
  }

  public handleCloseFormModal(form: FormGroup) {
    form.reset();
    form.markAsUntouched();
    form.setErrors(null);
    form.markAsPristine();
  }

  openGetDetailExportExcelModal() {
    this.modalService.open(this.getDetailExportTabModel, {
      size: "lg",
      backdrop: "static",
    });
  }

  /**
   * Sort table data
   * @param param0 sort the column
   *
   */
  onSort({ column, direction }: SortEvent) {
    // resetting other brands
    this.orders.forEach((product) => {
      if (product.sortable !== column) {
        product.direction = "";
      }
    });

    this.service.sortColumn = column;
    this.service.sortDirection = direction;
  }

  openMoreDetailOrder(item: any) {
    this.selectedOrder = item;
    this.ordersClient
      .getOrderInfo(item.id)
      .subscribe((c) => (this.selectedOrderInfo = c));
    this.modalService.open(this.productTabModel, {
      size: "lg",
      backdrop: "static",
    });
  }
  refreshGrid() {
    this.service.refreshOrders();
    this.gridjsList$ = this.service.orders$;
  }
}
