
import { Component, Input, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { DecimalPipe } from "@angular/common";
import { NgbAlertConfig, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from "@angular/forms";
import { Observable } from "rxjs";
import Swal from "sweetalert2";

import { ReturnOrderItemGroupGridService } from "./return-order-item-group-grid.service";
import { ReturnOrderItemGroupModel } from "./return-order-item-group.model";
import { ProductOptionColorsClient } from "src/app/web-api-client";


@Component({
  selector: 'app-return-order-item-group',
  templateUrl: './return-order-item-group.component.html',
  styleUrls: ['./return-order-item-group.component.scss'],
  providers: [ReturnOrderItemGroupGridService, DecimalPipe, NgbAlertConfig],
})
export class ReturnOrderItemGroupComponent {
  // bread crumb items
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItemGroup?: ReturnOrderItemGroupModel;
  
  gridjsList$!: Observable<ReturnOrderItemGroupModel[]>;
  total$: Observable<number>;
  optionColorList?: any[];
  productAttributesList?: any[];
  optionColorValuesList?: any[];
  productAttributesGroupId?: any;
  productAttributes?:any;
  nano!: string;
  @ViewChild('confirmationModal') confirmationModal: any;
  @ViewChild('detailsModal') detailsModal: any = [];
  @Input() returnOrder?: any;

  checkedItems: Set<number> = new Set<number>();
  constructor(alertConfig: NgbAlertConfig, public client: ProductOptionColorsClient, 
     private fb: FormBuilder, private modalService: NgbModal, public service: ReturnOrderItemGroupGridService) {
    this.gridjsList$ = service.returnOrderItemGroups$;
    this.total$ = service.total$;
    

  }

  ngOnInit(): void {
    this.service.getAllReturnOrderItemGroupsByReturnOrderId(this.returnOrder.id);
  }

  openMoreDetail(item: any)
  {
    this.selectedItemGroup =item;
    this.modalService.open(this.detailsModal, { size: 'lg', backdrop: 'static' });
  }



}



