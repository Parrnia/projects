
import { Component, Input, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { DecimalPipe } from "@angular/common";
import { NgbAlertConfig, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from "@angular/forms";
import { Observable } from "rxjs";
import Swal from "sweetalert2";

import { OrderItemsGridService } from "./order-item-grid.service";
import { OrderItemModel } from "./order-item.model";
import { ProductOptionColorsClient } from "src/app/web-api-client";


@Component({
  selector: 'app-order-item',
  templateUrl: './order-item.component.html',
  styleUrls: ['./order-item.component.scss'],
  providers: [OrderItemsGridService, DecimalPipe, NgbAlertConfig],
})
export class OrderItemsComponent {
  // bread crumb items
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: OrderItemModel;
  
  gridjsList$!: Observable<OrderItemModel[]>;
  total$: Observable<number>;
  optionColorList?: any[];
  productAttributesList?: any[];
  optionColorValuesList?: any[];
  productAttributesGroupId?: any;
  productAttributes?:any;
  nano!: string;
  // @ViewChildren(NgbdProductImagesSortableHeader) productImages!: QueryList<NgbdProductImagesSortableHeader>;
  @ViewChild('confirmationModal') confirmationModal: any;
  @ViewChild('optionModal') optionModal: any = [];
  @Input() order?: any;

  checkedItems: Set<number> = new Set<number>();
  constructor(alertConfig: NgbAlertConfig, public client: ProductOptionColorsClient, 
     private fb: FormBuilder, private modalService: NgbModal, public service: OrderItemsGridService) {
      
    this.gridjsList$ = service.orderItems$;
    this.total$ = service.total$;
    

  }

  ngOnInit(): void {

    this.service.getAllOrderItemsByOrderId(this.order.id);
  
  }

  openMoreDetail(item: any)
  {
    this.selectedItem =item;
    this.modalService.open(this.optionModal, { size: 'lg', backdrop: 'static' });
  }



}



