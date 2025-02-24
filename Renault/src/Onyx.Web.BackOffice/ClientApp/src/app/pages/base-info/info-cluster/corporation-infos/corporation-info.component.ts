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
  CorporationInfosClient,
  CountriesClient,
  CountryDto,
  CreateCorporationInfoCommand,
  FileParameter,
  FileUploadMetadataDto,
  UpdateCorporationInfoCommand,
} from "src/app/web-api-client";
import { DecimalPipe } from "@angular/common";
import Swal from "sweetalert2";
import { CorporationInfoGridService } from "./corporation-info-grid.service";
import { CorporationInfoModel } from "./corporation-info.model";
import {
  NgbdCorporationInfoSortableHeader,
  SortEvent,
} from "./corporation-info-sortable.directive";
import { ImageService } from "src/app/core/services/image.service";
import { NonNullAssert } from "@angular/compiler";

@Component({
  selector: "app-corporation-info",
  templateUrl: "./corporation-info.component.html",
  styleUrls: ["./corporation-info.component.scss"],
  providers: [CorporationInfoGridService, DecimalPipe, NgbAlertConfig],
})
export class CorporationInfoComponent {
  selectedFileUrlDesktop: string | undefined = undefined;
  selectedFileUrlMobile: string | undefined = undefined;
  selectedFileUrlFooter: string | undefined = undefined;
  selectedFileUrlSliderBackGroundImage: string | undefined = undefined;
 
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: CorporationInfoModel;
  gridjsList$!: Observable<CorporationInfoModel[]>;
  total$: Observable<number>;
  inProgress = false;
  @ViewChildren(NgbdCorporationInfoSortableHeader)
  corporationInfos!: QueryList<NgbdCorporationInfoSortableHeader>;
  @ViewChild("confirmationModal") confirmationModal: any;
  @ViewChild("formModal") formModal: any = [];
  @ViewChild("fileInputFooter") fileInputFooter!: ElementRef<HTMLInputElement>;
  @ViewChild("fileInputMobile") fileInputMobile!: ElementRef<HTMLInputElement>;
  @ViewChild("fileInputDesktop")
  fileInputDesktop!: ElementRef<HTMLInputElement>;
  @ViewChild("corporationInfoTabModel") corporationInfoTabModel: any = [];
  formData = new FormData();
  checkedItems: Set<number> = new Set<number>();
  constructor(
    alertConfig: NgbAlertConfig,
    public client: CorporationInfosClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    public service: CorporationInfoGridService,
    private imageService: ImageService
  ) {
    this.gridjsList$ = service.corporationInfos$;
    this.total$ = service.total$;

    this.myForm = this.fb.group({
      id: 0,
      contactUsMessage: ["", [Validators.required]],
      desktopLogo: ["", [Validators.required]],
      mobileLogo: ["", [Validators.required]],
      footerLogo: ["", [Validators.required]],
      sliderBackGroundImage: ["", [Validators.required]],
      poweredBy: ["", [Validators.required]],
      callUs: ["", [Validators.required]],
      isDefault: false,
    });

    alertConfig.type = "success";
  }

  ngOnInit(): void {
    this.breadCrumbItems = [
      { label: "خوشه اطلاعات" },
      { label: "اطلاعات شرکت ", active: true },
    ];
  }

  onFileSelectedFooter(event: FileUploadMetadataDto | null): void {
    if (event) {
      this.myForm.controls["footerLogo"].setValue(event.fileId);
    } else {
      this.myForm.controls["footerLogo"].setValue(null);
    }
  }


  onFileSelectedMobile(event: FileUploadMetadataDto | null): void {
    if (event) {
      this.myForm.controls["mobileLogo"].setValue(event.fileId);
    } else {
      this.myForm.controls["mobileLogo"].setValue(null);
    }
  }


  onFileSelectedSliderBackGroundImage(
    event: FileUploadMetadataDto | null
  ): void {
    if (event) {
      this.myForm.controls["sliderBackGroundImage"].setValue(event.fileId);
    } else {
      this.myForm.controls["sliderBackGroundImage"].setValue(null);
    }
  }

