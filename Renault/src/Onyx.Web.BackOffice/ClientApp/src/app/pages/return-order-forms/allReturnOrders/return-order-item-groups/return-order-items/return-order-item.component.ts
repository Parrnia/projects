
import { Component, Input, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { DecimalPipe } from "@angular/common";
import { NgbAlertConfig, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from "@angular/forms";
import { Observable } from "rxjs";
import Swal from "sweetalert2";

import { ReturnOrderItemsGridService } from "./return-order-item-grid.service";
import { ReturnOrderItemModel } from "./return-order-item.model";
import { ProductOptionColorsClient } from "src/app/web-api-client";
import { ReturnOrderItemGroupModel } from "../return-order-item-group.model";


@Component({
  selector: 'app-return-order-item',
  templateUrl: './return-order-item.component.html',
  styleUrls: ['./return-order-item.component.scss'],
  providers: [ReturnOrderItemsGridService, DecimalPipe, NgbAlertConfig],
})
export class ReturnOrderItemsComponent {
  // bread crumb items
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: ReturnOrderItemModel;
  
  gridjsList$!: Observable<ReturnOrderItemModel[]>;
  total$: Observable<number>;
  optionColorList?: any[];
  productAttributesList?: any[];
  optionColorValuesList?: any[];
  productAttributesGroupId?: any;
  productAttributes?:any;
  @ViewChild('confirmationModal') confirmationModal: any;
  @ViewChild('documentModal') documentModal: any = [];
  @Input() returnOrderItemGroup?: ReturnOrderItemGroupModel;

  checkedItems: Set<number> = new Set<number>();
  constructor(alertConfig: NgbAlertConfig, public client: ProductOptionColorsClient, 
     private fb: FormBuilder, private modalService: NgbModal, public service: ReturnOrderItemsGridService) {
      
    this.gridjsList$ = service.returnOrderItems$;
    this.total$ = service.total$;
    

  }

  ngOnInit(): void {

    this.service.getAllReturnOrderItemsByReturnOrderItemGroupId(this.returnOrderItemGroup!.id);
  
  }

  openMoreDetail(item: any)
  {
    this.selectedItem =item;
    this.modalService.open(this.documentModal, { size: 'lg', backdrop: 'static' });
  }



}



