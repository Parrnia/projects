import { Component, ElementRef, QueryList, Renderer2, ViewChild, ViewChildren } from '@angular/core';

import { Observable, of } from 'rxjs';
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from '@angular/forms';

import { NgbAlertConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { KindsClient, CountriesClient, CountryDto, FileParameter, FamiliesClient, ModelDto, ModelsClient, UpdateKindCommand } from 'src/app/web-api-client';
import { DecimalPipe } from '@angular/common';
import Swal from 'sweetalert2';
import { KindGridService } from './kind-grid.service';
import { KindModel } from './kind.model';
import { NgbdKindSortableHeader, SortEvent } from './kind-sortable.directive';
import { ImageService } from 'src/app/core/services/image.service';
import { NonNullAssert } from '@angular/compiler';
import { KindValidators } from './kind-validators';
import { ExportFileService } from 'src/app/shared/services/export-file/export-file.service';

@Component({
  selector: "app-kind",
  templateUrl: "./kind.component.html",
  styleUrls: ["./kind.component.scss"],
  providers: [KindGridService, DecimalPipe, NgbAlertConfig],
})
export class KindComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: KindModel;
  gridjsList$!: Observable<KindModel[]>;
  total$: Observable<number>;
  modelsList?: ModelDto[];
  inProgress = false;
  inProgressAllExportbtn = false;
  @ViewChildren(NgbdKindSortableHeader)
  kinds!: QueryList<NgbdKindSortableHeader>;
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("confirmationModal") confirmationModal: any;
  @ViewChild("formModal") formModal: any = [];
  @ViewChild("fileInput") fileInput!: ElementRef<HTMLInputElement>;
  modelId?: any;
  formData = new FormData();
  checkedItems: Set<number> = new Set<number>();
  constructor(
    alertConfig: NgbAlertConfig,
    public client: KindsClient,
    private fb: FormBuilder,
    private exportService: ExportFileService,
    private modalService: NgbModal,
    public service: KindGridService,
    public modelsClient: ModelsClient
  ) {
    this.gridjsList$ = service.kinds$;
    this.total$ = service.total$;

    this.myForm = this.fb.group({
      id: 0,
      name: ["", [Validators.required, Validators.maxLength(250)]],
      localizedName: ["", [Validators.required, Validators.maxLength(250)]],
      modelId: [0, [Validators.required]],
      isActive: false,
    });

    this.form.name.addAsyncValidators(
      KindValidators.validKindName(this.client, 0)
    );
    this.form.localizedName.addAsyncValidators(
      KindValidators.validKindLocalizedName(this.client, 0)
    );

    alertConfig.type = "success";
    this.form.id.valueChanges.subscribe((id) => {
      this.form.name.setAsyncValidators(
        KindValidators.validKindName(this.client, id != null ? id : 0)
      );
      this.form.localizedName.setAsyncValidators(
        KindValidators.validKindLocalizedName(this.client, id != null ? id : 0)
      );
    });
  }

  ngOnInit(): void {
    this.getAllModels();

    this.breadCrumbItems = [
      { label: "خوشه برند" },
      { label: "نوع", active: true },
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
    // resetting other kinds
    this.kinds.forEach((kind) => {
      if (kind.sortable !== column) {
        kind.direction = "";
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
    console.log("selecte kind type", this.modelId);
    this.submit = true;
    this.myForm.value.modelId = parseInt(this.modelId);
    if (this.myForm.valid) {
      console.log("form value", this.myForm.value);
      if (this.selectedId > 0) {
        this.myForm.controls["id"].setValue(this.selectedId);
        let cmd = new UpdateKindCommand();
        cmd.id = this.myForm.controls["id"].value;
        cmd.name = this.myForm.controls["name"].value;
        cmd.localizedName = this.myForm.controls["localizedName"].value;
        cmd.modelId = this.myForm.controls["modelId"].value;
        cmd.isActive = this.myForm.controls["isActive"].value;

        this.client.update(this.myForm.controls["id"].value, cmd).subscribe(
          (result) => {
            if (result == null) {
              this.inProgress = false;
              this.service.getAllKind();
              Swal.fire({
                title: "ذخیره نوع با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره نوع با خطا مواجه شد",
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
            if (result > 0) {
              this.inProgress = false;
              this.service.getAllKind();
              Swal.fire({
                title: "ذخیره نوع با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });

              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره نوع با خطا مواجه شد",
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

  edit(item: KindModel) {
    debugger;
    this.selectedId = item.id;

    this.myForm.controls["id"].setValue(this.selectedId ?? null);
    this.myForm.controls["name"].setValue(item.name ?? null);
    this.myForm.controls["localizedName"].setValue(item.localizedName ?? null);
    this.myForm.controls["modelId"].setValue(item.modelId ?? null);
    this.myForm.controls["isActive"].setValue(item.isActive ?? null);

    this.modelId = item.modelId ?? null;

    this.myForm.controls["name"].disable();
    this.myForm.controls["localizedName"].disable();
    this.myForm.controls["modelId"].disable();
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
    this.client.deleteRangeKinds([...this.checkedItems]).subscribe(
      (result) => {
        this.inProgress = false;
        if (result == null) {
          Swal.fire({
            title: "حذف نوع با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.service.getAllKind();
        } else {
          Swal.fire({
            title: "حذف نوع با خطا مواجه شد",
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

  public getAllModels() {
    this.modelsClient.getAllModels().subscribe(
      (result) => {
        this.modelsList = result;
      },
      (error) => console.error(error)
    );
  }

  public handleCloseFormModal() {
    this.myForm.reset();
    this.myForm.markAsUntouched();
    this.myForm.setErrors(null);
    this.myForm.markAsPristine();
    this.myForm.controls["name"].enable();
    this.myForm.controls["localizedName"].enable();
    this.myForm.controls["modelId"].enable();
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

