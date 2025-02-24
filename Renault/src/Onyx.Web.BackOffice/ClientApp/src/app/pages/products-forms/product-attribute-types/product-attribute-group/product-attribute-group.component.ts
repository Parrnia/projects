



import { Component, Input, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { DecimalPipe } from "@angular/common";
import { NgbAlertConfig, NgbModal, NgbModalRef } from "@ng-bootstrap/ng-bootstrap";
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from "@angular/forms";
import { Observable } from "rxjs";
import Swal from "sweetalert2";
import { ProductAttributeGroupGridService } from "./product-attribute-group-grid.service";
import { ProductAttributeGroupModel } from "./product-attribute-group.model";
import { AddProductAttributeGroupsCommand, AllProductTypeAttributeGroupDropDownDto, ProductAttributeTypesClient, ProductAttributesClient, ProductTypeAttributeGroupsClient } from "src/app/web-api-client";

@Component({
  selector: 'app-product-attribute-group',
  templateUrl: './product-attribute-group.component.html',
  styleUrls: ['./product-attribute-group.component.scss'],
  providers: [ProductAttributeGroupGridService, DecimalPipe, NgbAlertConfig],
})
export class ProductAttributeGroupComponent {
  // bread crumb items
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: AllProductTypeAttributeGroupDropDownDto;
  gridjsList$!: Observable<ProductAttributeGroupModel[]>;
  total$: Observable<number>;
  inProgress = false;
  productAttributeGroupList?: AllProductTypeAttributeGroupDropDownDto[];
  productAttributeGroupId?: any;
  @ViewChild('confirmationModal') confirmationModal: any;
  confirmationModalRef: NgbModalRef | undefined;
  @ViewChild('formModal') formModal: any = [];
  formModalRef: NgbModalRef | undefined;
  @ViewChild('addModel') addModel: any = [];
  addModelRef: NgbModalRef | undefined;
  @Input() productAttributeType?: any;
  checkedItems: Set<number> = new Set<number>();
  constructor(alertConfig: NgbAlertConfig, public client: ProductAttributeTypesClient, private fb: FormBuilder, private modalService: NgbModal, public service: ProductAttributeGroupGridService, private formBuilder: UntypedFormBuilder) {
    this.gridjsList$ = service.productAttributeGroups$;
    this.total$ = service.total$;
  }

  ngOnInit(): void {
    this.service.getAllProductAttributeGroups();
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



  addItems() {
    this.inProgress = true;
    let command = new AddProductAttributeGroupsCommand();
    command.attributeGroupIds = [...this.checkedItems];
    command.id = this.productAttributeType?.id;
    this.client.addProductAttributeGroups(this.productAttributeType?.id, command).subscribe(result => {
      this.inProgress = false;
      if (result == null) {
        Swal.fire({
          title: 'اضافه کردن گروه بندی ویژگی با موفقیت انجام شد',
          icon: 'success',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        })
        debugger;
        this.confirmationModalRef?.close();
        this.checkedItems = new Set<number>();
      } else {

        Swal.fire({
          title: 'اضافه کردن گروه بندی ویژگی با خطا مواجه شد',
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

  resetForm(){
    var checkboxes: any = document.getElementsByName('checkAll');
    for (var i = 0; i < checkboxes.length; i++) {
        checkboxes[i].checked = false;
    }
  }
}



