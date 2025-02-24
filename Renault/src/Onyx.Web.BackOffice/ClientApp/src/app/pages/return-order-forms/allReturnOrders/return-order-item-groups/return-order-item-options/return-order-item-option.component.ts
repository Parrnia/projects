

import { Component, Input, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { DecimalPipe } from "@angular/common";
import { NgbAlertConfig, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from "@angular/forms";
import { Observable } from "rxjs";
import Swal from "sweetalert2";

import { ReturnOrderItemOptionGridService } from "./return-order-item-option-grid.service";
import { ReturnOrderItemOptionModel } from "./return-order-item-option.model";


@Component({
  selector: 'app-return-order-item-option',
  templateUrl: './return-order-item-option.component.html',
  styleUrls: ['./return-order-item-option.component.scss'],
  providers: [ReturnOrderItemOptionGridService, DecimalPipe, NgbAlertConfig],
})
export class ReturnOrderItemOptionComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: ReturnOrderItemOptionModel;
  gridjsList$!: Observable<ReturnOrderItemOptionModel[]>;
  total$: Observable<number>;
  @ViewChild('confirmationModal') confirmationModal: any;
  @ViewChild('formModal') formModal: any = [];
  @Input() returnOrderItemGroup?: any;

  checkedItems: Set<number> = new Set<number>();
  constructor(alertConfig: NgbAlertConfig,
    private fb: FormBuilder,
    private modalService: NgbModal,
    public service: ReturnOrderItemOptionGridService
  ) {

    this.gridjsList$ = service.returnOrderItemOption$;
    this.total$ = service.total$;


  }

  ngOnInit(): void {
    this.service.getOptionValuesByReturnOrderItemGroupId(this.returnOrderItemGroup.id);
  }





}



