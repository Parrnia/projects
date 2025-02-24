
import { Component, Input, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { DecimalPipe } from "@angular/common";
import { NgbAlertConfig, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from "@angular/forms";
import { Observable } from "rxjs";
import Swal from "sweetalert2";

import { OrderTotalsGridService } from "./order-total-grid.service";
import { OrderTotalsModel } from "./order-total.model";
import { ProductOptionColorsClient } from "src/app/web-api-client";


@Component({
  selector: 'app-order-total',
  templateUrl: './order-total.component.html',
  styleUrls: ['./order-total.component.scss'],
  providers: [OrderTotalsGridService, DecimalPipe, NgbAlertConfig],
})
export class OrderTotalsComponent {
  // bread crumb items
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: OrderTotalsModel;
  gridjsList$!: Observable<OrderTotalsModel[]>;
  total$: Observable<number>;
  nano!: string;
  // @ViewChildren(NgbdProductImagesSortableHeader) productImages!: QueryList<NgbdProductImagesSortableHeader>;
  @ViewChild('confirmationModal') confirmationModal: any;
  @ViewChild('formModal') formModal: any = [];
  @Input() order?: any;

  checkedItems: Set<number> = new Set<number>();
  constructor(alertConfig: NgbAlertConfig, public client: ProductOptionColorsClient, 
     private fb: FormBuilder, private modalService: NgbModal, public service: OrderTotalsGridService) {
      
    this.gridjsList$ = service.orderTotals$;
    this.total$ = service.total$;
    

  }

  ngOnInit(): void {

    this.service.getAllOrderTotalsByOrderId(this.order.id);
  
  }





}



