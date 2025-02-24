import { Component, ElementRef, QueryList, Renderer2, ViewChild, ViewChildren } from '@angular/core';

import { Observable, of } from 'rxjs';
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from '@angular/forms';

import { NgbAlertConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FooterLinkContainersClient, CountriesClient, CountryDto, FileParameter, FamiliesClient, ModelDto, ModelsClient, UpdateFooterLinkContainerCommand } from 'src/app/web-api-client';
import { DecimalPipe } from '@angular/common';
import Swal from 'sweetalert2';
import { FooterLinkContainerGridService } from './footer-link-container-grid.service';
import { FooterLinkContainerModel } from './footer-link-container.model';
import { NgbdFooterLinkContainerSortableHeader, SortEvent } from './footer-link-container-sortable.directive';
import { ImageService } from 'src/app/core/services/image.service';
import { NonNullAssert } from '@angular/compiler';
import { ExportFileService } from 'src/app/shared/services/export-file/export-file.service';

@Component({
  selector: "app-footer-link-container",
  templateUrl: "./footer-link-container.component.html",
  styleUrls: ["./footer-link-container.component.scss"],
  providers: [FooterLinkContainerGridService, DecimalPipe, NgbAlertConfig],
})
export class FooterLinkContainerComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  selectedFooterLinkContainer?: any;
  submit!: boolean;
  selectedItem?: FooterLinkContainerModel;
  gridjsList$!: Observable<FooterLinkContainerModel[]>;
  total$: Observable<number>;
  modelsList?: ModelDto[];
  inProgress = false;
  inProgressAllExportbtn = false;
  @ViewChildren(NgbdFooterLinkContainerSortableHeader)
  footerLinkContainers!: QueryList<NgbdFooterLinkContainerSortableHeader>;
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("confirmationModal") confirmationModal: any;
  @ViewChild("formModal") formModal: any = [];
  @ViewChild("fileInput") fileInput!: ElementRef<HTMLInputElement>;
  @ViewChild("footerLinkContainerModal") footerLinkContainerModal: any = [];
  modelId?: any;
  formData = new FormData();
  checkedItems: Set<number> = new Set<number>();
  constructor(
    alertConfig: NgbAlertConfig,
    public client: FooterLinkContainersClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    private exportService: ExportFileService,
    public service: FooterLinkContainerGridService,
    public modelsClient: ModelsClient
  ) {
    this.gridjsList$ = service.footerLinkContainers$;
    this.total$ = service.total$;

    this.myForm = this.fb.group({
      id: 0,
      header: ["", [Validators.required]],
      order: [0, [Validators.required]],
      isActive: false,
    });
  }

  ngOnInit(): void {
    this.getAllModels();

    this.breadCrumbItems = [
      { label: "خوشه شخصی سازی قالب" },
      { label: "دسته بندی لینک های فوتر", active: true },
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
    // resetting other footerLinkContainers
    this.footerLinkContainers.forEach((footerLinkContainer) => {
      if (footerLinkContainer.sortable !== column) {
        footerLinkContainer.direction = "";
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
    console.log("selecte footerLinkContainer type", this.modelId);
    this.submit = true;
    this.myForm.value.modelId = parseInt(this.modelId);
    if (this.myForm.valid) {
      console.log("form value", this.myForm.value);
      if (this.selectedId > 0) {
        this.myForm.controls["id"].setValue(this.selectedId);
        let cmd = new UpdateFooterLinkContainerCommand();
        cmd.id = this.myForm.controls["id"].value;
        cmd.header = this.myForm.controls["header"].value;
        cmd.order = this.myForm.controls["order"].value;
        cmd.isActive = this.myForm.controls["isActive"].value;

        this.client.update(this.myForm.controls["id"].value, cmd).subscribe(
          (result) => {
            this.inProgress = false;
            if (result == null) {
              this.service.getAllFooterLinkContainer();
              Swal.fire({
                title: "ذخیره دسته بندی لینک های فوتر با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره دسته بندی لینک های فوتر با خطا مواجه شد",
                icon: "error",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.modalService.dismissAll();
            }
          },
          (error) => {
            console.error(error);
            this.inProgress = false;
          }
        );
      } else {
        this.client.create(this.myForm.value).subscribe(
          (result) => {
            this.inProgress = false;
            if (result > 0) {
              this.service.getAllFooterLinkContainer();
              Swal.fire({
                title: "ذخیره دسته بندی لینک های فوتر با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });

              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره دسته بندی لینک های فوتر با خطا مواجه شد",
                icon: "error",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.modalService.dismissAll();
            }
          },
          (error) => {
            console.error(error);
            this.inProgress = false;
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

  edit(item: FooterLinkContainerModel) {
    debugger;
    this.selectedId = item.id;

    this.myForm.controls["id"].setValue(this.selectedId ?? null);
    this.myForm.controls["header"].setValue(item.header ?? null);
    this.myForm.controls["order"].setValue(item.order ?? null);
    this.myForm.controls["isActive"].setValue(item.isActive ?? null);

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

  openMoreDetail(item: any) {
    this.selectedFooterLinkContainer = item;
    this.modalService.open(this.footerLinkContainerModal, {
      size: "lg",
      backdrop: "static",
    });
  }

  deleteItems() {
    this.inProgress = true;
    this.client
      .deleteRangeFooterLinkContainer([...this.checkedItems])
      .subscribe(
        (result) => {
          this.inProgress = false;
          if (result == null) {
            Swal.fire({
              title: "حذف دسته بندی لینک های فوتر با موفقیت انجام شد",
              icon: "success",
              iconHtml: "!",
              confirmButtonText: "تایید",
            });
            this.modalService.dismissAll();
            this.service.getAllFooterLinkContainer();
          } else {
            Swal.fire({
              title: "حذف دسته بندی لینک های فوتر با خطا مواجه شد",
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

