import { Component, ElementRef, QueryList, Renderer2, ViewChild, ViewChildren } from '@angular/core';

import { Observable, of } from 'rxjs';
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from '@angular/forms';

import { NgbAlertConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CustomerTypesClient, CountriesClient, CountryDto, FileParameter } from 'src/app/web-api-client';
import { DecimalPipe } from '@angular/common';
import Swal from 'sweetalert2';
import { CustomerTypeGridService } from './customer-type-grid.service';
import { CustomerTypeModel } from './customer-type.model';
import { NgbdCustomerTypeSortableHeader, SortEvent } from './customer-type-sortable.directive';
import { ImageService } from 'src/app/core/services/image.service';
import { NonNullAssert } from '@angular/compiler';
import { CustomerTypeValidators } from './customer-type-validators';
import { ExportFileService } from 'src/app/shared/services/export-file/export-file.service';

@Component({
  selector: "app-customer-type",
  templateUrl: "./customer-type.component.html",
  styleUrls: ["./customer-type.component.scss"],
  providers: [CustomerTypeGridService, DecimalPipe, NgbAlertConfig],
})
export class CustomerTypeComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: CustomerTypeModel;
  gridjsList$!: Observable<CustomerTypeModel[]>;
  total$: Observable<number>;
  inProgress = false;
  inProgressAllExportbtn = false;
  customerTypesList? = [1, 2, 3, 4];
  customerType?: any;
  @ViewChildren(NgbdCustomerTypeSortableHeader)
  customerTypes!: QueryList<NgbdCustomerTypeSortableHeader>;
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("confirmationModal") confirmationModal: any;
  @ViewChild("formModal") formModal: any = [];
  @ViewChild("fileInput") fileInput!: ElementRef<HTMLInputElement>;
  customerTypeTypeId?: any;
  countryId?: any;
  formData = new FormData();
  checkedItems: Set<number> = new Set<number>();
  constructor(
    alertConfig: NgbAlertConfig,
    public client: CustomerTypesClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    private exportService: ExportFileService,
    public service: CustomerTypeGridService
  ) {
    this.gridjsList$ = service.customerTypes$;
    this.total$ = service.total$;

    this.myForm = this.fb.group({
      id: 0,
      discountPercent: [null, [Validators.required]],
      customerTypeEnum: [0, [Validators.required]],
    });

    this.form.customerTypeEnum.addAsyncValidators(
      CustomerTypeValidators.validCustomerType(this.client, 0)
    );
    alertConfig.type = "success";

    this.form.id.valueChanges.subscribe((id) => {
      this.form.customerTypeEnum.setAsyncValidators(
        CustomerTypeValidators.validCustomerType(
          this.client,
          id != null ? id : 0
        )
      );
    });
  }

  ngOnInit(): void {
    this.breadCrumbItems = [
      { label: "خوشه مدیریت کاربران" },
      { label: "نوع مشتری ", active: true },
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
    // resetting other customerTypes
    this.customerTypes.forEach((customerType) => {
      if (customerType.sortable !== column) {
        customerType.direction = "";
      }
    });

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
    this.myForm.value.customerTypeEnum = parseInt(this.customerType);
    if (this.myForm.valid) {
      debugger;
      if (this.selectedId > 0) {
        this.myForm.controls["id"].setValue(this.selectedId);

        this.client
          .update(this.myForm.controls["id"].value, this.myForm.value)
          .subscribe(
            (result) => {
              this.inProgress = false;
              if (result == null) {
                this.service.getAllCustomerType();
                Swal.fire({
                  title: "ذخیره نوع مشتری با موفقیت انجام شد",
                  icon: "success",
                  iconHtml: "!",
                  confirmButtonText: "تایید",
                });
                this.modalService.dismissAll();
                this.handleCloseFormModal();
              } else {
                Swal.fire({
                  title: "ذخیره نوع مشتری با خطا مواجه شد",
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
              this.service.getAllCustomerType();
              Swal.fire({
                title: "ذخیره نوع مشتری با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره نوع مشتری با خطا مواجه شد",
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

  edit(item: CustomerTypeModel) {
    debugger;
    this.selectedId = item.id;

    this.myForm.controls["id"].setValue(this.selectedId ?? null);
    this.myForm.controls["discountPercent"].setValue(
      item.discountPercent ?? null
    );
    this.myForm.controls["customerTypeEnum"].setValue(
      item.customerTypeEnum ?? null
    );

    this.customerType = item.customerTypeEnum ?? null;

    this.modalService.open(this.formModal, { size: "lg", backdrop: "static" });
  }

  openInsertModal() {
    this.modalService.open(this.formModal, { size: "lg", backdrop: "static" });
  }

  openDeleteConfirmationModal(id: any) {
    this.selectedId = id;
    this.modalService.open(this.confirmationModal);
  }

  deleteItems() {
    this.inProgress = true;
    this.client.deleteRangeCustomerType([...this.checkedItems]).subscribe(
      (result) => {
        this.inProgress = false;
        if (result == null) {
          Swal.fire({
            title: "حذف نوع مشتری با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.service.getAllCustomerType();
        } else {
          Swal.fire({
            title: "حذف نوع مشتری با خطا مواجه شد",
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
    debugger;
    this.myForm.reset();
    this.myForm.markAsUntouched();
    this.myForm.setErrors(null);
    this.myForm.markAsPristine();
    this.selectedId = 0;
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

  public getCustomerTypeNameByEnglishName(name: string) {
    switch (name) {
      case "Personal":
        return "شخصی";
      case "Store":
        return "فروشگاهی";
      case "Agency":
        return "نمایندگی";
      case "CentralRepairShop":
        return "تعمیرگاه مرکزی";
      default:
        return "";
    }
  }
  public getCustomerTypeName(id: number) {
    switch (id) {
      case 1:
        return "شخصی";
      case 2:
        return "فروشگاهی";
      case 3:
        return "نمایندگی";
      case 4:
        return "تعمیرگاه مرکزی";
      default:
        return "";
    }
  }

  public getCustomerTypeId(name: string) {
    switch (name) {
      case "Personal":
        return "1";
      case "Store":
        return "2";
      case "Agency":
        return "3";
      case "CentralRepairShop":
        return "4";
      default:
        return "";
    }
  }
}

