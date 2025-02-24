import { Component, ElementRef, Input, QueryList, Renderer2, ViewChild, ViewChildren } from '@angular/core';

import { Observable, of } from 'rxjs';
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from '@angular/forms';

import { NgbAlertConfig, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { FooterLinksClient, CountriesClient, CountryDto, FileParameter, FamiliesClient, ModelDto, ModelsClient, UpdateFooterLinkCommand } from 'src/app/web-api-client';
import { DecimalPipe } from '@angular/common';
import Swal from 'sweetalert2';
import { FooterLinkGridService } from './footer-link-grid.service';
import { FooterLinkModel } from './footer-link.model';
import { ImageService } from 'src/app/core/services/image.service';
import { NonNullAssert } from '@angular/compiler';
import { ExportFileService } from 'src/app/shared/services/export-file/export-file.service';

@Component({
  selector: "app-footer-link",
  templateUrl: "./footer-link.component.html",
  styleUrls: ["./footer-link.component.scss"],
  providers: [FooterLinkGridService, DecimalPipe, NgbAlertConfig],
})
export class FooterLinkComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: FooterLinkModel;
  inProgress = false;
  inProgressAllExportbtn = false;
  gridjsList$!: Observable<FooterLinkModel[]>;
  total$: Observable<number>;
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("confirmationModal") confirmationModal: any;
  confirmationModalRef: NgbModalRef | undefined;
  @ViewChild("formModal") formModal: any = [];
  formModalRef: NgbModalRef | undefined;
  @ViewChild("fileInput") fileInput!: ElementRef<HTMLInputElement>;
  formData = new FormData();
  @Input() footerLinkContainer?: any;
  checkedItems: Set<number> = new Set<number>();
  constructor(
    alertConfig: NgbAlertConfig,
    public client: FooterLinksClient,
    private fb: FormBuilder,
    private exportService: ExportFileService,
    private modalService: NgbModal,
    public service: FooterLinkGridService,
    public modelsClient: ModelsClient
  ) {
    this.gridjsList$ = service.footerLinks$;
    this.total$ = service.total$;

    this.myForm = this.fb.group({
      id: 0,
      title: ["", [Validators.required]],
      url: ["", [Validators.required]],
      footerLinkContainerId: [0, [Validators.required]],
      isActive: false,
    });
  }

  ngOnInit(): void {
    this.service.getFooterLinksByFooterLinkContainerId(
      this.footerLinkContainer.id
    );
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
      this.confirmationModalRef = this.modalService.open(content, {
        centered: true,
      });
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

  get form() {
    return this.myForm.controls;
  }

  onSubmit(): void {
    this.inProgress = true;
    this.markAllControlsAsTouched(this.myForm);
    this.myForm.controls["footerLinkContainerId"].setValue(
      this.footerLinkContainer.id
    );
    debugger;
    this.submit = true;
    if (this.myForm.valid) {
      if (this.selectedId > 0) {
        this.myForm.controls["id"].setValue(this.selectedId);
        let cmd = new UpdateFooterLinkCommand();
        cmd.id = this.myForm.controls["id"].value;
        cmd.title = this.myForm.controls["title"].value;
        cmd.url = this.myForm.controls["url"].value;
        cmd.footerLinkContainerId =
          this.myForm.controls["footerLinkContainerId"].value;
        cmd.isActive = this.myForm.controls["isActive"].value;
        this.client.update(this.myForm.controls["id"].value, cmd).subscribe(
          (result) => {
            this.inProgress = false;
            if (result == null) {
              this.service.getFooterLinksByFooterLinkContainerId(
                this.footerLinkContainer.id
              );
              Swal.fire({
                title: "ذخیره لینک فوتر با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره لینک فوتر با خطا مواجه شد",
                icon: "error",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
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
              this.service.getFooterLinksByFooterLinkContainerId(
                this.footerLinkContainer.id
              );
              Swal.fire({
                title: "ذخیره لینک فوتر با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });

              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره لینک فوتر با خطا مواجه شد",
                icon: "error",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
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

  edit(item: FooterLinkModel) {
    debugger;
    this.selectedId = item.id;

    this.myForm.controls["id"].setValue(this.selectedId ?? null);
    this.myForm.controls["title"].setValue(item.title ?? null);
    this.myForm.controls["url"].setValue(item.url ?? null);
    this.myForm.controls["footerLinkContainerId"].setValue(
      this.footerLinkContainer.id ?? null
    );
    this.myForm.controls["isActive"].setValue(item.isActive ?? null);

    this.formModalRef = this.modalService.open(this.formModal, {
      size: "lg",
      backdrop: "static",
    });
  }

  openInsertModal() {
    this.formModalRef = this.modalService.open(this.formModal, {
      size: "lg",
      backdrop: "static",
    });
  }

  openDeleteConfirmationModal(id: any) {
    this.selectedId = id;
    console.log("    this.selectedId = id;", this.selectedId);
    this.modalService.open(this.confirmationModal);
  }

  deleteItems() {
    this.inProgress = true;
    this.client.deleteRangeFooterLink([...this.checkedItems]).subscribe(
      (result) => {
        this.inProgress = false;
        if (result == null) {
          Swal.fire({
            title: "حذف لینک فوتر با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.confirmationModalRef?.close();
          this.service.getFooterLinksByFooterLinkContainerId(
            this.footerLinkContainer.id
          );
        } else {
          Swal.fire({
            title: "حذف لینک فوتر با خطا مواجه شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.confirmationModalRef?.close();
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
    this.formModalRef?.close();
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

