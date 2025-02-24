


import { Component, Input, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { DecimalPipe } from "@angular/common";
import { NgbAlertConfig, NgbModal, NgbModalRef } from "@ng-bootstrap/ng-bootstrap";
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from "@angular/forms";
import { Observable } from "rxjs";
import Swal from "sweetalert2";
import { FileParameter, KindDto, KindsClient, ProductAttributeTypesClient, ProductAttributesClient, ProductKindsClient } from "src/app/web-api-client";
import { ProductAttributesGridService } from "./product-attributes-grid.service";
import { ProductAttributesModel } from "./product-attributes.model";

@Component({
  selector: 'app-product-attributes',
  templateUrl: './product-attributes.component.html',
  styleUrls: ['./product-attributes.component.scss'],
  providers: [ProductAttributesGridService, DecimalPipe, NgbAlertConfig],
})
export class ProductAttributesComponent {
  // bread crumb items
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myFormEdit!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: ProductAttributesModel;
  gridjsList$!: Observable<ProductAttributesModel[]>;
  total$: Observable<number>;
  inProgress = false;
  productAttributesGroupList?: any[];
  productAttributesList?: any[];
  productAttributesGroupId?: any;
  productAttributes?:any;
  // @ViewChildren(NgbdProductImagesSortableHeader) productImages!: QueryList<NgbdProductImagesSortableHeader>;
  @ViewChild('confirmationModal') confirmationModal: any;
  @ViewChild('formModal') formModal: any = [];
  @ViewChild('formEditModal') formEditModal: any = [];
  formModalUpdateRef: NgbModalRef | undefined;
  @Input() product?: any;

  checkedItems: Set<number> = new Set<number>();
  constructor(alertConfig: NgbAlertConfig, public client: ProductAttributesClient, public groupsClient: ProductAttributeTypesClient, private fb: FormBuilder, private modalService: NgbModal, public service: ProductAttributesGridService, private formBuilder: UntypedFormBuilder) {
    this.gridjsList$ = service.productAttributes$;
    this.total$ = service.total$;
    this.myFormEdit = this.fb.group({
      id: [0, [Validators.required]],
      valueName: ['', [Validators.required]],
      featured: false,
      name: ['', [Validators.required]]
    });

    alertConfig.type = 'success';
  }

  ngOnInit(): void {

    this.service.getAllProductAttributes(this.product.id);
    //this.getAllAttributesGroup();
  }

  // onSelectedGroup(event:any) {
       
  //   if (event.target.value != undefined && event.target.value  != null && event.target.value  >0 )
  //   {
  //     var id = parseInt(event.target.value );
  //     this.getAllAttributesByGroup(id);
  //   }
    
  // }


  // checkedValGet: any[] = [];
  // deleteMultiple(content: any) {
  //   var checkboxes: any = document.getElementsByName('checkAll');
  //   var result
  //   var checkedVal: any[] = [];
  //   for (var i = 0; i < checkboxes.length; i++) {
  //     if (checkboxes[i].checked) {
  //       result = parseInt(checkboxes[i].value);
  //       checkedVal.push(result);
  //     }
  //   }
  //   if (checkedVal.length > 0) {
  //     this.modalService.open(content, { centered: true });
  //   }
  //   else {
  //     Swal.fire({
  //       title: 'حداقل یک مورد را انتخاب کنید!',
  //       icon: 'question',
  //       iconHtml: '!',
  //       confirmButtonText: 'تایید'
  //     })

  //     // Swal.fire({ text: 'Please select at least one checkbox', confirmButtonColor: '#299cdb', });
  //   }
  //   this.checkedValGet = checkedVal;

  // }


  get form() {
    return this.myFormEdit.controls;
  }


  onSubmit(): void {
    this.inProgress = true;
    this.markAllControlsAsTouched(this.myFormEdit);
    this.myFormEdit.value.productId = this.product.id;
    this.submit = true;
    if (this.myFormEdit.valid) {

      this.client.update(this.selectedId ,this.myFormEdit.value).subscribe(result => {
        this.inProgress = false;
        if (result == null) {
          this.service.getAllProductAttributes(this.product.id);
          Swal.fire({
            title: 'ذخیره ویژگی محصول با موفقیت انجام شد',
            icon: 'success',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });

          //this.formModal.nativeElement.click();

          this.formModalUpdateRef?.close();
          this.myFormEdit.reset();

        } else {
          Swal.fire({
            title: 'ذخیره ویژگی محصول با خطا مواجه شد',
            icon: 'error',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });
          this.formModalUpdateRef?.close();
        }
      }, error => {
        this.inProgress = false;
       console.error(error)});
    }else{
      this.inProgress = false;
    }
  }

  edit(item: ProductAttributesModel) {
    debugger;
    this.selectedId = item.id;
    this.myFormEdit.controls['id'].setValue(this.selectedId ?? null);
    this.myFormEdit.controls['valueName'].setValue(item.valueName ?? null);
    this.myFormEdit.controls['featured'].setValue(item.featured ?? null);
    this.myFormEdit.controls['name'].setValue(item.name ?? null);
    this.myFormEdit.controls['name'].disable();
    this.openEditModal();
  }

  resetForm(): void {
    Object.keys(this.myFormEdit.controls).forEach(controlName => {
      const control = this.myFormEdit.controls[controlName];
      if (control.enabled) {
        control.markAsPristine();
        control.markAsUntouched();
        control.reset();
      }
    });
  }


  openInsertModal() {
    this.modalService.open(this.formModal);
  }

  openEditModal() {
    debugger;
    this.formModalUpdateRef = this.modalService.open(this.formEditModal);
  }



  // openDeleteConfirmationModal(id: any) {

  //   this.selectedId = id;
  //   this.modalService.open(this.confirmationModal);
  // }


  // deleteItems() {
  //   this.client.deleteRange([...this.checkedItems]).subscribe(result => {
       
  //     if (result == null) {

  //       Swal.fire({
  //         title: 'حذف ویژگی محصول با موفقیت انجام شد',
  //         icon: 'success',
  //         iconHtml: '!',
  //         confirmButtonText: 'تایید'
  //       })
  //       // this.modalService.dismissAll();
  //       this.service.getAllProductAttributes(this.product.id);

  //     } else {

  //       Swal.fire({
  //         title: 'حذف ویژگی محصول با خطا مواجه شد',
  //         icon: 'success',
  //         iconHtml: '!',
  //         confirmButtonText: 'تایید'
  //       })

  //       this.modalService.dismissAll();


  //     }
  //   }, error => console.error(error));

  //   this.selectedId = 0;

  // }

  public getAllAttributesGroup() {
    this.groupsClient.getAllProductAttributeTypesGroup().subscribe(result => {
      this.productAttributesGroupList = result;
    }, error => console.error(error));

  }

  public getAllAttributesByGroup(id : number) {
   
    this.groupsClient.getAllProductTypeAttributeGroupAttributeByGroupId(id).subscribe(result => {
      this.productAttributesList = result;
    }, error => console.error(error));

  }
  public handleCloseFormModal(){
    this.myFormEdit.reset();
    this.myFormEdit.markAsUntouched();
    this.myFormEdit.setErrors(null);
    this.myFormEdit.markAsPristine();
    this.selectedId = 0;
  }
  markAllControlsAsTouched(formGroup: FormGroup): void {
    Object.values(formGroup.controls).forEach(control => {
      if (control instanceof FormGroup) {
        this.markAllControlsAsTouched(control);
      } else {
        control.markAsTouched();
      }
    });
  }
}



