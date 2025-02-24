



import { Component, Input, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { DecimalPipe } from "@angular/common";
import { NgbAlertConfig, NgbModal, NgbModalRef } from "@ng-bootstrap/ng-bootstrap";
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from "@angular/forms";
import { Observable } from "rxjs";
import Swal from "sweetalert2";
import { FileParameter, KindDto, KindsClient, ProductKindsClient } from "src/app/web-api-client";
import { ProductKindsGridService } from "./product-kinds-grid.service";
import { ProductKindsModel } from "./product-kinds.model";

@Component({
  selector: 'app-product-kinds',
  templateUrl: './product-kinds.component.html',
  styleUrls: ['./product-kinds.component.scss'],
  providers: [ProductKindsGridService, DecimalPipe, NgbAlertConfig],
})
export class ProductKindsComponent {
  // bread crumb items
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: ProductKindsModel;
  gridjsList$!: Observable<ProductKindsModel[]>;
  total$: Observable<number>;
  kindList?: KindDto[];
  kindId?: any;
  inProgress = false;
  // @ViewChildren(NgbdProductImagesSortableHeader) productImages!: QueryList<NgbdProductImagesSortableHeader>;
  @ViewChild('confirmationModal') confirmationModal: any;
  confirmationModalRef: NgbModalRef | undefined;
  @ViewChild('formModal') formModal: any = [];
  @Input() product?: any;

  checkedItems: Set<number> = new Set<number>();
  constructor(alertConfig: NgbAlertConfig, public client: ProductKindsClient, public kindsClient: KindsClient, private fb: FormBuilder, private modalService: NgbModal, public service: ProductKindsGridService, private formBuilder: UntypedFormBuilder) {
    this.gridjsList$ = service.productKinds$;
    this.total$ = service.total$;
  }

  ngOnInit(): void {
    this.service.getAllProductKinds(this.product.id);
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

  deleteMultiple(content: any) {
    if ([...this.checkedItems].length > 0) {
      this.modalService.open(content, { centered: true });
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

  openInsertModal() {
    this.modalService.open(this.formModal);
  }


  openDeleteConfirmationModal(id: any) {

    this.selectedId = id;
    this.modalService.open(this.confirmationModal);
  }


  deleteItems() {
    this.inProgress = true;
    this.client.deleteRange([...this.checkedItems]).subscribe(result => {
      this.inProgress = false;
      if (result == null) {

        Swal.fire({
          title: 'حذف نوع محصول با موفقیت انجام شد',
          icon: 'success',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        })
        this.confirmationModalRef?.close();
        this.service.refresh(this.product.id);

      } else {
        this.confirmationModalRef?.close();
        Swal.fire({
          title: 'حذف نوع محصول با خطا مواجه شد',
          icon: 'success',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        })
      }
    }, error => {
      this.inProgress = false;
     console.error(error)
    });

    this.checkedItems = new Set<number>();
  }

}



