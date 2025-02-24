
import { Component, Input, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { DecimalPipe } from "@angular/common";
import { NgbAlertConfig, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from "@angular/forms";
import { Observable } from "rxjs";
import Swal from "sweetalert2";

import { ReturnOrderStateGridService } from "./return-order-state-grid.service";
import { ReturnOrderStateModel } from "./return-order-state.model";
import { ProductOptionColorsClient } from "src/app/web-api-client";


@Component({
  selector: 'app-return-order-state',
  templateUrl: './return-order-state.component.html',
  styleUrls: ['./return-order-state.component.scss'],
  providers: [ReturnOrderStateGridService, DecimalPipe, NgbAlertConfig],
})
export class ReturnOrderStateComponent {
  // bread crumb items
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: ReturnOrderStateModel;
  gridjsList$!: Observable<ReturnOrderStateModel[]>;
  total$: Observable<number>;
  nano!: string;
  // @ViewChildren(NgbdProductImagesSortableHeader) productImages!: QueryList<NgbdProductImagesSortableHeader>;
  @ViewChild('confirmationModal') confirmationModal: any;
  @ViewChild('formModal') formModal: any = [];
  @Input() returnOrder?: any;

  checkedItems: Set<number> = new Set<number>();
  constructor(alertConfig: NgbAlertConfig,
     private fb: FormBuilder, private modalService: NgbModal, public service: ReturnOrderStateGridService) {
      
    this.gridjsList$ = service.returnOrderStatus$;
    this.total$ = service.total$;
    

  }

  ngOnInit(): void {
    
    this.service.getReturnOrderStatesByReturnOrderId(this.returnOrder.id);
  
  }





}



