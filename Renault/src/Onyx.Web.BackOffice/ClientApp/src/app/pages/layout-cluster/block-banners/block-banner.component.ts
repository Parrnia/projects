import {
  Component,
  ElementRef,
  QueryList,
  Renderer2,
  ViewChild,
  ViewChildren,
} from "@angular/core";

import { Observable, of } from "rxjs";
import {
  AsyncValidatorFn,
  FormBuilder,
  FormGroup,
  UntypedFormBuilder,
  Validators,
} from "@angular/forms";

import { NgbAlertConfig, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import {
  BlockBannerPosition,
  BlockBannersClient,
  CountriesClient,
  CountryDto,
  CreateBlockBannerCommand,
  FileParameter,
  FileUploadMetadataDto,
  UpdateBlockBannerCommand,
} from "src/app/web-api-client";
import { DecimalPipe } from "@angular/common";
import Swal from "sweetalert2";
import { BlockBannerGridService } from "./block-banner-grid.service";
import { BlockBannerModel } from "./block-banner.model";
import {
  NgbdBlockBannerSortableHeader,
  SortEvent,
} from "./block-banner-sortable.directive";
import { ImageService } from "src/app/core/services/image.service";
import { NonNullAssert } from "@angular/compiler";
import { BlockBannerValidators } from "./block-banner-validators";
import { number } from "echarts";
import { ExportFileService } from "src/app/shared/services/export-file/export-file.service";
export type BlockBannerPositionKey = keyof typeof BlockBannerPosition;

@Component({
  selector: "app-blockBanner",
  templateUrl: "./block-banner.component.html",
  styleUrls: ["./block-banner.component.scss"],
  providers: [BlockBannerGridService, DecimalPipe, NgbAlertConfig],
})
export class BlockBannerComponent {
  selectedFileUrl: string | undefined = undefined;
  inProgress = false;
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: BlockBannerModel;
  gridjsList$!: Observable<BlockBannerModel[]>;
  total$: Observable<number>;
  @ViewChildren(NgbdBlockBannerSortableHeader)
  blockBanners!: QueryList<NgbdBlockBannerSortableHeader>;
  inProgressAllExportbtn = false;
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("confirmationModal") confirmationModal: any;
  @ViewChild("formModal") formModal: any = [];
  @ViewChild("fileInput") fileInput!: ElementRef<HTMLInputElement>;
  blockBannerPositions = BlockBannerPosition;
  blockBannerPosition?: any;
  formData = new FormData();
  isShownonDefaultOption: boolean = true;
  isShowDefaultOption: boolean = true;
  checkedItems: Set<number> = new Set<number>();
  constructor(
    alertConfig: NgbAlertConfig,
    public client: BlockBannersClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    private exportService: ExportFileService,
    public service: BlockBannerGridService,
    private imageService: ImageService
  ) {
    this.gridjsList$ = service.blockBanners$;
    this.total$ = service.total$;
    this.myForm = this.fb.group({
      id: 0,
      title: ["", [Validators.required]],
      image: ["", [Validators.required]],
      subtitle: ["", [Validators.required]],
      buttonText: ["", [Validators.required]],
      blockBannerPosition: [0, [Validators.required]],
      isActive: false,
    });

    this.form.blockBannerPosition.addAsyncValidators(
      BlockBannerValidators.validBlockBanner(this.client, 0)
    );
    alertConfig.type = "success";

    this.form.id.valueChanges.subscribe((id) => {
      this.form.blockBannerPosition.setAsyncValidators(
        BlockBannerValidators.validBlockBanner(this.client, id != null ? id : 0)
      );
    });
  }

  ngOnInit(): void {
    this.breadCrumbItems = [
      { label: "خوشه شخصی سازی قالب" },
      { label: "بلاک بنر ", active: true },
    ];
  }

  getNumericKeys(): BlockBannerPositionKey[] {
    return Object.entries(this.blockBannerPositions)
      .filter(([key, value]) => typeof value === "number")
      .map(([key, value]) => key as BlockBannerPositionKey);
  }

  mapNumericKeyToString(key: BlockBannerPositionKey): string {
    debugger;
    let res = this.mapNemberKeyToString(
      this.blockBannerPositions[key].toString()
    );
    return res;
  }
  mapNemberKeyToString(key: string): string {
    switch (parseInt(key)) {
      case BlockBannerPosition.LeftSide:
        return "سمت چپ";
      case BlockBannerPosition.RightSide:
        return "سمت راست";
      default:
        return "";
    }
  }

  onFileSelected(event: FileUploadMetadataDto | null): void {
    if (event) {
      this.myForm.controls["image"].setValue(event.fileId);
    } else {
      this.myForm.controls["image"].setValue(null);
    }
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
    // resetting other blockBanners
    this.blockBanners.forEach((blockBanner) => {
      if (blockBanner.sortable !== column) {
        blockBanner.direction = "";
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
    debugger;
    this.markAllControlsAsTouched(this.myForm);
    this.submit = true;
    this.myForm.value.blockBannerPosition = parseInt(this.blockBannerPosition);
    if (this.myForm.valid) {
      debugger;

      if (this.selectedId > 0) {
        this.myForm.controls["id"].setValue(this.selectedId);
        let cmd = new UpdateBlockBannerCommand();
        cmd.id = this.myForm.controls["id"].value;
        cmd.title = this.myForm.controls["title"].value;
        cmd.subtitle = this.myForm.controls["subtitle"].value;
        cmd.buttonText = this.myForm.controls["buttonText"].value;
        cmd.image = this.myForm.controls["image"].value;
        cmd.blockBannerPosition =
          this.myForm.controls["blockBannerPosition"].value;
        cmd.isActive = this.myForm.controls["isActive"].value;
        this.client.update(this.myForm.controls["id"].value, cmd).subscribe(
          (result) => {
            this.inProgress = false;
            if (result == null) {
              this.service.getAllBlockBanner();
              Swal.fire({
                title: "ذخیره بلوک بنر با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره بلوک بنر با خطا مواجه شد",
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
        let cmd = new CreateBlockBannerCommand();
        cmd.title = this.myForm.controls["title"].value;
        cmd.subtitle = this.myForm.controls["subtitle"].value;
        cmd.buttonText = this.myForm.controls["buttonText"].value;
        cmd.blockBannerPosition =
          this.myForm.controls["blockBannerPosition"].value;
        cmd.isActive = this.myForm.controls["isActive"].value;
        cmd.image = this.myForm.controls["image"].value;
        debugger;
        this.client.create(cmd).subscribe(
          (result) => {
            this.inProgress = false;
            if (result > 0) {
              this.service.getAllBlockBanner();
              Swal.fire({
                title: "ذخیره بلوک بنر با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });

              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره بلوک بنر با خطا مواجه شد",
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

  edit(item: BlockBannerModel) {
    debugger;
    this.selectedId = item.id;
    this.myForm.controls["id"].setValue(this.selectedId ?? null);
    this.myForm.controls["title"].setValue(item.title ?? null);
    this.myForm.controls["subtitle"].setValue(item.subtitle ?? null);
    this.myForm.controls["buttonText"].setValue(item.buttonText ?? null);
    this.myForm.controls["image"].setValue(item.image ?? null);
    this.myForm.controls["blockBannerPosition"].setValue(
      item.blockBannerPosition ?? null
    );
    this.blockBannerPosition = item.blockBannerPosition;
    this.myForm.controls["isActive"].setValue(item.isActive ?? null);
    this.selectedFileUrl = item.imageSrc;
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
    this.client.deleteRangeBlockBanner([...this.checkedItems]).subscribe(
      (result) => {
        this.inProgress = false;
        if (result == null) {
          Swal.fire({
            title: "حذف بلوک بنر با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.service.getAllBlockBanner();
        } else {
          Swal.fire({
            title: "حذف بلوک بنر با خطا مواجه شد",
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
    this.myForm.markAsPristine();
    Object.keys(this.myForm.controls).forEach((key) => {
      const control = this.myForm.get(key);
      debugger;
      control?.setErrors(null);
      control?.updateValueAndValidity();
    });
    this.selectedId = 0;
    this.selectedFileUrl = undefined;
    debugger;
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
