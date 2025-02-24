import { icon } from "leaflet";
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
  SocialLinksClient,
  CountriesClient,
  CountryDto,
  FileParameter,
  FileUploadMetadataDto,
  UpdateSocialLinkCommand,
  CreateSocialLinkCommand,
} from "src/app/web-api-client";
import { DecimalPipe } from "@angular/common";
import Swal from "sweetalert2";
import { SocialLinkGridService } from "./social-link-grid.service";
import { SocialLinkModel } from "./social-link.model";
import {
  NgbdSocialLinkSortableHeader,
  SortEvent,
} from "./social-link-sortable.directive";
import { ImageService } from "src/app/core/services/image.service";
import { NonNullAssert } from "@angular/compiler";
import { ExportFileService } from "src/app/shared/services/export-file/export-file.service";

@Component({
  selector: "app-social-link",
  templateUrl: "./social-link.component.html",
  styleUrls: ["./social-link.component.scss"],
  providers: [SocialLinkGridService, DecimalPipe, NgbAlertConfig],
})
export class SocialLinkComponent {
  selectedFileUrl: string | undefined = undefined;
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: SocialLinkModel;
  gridjsList$!: Observable<SocialLinkModel[]>;
  total$: Observable<number>;
  countryList?: CountryDto[];
  inProgress = false;
  inProgressAllExportbtn = false;
  @ViewChildren(NgbdSocialLinkSortableHeader)
  socialLinks!: QueryList<NgbdSocialLinkSortableHeader>;
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("confirmationModal") confirmationModal: any;
  @ViewChild("formModal") formModal: any = [];
  @ViewChild("fileInput") fileInput!: ElementRef<HTMLInputElement>;
  countryId?: any;
  formData = new FormData();
  checkedItems: Set<number> = new Set<number>();
  constructor(
    alertConfig: NgbAlertConfig,
    public client: SocialLinksClient,
    private fb: FormBuilder,
    private exportService: ExportFileService,
    private modalService: NgbModal,
    public service: SocialLinkGridService,
    public countryClient: CountriesClient
  ) {
    this.gridjsList$ = service.socialLinks$;
    this.total$ = service.total$;

    this.myForm = this.fb.group({
      id: 0,
      url: ["", [Validators.required]],
      icon: ["", [Validators.required]],
      isActive: false,
    });

    alertConfig.type = "success";
  }

  ngOnInit(): void {
    this.getAllCountries();

    this.breadCrumbItems = [
      { label: "خوشه شخصی سازی قالب" },
      { label: "لینک شبکه های اجتماعی", active: true },
    ];
  }

  onFileSelected(event: FileUploadMetadataDto | null): void {
    if (event) {
      this.myForm.controls["icon"].setValue(event.fileId);
    } else {
      this.myForm.controls["icon"].setValue(null);
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
    // resetting other socialLinks
    this.socialLinks.forEach((socialLink) => {
      if (socialLink.sortable !== column) {
        socialLink.direction = "";
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
    if (this.myForm.valid) {
      if (this.selectedId > 0) {
        this.myForm.controls["id"].setValue(this.selectedId);
        let cmd = new UpdateSocialLinkCommand();
        cmd.id = this.myForm.controls["id"].value;
        cmd.url = this.myForm.controls["url"].value;
        cmd.icon = this.myForm.controls["icon"].value;
        cmd.isActive = this.myForm.controls["isActive"].value ?? false;

        this.client.update(this.myForm.controls["id"].value, cmd).subscribe(
          (result) => {
            this.inProgress = false;
            if (result == null) {
              this.service.getAllSocialLink();
              Swal.fire({
                title: "ذخیره لینک شبکه های اجتماعی با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره لینک شبکه های اجتماعی با خطا مواجه شد",
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
        let cmd = new CreateSocialLinkCommand();
        cmd.url = this.myForm.controls["url"].value;
        cmd.icon = this.myForm.controls["icon"].value;
        cmd.isActive = this.myForm.controls["isActive"].value ?? false;

        this.client.create(cmd).subscribe(
          (result) => {
            this.inProgress = false;
            if (result > 0) {
              this.service.getAllSocialLink();
              Swal.fire({
                title: "ذخیره لینک شبکه های اجتماعی با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });

              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره لینک شبکه های اجتماعی با خطا مواجه شد",
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

  edit(item: SocialLinkModel) {
    debugger;
    this.selectedId = item.id;

    this.myForm.controls["id"].setValue(this.selectedId ?? null);
    this.myForm.controls["url"].setValue(item.url ?? null);
    this.myForm.controls["icon"].setValue(item.icon ?? null);
    this.myForm.controls["isActive"].setValue(item.isActive ?? null);
    this.selectedFileUrl = item.iconSrc;
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
    this.client.deleteRangeSocialLink([...this.checkedItems]).subscribe(
      (result) => {
        this.inProgress = false;
        if (result == null) {
          Swal.fire({
            title: "حذف لینک شبکه های اجتماعی با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.service.getAllSocialLink();
        } else {
          Swal.fire({
            title: "حذف لینک شبکه های اجتماعی با خطا مواجه شد",
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

  public getAllCountries() {
    this.countryClient.getAllCountries().subscribe(
      (result) => {
        this.countryList = result;
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
