import { ResendCancelOrderRequestsCommand } from './../../../../web-api-client';
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
  RequestLogsClient,
  CountriesClient,
  CountryDto,
  FileParameter,
} from "src/app/web-api-client";
import { DecimalPipe } from "@angular/common";
import Swal from "sweetalert2";
import { RequsetLogGridService } from "./request-log-grid.service";
import { RequestLogModel } from "./request-log.model";
import {
  NgbdRequestLogSortableHeader,
  SortEvent,
} from "./request-log-sortable.directive";
import { ExportFileService } from 'src/app/shared/services/export-file/export-file.service';

@Component({
  selector: "app-requset-log",
  templateUrl: "./request-log.component.html",
  styleUrls: ["./request-log.component.scss"],
  providers: [RequsetLogGridService, DecimalPipe, NgbAlertConfig],
})
export class RequestLogComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: RequestLogModel;
  gridjsList$!: Observable<RequestLogModel[]>;
  total$: Observable<number>;
  inProgress = false;
  inProgressAllExportbtn = false;
  @ViewChildren(NgbdRequestLogSortableHeader)
  requestLogs!: QueryList<NgbdRequestLogSortableHeader>;
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("confirmationModal") confirmationModal: any;
  @ViewChild("formModal") formModal: any = [];
  @ViewChild("fileInput") fileInput!: ElementRef<HTMLInputElement>;
  formData = new FormData();
  checkedDeleteItems: Set<number> = new Set<number>();
  checkedResendItems: Set<number> = new Set<number>();
  constructor(
    alertConfig: NgbAlertConfig,
    public client: RequestLogsClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    private exportService: ExportFileService,
    public service: RequsetLogGridService
  ) {
    this.gridjsList$ = service.requestLogs$;
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
      { label: "خوشه درخواست های api" },
      { label: "درخواست", active: true },
    ];
  }

  toggleDeleteCheckbox(itemId: number) {
    if (this.checkedDeleteItems.has(itemId)) {
      this.checkedDeleteItems.delete(itemId);
    } else {
      this.checkedDeleteItems.add(itemId);
    }
  }

  isCheckedDelete(itemId: number): boolean {
    return this.checkedDeleteItems.has(itemId);
  }

  deleteMultiple(content: any) {
    if ([...this.checkedDeleteItems].length > 0) {
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
  
  toggleResendCheckbox(itemId: number) {
    if (this.checkedResendItems.has(itemId)) {
      this.checkedResendItems.delete(itemId);
    } else {
      this.checkedResendItems.add(itemId);
    }
  }

  isCheckedResend(itemId: number): boolean {
    return this.checkedResendItems.has(itemId);
  }

  resendMultiple(content: any) {
    if ([...this.checkedResendItems].length > 0) {
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

  resendAll(content: any) {
    this.modalService.open(content, { centered: true });
  }

  /**
   * Sort table data
   * @param param0 sort the column
   *
   */
  onSort({ column, direction }: SortEvent) {
    // resetting other requestLogs
    this.requestLogs.forEach((requestLog) => {
      if (requestLog.sortable !== column) {
        requestLog.direction = "";
      }
    });

    this.service.sortColumn = column;
    this.service.sortDirection = direction;
  }
  get form() {
    return this.myForm.controls;
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

  openInsertModal() {
    this.modalService.open(this.formModal, { size: "lg", backdrop: "static" });
  }

  openDeleteConfirmationModal(id: any) {
    this.selectedId = id;
    this.modalService.open(this.confirmationModal);
  }

  deleteItems() {
    this.inProgress = true;
    this.client.deleteRangeRequestLog([...this.checkedDeleteItems]).subscribe(
      (result) => {
        this.inProgress = false;
        if (result == null) {
          Swal.fire({
            title: "حذف لاگ درخواست با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.service.refreshRequestLogs();
        } else {
          Swal.fire({
            title: "حذف لاگ درخواست با خطا مواجه شد",
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
    this.checkedDeleteItems = new Set<number>();
  }
  resendAllItems() {
    this.inProgress = true;
    this.client.resendAllCancelOrderRequests().subscribe(
      (result) => {
        this.inProgress = false;
        if (result == true) {
          Swal.fire({
            title:
              "ارسال دوباره همه درخواست های لغو سفارش ناموفق با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.service.refreshRequestLogs();
        } else {
          Swal.fire({
            title:
              "ارسال دوباره همه درخواست های لغو سفارش ناموفق با خطا مواجه شد",
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

    this.checkedResendItems = new Set<number>();
  }

  resendItems() {
    this.inProgress = true;
    let resendCancelOrderRequestsCommand =
      new ResendCancelOrderRequestsCommand();
    resendCancelOrderRequestsCommand.ids = [...this.checkedResendItems];
    this.client
      .resendCancelOrderRequests(resendCancelOrderRequestsCommand)
      .subscribe(
        (result) => {
          this.inProgress = false;
          if (result == true) {
            Swal.fire({
              title: "ارسال دوباره درخواست (ها) با موفقیت انجام شد",
              icon: "success",
              iconHtml: "!",
              confirmButtonText: "تایید",
            });
            this.modalService.dismissAll();
            this.service.refreshRequestLogs();
          } else {
            Swal.fire({
              title: "ارسال دوباره درخواست (ها) با خطا مواجه شد",
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

    this.checkedResendItems = new Set<number>();
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
