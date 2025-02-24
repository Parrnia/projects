import { UpdateProductTypeAttributeGroupAttributeCommand } from './../../../web-api-client';
import { Component, ElementRef, QueryList, ViewChild, ViewChildren } from '@angular/core';

import { Observable } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { NgbAlertConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ProductTypeAttributeGroupAttributesClient, ProductTypeAttributeGroupDto, ProductTypeAttributeGroupsClient } from 'src/app/web-api-client';
import { DecimalPipe } from '@angular/common';
import Swal from 'sweetalert2';
import { ProductAttributeGroupAttributeGridService } from './product-attribute-group-attribute-grid.service';
import { ProductAttributeGroupAttributeModel } from './product-attribute-group-attribute.model';
import { NgbdProductAttributeGroupAttributeSortableHeader, SortEvent } from './product-attribute-group-attribute-sortable.directive';
import { ExportFileService } from 'src/app/shared/services/export-file/export-file.service';

@Component({
  selector: "app-product-attribute-group-attribute",
  templateUrl: "./product-attribute-group-attribute.component.html",
  styleUrls: ["./product-attribute-group-attribute.component.scss"],
  providers: [
    ProductAttributeGroupAttributeGridService,
    DecimalPipe,
    NgbAlertConfig,
  ],
})
export class ProductAttributeGroupAttributeComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: ProductAttributeGroupAttributeModel;
  gridjsList$!: Observable<ProductAttributeGroupAttributeModel[]>;
  total$: Observable<number>;
  inProgress = false;
  inProgressAllExportbtn = false;
  productTypeAttributeGroupsList?: ProductTypeAttributeGroupDto[];
  @ViewChildren(NgbdProductAttributeGroupAttributeSortableHeader)
  productAttributeGroupAttributes!: QueryList<NgbdProductAttributeGroupAttributeSortableHeader>;
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("confirmationModal") confirmationModal: any;
  @ViewChild("formModal") formModal: any = [];
  @ViewChild("fileInput") fileInput!: ElementRef<HTMLInputElement>;
  productTypeAttributeGroupId?: any;
  formData = new FormData();
  checkedItems: Set<number> = new Set<number>();
  constructor(
    alertConfig: NgbAlertConfig,
    public client: ProductTypeAttributeGroupAttributesClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    private exportService: ExportFileService,
    public service: ProductAttributeGroupAttributeGridService,
    public productTypeAttributeGroupsClient: ProductTypeAttributeGroupsClient
  ) {
    this.gridjsList$ = service.productAttributeGroupAttributes$;
    this.total$ = service.total$;

    this.myForm = this.fb.group({
      id: 0,
      value: ["", [Validators.required]],
      productTypeAttributeGroupId: [0, [Validators.required]],
    });

    alertConfig.type = "success";
  }

  ngOnInit(): void {
    this.getAllProductTypeAttributeGroups();

    this.breadCrumbItems = [
      { label: "خوشه محصول" },
      { label: "ویژگی محصول", active: true },
    ];
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
    } else {
      Swal.fire({
        title: "حداقل یک مورد را انتخاب کنید!",
        icon: "question",
        iconHtml: "!",
        confirmButtonText: "تایید",
      });
    }
  }

  exportExcel() {
    this.inProgressAllExportbtn = true;
    this.client
      .exportExcelQuery(null, 1, null, null, null, null, null)
      .subscribe({
        next: (response) => {
          this.inProgressAllExportbtn = false;
          this.exportService.exportFile(response);
        },
        error: (error) => {
          this.inProgressAllExportbtn = false;
          console.error(error);
        },
      });
  }

  openGetDetailExportExcelModal() {
    this.modalService.open(this.getDetailExportTabModel, {
      size: "lg",
      backdrop: "static",
    });
  }

  /**
   * Sort table data
   * @param param0 sort the column
   *
   */
  onSort({ column, direction }: SortEvent) {
    // resetting other productAttributeGroupAttributes
    this.productAttributeGroupAttributes.forEach(
      (productAttributeGroupAttribute) => {
        if (productAttributeGroupAttribute.sortable !== column) {
          productAttributeGroupAttribute.direction = "";
        }
      }
    );

    this.service.sortColumn = column;
    this.service.sortDirection = direction;
  }
  get form() {
    return this.myForm.controls;
  }

  onSubmit(): void {
    this.inProgress = true;
    this.markAllControlsAsTouched(this.myForm);
    this.submit = true;
    this.myForm.value.modelId = parseInt(this.productTypeAttributeGroupId);
    if (this.myForm.valid) {
      if (this.selectedId > 0) {
        this.myForm.controls["id"].setValue(this.selectedId);
        let cmd = new UpdateProductTypeAttributeGroupAttributeCommand();
        cmd.id = this.myForm.controls["id"].value;
        cmd.productTypeAttributeGroupId =
          this.myForm.controls["productTypeAttributeGroupId"].value;
        cmd.value = this.myForm.controls["value"].value;

        this.client.update(this.myForm.controls["id"].value, cmd).subscribe(
          (result) => {
            this.inProgress = false;
            if (result == null) {
              this.service.getAllProductAttributeGroupAttribute();
              Swal.fire({
                title: "ذخیره ویژگی محصول با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره ویژگی محصول با خطا مواجه شد",
                icon: "error",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.modalService.dismissAll();
            }
          },
          (error) => {
            this.inProgress = false;
            console.error(error);
          }
        );
      } else {
        this.client.create(this.myForm.value).subscribe(
          (result) => {
            this.inProgress = false;
            if (result > 0) {
              this.service.getAllProductAttributeGroupAttribute();
              Swal.fire({
                title: "ذخیره ویژگی محصول با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });

              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره ویژگی محصول با خطا مواجه شد",
                icon: "error",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.modalService.dismissAll();
            }
          },
          (error) => {
            this.inProgress = false;
            console.error(error);
          }
        );
      }
    } else {
      this.inProgress = false;
    }
  }

  resetForm(): void {
    Object.keys(this.myForm.controls).forEach((controlName) => {
      const control = this.myForm.controls[controlName];
      if (control.enabled) {
        control.markAsPristine();
        control.markAsUntouched();
        control.reset();
      }
    });
  }

  edit(item: ProductAttributeGroupAttributeModel) {
    debugger;
    this.selectedId = item.id;

    this.myForm.controls["id"].setValue(this.selectedId ?? null);
    this.myForm.controls["value"].setValue(item.value ?? null);
    this.myForm.controls["productTypeAttributeGroupId"].setValue(
      item.productTypeAttributeGroupId ?? null
    );

    this.productTypeAttributeGroupId = item.productTypeAttributeGroupId ?? null;

    this.myForm.controls["value"].disable();
    this.myForm.controls["productTypeAttributeGroupId"].disable();

    this.modalService.open(this.formModal, { size: "lg", backdrop: "static" });
  }

  openInsertModal() {
    this.modalService.open(this.formModal, { size: "lg", backdrop: "static" });
  }

  openDeleteConfirmationModal(id: any) {
    this.selectedId = id;
    console.log("    this.selectedId = id;", this.selectedId);
    this.modalService.open(this.confirmationModal);
  }

  deleteItems() {
    this.inProgress = true;
    this.client.deleteRange([...this.checkedItems]).subscribe(
      (result) => {
        this.inProgress = false;
        if (result == null) {
          Swal.fire({
            title: "حذف ویژگی محصول با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.service.getAllProductAttributeGroupAttribute();
        } else {
          Swal.fire({
            title: "حذف ویژگی محصول با خطا مواجه شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });

          this.modalService.dismissAll();
        }
      },
      (error) => {
        this.inProgress = false;
        console.error(error);
      }
    );

    this.checkedItems = new Set<number>();
  }

  public handleCloseFormModal() {
    this.myForm.reset();
    this.myForm.markAsUntouched();
    this.myForm.setErrors(null);
    this.myForm.markAsPristine();
    this.myForm.controls["value"].enable();
    this.myForm.controls["productTypeAttributeGroupId"].enable();
    this.selectedId = 0;
  }

  public getAllProductTypeAttributeGroups() {
    this.productTypeAttributeGroupsClient
      .getAllProductTypeAttributeGroupsDropDown()
      .subscribe(
        (result) => {
          this.productTypeAttributeGroupsList = result;
        },
        (error) => console.error(error)
      );
  }
  markAllControlsAsTouched(formGroup: FormGroup): void {
    Object.values(formGroup.controls).forEach((control) => {
      if (control instanceof FormGroup) {
        this.markAllControlsAsTouched(control);
      } else {
        control.markAsTouched();
      }
    });
  }
}

