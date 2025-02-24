import { Component, ElementRef, QueryList, Renderer2, ViewChild, ViewChildren } from '@angular/core';

import { Observable, of } from 'rxjs';
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from '@angular/forms';

import { NgbAlertConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ModelsClient, CountriesClient, CountryDto, FileParameter, FamiliesClient, UpdateModelCommand, VehicleBrandDto } from 'src/app/web-api-client';
import { DecimalPipe } from '@angular/common';
import Swal from 'sweetalert2';
import { ModelGridService } from './model-grid.service';
import { ModelModel } from './model.model';
import { NgbdModelSortableHeader, SortEvent } from './model-sortable.directive';
import { ImageService } from 'src/app/core/services/image.service';
import { NonNullAssert } from '@angular/compiler';
import { ModelValidators } from './model-validators';
import { ExportFileService } from 'src/app/shared/services/export-file/export-file.service';

@Component({
  selector: "app-model",
  templateUrl: "./model.component.html",
  styleUrls: ["./model.component.scss"],
  providers: [ModelGridService, DecimalPipe, NgbAlertConfig],
})
export class ModelComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: ModelModel;
  gridjsList$!: Observable<ModelModel[]>;
  total$: Observable<number>;
  brandsList?: VehicleBrandDto[];
  inProgress = false;
  inProgressAllExportbtn = false;
  @ViewChildren(NgbdModelSortableHeader)
  models!: QueryList<NgbdModelSortableHeader>;
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("confirmationModal") confirmationModal: any;
  @ViewChild("formModal") formModal: any = [];
  @ViewChild("fileInput") fileInput!: ElementRef<HTMLInputElement>;
  familyId?: any;
  formData = new FormData();
  checkedItems: Set<number> = new Set<number>();
  constructor(
    alertConfig: NgbAlertConfig,
    public client: ModelsClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    private exportService: ExportFileService,
    public service: ModelGridService,
    public familiesClient: FamiliesClient
  ) {
    this.gridjsList$ = service.models$;
    this.total$ = service.total$;

    this.myForm = this.fb.group({
      id: 0,
      name: ["", [Validators.required, Validators.maxLength(250)]],
      localizedName: ["", [Validators.required, Validators.maxLength(250)]],
      familyId: [0, [Validators.required]],
      isActive: false,
    });

    this.form.name.addAsyncValidators(
      ModelValidators.validModelName(this.client, 0)
    );
    this.form.localizedName.addAsyncValidators(
      ModelValidators.validModelLocalizedName(this.client, 0)
    );

    alertConfig.type = "success";

    this.form.id.valueChanges.subscribe((id) => {
      this.form.name.setAsyncValidators(
        ModelValidators.validModelName(this.client, id != null ? id : 0)
      );
      this.form.localizedName.setAsyncValidators(
        ModelValidators.validModelLocalizedName(
          this.client,
          id != null ? id : 0
        )
      );
    });
  }

  ngOnInit(): void {
    this.getAllFamilies();

    this.breadCrumbItems = [
      { label: "خوشه برند" },
      { label: "مدل ", active: true },
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
    // resetting other models
    this.models.forEach((model) => {
      if (model.sortable !== column) {
        model.direction = "";
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
    console.log("selecte model type", this.familyId);
    this.submit = true;
    this.myForm.value.familyId = parseInt(this.familyId);
    if (this.myForm.valid) {
      console.log("form value", this.myForm.value);
      if (this.selectedId > 0) {
        this.myForm.controls["id"].setValue(this.selectedId);
        let cmd = new UpdateModelCommand();
        cmd.id = this.myForm.controls["id"].value;
        cmd.name = this.myForm.controls["name"].value;
        cmd.localizedName = this.myForm.controls["localizedName"].value;
        cmd.familyId = this.myForm.controls["familyId"].value;
        cmd.isActive = this.myForm.controls["isActive"].value;

        this.client.update(this.myForm.controls["id"].value, cmd).subscribe(
          (result) => {
            this.inProgress = false;
            if (result == null) {
              this.service.getAllModel();
              Swal.fire({
                title: "ذخیره مدل با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره مدل با خطا مواجه شد",
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
              this.service.getAllModel();
              Swal.fire({
                title: "ذخیره مدل با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });

              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره مدل با خطا مواجه شد",
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

  edit(item: ModelModel) {
    debugger;
    this.selectedId = item.id;

    this.myForm.controls["id"].setValue(this.selectedId ?? null);
    this.myForm.controls["name"].setValue(item.name ?? null);
    this.myForm.controls["localizedName"].setValue(item.localizedName ?? null);
    this.myForm.controls["familyId"].setValue(item.familyId ?? null);
    this.myForm.controls["isActive"].setValue(item.isActive ?? null);

    this.familyId = item.familyId ?? null;

    this.myForm.controls["name"].disable();
    this.myForm.controls["localizedName"].disable();
    this.myForm.controls["familyId"].disable();
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
    this.client.deleteRangeModel([...this.checkedItems]).subscribe(
      (result) => {
        this.inProgress = false;
        if (result == null) {
          Swal.fire({
            title: "حذف مدل با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.service.getAllModel();
        } else {
          Swal.fire({
            title: "حذف مدل با خطا مواجه شد",
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

  public getAllFamilies() {
    this.familiesClient.getAllFamilies().subscribe(
      (result) => {
        this.brandsList = result;
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
    this.myForm.controls["familyId"].enable();
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

