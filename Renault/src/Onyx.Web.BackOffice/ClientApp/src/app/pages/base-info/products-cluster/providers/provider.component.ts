import { Component, ElementRef, QueryList, Renderer2, ViewChild, ViewChildren } from '@angular/core';

import { Observable, of } from 'rxjs';
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from '@angular/forms';

import { NgbAlertConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ProvidersClient, CountriesClient, CountryDto, FileParameter, UpdateProviderCommand } from 'src/app/web-api-client';
import { DecimalPipe } from '@angular/common';
import Swal from 'sweetalert2';
import { ProviderGridService } from './provider-grid.service';
import { ProviderModel } from './provider.model';
import { NgbdProviderSortableHeader, SortEvent } from './provider-sortable.directive';
import { ImageService } from 'src/app/core/services/image.service';
import { NonNullAssert } from '@angular/compiler';
import { ProviderValidators } from './provider-validators';
import { ExportFileService } from 'src/app/shared/services/export-file/export-file.service';

@Component({
  selector: "app-provider",
  templateUrl: "./provider.component.html",
  styleUrls: ["./provider.component.scss"],
  providers: [ProviderGridService, DecimalPipe, NgbAlertConfig],
})
export class ProviderComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: ProviderModel;
  gridjsList$!: Observable<ProviderModel[]>;
  total$: Observable<number>;
  inProgress = false;
  inProgressAllExportbtn = false;
  @ViewChildren(NgbdProviderSortableHeader)
  providers!: QueryList<NgbdProviderSortableHeader>;
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("confirmationModal") confirmationModal: any;
  @ViewChild("formModal") formModal: any = [];
  @ViewChild("fileInput") fileInput!: ElementRef<HTMLInputElement>;
  formData = new FormData();
  checkedItems: Set<number> = new Set<number>();
  constructor(
    alertConfig: NgbAlertConfig,
    public client: ProvidersClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    private exportService: ExportFileService,
    public service: ProviderGridService
  ) {
    this.gridjsList$ = service.providers$;
    this.total$ = service.total$;

    this.myForm = this.fb.group({
      id: 0,
      code: [0, [Validators.required]],
      name: ["", [Validators.required, Validators.maxLength(100)]],
      localizedName: ["", [Validators.required, Validators.maxLength(100)]],
      localizedCode: ["", [Validators.required, Validators.maxLength(20)]],
      description: ["", [Validators.maxLength(500)]],
    });

    this.form.name.addAsyncValidators(
      ProviderValidators.validProviderName(this.client, 0)
    );
    this.form.localizedName.addAsyncValidators(
      ProviderValidators.validProviderLocalizedName(this.client, 0)
    );
    alertConfig.type = "success";

    this.form.id.valueChanges.subscribe((id) => {
      this.form.name.setAsyncValidators(
        ProviderValidators.validProviderName(this.client, id != null ? id : 0)
      );
      this.form.localizedName.setAsyncValidators(
        ProviderValidators.validProviderLocalizedName(
          this.client,
          id != null ? id : 0
        )
      );
    });
  }

  ngOnInit(): void {
    this.breadCrumbItems = [
      { label: "خوشه ویژگی های محصول" },
      { label: "تامین کننده", active: true },
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
    // resetting other providers
    this.providers.forEach((provider) => {
      if (provider.sortable !== column) {
        provider.direction = "";
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
    debugger;
    if (this.myForm.valid) {
      if (this.selectedId > 0) {
        this.myForm.controls["id"].setValue(this.selectedId);
        let cmd = new UpdateProviderCommand();
        cmd.id = this.myForm.controls["id"].value;
        cmd.code = this.myForm.controls["code"].value;
        cmd.name = this.myForm.controls["name"].value;
        cmd.localizedName = this.myForm.controls["localizedName"].value;
        cmd.localizedCode = this.myForm.controls["localizedCode"].value;
        cmd.description = this.myForm.controls["description"].value;

        this.client.update(this.myForm.controls["id"].value, cmd).subscribe(
          (result) => {
            this.inProgress = false;
            if (result == null) {
              this.service.refreshProviders();
              Swal.fire({
                title: "ذخیره تامین کننده با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره تامین کننده با خطا مواجه شد",
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
              this.service.refreshProviders();
              Swal.fire({
                title: "ذخیره تامین کننده با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });

              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره تامین کننده با خطا مواجه شد",
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

  edit(item: ProviderModel) {
    debugger;
    this.selectedId = item.id;

    this.myForm.controls["id"].setValue(this.selectedId ?? null);
    this.myForm.controls["code"].setValue(item.code ?? null);
    this.myForm.controls["description"].setValue(item.description ?? null);
    this.myForm.controls["localizedCode"].setValue(item.localizedCode ?? null);
    this.myForm.controls["localizedName"].setValue(item.localizedName ?? null);
    this.myForm.controls["name"].setValue(item.name ?? null);

    this.myForm.controls["code"].disable();
    this.myForm.controls["description"].disable();
    this.myForm.controls["localizedCode"].disable();
    this.myForm.controls["localizedName"].disable();
    this.myForm.controls["name"].disable();
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
    this.client.deleteRangeProvider([...this.checkedItems]).subscribe(
      (result) => {
        this.inProgress = false;
        if (result == null) {
          Swal.fire({
            title: "حذف تامین کننده با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.service.refreshProviders();
        } else {
          Swal.fire({
            title: "حذف تامین کننده با خطا مواجه شد",
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

    this.myForm.controls["code"].enable();
    this.myForm.controls["description"].enable();
    this.myForm.controls["localizedCode"].enable();
    this.myForm.controls["localizedName"].enable();
    this.myForm.controls["name"].enable();
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

