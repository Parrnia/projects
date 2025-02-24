import {
  Component,
  ElementRef,
  Input,
  QueryList,
  ViewChild,
  ViewChildren,
} from "@angular/core";
import { DecimalPipe } from "@angular/common";
import {
  NgbAlertConfig,
  NgbModal,
  NgbModalRef,
} from "@ng-bootstrap/ng-bootstrap";
import {
  FormBuilder,
  FormGroup,
  UntypedFormBuilder,
  Validators,
} from "@angular/forms";
import { Observable, of } from "rxjs";
import Swal from "sweetalert2";
import {
  CreateProductImageCommand,
  FileParameter,
  FileUploadMetadataDto,
  ProductImagesClient,
  ProductTypesClient,
  UpdateProductImageCommand,
} from "src/app/web-api-client";
import { ProductImagesGridService } from "./product-images-grid.service";
import { ProductImagesModel } from "./product-images.model";
import { ImageService } from "src/app/core/services/image.service";
import { C } from "@fullcalendar/core/internal-common";
// import { NgbdProductImagesSortableHeader, SortEvent } from "./product-images-sortable.directive";

@Component({
  selector: "app-product-images",
  templateUrl: "./product-images.component.html",
  styleUrls: ["./product-images.component.scss"],
  providers: [ProductImagesGridService, DecimalPipe, NgbAlertConfig],
})
export class ProductImagesComponent {
  // bread crumb items
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: ProductImagesModel;
  gridjsList$!: Observable<ProductImagesModel[]>;
  total$: Observable<number>;
  inProgress = false;
  @ViewChild("confirmationModal") confirmationModal: any;
  confirmationModalRef: NgbModalRef | undefined;
  @ViewChild("formModal") formModal: any = [];
  formModalInsertRef: NgbModalRef | undefined;
  formModalUpdateRef: NgbModalRef | undefined;
  @ViewChild("fileInput") fileInput!: ElementRef<HTMLInputElement>;
  // selectedFile?: FileParameter;
  // selectedFileName?: any;
  // selectedFileUrl: Observable<string | undefined> = of(undefined);
  selectedFileUrl: string | undefined = undefined;
  @Input() product?: any;
  checkedItems: Set<number> = new Set<number>();
  constructor(
    alertConfig: NgbAlertConfig,
    public client: ProductImagesClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    public service: ProductImagesGridService,
    private formBuilder: UntypedFormBuilder,
    private imageService: ImageService
  ) {
    this.gridjsList$ = service.productImages$;
    this.total$ = service.total$;
    this.myForm = this.fb.group({
      id: 0,
      image: ["", [Validators.required]],
      order: [0, [Validators.required]],
      productId: [0, [Validators.required]],
      isActive: true,
    });
  }

  ngOnInit(): void {
    this.service.getAllproductImages(this.product.id);

    console.log("   this.gridjsList$ ", this.gridjsList$);
  }

  // onFileSelected(event: any): void {

  //   var file = event.target.files[0];
  //   const newFile = new File([file], file.name, { type: file.type });
  //   this.selectedFileName = newFile.name;
  //   this.selectedFile = { data: newFile, fileName: newFile.name };
  //   this.selectedFileUrl = this.imageService.createImageUrl(this.selectedFile);
  // }
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
        let cmd = new UpdateProductImageCommand();
        cmd.id = this.myForm.controls["id"].value;
        cmd.order = this.myForm.controls["order"].value;
        cmd.productId = this.product.id;
        cmd.image = this.myForm.controls["image"].value;
        cmd.isActive = this.myForm.controls["isActive"].value;

        this.client.update(this.myForm.controls["id"].value, cmd).subscribe(
          (result) => {
            this.inProgress = false;
            if (result == null) {
              this.service.getAllproductImages(this.product.id);
              Swal.fire({
                title: "ذخیره تصویر محصول با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              //this.formModal.nativeElement.click();

              this.formModalUpdateRef?.close();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره تصویر محصول با خطا مواجه شد",
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
        let cmd = new CreateProductImageCommand();
        cmd.order = this.myForm.controls["order"].value;
        cmd.productId = this.product.id;
        cmd.image = this.myForm.controls["image"].value;
        cmd.isActive = this.myForm.controls["isActive"].value;

        this.client.create(cmd).subscribe(
          (result) => {
            this.inProgress = false;
            if (result > 0) {
              this.service.getAllproductImages(this.product.id);
              Swal.fire({
                title: "ذخیره تصویر محصول با موفقیت انجام شد",
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
                title: "ذخیره تصویر محصول با خطا مواجه شد",
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
    }else{
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

  edit(item: ProductImagesModel) {
    debugger;
    this.selectedId = item.id;

    this.myForm.controls["id"].setValue(this.selectedId ?? null);
    this.myForm.controls["order"].setValue(item.order ?? null);
    this.myForm.controls["productId"].setValue(item.productId ?? null);
    this.myForm.controls["image"].setValue(item.image ?? null);
    this.myForm.controls["isActive"].setValue(item.isActive ?? null);

    // this.selectedFileUrl = of(item.image);
    // if (this.fileInput) {
    //   this.fileInput.nativeElement.click();
    // }
    this.selectedFileUrl = item.imageSrc;
    // this.selectedFile = this.imageService.convertSrcToFileParameter(item.image);
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
    this.client.deleteRange([...this.checkedItems]).subscribe(
      (result) => {
        if (result == null) {
          Swal.fire({
            title: "حذف تصویر محصول با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.confirmationModalRef?.close();
          this.service.getAllproductImages(this.product.id);
        } else {
          Swal.fire({
            title: "حذف تصویر محصول با خطا مواجه شد",
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
    // this.selectedFile = undefined;
    // this.selectedFileUrl = of(undefined);
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
