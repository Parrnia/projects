

import { Component, Input, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { DecimalPipe } from "@angular/common";
import { NgbAlertConfig, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from "@angular/forms";
import { Observable } from "rxjs";
import Swal from "sweetalert2";

import { OptionGridService } from "./option-grid.service";
import { OptionModel } from "./option.model";
import { ProductOptionColorsClient } from "src/app/web-api-client";


@Component({
  selector: 'app-option',
  templateUrl: './option.component.html',
  styleUrls: ['./option.component.scss'],
  providers: [OptionGridService, DecimalPipe, NgbAlertConfig],
})
export class OptionComponent {
  // bread crumb items
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: OptionModel;
  gridjsList$!: Observable<OptionModel[]>;
  total$: Observable<number>;
  nano!: string;
  // @ViewChildren(NgbdProductImagesSortableHeader) productImages!: QueryList<NgbdProductImagesSortableHeader>;
  @ViewChild('confirmationModal') confirmationModal: any;
  @ViewChild('formModal') formModal: any = [];
  @Input() orderItem?: any;

  checkedItems: Set<number> = new Set<number>();
  constructor(alertConfig: NgbAlertConfig, public client: ProductOptionColorsClient, 
     private fb: FormBuilder, private modalService: NgbModal, public service: OptionGridService) {
      
    this.gridjsList$ = service.option$;
    this.total$ = service.total$;
    

  }

  ngOnInit(): void {

    this.service.getAllOptionByOrderItemId(this.orderItem.id);
  
  }





}