  onFileSelectedDesktop(event: FileUploadMetadataDto | null): void {
    if (event) {
      this.myForm.controls["desktopLogo"].setValue(event.fileId);
    } else {
      this.myForm.controls["desktopLogo"].setValue(null);
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
    // resetting other corporationInfos
    this.corporationInfos.forEach((corporationInfo) => {
      if (corporationInfo.sortable !== column) {
        corporationInfo.direction = "";
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
    if (this.myForm.valid) {
      if (this.selectedId > 0) {
        this.myForm.controls["id"].setValue(this.selectedId);
        let cmd = new UpdateCorporationInfoCommand();
        cmd.id = this.myForm.controls["id"].value;
        cmd.contactUsMessage = this.myForm.controls["contactUsMessage"].value;
        cmd.poweredBy = this.myForm.controls["poweredBy"].value;
        cmd.callUs = this.myForm.controls["callUs"].value;
        cmd.isDefault = this.myForm.controls["isDefault"].value;
        cmd.mobileLogo = this.myForm.controls["mobileLogo"].value;
        cmd.desktopLogo = this.myForm.controls["desktopLogo"].value;
        cmd.footerLogo = this.myForm.controls["footerLogo"].value;
        cmd.sliderBackGroundImage =
          this.myForm.controls["sliderBackGroundImage"].value;
        this.client.update(this.myForm.controls["id"].value, cmd).subscribe(
          (result) => {
            this.inProgress = false;
            if (result == null) {
              this.service.getAllCorporationInfo();
              Swal.fire({
                title: "ذخیره اطلاعات شرکت با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره اطلاعات شرکت با خطا مواجه شد",
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
        let cmd = new CreateCorporationInfoCommand();
        cmd.contactUsMessage = this.myForm.controls["contactUsMessage"].value;
        cmd.poweredBy = this.myForm.controls["poweredBy"].value;
        cmd.callUs = this.myForm.controls["callUs"].value;
        cmd.isDefault = this.myForm.controls["isDefault"].value;
        cmd.mobileLogo = this.myForm.controls["mobileLogo"].value;
        cmd.desktopLogo = this.myForm.controls["desktopLogo"].value;
        cmd.footerLogo = this.myForm.controls["footerLogo"].value;
        cmd.sliderBackGroundImage =
          this.myForm.controls["sliderBackGroundImage"].value;

        this.client.create(cmd).subscribe(
          (result) => {
            this.inProgress = false;
            if (result > 0) {
              this.service.getAllCorporationInfo();
              Swal.fire({
                title: "ذخیره اطلاعات شرکت با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });

              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره اطلاعات شرکت با خطا مواجه شد",
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

  edit(item: CorporationInfoModel) {
    debugger;
    this.selectedId = item.id;

    this.myForm.controls["id"].setValue(this.selectedId ?? null);
    this.myForm.controls["contactUsMessage"].setValue(
      item.contactUsMessage ?? null
    );
    this.myForm.controls["poweredBy"].setValue(item.poweredBy ?? null);
    this.myForm.controls["callUs"].setValue(item.callUs ?? null);
    this.myForm.controls["isDefault"].setValue(item.isDefault ?? null);
    this.myForm.controls["mobileLogo"].setValue(item.mobileLogo ?? null);
    this.myForm.controls["desktopLogo"].setValue(item.desktopLogo ?? null);
    this.myForm.controls["footerLogo"].setValue(item.footerLogo ?? null);
    this.myForm.controls["sliderBackGroundImage"].setValue(
      item.sliderBackGroundImage ?? null
    );

    this.selectedFileUrlMobile = item.mobileLogoSrc;
    this.selectedFileUrlDesktop = item.desktopLogoSrc;
    this.selectedFileUrlFooter = item.footerLogoSrc;
    this.selectedFileUrlSliderBackGroundImage = item.sliderBackGroundImageSrc;

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
    this.client.deleteRangeCorporationInfo([...this.checkedItems]).subscribe(
      (result) => {
        this.inProgress = false;
        if (result == null) {
          Swal.fire({
            title: "حذف اطلاعات شرکت با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.service.getAllCorporationInfo();
        } else {
          Swal.fire({
            title: "حذف اطلاعات شرکت با خطا مواجه شد",
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
    this.selectedFileUrlSliderBackGroundImage = undefined;
    this.selectedFileUrlFooter = undefined;
    this.selectedFileUrlMobile = undefined;
    this.selectedFileUrlDesktop = undefined;
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
    this.client.getCorporationInfoById(item.id).subscribe({
      next: (res) => {
        this.selectedItem = res as any;
        this.modalService.open(this.corporationInfoTabModel, {
          size: "lg",
          backdrop: "static",
        });
      },
    });
  }
}
