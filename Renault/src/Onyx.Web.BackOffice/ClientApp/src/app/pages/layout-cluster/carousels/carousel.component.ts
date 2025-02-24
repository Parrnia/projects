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
  FormBuilder,
  FormGroup,
  UntypedFormBuilder,
  Validators,
} from "@angular/forms";

import { NgbAlertConfig, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import {
  CarouselsClient,
  CountriesClient,
  CountryDto,
  CreateCarouselCommand,
  FileParameter,
  FileUploadMetadataDto,
  UpdateCarouselCommand,
} from "src/app/web-api-client";
import { DecimalPipe } from "@angular/common";
import Swal from "sweetalert2";
import { CarouselGridService } from "./carousel-grid.service";
import { CarouselModel } from "./carousel.model";
import {
  NgbdCarouselSortableHeader,
  SortEvent,
} from "./carousel-sortable.directive";
import { ImageService } from "src/app/core/services/image.service";
import { NonNullAssert } from "@angular/compiler";
import { CarouselValidators } from "./carousel-validators";
import { ExportFileService } from "src/app/shared/services/export-file/export-file.service";

@Component({
  selector: "app-carousel",
  templateUrl: "./carousel.component.html",
  styleUrls: ["./carousel.component.scss"],
  providers: [CarouselGridService, DecimalPipe, NgbAlertConfig],
})
export class CarouselComponent {
  selectedDesktopFileUrl: string | undefined = undefined;
  selectedMobileFileUrl: string | undefined = undefined;
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: CarouselModel;
  gridjsList$!: Observable<CarouselModel[]>;
  total$: Observable<number>;
  inProgress = false;
  inProgressAllExportbtn = false;

  @ViewChildren(NgbdCarouselSortableHeader)
  carousels!: QueryList<NgbdCarouselSortableHeader>;
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("confirmationModal") confirmationModal: any;
  @ViewChild("formModal") formModal: any = [];
  @ViewChild("fileInputMobile") fileInputMobile!: ElementRef<HTMLInputElement>;
  @ViewChild("fileInputDesktop")
  fileInputDesktop!: ElementRef<HTMLInputElement>;
  @ViewChild("carouselTabModel") carouselTabModel: any = [];
  formData = new FormData();
  checkedItems: Set<number> = new Set<number>();
  constructor(
    alertConfig: NgbAlertConfig,
    public client: CarouselsClient,
    private fb: FormBuilder,
    private exportService: ExportFileService,
    private modalService: NgbModal,
    public service: CarouselGridService
  ) {
    this.gridjsList$ = service.carousels$;
    this.total$ = service.total$;

    this.myForm = this.fb.group({
      id: 0,
      url: ["", [Validators.required]],
      desktopImage: ["", [Validators.required]],
      mobileImage: ["", [Validators.required]],
      offer: ["", [Validators.required]],
      title: ["", [Validators.required]],
      details: ["", [Validators.required]],
      buttonLabel: ["", [Validators.required]],
      order: ["", [Validators.required]],
      isActive: false,
    });

    this.form.title.addAsyncValidators(
      CarouselValidators.validCarouselTitle(this.client, this.form.title.value)
    );

    alertConfig.type = "success";

    this.form.id.valueChanges.subscribe((id) => {
      this.form.title.setAsyncValidators(
        CarouselValidators.validCarouselTitle(this.client, id != null ? id : 0)
      );
    });
  }

  ngOnInit(): void {
    this.breadCrumbItems = [
      { label: "خوشه شخصی سازی قالب" },
      { label: "اسلایدر ", active: true },
    ];
  }

  onFileSelectedDesktop(event: FileUploadMetadataDto | null): void {
    if (event) {
      this.myForm.controls["desktopImage"].setValue(event.fileId);
    } else {
      this.myForm.controls["desktopImage"].setValue(null);
    }
  }
  onFileSelectedMobile(event: FileUploadMetadataDto | null): void {
    if (event) {
      this.myForm.controls["mobileImage"].setValue(event.fileId);
    } else {
      this.myForm.controls["mobileImage"].setValue(null);
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
    // resetting other carousels
    this.carousels.forEach((carousel) => {
      if (carousel.sortable !== column) {
        carousel.direction = "";
      }
    });

    this.service.sortColumn = column;
    this.service.sortDirection = direction;
  }
  get form() {
    return this.myForm.controls;
  }

  onSubmit(): void {
    this.markAllControlsAsTouched(this.myForm);
    this.inProgress = true;
    this.submit = true;
    if (this.myForm.valid) {
      console.log("form value", this.myForm.value);
      if (this.selectedId > 0) {
        this.myForm.controls["id"].setValue(this.selectedId);

        let cmd = new UpdateCarouselCommand();
        cmd.id = this.myForm.controls["id"].value;
        cmd.url = this.myForm.controls["url"].value;
        cmd.desktopImage = this.myForm.controls["desktopImage"].value;
        cmd.mobileImage = this.myForm.controls["mobileImage"].value;
        cmd.offer = this.myForm.controls["offer"].value;
        cmd.title = this.myForm.controls["title"].value;
        cmd.details = this.myForm.controls["details"].value;
        cmd.buttonLabel = this.myForm.controls["buttonLabel"].value;
        cmd.order = this.myForm.controls["order"].value;
        cmd.isActive = this.myForm.controls["isActive"].value;

        this.client.update(this.myForm.controls["id"].value, cmd).subscribe(
          (result) => {
            this.inProgress = true;
            if (result == null) {
              this.service.getAllCarousel();
              Swal.fire({
                title: "ذخیره اسلایدر با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره اسلایدر با خطا مواجه شد",
                icon: "error",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.modalService.dismissAll();
            }
          },
          (error) => {
            console.error(error);
            this.inProgress = true;
          }
        );
      } else {
        let cmd = new CreateCarouselCommand();
        cmd.url = this.myForm.controls["url"].value;
        cmd.offer = this.myForm.controls["offer"].value;
        cmd.desktopImage = this.myForm.controls["desktopImage"].value;
        cmd.mobileImage = this.myForm.controls["mobileImage"].value;
        cmd.title = this.myForm.controls["title"].value;
        cmd.details = this.myForm.controls["details"].value;
        cmd.buttonLabel = this.myForm.controls["buttonLabel"].value;
        cmd.order = this.myForm.controls["order"].value;
        cmd.isActive = this.myForm.controls["isActive"].value;

        this.client.create(cmd).subscribe(
          (result) => {
            this.inProgress = false;
            if (result > 0) {
              this.service.getAllCarousel();
              Swal.fire({
                title: "ذخیره اسلایدر با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });

              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره اسلایدر با خطا مواجه شد",
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

  edit(item: CarouselModel) {
    debugger;
    this.selectedId = item.id;

    this.myForm.controls["id"].setValue(this.selectedId ?? null);
    this.myForm.controls["url"].setValue(item.url ?? null);
    this.myForm.controls["desktopImage"].setValue(item.desktopImage ?? null);
    this.myForm.controls["mobileImage"].setValue(item.mobileImage ?? null);
    this.myForm.controls["offer"].setValue(item.offer ?? null);
    this.myForm.controls["title"].setValue(item.title ?? null);
    this.myForm.controls["details"].setValue(item.details ?? null);
    this.myForm.controls["buttonLabel"].setValue(item.buttonLabel ?? null);
    this.myForm.controls["order"].setValue(item.order ?? null);
    this.myForm.controls["isActive"].setValue(item.isActive ?? null);
    this.selectedDesktopFileUrl = item.desktopImageSrc;
    this.selectedMobileFileUrl = item.mobileImageSrc;
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
    this.client.deleteRangeCarousel([...this.checkedItems]).subscribe(
      (result) => {
        this.inProgress = false;
        if (result == null) {
          Swal.fire({
            title: "حذف اسلایدر با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.service.getAllCarousel();
        } else {
          Swal.fire({
            title: "حذف اسلایدر با خطا مواجه شد",
            icon: "success",
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

    this.checkedItems = new Set<number>();
  }

  public handleCloseFormModal() {
    this.selectedDesktopFileUrl = undefined;
    this.selectedMobileFileUrl = undefined;
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
  openMoreDetailUser(item: any) {
    debugger;
    this.client.getCarouselById(item.id).subscribe({
      next: (res) => {
        this.selectedItem = res as any;
        this.modalService.open(this.carouselTabModel, {
          size: "lg",
          backdrop: "static",
        });
      },
    });
  }
}
