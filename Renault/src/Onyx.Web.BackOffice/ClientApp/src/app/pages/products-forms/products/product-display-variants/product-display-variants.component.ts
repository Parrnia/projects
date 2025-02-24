import {  Component,  ElementRef,  Input,  QueryList,  ViewChild,  ViewChildren,} from "@angular/core";
import { DecimalPipe } from "@angular/common";
import {  NgbAlertConfig,  NgbModal,  NgbModalRef,} from "@ng-bootstrap/ng-bootstrap";
import {  FormBuilder,  FormGroup,  UntypedFormBuilder,  Validators,} from "@angular/forms";
import { Observable, of } from "rxjs";
import Swal from "sweetalert2";
import {  CreateVariantCommand, FileParameter,  FileUploadMetadataDto,  ProductDisplayVariantsClient,  ProductTypesClient, UpdateVariantCommand,  } from "src/app/web-api-client";
import { C } from "@fullcalendar/core/internal-common";
import { ProductDisplayVariantsGridService } from "./product-display-variants-grid.service";
import { ProductDisplayVariantsModel } from "./product-display-variants.model";
// import { NgbdProductDisplayVariantsSortableHeader, SortEvent } from "./product-displayVariants-sortable.directive";

@Component({
  selector: "app-product-display-variants",
  templateUrl: "./product-display-variants.component.html",
  styleUrls: ["./product-display-variants.component.scss"],
  providers: [ProductDisplayVariantsGridService, DecimalPipe, NgbAlertConfig],
})
export class ProductDisplayVariantsComponent {
  // bread crumb items
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: ProductDisplayVariantsModel;
  gridjsList$!: Observable<ProductDisplayVariantsModel[]>;
  total$: Observable<number>;
  inProgress = false;
  @ViewChild("confirmationModal") confirmationModal: any;
  confirmationModalRef: NgbModalRef | undefined;
  @ViewChild("formModal") formModal: any = [];
  formModalInsertRef: NgbModalRef | undefined;
  formModalUpdateRef: NgbModalRef | undefined;
  @Input() product?: any;
  checkedItems: Set<number> = new Set<number>();
  constructor(
    alertConfig: NgbAlertConfig,
    public client: ProductDisplayVariantsClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    public service: ProductDisplayVariantsGridService,
    private formBuilder: UntypedFormBuilder,
  ) {
    this.gridjsList$ = service.productDisplayVariants$;
    this.total$ = service.total$;
    this.myForm = this.fb.group({
      id: 0,
      name: ["", [Validators.required]],
      isBestSeller: false,
      isFeatured: false,
      isLatest: false,
      isNew: false,
      isPopular: false,
      isSale: false,
      isSpecialOffer: false,
      isTopRated: false,
      isActive: true,
    });
  }

