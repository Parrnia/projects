import { Component, ElementRef, QueryList, ViewChild, ViewChildren } from '@angular/core';

import { Observable } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { NgbAlertConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { QuestionsClient, ModelsClient, QuestionType, UpdateQuestionCommand, CreateQuestionCommand } from 'src/app/web-api-client';
import { DecimalPipe } from '@angular/common';
import Swal from 'sweetalert2';
import { QuestionGridService } from './question-grid.service';
import { QuestionModel } from './question.model';
import { NgbdQuestionSortableHeader, SortEvent } from './question-sortable.directive';
import { QuestionValidators } from './question-validators';
import { ExportFileService } from 'src/app/shared/services/export-file/export-file.service';
export type QuestionTypeKey = keyof typeof QuestionType;
@Component({
  selector: "app-question",
  templateUrl: "./question.component.html",
  styleUrls: ["./question.component.scss"],
  providers: [QuestionGridService, DecimalPipe, NgbAlertConfig],
})
export class QuestionComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: QuestionModel;
  gridjsList$!: Observable<QuestionModel[]>;
  total$: Observable<number>;
  inProgress = false;
  inProgressAllExportbtn = false;
  questionTypes = QuestionType;
  questionTypeId?: any;
  @ViewChildren(NgbdQuestionSortableHeader)
  questions!: QueryList<NgbdQuestionSortableHeader>;
  @ViewChild("getDetailExportTabModel") getDetailExportTabModel: any = [];
  @ViewChild("confirmationModal") confirmationModal: any;
  @ViewChild("formModal") formModal: any = [];
  @ViewChild("fileInput") fileInput!: ElementRef<HTMLInputElement>;
  formData = new FormData();
  checkedItems: Set<number> = new Set<number>();
  constructor(
    alertConfig: NgbAlertConfig,
    public client: QuestionsClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    private exportService: ExportFileService,
    public service: QuestionGridService,
    public modelsClient: ModelsClient
  ) {
    this.gridjsList$ = service.questions$;
    this.total$ = service.total$;

    this.myForm = this.fb.group({
      id: 0,
      questionText: ["", [Validators.required]],
      answerText: ["", [Validators.required]],
      questionTypeId: [0, [Validators.required]],
      isActive: false,
    });

    this.form.questionText.addAsyncValidators(
      QuestionValidators.validQuestionText(
        this.client,
        this.form.questionText.value
      )
    );

    alertConfig.type = "success";

    this.form.id.valueChanges.subscribe((id) => {
      this.form.questionText.setAsyncValidators(
        QuestionValidators.validQuestionText(this.client, id != null ? id : 0)
      );
    });
  }

  ngOnInit(): void {
    this.breadCrumbItems = [
      { label: "خوشه اطلاعات" },
      { label: "پرسش", active: true },
    ];
  }

  getNumericKeys(): QuestionTypeKey[] {
    return Object.entries(this.questionTypes)
      .filter(([key, value]) => typeof value === "number")
      .map(([key, value]) => key as QuestionTypeKey);
  }

  mapNumericKeyToString(key: QuestionTypeKey): string {
    return this.mapNemberKeyToString(this.questionTypes[key].toString());
  }
  mapNemberKeyToString(key: string): string {
    switch (parseInt(key)) {
      case QuestionType.OrdersAndReturns:
        return "سفارشات و مرجوعی ها";
      case QuestionType.PaymentInformation:
        return "اطلاعات پرداخت";
      case QuestionType.ShippingInformation:
        return "اطلاعات ارسال";
      default:
        return "";
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
    // resetting other questions
    this.questions.forEach((question) => {
      if (question.sortable !== column) {
        question.direction = "";
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
    this.myForm.value.questionTypeId = parseInt(this.questionTypeId);
    if (this.myForm.valid) {
      console.log("form value", this.myForm.value);
      if (this.selectedId > 0) {
        this.myForm.controls["id"].setValue(this.selectedId);
        let cmd = new UpdateQuestionCommand();
        cmd.id = this.myForm.controls["id"].value;
        cmd.questionText = this.myForm.controls["questionText"].value;
        cmd.answerText = this.myForm.controls["answerText"].value;
        cmd.questionType = this.myForm.controls["questionTypeId"].value;
        this.client.update(this.myForm.controls["id"].value, cmd).subscribe(
          (result) => {
            this.inProgress = false;
          if (result == null) {

             
            this.service.getAllQuestion();
            Swal.fire({
              title: 'ذخیره پرسش با موفقیت انجام شد',
              icon: 'success',
              iconHtml: '!',
              confirmButtonText: 'تایید'
            });
            this.modalService.dismissAll();
            this.handleCloseFormModal();

          } else {

            Swal.fire({
              title: 'ذخیره پرسش با خطا مواجه شد',
              icon: 'error',
              iconHtml: '!',
              confirmButtonText: 'تایید'
            });
            this.modalService.dismissAll();
          }
        }, error => {
          this.inProgress = false;
         console.error(error)
        });

      } else {
        let cmd = new CreateQuestionCommand();
        cmd.questionText = this.myForm.controls['questionText'].value;
        cmd.answerText = this.myForm.controls['answerText'].value;
        cmd.questionType = this.myForm.controls['questionTypeId'].value;
        this.client.create(cmd).subscribe(result => {
          this.inProgress = false;
          if (result > 0) {
            this.service.getAllQuestion();
            Swal.fire({
              title: 'ذخیره پرسش با موفقیت انجام شد',
              icon: 'success',
              iconHtml: '!',
              confirmButtonText: 'تایید'
            });

            this.modalService.dismissAll();
            this.handleCloseFormModal();

          } else {
            Swal.fire({
              title: 'ذخیره پرسش با خطا مواجه شد',
              icon: 'error',
              iconHtml: '!',
              confirmButtonText: 'تایید'
            });
            this.modalService.dismissAll();
          }
        }, error => {
          this.inProgress = false;
         console.error(error)});
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

  edit(item: QuestionModel) {
    debugger;
    this.selectedId = item.id;

    this.myForm.controls["id"].setValue(this.selectedId ?? null);
    this.myForm.controls["questionText"].setValue(item.questionText ?? null);
    this.myForm.controls["answerText"].setValue(item.answerText ?? null);
    this.myForm.controls["questionTypeId"].setValue(item.questionType ?? null);
    this.myForm.controls["isActive"].setValue(item.isActive ?? null);

    this.questionTypeId = item.questionType ?? null;
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
    this.client.deleteRangeQuestion([...this.checkedItems]).subscribe(result => {
      this.inProgress = false;
      if (result == null) {

        Swal.fire({
          title: 'حذف پرسش با موفقیت انجام شد',
          icon: 'success',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        })
        this.modalService.dismissAll();
        this.service.getAllQuestion();

      } else {

        Swal.fire({
          title: 'حذف پرسش با خطا مواجه شد',
          icon: 'success',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        })

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

