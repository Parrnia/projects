
import { Component, Input, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { DecimalPipe } from "@angular/common";
import { NgbAlertConfig, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from "@angular/forms";
import { Observable } from "rxjs";
import Swal from "sweetalert2";

import { OrderStateGridService } from "./order-state-grid.service";
import { OrderStateModel } from "./order-state.model";
import { ProductOptionColorsClient } from "src/app/web-api-client";


@Component({
  selector: 'app-order-state',
  templateUrl: './order-state.component.html',
  styleUrls: ['./order-state.component.scss'],
  providers: [OrderStateGridService, DecimalPipe, NgbAlertConfig],
})
export class OrderStateComponent {
  // bread crumb items
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: OrderStateModel;
  gridjsList$!: Observable<OrderStateModel[]>;
  total$: Observable<number>;
  nano!: string;
  // @ViewChildren(NgbdProductImagesSortableHeader) productImages!: QueryList<NgbdProductImagesSortableHeader>;
  @ViewChild('confirmationModal') confirmationModal: any;
  @ViewChild('formModal') formModal: any = [];
  @Input() order?: any;

  checkedItems: Set<number> = new Set<number>();
  constructor(alertConfig: NgbAlertConfig,
     private fb: FormBuilder, private modalService: NgbModal, public service: OrderStateGridService) {
      
    this.gridjsList$ = service.orderStatus$;
    this.total$ = service.total$;
    

  }

  ngOnInit(): void {
    
    this.service.getAllOrderStatusByOrderId(this.order.id);
  
  }





}



