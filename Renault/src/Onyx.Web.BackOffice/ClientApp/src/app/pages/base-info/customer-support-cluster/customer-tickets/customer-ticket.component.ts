import { Component, ElementRef, QueryList, Renderer2, ViewChild, ViewChildren } from '@angular/core';

import { Observable, of } from 'rxjs';
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from '@angular/forms';

import { NgbAlertConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CustomerTicketsClient, CountriesClient, CountryDto, FileParameter, UpdateCustomerTicketCommand, CreateCustomerTicketCommand } from 'src/app/web-api-client';
import { DecimalPipe } from '@angular/common';
import Swal from 'sweetalert2';
import { CustomerTicketGridService } from './customer-ticket-grid.service';
import { CustomerTicketModel } from './customer-ticket.model';
import { NgbdCustomerTicketSortableHeader, SortEvent } from './customer-ticket-sortable.directive';
import { ExportFileService } from 'src/app/shared/services/export-file/export-file.service';


@Component({
  selector: "app-customer-ticket",
  templateUrl: "./customer-ticket.component.html",
  styleUrls: ["./customer-ticket.component.scss"],
  providers: [CustomerTicketGridService, DecimalPipe, NgbAlertConfig],
})
export class CustomerTicketComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: CustomerTicketModel;
  gridjsList$!: Observable<CustomerTicketModel[]>;
  total$: Observable<number>;
  inProgress = false;
  inProgressAllExportbtn = false;
  @ViewChildren(NgbdCustomerTicketSortableHeader)
  customerTickets!: QueryList<NgbdCustomerTicketSortableHeader>;
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("confirmationModal") confirmationModal: any;
  @ViewChild("formModal") formModal: any = [];
  @ViewChild("fileInput") fileInput!: ElementRef<HTMLInputElement>;
  formData = new FormData();
  checkedItems: Set<number> = new Set<number>();
  constructor(
    alertConfig: NgbAlertConfig,
    public client: CustomerTicketsClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    private exportService: ExportFileService,
    public service: CustomerTicketGridService
  ) {
    this.gridjsList$ = service.customerTickets$;
    this.total$ = service.total$;

    this.myForm = this.fb.group({
      id: 0,
      subject: ["", [Validators.required]],
      message: ["", [Validators.required]],
      customerPhoneNumber: ["", [Validators.required]],
      customerName: ["", [Validators.required]],
      customerId: "",
      isActive: false,
    });

    alertConfig.type = "success";
  }

  ngOnInit(): void {
    this.breadCrumbItems = [
      { label: "خوشه پشتیبانی مشتری" },
      { label: "تیکت", active: true },
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
    // resetting other customerTickets
    this.customerTickets.forEach((customerTicket) => {
      if (customerTicket.sortable !== column) {
        customerTicket.direction = "";
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

    if (this.myForm.valid) {
      if (this.selectedId > 0) {
        this.myForm.controls["id"].setValue(this.selectedId);
        let cmd = new UpdateCustomerTicketCommand();
        cmd.id = this.selectedId;
        cmd.subject = this.myForm.controls["subject"].value;
        cmd.message = this.myForm.controls["message"].value;
        cmd.isActive = this.myForm.controls["isActive"].value;
        this.client.update(this.myForm.controls["id"].value, cmd).subscribe(
          (result) => {
            this.inProgress = false;
            if (result == null) {
              this.service.refreshCustomerTickets();
              Swal.fire({
                title: "ذخیره تیکت با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره تیکت با خطا مواجه شد",
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
        this.myForm.controls["customerId"].setValue(
          localStorage.getItem("userId") ?? null
        );
        let cmd = new CreateCustomerTicketCommand();
        cmd.subject = this.myForm.controls["subject"].value;
        cmd.message = this.myForm.controls["message"].value;
        cmd.customerName = this.myForm.controls["customerName"].value;
        cmd.customerPhoneNumber =
          this.myForm.controls["customerPhoneNumber"].value;
        this.client.create(cmd).subscribe(
          (result) => {
            this.inProgress = false;
            if (result > 0) {
              this.service.refreshCustomerTickets();
              Swal.fire({
                title: "ذخیره تیکت با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });

              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره تیکت با خطا مواجه شد",
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

  edit(item: CustomerTicketModel) {
    debugger;
    this.selectedId = item.id;

    this.myForm.controls["id"].setValue(this.selectedId ?? null);
    this.myForm.controls["subject"].setValue(item.subject ?? null);
    this.myForm.controls["message"].setValue(item.message ?? null);
    this.myForm.controls["customerPhoneNumber"].setValue(
      item.customerPhoneNumber ?? null
    );
    this.myForm.controls["customerName"].setValue(item.customerName ?? null);
    this.myForm.controls["isActive"].setValue(item.isActive ?? null);

    this.myForm.controls["customerPhoneNumber"].disable();
    this.myForm.controls["customerName"].disable();

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
    this.client.deleteRangeCustomerTicket([...this.checkedItems]).subscribe(
      (result) => {
        this.inProgress = false;
        if (result == null) {
          Swal.fire({
            title: "حذف تیکت با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.service.refreshCustomerTickets();
        } else {
          Swal.fire({
            title: "حذف تیکت با خطا مواجه شد",
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
    this.selectedId = 0;
    this.myForm.controls["customerPhoneNumber"].enable();
    this.myForm.controls["customerName"].enable();
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

