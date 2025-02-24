



import { Component, Input, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { DecimalPipe } from "@angular/common";
import { NgbAlertConfig, NgbModal, NgbModalRef } from "@ng-bootstrap/ng-bootstrap";
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from "@angular/forms";
import { Observable } from "rxjs";
import Swal from "sweetalert2";
import { AddRangeProductKindsCommand, AllKindDropDownDto, FileParameter, KindDto, KindsClient, ProductKindsClient } from "src/app/web-api-client";
import { KindsGridService } from "./kinds-grid.service";
import { KindsModel } from "./kinds.model";

@Component({
  selector: 'app-kinds',
  templateUrl: './kinds.component.html',
  styleUrls: ['./kinds.component.scss'],
  providers: [KindsGridService, DecimalPipe, NgbAlertConfig],
})
export class KindsComponent {
  // bread crumb items
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: AllKindDropDownDto;
  gridjsList$!: Observable<KindsModel[]>;
  total$: Observable<number>;
  inProgress = false;
  kindList?: AllKindDropDownDto[];
  kindId?: any;
  @ViewChild('confirmationModal') confirmationModal: any;
  confirmationModalRef: NgbModalRef | undefined;
  @ViewChild('formModal') formModal: any = [];
  formModalRef: NgbModalRef | undefined;
  @ViewChild('addModel') addModel: any = [];
  addModelRef: NgbModalRef | undefined;
  @Input() product?: any;
  checkedItems: Set<number> = new Set<number>();
  constructor(alertConfig: NgbAlertConfig, public client: ProductKindsClient, public kindsClient: KindsClient, private fb: FormBuilder, private modalService: NgbModal, public service: KindsGridService, private formBuilder: UntypedFormBuilder) {
    this.gridjsList$ = service.kinds$;
    this.total$ = service.total$;
  }

  ngOnInit(): void {

    this.service.getAllKinds();
  }

  toggleCheckbox(itemId: number) {
    if (this.checkedItems.has(itemId)) {
      this.checkedItems.delete(itemId);
    } else {
      this.checkedItems.add(itemId);
    }
  }

  isChecked(itemId: number): boolean {
    return this.checkedItems.has(itemId);
  }

  addMultiple(content: any) {
    if ([...this.checkedItems].length > 0) {
      this.confirmationModalRef = this.modalService.open(content, { centered: true });
    }
    else {
      Swal.fire({
        title: 'حداقل یک مورد را انتخاب کنید!',
        icon: 'question',
        iconHtml: '!',
        confirmButtonText: 'تایید'
      })

    }
  }



  addItems() {
    this.inProgress = true;
    let command = new AddRangeProductKindsCommand();
    command.kindIds = [...this.checkedItems];
    command.productId = this.product?.id;
    this.client.addRange(command).subscribe(result => {
      this.inProgress = false;
      if (result == null) {
        Swal.fire({
          title: 'اضافه کردن نوع با موفقیت انجام شد',
          icon: 'success',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        })
        this.confirmationModalRef?.close();
        this.checkedItems = new Set<number>();
      } else {

        Swal.fire({
          title: 'اضافه کردن نوع با خطا مواجه شد',
          icon: 'success',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        })

        this.confirmationModalRef?.close();
        this.checkedItems = new Set<number>();
      }
    }, error => {
      this.inProgress = false;
      console.error(error)
    });

  }

  resetForm() {
    var checkboxes: any = document.getElementsByName('checkAll');
    for (var i = 0; i < checkboxes.length; i++) {
      checkboxes[i].checked = false;
    }
  }
}



