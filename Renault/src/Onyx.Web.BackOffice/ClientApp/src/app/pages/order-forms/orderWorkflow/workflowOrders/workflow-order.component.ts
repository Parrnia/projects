export interface ITab {
  id: number;
  title: string;
}

import { Component, Input, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { Observable } from "rxjs";
import { ActivatedRoute, Router } from "@angular/router";
import { DecimalPipe } from "@angular/common";
import { FormGroup, FormBuilder, UntypedFormBuilder } from "@angular/forms";
import { NgbAlertConfig, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import * as moment from "moment";
import { ExportFileService } from "src/app/shared/services/export-file/export-file.service";
import { OrdersClient } from "src/app/web-api-client";
import { WorkflowOrderGridService } from "./workflow-order-grid.service";
import { WorkflowOrderModel } from "./workflow-order.model";


@Component({
  selector: "app-workflow-Order",
  templateUrl: "./workflow-Order.component.html",
  styleUrls: ["./workflow-Order.component.scss"],
  providers: [WorkflowOrderGridService, DecimalPipe, NgbAlertConfig],
})
export class WorkflowOrderComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  cancelForm!: FormGroup;
  processForm!: FormGroup;
  unConfirmForm!: FormGroup;
  refundCostForm!: FormGroup;
  unSuccessfulPaymentForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: WorkflowOrderModel;
  gridjsList$!: Observable<WorkflowOrderModel>;
  selectedOrder?: any;
  selectedOrderInfo?: any;
  inProgressAllExportbtn = false;
  inProgressExportForm = false;
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
  @ViewChild("unSuccessfulPaymentFormModal") unSuccessfulPaymentFormModal: any = [];

  providerId?: any;
  countryId?: any;
  productTypeId?: any;
  productStatusId?: any;
  brandId?: any;
  productCategoryId?: any;
  availabilityId?: any;
  productAttributeTypeId?: null;
  formData = new FormData();
  exportForm!: FormGroup;

  activeTab = 1;
  checkedItems: Set<number> = new Set<number>();

  @Input() orderId?: any;
  @Input() stepId?: any;

  constructor(
    alertConfig: NgbAlertConfig,
    private fb: FormBuilder,
    private modalService: NgbModal,
    public service: WorkflowOrderGridService,
    private exportService: ExportFileService,
    private formBuilder: UntypedFormBuilder,
    private ordersClient: OrdersClient,
    private router: Router
  ) {
    this.gridjsList$ = service.orders$;

    this.exportForm = this.fb.group({
      searchText: "",
      pageSize: 1,
      startCreationDate: null,
      endCreationDate: null,
      startChangeDate: null,
      endChangeDate: null,
    });
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
  get form() {
    return this.exportForm.controls;
  }

  public handleCloseFormModal(form: FormGroup) {
    form.reset();
    form.markAsUntouched();
    form.setErrors(null);
    form.markAsPristine();
  }
  submitExportForm() {
    this.inProgressExportForm = true;
    debugger;
    if (this.exportForm.valid) {
      const startCreationDateValue =
        this.exportForm.controls["startCreationDate"].value;
      const startCreationDate = startCreationDateValue
        ? moment(startCreationDateValue, "jYYYY/jMM/jDD")
          .utcOffset(0, true)
          .toDate()
        : null; // Set to null if no date is selected

      // End Creation Date
      const endCreationDateValue =
        this.exportForm.controls["endCreationDate"].value;
      const endCreationDate = endCreationDateValue
        ? moment(endCreationDateValue, "jYYYY/jMM/jDD")
          .utcOffset(0, true)
          .toDate()
        : null;

      // Start Change Date
      const startChangeDateValue =
        this.exportForm.controls["startChangeDate"].value;
      const startChangeDate = startChangeDateValue
        ? moment(startChangeDateValue, "jYYYY/jMM/jDD")
          .utcOffset(0, true)
          .toDate()
        : null;

      // End Change Date
      const endChangeDateValue =
        this.exportForm.controls["endChangeDate"].value;
      const endChangeDate = endChangeDateValue
        ? moment(endChangeDateValue, "jYYYY/jMM/jDD")
          .utcOffset(0, true)
          .toDate()
        : null;

      this.ordersClient
        .exportExcelQuery(
          this.form.searchText.value,
          1,
          this.form.pageSize.value,
          startCreationDate,
          endCreationDate,
          startChangeDate,
          endChangeDate
        )
        .subscribe({
          next: (response) => {
            this.inProgressExportForm = false;
            this.exportService.exportFile(response);
          },
          error: (error) => {
            this.inProgressExportForm = false;
            console.error(error);
          },
        });
    } else {
      this.inProgressExportForm = false;
    }
  }

  openGetDetailExportExcelModal() {
    this.modalService.open(this.getDetailExportTabModel, {
      size: "lg",
      backdrop: "static",
    });
  }


  openMoreDetailOrder(item: Observable<WorkflowOrderModel>) {
    item.subscribe({
      next: (item) => {
        this.selectedOrder = item;
        this.ordersClient
          .getOrderInfo(item.id)
          .subscribe((c) => (this.selectedOrderInfo = c));
        this.modalService.open(this.productTabModel, {
          size: "lg",
          backdrop: "static",
        });
      }
    });
  }
  
  refreshGrid() {
    this.service.refreshOrders(this.orderId);
    this.gridjsList$ = this.service.orders$;
  }
  navigateToOrder(orderId: string) {
    this.router.navigate(["/orderWorkflow", orderId]);
  }
}
