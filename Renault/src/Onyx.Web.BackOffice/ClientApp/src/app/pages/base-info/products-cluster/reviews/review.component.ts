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
  ReviewsClient,
  CountriesClient,
  CountryDto,
  FileParameter,
  UpdateReviewCommand,
} from "src/app/web-api-client";
import { DecimalPipe } from "@angular/common";
import Swal from "sweetalert2";
import { ReviewGridService } from "./review-grid.service";
import { ReviewModel } from "./review.model";
import {
  NgbdReviewSortableHeader,
  SortEvent,
} from "./review-sortable.directive";
import { ExportFileService } from "src/app/shared/services/export-file/export-file.service";

@Component({
  selector: "app-review",
  templateUrl: "./review.component.html",
  styleUrls: ["./review.component.scss"],
  providers: [ReviewGridService, DecimalPipe, NgbAlertConfig],
})
export class ReviewComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: ReviewModel;
  gridjsList$!: Observable<ReviewModel[]>;
  total$: Observable<number>;
  inProgress = false;
  inProgressAllExportbtn = false;
  @ViewChildren(NgbdReviewSortableHeader)
  reviews!: QueryList<NgbdReviewSortableHeader>;
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("confirmationModal") confirmationModal: any;
  @ViewChild("formModal") formModal: any = [];
  @ViewChild("fileInput") fileInput!: ElementRef<HTMLInputElement>;
  formData = new FormData();
  checkedItems: Set<number> = new Set<number>();
  constructor(
    alertConfig: NgbAlertConfig,
    public client: ReviewsClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    private exportService: ExportFileService,
    public service: ReviewGridService
  ) {
    this.gridjsList$ = service.reviews$;
    this.total$ = service.total$;

    this.myForm = this.fb.group({
      id: 0,
      date: ["", [Validators.required]],
      rating: ["", [Validators.required]],
      content: ["", [Validators.required]],
      authorName: ["", [Validators.required]],
      isActive: false,
    });

    alertConfig.type = "success";
  }

  ngOnInit(): void {
    this.breadCrumbItems = [
      { label: "خوشه ویژگی های محصول" },
      { label: "دیدگاه", active: true },
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
    // resetting other reviews
    this.reviews.forEach((review) => {
      if (review.sortable !== column) {
        review.direction = "";
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
    this.submit = true;
    if (this.myForm.valid) {
      if (this.selectedId > 0) {
        this.myForm.controls["id"].setValue(this.selectedId);
        let cmd = new UpdateReviewCommand();

        cmd.id = this.myForm.controls["id"].value;
        // this.myForm.controls["date"].value;
        cmd.rating = this.myForm.controls["rating"].value;
        cmd.content = this.myForm.controls["content"].value;
        cmd.authorName = this.myForm.controls["authorName"].value;
        cmd.isActive = this.myForm.controls["isActive"].value;

        this.client.update(this.myForm.controls["id"].value, cmd).subscribe(
          (result) => {
            if (result == null) {
              this.service.refreshReviews();
              Swal.fire({
                title: "ذخیره دیدگاه با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره دیدگاه با خطا مواجه شد",
                icon: "error",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.modalService.dismissAll();
            }
          },
          (error) => console.error(error)
        );
      } else {
        // let cmd = new CreateReviewCommand();
        // cmd.rating = this.myForm.controls["rating"].value;
        // cmd.content = this.myForm.controls["content"].value;
        // cmd.authorName = this.myForm.controls["authorName"].value;
        // cmd.productId = this.myForm.controls["productId"].value;
        // cmd.customerId = this.myForm.controls["customerId"].value;
        // this.client.create(this.myForm.value).subscribe(
        //   (result) => {
        //     if (result > 0) {
        //       this.service.refreshReviews();
        //       Swal.fire({
        //         title: "ذخیره دیدگاه با موفقیت انجام شد",
        //         icon: "success",
        //         iconHtml: "!",
        //         confirmButtonText: "تایید",
        //       });
        //       this.modalService.dismissAll();
        //       this.handleCloseFormModal();
        //     } else {
        //       Swal.fire({
        //         title: "ذخیره دیدگاه با خطا مواجه شد",
        //         icon: "error",
        //         iconHtml: "!",
        //         confirmButtonText: "تایید",
        //       });
        //       this.modalService.dismissAll();
        //     }
        //   },
        //   (error) => console.error(error)
        // );
      }
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

  edit(item: ReviewModel) {
    debugger;
    this.selectedId = item.id;

    this.myForm.controls["id"].setValue(this.selectedId ?? null);
    this.myForm.controls["date"].setValue(item.date ?? null);
    this.myForm.controls["rating"].setValue(item.rating ?? null);
    this.myForm.controls["content"].setValue(item.content ?? null);
    this.myForm.controls["authorName"].setValue(item.authorName ?? null);
    this.myForm.controls["isActive"].setValue(item.isActive ?? null);

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
    this.client.deleteRangeReview([...this.checkedItems]).subscribe(
      (result) => {
        this.inProgress = false;
        if (result == null) {
          Swal.fire({
            title: "حذف دیدگاه با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.service.refreshReviews();
        } else {
          Swal.fire({
            title: "حذف دیدگاه با خطا مواجه شد",
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
