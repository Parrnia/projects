import {
  Component,
  ElementRef,
  QueryList,
  ViewChild,
  ViewChildren,
} from "@angular/core";

import { Observable, of } from "rxjs";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";

import { NgbAlertConfig, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import {
  TestimonialsClient,
  FileParameter,
  AboutUsDto,
  AboutUsClient,
  FileUploadMetadataDto,
  UpdateTestimonialCommand,
  CreateTestimonialCommand,
} from "src/app/web-api-client";
import { DecimalPipe } from "@angular/common";
import Swal from "sweetalert2";
import { TestimonialGridService } from "./testimonial-grid.service";
import { TestimonialModel } from "./testimonial.model";
import {
  NgbdTestimonialSortableHeader,
  SortEvent,
} from "./testimonial-sortable.directive";
import { ImageService } from "src/app/core/services/image.service";
import { TestimonialValidators } from "./testimonial-validators";
import { ExportFileService } from "src/app/shared/services/export-file/export-file.service";

@Component({
  selector: "app-testimonial",
  templateUrl: "./testimonial.component.html",
  styleUrls: ["./testimonial.component.scss"],
  providers: [TestimonialGridService, DecimalPipe, NgbAlertConfig],
})
export class TestimonialComponent {
  selectedFileUrl: string | undefined = undefined;
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: TestimonialModel;
  gridjsList$!: Observable<TestimonialModel[]>;
  total$: Observable<number>;
  inProgress = false;
  inProgressAllExportbtn = false;
  aboutUsList?: AboutUsDto[];
  @ViewChildren(NgbdTestimonialSortableHeader)
  testimonials!: QueryList<NgbdTestimonialSortableHeader>;
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("confirmationModal") confirmationModal: any;
  @ViewChild("formModal") formModal: any = [];
  @ViewChild("fileInput") fileInput!: ElementRef<HTMLInputElement>;
  aboutUsId?: any;
  formData = new FormData();
  checkedItems: Set<number> = new Set<number>();
  constructor(
    alertConfig: NgbAlertConfig,
    public client: TestimonialsClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    private exportService: ExportFileService,
    public service: TestimonialGridService,
    public aboutUsClient: AboutUsClient,
    private imageService: ImageService
  ) {
    this.gridjsList$ = service.testimonials$;
    this.total$ = service.total$;

    this.myForm = this.fb.group({
      id: 0,
      name: ["", [Validators.required]],
      position: ["", [Validators.required]],
      avatar: ["", [Validators.required]],
      rating: [0, [Validators.required]],
      review: ["", [Validators.required]],
      isActive: false,
      aboutUsId: [0, [Validators.required]],
    });

    this.form.name.addAsyncValidators(
      TestimonialValidators.validTestimonialName(
        this.client,
        this.form.name.value
      )
    );

    this.form.id.valueChanges.subscribe((id) => {
      this.form.name.setAsyncValidators(
        TestimonialValidators.validTestimonialName(
          this.client,
          id != null ? id : 0
        )
      );
    });
    alertConfig.type = "success";
  }

  ngOnInit(): void {
    this.getAllModels();

    this.breadCrumbItems = [
      { label: "خوشه اطلاعات" },
      { label: "شواهد", active: true },
    ];
  }

  onFileSelected(event: FileUploadMetadataDto | null): void {
    if (event) {
      this.myForm.controls["avatar"].setValue(event.fileId);
    } else {
      this.myForm.controls["avatar"].setValue(null);
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
    // resetting other testimonials
    this.testimonials.forEach((testimonial) => {
      if (testimonial.sortable !== column) {
        testimonial.direction = "";
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
    console.log("selecte testimonial type", this.aboutUsId);
    this.submit = true;
    this.myForm.value.modelId = parseInt(this.aboutUsId);
    if (this.myForm.valid) {
      console.log("form value", this.myForm.value);
      if (this.selectedId > 0) {
        this.myForm.controls["id"].setValue(this.selectedId);
        let cmd = new UpdateTestimonialCommand();
        cmd.id = this.myForm.controls["id"].value;
        cmd.name = this.myForm.controls["name"].value;
        cmd.position = this.myForm.controls["position"].value;
        cmd.avatar = this.myForm.controls["avatar"].value;
        cmd.rating = this.myForm.controls["rating"].value;
        cmd.review = this.myForm.controls["review"].value;
        cmd.isActive = this.myForm.controls["isActive"].value;
        cmd.aboutUsId = this.myForm.controls["aboutUsId"].value;
        this.client.update(this.myForm.controls["id"].value, cmd).subscribe(
          (result) => {
            this.inProgress = false;
            if (result == null) {
              this.service.getAllTestimonial();
              Swal.fire({
                title: "ذخیره نوع با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره نوع با خطا مواجه شد",
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
        let cmd = new CreateTestimonialCommand();
        cmd.name = this.myForm.controls["name"].value;
        cmd.position = this.myForm.controls["position"].value;
        cmd.avatar = this.myForm.controls["avatar"].value;
        cmd.rating = this.myForm.controls["rating"].value;
        cmd.review = this.myForm.controls["review"].value;
        cmd.isActive = this.myForm.controls["isActive"].value;
        cmd.aboutUsId = this.myForm.controls["aboutUsId"].value;

        this.client.create(cmd).subscribe(
          (result) => {
            this.inProgress = false;
            if (result > 0) {
              this.service.getAllTestimonial();
              Swal.fire({
                title: "ذخیره نوع با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });

              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره نوع با خطا مواجه شد",
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
    this.myForm.reset();
    Object.keys(this.myForm.controls).forEach((controlName) => {
      const control = this.myForm.controls[controlName];
      control.markAsPristine();
      control.markAsUntouched();
    });
  }

  edit(item: TestimonialModel) {
    debugger;
    this.selectedId = item.id;

    this.myForm.controls["id"].setValue(this.selectedId ?? null);
    this.myForm.controls["name"].setValue(item.name ?? null);
    this.myForm.controls["position"].setValue(item.position ?? null);
    this.myForm.controls["rating"].setValue(item.rating ?? null);
    this.myForm.controls["review"].setValue(item.review ?? null);
    this.myForm.controls["aboutUsId"].setValue(item.aboutUsId ?? null);
    this.myForm.controls["isActive"].setValue(item.isActive ?? null);
    this.myForm.controls["avatar"].setValue(item.avatar ?? null);
    this.selectedFileUrl = item.avatarSrc;
    this.aboutUsId = item.aboutUsId ?? null;
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
    this.client.deleteRangeTestimonial([...this.checkedItems]).subscribe(
      (result) => {
        this.inProgress = false;
        if (result == null) {
          Swal.fire({
            title: "حذف نوع با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.service.getAllTestimonial();
        } else {
          Swal.fire({
            title: "حذف نوع با خطا مواجه شد",
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
    this.aboutUsClient.getAllAboutUs().subscribe(
      (result) => {
        this.aboutUsList = result;
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
