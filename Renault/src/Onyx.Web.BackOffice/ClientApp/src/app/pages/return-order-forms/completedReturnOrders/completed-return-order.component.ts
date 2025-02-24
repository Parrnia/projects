
export interface ITab {
  id: number;
  title: string;
}

import { Component, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { Observable, of, switchAll } from 'rxjs';
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from '@angular/forms';
import { NgbAlertConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ReturnOrdersClient } from 'src/app/web-api-client';
import { DecimalPipe } from '@angular/common';
import Swal from 'sweetalert2';
import { CompletedReturnOrderGridService } from './completed-return-order-grid.service';
import { NgbdCompletedReturnOrderSortableHeader, SortEvent } from './completed-return-order-sortable.directive';
import { ReturnOrderModel } from '../return-order.model';
import { ExportFileService } from 'src/app/shared/services/export-file/export-file.service';

@Component({
  selector: "app-completed-return-order",
  templateUrl: "./completed-return-order.component.html",
  styleUrls: ["./completed-return-order.component.scss"],
  providers: [CompletedReturnOrderGridService, DecimalPipe, NgbAlertConfig],
})
export class CompletedReturnOrderComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;

  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: ReturnOrderModel;
  gridjsList$!: Observable<ReturnOrderModel[]>;
  total$: Observable<number>;
  selectedReturnOrder?: any;
  selectedReturnOrderInfo?: any;
  inProgress = false;
  inProgressAllExportbtn = false;

  @ViewChildren(NgbdCompletedReturnOrderSortableHeader)
  returnOrders!: QueryList<NgbdCompletedReturnOrderSortableHeader>;
  @ViewChild("productTabModel") productTabModel: any = [];
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("acceptModel") acceptModel: any = [];
  @ViewChild("cancelModel") cancelModel: any = [];
  @ViewChild("completeModel") completeModel: any = [];
  @ViewChild("confirmAllModel") confirmAllModel: any = [];
  @ViewChild("confirmSomeModel") confirmSomeModel: any = [];
  @ViewChild("receiveModel") receiveModel: any = [];
  @ViewChild("refundCostModel") refundCostModel: any = [];
  @ViewChild("rejectModel") rejectModel: any = [];
  @ViewChild("sendModel") sendModel: any = [];


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
    public client: ReturnOrdersClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    public service: CompletedReturnOrderGridService,
    private exportService: ExportFileService,
    private formBuilder: UntypedFormBuilder,
    private returnOrdersClient: ReturnOrdersClient
  ) {
    this.gridjsList$ = service.returnOrders$;
    this.total$ = service.total$;

  }

  ngOnInit(): void {
    this.breadCrumbItems = [
      { label: 'سفارش بازگشت' },
      { label: 'لیست سفارشات بازگشت کامل شده ', active: true }
    ];
  }

  /**
   * Sort table data
   * @param param0 sort the column
   *
   */
  onSort({ column, direction }: SortEvent) {
    // resetting other brands
    this.returnOrders.forEach((product) => {
      if (product.sortable !== column) {
        product.direction = "";
      }
    });

    this.service.sortColumn = column;
    this.service.sortDirection = direction;
  }

  openMoreDetailReturnOrder(item: any) {
    this.selectedReturnOrder = item;
    this.returnOrdersClient
      .getReturnOrderInfo(item.id)
      .subscribe((c) => (this.selectedReturnOrderInfo = c));
    this.modalService.open(this.productTabModel, {
      size: "lg",
      backdrop: "static",
    });
  }

  refreshGrid() {
    this.service.refreshReturnOrders();
    this.gridjsList$ = this.service.returnOrders$;
  }

  exportExcel() {
    this.inProgressAllExportbtn = true;
    this.client
      .exportExcelCompletedQuery(null, 1, null, null, null, null, null)
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

  openGetDetailExportExcelModal() {
    this.modalService.open(this.getDetailExportTabModel, {
      size: "lg",
      backdrop: "static",
    });
  }
}

