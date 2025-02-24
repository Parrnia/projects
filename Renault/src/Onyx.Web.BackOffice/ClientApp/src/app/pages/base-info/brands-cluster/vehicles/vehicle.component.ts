import { Component, ElementRef, QueryList, Renderer2, ViewChild, ViewChildren } from '@angular/core';

import { Observable, of } from 'rxjs';
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from '@angular/forms';

import { NgbAlertConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { VehiclesClient, CountriesClient, CountryDto, FileParameter, FamiliesClient, ModelDto, ModelsClient, KindDto, KindsClient, UpdateVehicleCommand } from 'src/app/web-api-client';
import { DecimalPipe } from '@angular/common';
import Swal from 'sweetalert2';
import { VehicleGridService } from './vehicle-grid.service';
import { VehicleModel } from './vehicle.model';
import { NgbdVehicleSortableHeader, SortEvent } from './vehicle-sortable.directive';
import { ImageService } from 'src/app/core/services/image.service';
import { NonNullAssert } from '@angular/compiler';
import { User } from 'src/app/core/services/authService/models/entities/User';
import { AuthenticationService } from 'src/app/core/services/authService/auth.service';
import { VehicleValidators } from './vehicle-validators';
import { ExportFileService } from 'src/app/shared/services/export-file/export-file.service';

@Component({
  selector: "app-vehicle",
  templateUrl: "./vehicle.component.html",
  styleUrls: ["./vehicle.component.scss"],
  providers: [VehicleGridService, DecimalPipe, NgbAlertConfig],
})
export class VehicleComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: VehicleModel;
  gridjsList$!: Observable<VehicleModel[]>;
  total$: Observable<number>;
  kindsList?: KindDto[];
  customersList?: User[];
  inProgress = false;
  inProgressAllExportbtn = false;
  @ViewChildren(NgbdVehicleSortableHeader)
  vehicles!: QueryList<NgbdVehicleSortableHeader>;
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("confirmationModal") confirmationModal: any;
  @ViewChild("formModal") formModal: any = [];
  @ViewChild("fileInput") fileInput!: ElementRef<HTMLInputElement>;
  kindId?: any;
  customerId?: any;
  formData = new FormData();
  checkedItems: Set<number> = new Set<number>();
  constructor(
    alertConfig: NgbAlertConfig,
    public client: VehiclesClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    public service: VehicleGridService,
    private exportService: ExportFileService,
    public kindsClient: KindsClient,
    public authenticationService: AuthenticationService
  ) {
    this.gridjsList$ = service.vehicles$;
    this.total$ = service.total$;

    this.myForm = this.fb.group({
      id: 0,
      vinNumber: [
        "",
        [
          Validators.required,
          Validators.maxLength(17),
          Validators.minLength(17),
        ],
      ],
      kindId: [0, [Validators.required]],
      customerId: [0, [Validators.required]],
    });

    this.form.vinNumber.addAsyncValidators(
      VehicleValidators.validVehicleVinNumber(
        this.client,
        this.form.vinNumber.value
      )
    );
    this.form.kindId.addAsyncValidators(
      VehicleValidators.validVehicleKindId(this.client, this.form.kindId.value)
    );

    alertConfig.type = "success";

    this.form.id.valueChanges.subscribe((id) => {
      this.form.vinNumber.setAsyncValidators(
        VehicleValidators.validVehicleVinNumber(
          this.client,
          id != null ? id : 0
        )
      );
      this.form.kindId.setAsyncValidators(
        VehicleValidators.validVehicleKindId(this.client, id != null ? id : 0)
      );
    });
  }

  ngOnInit(): void {
    this.getAllKinds();
    this.getAllCustomers();

    this.breadCrumbItems = [
      { label: "خوشه برند" },
      { label: "خودرو", active: true },
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
    // resetting other vehicles
    this.vehicles.forEach((vehicle) => {
      if (vehicle.sortable !== column) {
        vehicle.direction = "";
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
    debugger;
    console.log("form value", this.myForm.value);
    this.submit = true;
    this.myForm.value.kindId = parseInt(this.kindId);
    this.myForm.value.customerId = this.customerId;

    if (this.myForm.valid) {
      console.log("form value", this.myForm.value);

      if (this.selectedId > 0) {
        this.myForm.controls["id"].setValue(this.selectedId);
        let cmd = new UpdateVehicleCommand();
        cmd.id = this.myForm.controls["id"].value;
        cmd.vinNumber = this.myForm.controls["vinNumber"].value;
        cmd.kindId = this.myForm.controls["kindId"].value;
        cmd.customerId = this.myForm.controls["customerId"].value;
        this.client.update(this.myForm.controls["id"].value, cmd).subscribe(
          (result) => {
            this.inProgress = false;
            if (result == null) {
              this.service.getAllVehicle();
              Swal.fire({
                title: "ذخیره خودرو با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره خودرو با خطا مواجه شد",
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
              this.service.getAllVehicle();
              Swal.fire({
                title: "ذخیره خودرو با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });

              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره خودرو با خطا مواجه شد",
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

  edit(item: VehicleModel) {
    debugger;
    this.selectedId = item.id;

    this.myForm.controls["id"].setValue(this.selectedId ?? null);
    this.myForm.controls["vinNumber"].setValue(item.vinNumber ?? null);
    this.myForm.controls["kindId"].setValue(item.kindId ?? null);
    this.myForm.controls["customerId"].setValue(item.customerId ?? null);

    this.kindId = item.kindId ?? null;
    this.customerId = item.customerId ?? null;

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
    this.client.deleteRangeVehicle([...this.checkedItems]).subscribe(
      (result) => {
        this.inProgress = false;
        if (result == null) {
          Swal.fire({
            title: "حذف خودرو با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.service.getAllVehicle();
        } else {
          Swal.fire({
            title: "حذف خودرو با خطا مواجه شد",
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

  public getAllKinds() {
    this.kindsClient.getAllKinds().subscribe(
      (result) => {
        this.kindsList = result;
      },
      (error) => console.error(error)
    );
  }
  public getAllCustomers() {
    this.authenticationService.getCustomers().subscribe(
      (result) => {
        this.customersList = result;
      },
      (error) => console.error(error)
    );
  }

  public handleCloseFormModal() {
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
}

