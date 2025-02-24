
import { Component, Input, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { DecimalPipe } from "@angular/common";
import { NgbAlertConfig, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from "@angular/forms";
import { Observable } from "rxjs";
import Swal from "sweetalert2";

import { ReturnOrderTotalModel } from "./return-order-total.model";
import { ProductOptionColorsClient } from "src/app/web-api-client";
import { ReturnOrderTotalsGridService } from "./return-order-total-grid.service";


@Component({
  selector: 'app-return-order-total',
  templateUrl: './return-order-total.component.html',
  styleUrls: ['./return-order-total.component.scss'],
  providers: [ReturnOrderTotalsGridService, DecimalPipe, NgbAlertConfig],
})
export class ReturnOrderTotalsComponent {
  // bread crumb items
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: ReturnOrderTotalModel;
  gridjsList$!: Observable<ReturnOrderTotalModel[]>;
  total$: Observable<number>;
  nano!: string;
  // @ViewChildren(NgbdProductImagesSortableHeader) productImages!: QueryList<NgbdProductImagesSortableHeader>;
  @ViewChild('confirmationModal') confirmationModal: any;
  @ViewChild('formModal') formModal: any = [];
  @ViewChild('documentModal') documentModal: any = [];
  @Input() returnOrder?: any;

  checkedItems: Set<number> = new Set<number>();
  constructor(alertConfig: NgbAlertConfig, public client: ProductOptionColorsClient, 
     private fb: FormBuilder, private modalService: NgbModal, public service: ReturnOrderTotalsGridService) {
      
    this.gridjsList$ = service.returnOrderTotals$;
    this.total$ = service.total$;
    

  }

  ngOnInit(): void {
    this.service.getReturnOrderTotalsByReturnOrderId(this.returnOrder.id);
  }
  openMoreDetail(item: any)
  {
    this.selectedItem =item;
    this.modalService.open(this.documentModal, { size: 'lg', backdrop: 'static' });
  }
}



