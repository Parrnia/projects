
import { EventEmitter, Input, Output } from "@angular/core";
import { DecimalPipe } from "@angular/common";
import { NgbAlertConfig, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from "@angular/forms";
import { Observable } from "rxjs";
import Swal from "sweetalert2";
import { Component, ViewChild, ViewChildren } from '@angular/core';
import { ReturnOrdersClient } from "src/app/web-api-client";
import { ReturnOrderInfoModel } from './return-order-info.model';


@Component({
  selector: 'app-return-order-info',
  templateUrl: './return-order-info.component.html',
  styleUrls: ['./return-order-info.component.scss'],
  providers: [DecimalPipe, NgbAlertConfig],
})
export class ReturnOrderInfoComponent {
  // bread crumb items
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  nano!: string;
  inProgress = false;
  @ViewChild('confirmationModal') confirmationModal: any;
  @ViewChild('formModal') formModal: any = [];
  @Input() returnOrder?: ReturnOrderInfoModel;
  @Output() afterSubmit: EventEmitter<void> = new EventEmitter<void>();
  
  checkedItems: Set<number> = new Set<number>();
  constructor(alertConfig: NgbAlertConfig, public client: ReturnOrdersClient,
    private fb: FormBuilder, private modalService: NgbModal) {
    debugger;
    

    alertConfig.type = 'success';


  }

  ngOnInit(): void {
    this.myForm = this.fb.group({
      id: this.returnOrder?.id,
      phoneNumber: [this.returnOrder?.phoneNumber, [Validators.required]],
      customerFirstName: [this.returnOrder?.customerFirstName, [Validators.required]],
      customerLastName: [this.returnOrder?.customerLastName, [Validators.required]],
    });
    this.myForm.controls["phoneNumber"].disable();
    this.myForm.controls["customerFirstName"].disable();
    this.myForm.controls["customerLastName"].disable();
  }
  get form() {
    return this.myForm.controls;
  }
  
  }






