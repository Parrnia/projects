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
  AboutUsClient,
  CountriesClient,
  CountryDto,
  CreateAboutUsCommand,
  FileParameter,
  FileUploadMetadataDto,
  UpdateAboutUsCommand,
} from "src/app/web-api-client";
import { DecimalPipe } from "@angular/common";
import Swal from "sweetalert2";
import { AboutUsGridService } from "./about-us-grid.service";
import { AboutUsModel } from "./about-us.model";
import {
  NgbdAboutUsSortableHeader,
  SortEvent,
} from "./about-us-sortable.directive";
import { ImageService } from "src/app/core/services/image.service";
import { NonNullAssert } from "@angular/compiler";
import { AboutUsValidators } from "./about-us-validators";

@Component({
  selector: "app-aboutUs",
  templateUrl: "./about-us.component.html",
  styleUrls: ["./about-us.component.scss"],
  providers: [AboutUsGridService, DecimalPipe, NgbAlertConfig],
})
export class AboutUsComponent {
  selectedFileUrl: string | undefined = undefined;
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: AboutUsModel;
  gridjsList$!: Observable<AboutUsModel[]>;
  total$: Observable<number>;
  inProgress = false;
  @ViewChildren(NgbdAboutUsSortableHeader)
  aboutUss!: QueryList<NgbdAboutUsSortableHeader>;
  @ViewChild("confirmationModal") confirmationModal: any;
  @ViewChild("formModal") formModal: any = [];
  @ViewChild("fileInput") fileInput!: ElementRef<HTMLInputElement>;
  aboutUsTypeId?: any;
  countryId?: any;
  formData = new FormData();
  isShownonDefaultOption: boolean = true;
  isShowDefaultOption: boolean = true;
  checkedItems: Set<number> = new Set<number>();
  constructor(
    alertConfig: NgbAlertConfig,
    public client: AboutUsClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    public service: AboutUsGridService,
    private imageService: ImageService
  ) {
    this.gridjsList$ = service.aboutUss$;
    this.total$ = service.total$;

    this.myForm = this.fb.group({
      id: 0,
      title: ["", [Validators.required]],
      textContent: ["", [Validators.required]],
      imageContent: ["", [Validators.required]],
      isDefault: false,
    });

    this.form.title.addAsyncValidators(
      AboutUsValidators.validAboutUsTitle(this.client, this.form.title.value)
    );

    this.form.id.valueChanges.subscribe((id) => {
      this.form.title.setAsyncValidators(
        AboutUsValidators.validAboutUsTitle(this.client, id != null ? id : 0)
      );
    });

    alertConfig.type = "success";
  }

  ngOnInit(): void {
    this.breadCrumbItems = [
      { label: "خوشه اطلاعات" },
      { label: "درباره ما ", active: true },
    ];
  }

  onFileSelected(event: FileUploadMetadataDto | null): void {
    if (event) {
      this.myForm.controls["imageContent"].setValue(event.fileId);
    } else {
      this.myForm.controls["imageContent"].setValue(null);
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

  /**
   * Sort table data
   * @param param0 sort the column
   *
   */
  onSort({ column, direction }: SortEvent) {
    // resetting other aboutUss
    this.aboutUss.forEach((aboutUs) => {
      if (aboutUs.sortable !== column) {
        aboutUs.direction = "";
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
    this.myForm.value.countryId = parseInt(this.countryId);
    this.myForm.value.aboutUsType = parseInt(this.aboutUsTypeId);
    if (this.myForm.valid) {
      if (this.selectedId > 0) {
        this.myForm.controls["id"].setValue(this.selectedId);
        let cmd = new UpdateAboutUsCommand();
        cmd.id = this.myForm.controls["id"].value;
        cmd.title = this.myForm.controls["title"].value;
        cmd.textContent = this.myForm.controls["textContent"].value;
        cmd.imageContent = this.myForm.controls["imageContent"].value;
        cmd.isDefault = this.myForm.controls["isDefault"].value;

        this.client.update(this.myForm.controls["id"].value, cmd).subscribe(
          (result) => {
            this.inProgress = false;
            if (result == null) {
              this.service.getAllAboutUs();
              Swal.fire({
                title: "ذخیره درباره ما با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره درباره ما با خطا مواجه شد",
                icon: "error",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.modalService.dismissAll();
            }
          },
          (error) => {
            this.inProgress = false;
            console.error(error)
          }
        );
      } else {
        let cmd = new CreateAboutUsCommand();
        cmd.title = this.myForm.controls["title"].value;
        cmd.textContent = this.myForm.controls["textContent"].value;
        cmd.imageContent = this.myForm.controls["imageContent"].value;
        cmd.isDefault = this.myForm.controls["isDefault"].value;

        this.client.create(cmd).subscribe(
          (result) => {
            this.inProgress = false;
            if (result > 0) {
              this.service.getAllAboutUs();
              Swal.fire({
                title: "ذخیره درباره ما با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });

              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره درباره ما با خطا مواجه شد",
                icon: "error",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.modalService.dismissAll();
            }
          },
          (error) => {
            this.inProgress = false;
            console.error(error)
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

  edit(item: AboutUsModel) {
    debugger;
    this.selectedId = item.id;
    this.myForm.controls["id"].setValue(this.selectedId ?? null);
    this.myForm.controls["title"].setValue(item.title ?? null);
    this.myForm.controls["textContent"].setValue(item.textContent ?? null);
    this.myForm.controls["imageContent"].setValue(item.imageContent ?? null);
    this.myForm.controls["isDefault"].setValue(item.isDefault ?? null);
    this.selectedFileUrl = item.imageContentSrc;
    if (this.fileInput) {
      this.fileInput.nativeElement.click();
    }
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
    this.client.deleteRangeAboutUs([...this.checkedItems]).subscribe(
      (result) => {
        this.inProgress = false;
        if (result == null) {
          Swal.fire({
            title: "حذف درباره ما با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.service.getAllAboutUs();
        } else {
          Swal.fire({
            title: "حذف درباره ما با خطا مواجه شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });

          this.modalService.dismissAll();
        }
      },
      (error) => {
        this.inProgress = false;
        console.error(error)
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
    this.selectedFileUrl = undefined;
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
