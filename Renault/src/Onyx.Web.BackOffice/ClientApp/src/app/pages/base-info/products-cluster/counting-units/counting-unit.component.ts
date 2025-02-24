import { Component, ElementRef, QueryList, Renderer2, ViewChild, ViewChildren } from '@angular/core';

import { Observable, of } from 'rxjs';
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from '@angular/forms';

import { NgbAlertConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CountingUnitTypeDto, CountingUnitTypesClient, CountingUnitsClient, CountriesClient, CountryDto, FileParameter, UpdateCountingUnitCommand } from 'src/app/web-api-client';
import { DecimalPipe } from '@angular/common';
import Swal from 'sweetalert2';
import { CountingUnitGridService } from './counting-unit-grid.service';
import { CountingUnitModel } from './counting-unit.model';
import { NgbdCountingUnitSortableHeader, SortEvent } from './counting-unit-sortable.directive';
import { ImageService } from 'src/app/core/services/image.service';
import { NonNullAssert } from '@angular/compiler';
import { CountingUnitValidators } from './counting-unit-validators';
import { ExportFileService } from 'src/app/shared/services/export-file/export-file.service';

@Component({
  selector: "app-counting-unit",
  templateUrl: "./counting-unit.component.html",
  styleUrls: ["./counting-unit.component.scss"],
  providers: [CountingUnitGridService, DecimalPipe, NgbAlertConfig],
})
export class CountingUnitComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: CountingUnitModel;
  gridjsList$!: Observable<CountingUnitModel[]>;
  total$: Observable<number>;
  inProgress = false;
  inProgressAllExportbtn = false;
  countingUnitTypesList?: CountingUnitTypeDto[];
  countingUnitTypeId?: any;
  @ViewChildren(NgbdCountingUnitSortableHeader)
  countingUnits!: QueryList<NgbdCountingUnitSortableHeader>;
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("confirmationModal") confirmationModal: any;
  @ViewChild("formModal") formModal: any = [];
  @ViewChild("fileInput") fileInput!: ElementRef<HTMLInputElement>;
  formData = new FormData();
  checkedItems: Set<number> = new Set<number>();
  constructor(
    alertConfig: NgbAlertConfig,
    public client: CountingUnitsClient,
    public countingUnitTypesClient: CountingUnitTypesClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    private exportService: ExportFileService,
    public service: CountingUnitGridService
  ) {
    this.gridjsList$ = service.countingUnits$;
    this.total$ = service.total$;

    this.myForm = this.fb.group({
      id: 0,
      code: [0, [Validators.required]],
      localizedName: ["", [Validators.required, Validators.maxLength(50)]],
      name: ["", [Validators.required, Validators.maxLength(50)]],
      isDecimal: false,
      countingUnitTypeId: null,
    });

    this.form.name.addAsyncValidators(
      CountingUnitValidators.validCountingUnitName(
        this.client,
        this.form.name.value
      )
    );
    this.form.localizedName.addAsyncValidators(
      CountingUnitValidators.validCountingUnitLocalizedName(
        this.client,
        this.form.localizedName.value
      )
    );

    alertConfig.type = "success";

    this.form.id.valueChanges.subscribe((id) => {
      this.form.name.setAsyncValidators(
        CountingUnitValidators.validCountingUnitName(
          this.client,
          id != null ? id : 0
        )
      );
      this.form.localizedName.setAsyncValidators(
        CountingUnitValidators.validCountingUnitLocalizedName(
          this.client,
          id != null ? id : 0
        )
      );
    });
  }

  ngOnInit(): void {
    this.getAllCountingUnitTypes();

    this.breadCrumbItems = [
      { label: "خوشه ویژگی های محصول" },
      { label: "واحد شمارش", active: true },
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
    // resetting other countingUnits
    this.countingUnits.forEach((countingUnit) => {
      if (countingUnit.sortable !== column) {
        countingUnit.direction = "";
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
    this.myForm.value.countingUnitTypeId = parseInt(this.countingUnitTypeId);
    if (this.myForm.valid) {
      if (this.selectedId > 0) {
        this.myForm.controls["id"].setValue(this.selectedId);
        let cmd = new UpdateCountingUnitCommand();
        cmd.id = this.myForm.controls["id"].value;
        cmd.code = this.myForm.controls["code"].value;
        cmd.localizedName = this.myForm.controls["localizedName"].value;
        cmd.name = this.myForm.controls["name"].value;
        cmd.isDecimal = this.myForm.controls["isDecimal"].value;
        cmd.countingUnitTypeId =
          this.myForm.controls["countingUnitTypeId"].value;

        this.client.update(this.myForm.controls["id"].value, cmd).subscribe(
          (result) => {
            this.inProgress = false;
            if (result == null) {
              this.service.getAllCountingUnit();
              Swal.fire({
                title: "ذخیره واحد شمارش با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره واحد شمارش با خطا مواجه شد",
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
              this.service.getAllCountingUnit();
              Swal.fire({
                title: "ذخیره واحد شمارش با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });

              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره واحد شمارش با خطا مواجه شد",
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

  edit(item: CountingUnitModel) {
    debugger;
    this.selectedId = item.id;

    this.myForm.controls["id"].setValue(this.selectedId ?? null);
    this.myForm.controls["code"].setValue(item.code ?? null);
    this.myForm.controls["localizedName"].setValue(item.localizedName ?? null);
    this.myForm.controls["name"].setValue(item.name ?? null);
    this.myForm.controls["isDecimal"].setValue(item.isDecimal ?? null);

    this.countingUnitTypeId = item.countingUnitTypeId ?? null;

    this.myForm.controls["id"].disable();
    this.myForm.controls["code"].disable();
    this.myForm.controls["localizedName"].disable();
    this.myForm.controls["name"].disable();
    this.myForm.controls["isDecimal"].disable();
    this.myForm.controls["countingUnitTypeId"].disable();
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
    this.client.deleteRangeCountingUnit([...this.checkedItems]).subscribe(
      (result) => {
        this.inProgress = false;
        if (result == null) {
          Swal.fire({
            title: "حذف واحد شمارش با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.service.getAllCountingUnit();
        } else {
          Swal.fire({
            title: "حذف واحد شمارش با خطا مواجه شد",
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

  public getAllCountingUnitTypes() {
    this.countingUnitTypesClient.getAllCountingUnitTypes().subscribe(
      (result) => {
        debugger;
        this.countingUnitTypesList = result;
      },
      (error) => console.error(error)
    );
  }

  public handleCloseFormModal() {
    this.myForm.reset();
    this.myForm.markAsUntouched();
    this.myForm.setErrors(null);
    this.myForm.markAsPristine();
    this.myForm.controls["id"].enable();
    this.myForm.controls["code"].enable();
    this.myForm.controls["localizedName"].enable();
    this.myForm.controls["name"].enable();
    this.myForm.controls["isDecimal"].enable();
    this.myForm.controls["countingUnitTypeId"].enable();
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