  ngOnInit(): void {
    this.service.getAllproductDisplayVariants(this.product.id);

    console.log("   this.gridjsList$ ", this.gridjsList$);
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

  get form() {
    return this.myForm.controls;
  }

  onSubmit(): void {
    this.inProgress = true;
    this.markAllControlsAsTouched(this.myForm);
    this.submit = true;
    if (this.myForm.valid) {
      console.log("productId", this.product);
      if (this.selectedId > 0) {
        this.myForm.controls["id"].setValue(this.selectedId);
        let cmd = new UpdateVariantCommand();
        cmd.id = this.myForm.controls["id"].value;
        cmd.productId = this.product.id;
        cmd.name = this.myForm.controls["name"].value;
        cmd.isBestSeller = this.myForm.controls["isBestSeller"].value;
        cmd.isFeatured = this.myForm.controls["isFeatured"].value;
        cmd.isLatest = this.myForm.controls["isLatest"].value;
        cmd.isNew = this.myForm.controls["isNew"].value;
        cmd.isPopular = this.myForm.controls["isPopular"].value;
        cmd.isSale = this.myForm.controls["isSale"].value;
        cmd.isSpecialOffer = this.myForm.controls["isSpecialOffer"].value;
        cmd.isTopRated = this.myForm.controls["isTopRated"].value;
        cmd.isActive = this.myForm.controls["isActive"].value;

        this.client.update(this.myForm.controls["id"].value, cmd).subscribe(
          (result) => {
            this.inProgress = false;
            if (result == null) {
              this.service.getAllproductDisplayVariants(this.product.id);
              Swal.fire({
                title: "ذخیره نمایش محصول با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              //this.formModal.nativeElement.click();

              this.formModalUpdateRef?.close();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره نمایش محصول با خطا مواجه شد",
                icon: "error",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.formModalUpdateRef?.close();
            }
          },
          (error) => {
            this.inProgress = false;
            console.error(error)
          }
        );
      } else {
        let cmd = new CreateVariantCommand();
        cmd.productId = this.product.id;
        cmd.name = this.myForm.controls["name"].value;
        cmd.isBestSeller = this.myForm.controls["isBestSeller"].value;
        cmd.isFeatured = this.myForm.controls["isFeatured"].value;
        cmd.isLatest = this.myForm.controls["isLatest"].value;
        cmd.isNew = this.myForm.controls["isNew"].value;
        cmd.isPopular = this.myForm.controls["isPopular"].value;
        cmd.isSale = this.myForm.controls["isSale"].value;
        cmd.isSpecialOffer = this.myForm.controls["isSpecialOffer"].value;
        cmd.isTopRated = this.myForm.controls["isTopRated"].value;
        cmd.isActive = this.myForm.controls["isActive"].value;
        debugger;
        this.client.create(cmd).subscribe(
          (result) => {
            this.inProgress = false;
            if (result > 0) {
              this.service.getAllproductDisplayVariants(this.product.id);
              Swal.fire({
                title: "ذخیره نمایش محصول با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              debugger;
              //this.formModal.nativeElement.click();

              this.formModalInsertRef?.close();
              this.myForm.reset();
            } else {
              Swal.fire({
                title: "ذخیره نمایش محصول با خطا مواجه شد",
                icon: "error",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.formModalInsertRef?.close();
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
    // this.selectedFile = undefined;
    // this.selectedFileUrl = of(undefined);
  }

  edit(item: ProductDisplayVariantsModel) {
    debugger;
    this.selectedId = item.id;

    this.myForm.controls["id"].setValue(this.selectedId ?? null);
    this.myForm.controls["name"].setValue(item.name ?? null);
    this.myForm.controls["isBestSeller"].setValue(item.isBestSeller ?? null);
    this.myForm.controls["isFeatured"].setValue(item.isFeatured ?? null);
    this.myForm.controls["isLatest"].setValue(item.isLatest ?? null);
    this.myForm.controls["isNew"].setValue(item.isNew ?? null);
    this.myForm.controls["isPopular"].setValue(item.isPopular ?? null);
    this.myForm.controls["isSale"].setValue(item.isSale ?? null);
    this.myForm.controls["isSpecialOffer"].setValue(item.isSpecialOffer ?? null);
    this.myForm.controls["isTopRated"].setValue(item.isTopRated ?? null);
    this.myForm.controls["isActive"].setValue(item.isActive ?? null);

    this.formModalUpdateRef = this.modalService.open(this.formModal, {
      size: "lg",
      backdrop: "static",
    });
  }

  openInsertModal() {
    this.myForm.controls["isActive"].setValue(true);
    this.formModalInsertRef = this.modalService.open(this.formModal);
  }

  openDeleteConfirmationModal(id: any) {
    this.selectedId = id;
    this.confirmationModalRef = this.modalService.open(this.confirmationModal);
  }

  deleteItems() {
    this.inProgress = true;
    this.client.deleteRangeProductDisplayVariant([...this.checkedItems]).subscribe(
      (result) => {
        this.inProgress = false;
        if (result == null) {
          Swal.fire({
            title: "حذف نمایش محصول با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.confirmationModalRef?.close();
          this.service.getAllproductDisplayVariants(this.product.id);
        } else {
          Swal.fire({
            title: "حذف نمایش محصول با خطا مواجه شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });

          this.confirmationModalRef?.close();
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
