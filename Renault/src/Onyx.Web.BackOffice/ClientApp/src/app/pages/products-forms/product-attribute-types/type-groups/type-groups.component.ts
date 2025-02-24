import { RemoveProductAttributeGroupsCommand } from './../../../../web-api-client';




import { Component, Input, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { DecimalPipe } from "@angular/common";
import { NgbAlertConfig, NgbModal, NgbModalRef } from "@ng-bootstrap/ng-bootstrap";
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from "@angular/forms";
import { Observable } from "rxjs";
import Swal from "sweetalert2";
import { FileParameter, KindDto, KindsClient, ProductAttributeTypesClient, ProductTypeAttributeGroupDto, ProductTypeAttributeGroupsClient } from "src/app/web-api-client";
import { TypeGroupsGridService } from "./type-groups-grid.service";
import { TypeGroupsModel } from "./type-groups.model";

@Component({
  selector: 'app-type-groups',
  templateUrl: './type-groups.component.html',
  styleUrls: ['./type-groups.component.scss'],
  providers: [TypeGroupsGridService, DecimalPipe, NgbAlertConfig],
})
export class TypeGroupsComponent {
  // bread crumb items
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: TypeGroupsModel;
  gridjsList$!: Observable<TypeGroupsModel[]>;
  total$: Observable<number>;
  inProgress = false;
  productTypeAttributeGroupList?: ProductTypeAttributeGroupDto[];
  productTypeAttributeGroupId?: any;
  // @ViewChildren(NgbdProductImagesSortableHeader) productImages!: QueryList<NgbdProductImagesSortableHeader>;
  @ViewChild('confirmationModal') confirmationModal: any;
  confirmationModalRef: NgbModalRef | undefined;
  @ViewChild('formModal') formModal: any = [];
  @Input() productAttributeType?: any;

  checkedItems: Set<number> = new Set<number>();
  constructor(alertConfig: NgbAlertConfig,
     public client: ProductAttributeTypesClient,
     public kindsClient: KindsClient, 
     private fb: FormBuilder, 
     private modalService: NgbModal, 
     public service: TypeGroupsGridService,
     private productTypeAttributeGroupsClient: ProductTypeAttributeGroupsClient) {
    this.gridjsList$ = service.typeGroups$;
    this.total$ = service.total$;
  }

  ngOnInit(): void {
    debugger;
    this.service.getAllProductTypeAttributeGroups(this.productAttributeType.id);
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
    let cmd = new RemoveProductAttributeGroupsCommand();
    cmd.id = this.productAttributeType.id;
    cmd.attributeGroupIds = [...this.checkedItems];
    this.client.removeProductAttributeGroups(
      this.productAttributeType.id,
      cmd).subscribe(result => {
        this.inProgress = false;
      if (result == null) {

        Swal.fire({
          title: 'حذف گروه ویژگی محصول با موفقیت انجام شد',
          icon: 'success',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        })
        this.confirmationModalRef?.close();
        this.service.getAllProductTypeAttributeGroups(this.productAttributeType.id);

      } else {

        Swal.fire({
          title: 'حذف گروه ویژگی محصول با خطا مواجه شد',
          icon: 'success',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        })
        this.modalService.dismissAll();
      }
    }, error => {
      this.inProgress = false;
     console.error(error)});

    this.selectedId = 0;
  }
}



