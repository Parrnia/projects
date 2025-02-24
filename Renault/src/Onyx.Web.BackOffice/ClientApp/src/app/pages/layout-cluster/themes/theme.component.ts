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
  ThemesClient,
  CountriesClient,
  CountryDto,
  FileParameter,
} from "src/app/web-api-client";
import { DecimalPipe } from "@angular/common";
import Swal from "sweetalert2";
import { ThemeGridService } from "./theme-grid.service";
import { ThemeModel } from "./theme.model";
import { NgbdThemeSortableHeader, SortEvent } from "./theme-sortable.directive";
import { ImageService } from "src/app/core/services/image.service";
import { NonNullAssert } from "@angular/compiler";
import { ThemeValidators } from "./theme-validators";

@Component({
  selector: "app-theme",
  templateUrl: "./theme.component.html",
  styleUrls: ["./theme.component.scss"],
  providers: [ThemeGridService, DecimalPipe, NgbAlertConfig],
})
export class ThemeComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: ThemeModel;
  gridjsList$!: Observable<ThemeModel[]>;
  total$: Observable<number>;
  @ViewChildren(NgbdThemeSortableHeader)
  themes!: QueryList<NgbdThemeSortableHeader>;
  @ViewChild("confirmationModal") confirmationModal: any;
  @ViewChild("formModal") formModal: any = [];
  @ViewChild("fileInput") fileInput!: ElementRef<HTMLInputElement>;
  formData = new FormData();
  checkedItems: Set<number> = new Set<number>();

  btnPrimaryColor: string = '#FFF';
  btnPrimaryHoverColor: string = '#FFF';
  btnSecondaryColor: string = '#FFF';
  btnSecondaryHoverColor: string = '#FFF';
  themeColor: string = '#FFF';
  headerAndFooterColor: string = '#FFF';

  constructor(
    alertConfig: NgbAlertConfig,
    public client: ThemesClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    public service: ThemeGridService
  ) {
    this.gridjsList$ = service.themes$;
    this.total$ = service.total$;

    this.myForm = this.fb.group({
      id: 0,
      title: ["", [Validators.required]],
      btnPrimaryColor: ['#FFF', [Validators.required]],
      btnPrimaryHoverColor: ['#FFF', [Validators.required]],
      btnSecondaryColor: ['#FFF', [Validators.required]],
      btnSecondaryHoverColor: ['#FFF', [Validators.required]],
      themeColor: ['#FFF', [Validators.required]],
      headerAndFooterColor: ['#FFF', [Validators.required]],
      isDefault: false,
    });

    this.form.title.addAsyncValidators(
      ThemeValidators.validThemeTitle(this.client, this.form.title.value)
    );

    alertConfig.type = "success";

    this.form.id.valueChanges.subscribe((id) => {
      this.form.title.setAsyncValidators(
        ThemeValidators.validThemeTitle(this.client, id != null ? id : 0)
      );
    });
  }

  ngOnInit(): void {
    this.breadCrumbItems = [
      { label: "خوشه شخصی سازی قالب" },
      { label: "تم ", active: true },
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

  /**
   * Sort table data
   * @param param0 sort the column
   *
   */
  onSort({ column, direction }: SortEvent) {
    // resetting other themes
    this.themes.forEach((theme) => {
      if (theme.sortable !== column) {
        theme.direction = "";
      }
    });

    this.service.sortColumn = column;
    this.service.sortDirection = direction;
  }
  get form() {
    return this.myForm.controls;
  }

  onSubmit(): void {
    debugger;
    this.markAllControlsAsTouched(this.myForm);
    this.myForm.controls["btnPrimaryColor"].setValue(this.btnPrimaryColor);
    this.myForm.controls["btnPrimaryHoverColor"].setValue(
      this.btnPrimaryHoverColor
    );
    this.myForm.controls["btnSecondaryColor"].setValue(this.btnSecondaryColor);
    this.myForm.controls["btnSecondaryHoverColor"].setValue(
      this.btnSecondaryHoverColor
    );
    this.myForm.controls["themeColor"].setValue(this.themeColor);
    this.myForm.controls["headerAndFooterColor"].setValue(
      this.headerAndFooterColor
    );

    this.submit = true;
    if (this.myForm.valid) {
      if (this.selectedId > 0) {
        this.myForm.controls["id"].setValue(this.selectedId);

        this.client
          .update(this.myForm.controls["id"].value, this.myForm.value)
          .subscribe(
            (result) => {
              if (result == null) {
                this.service.getAllTheme();
                Swal.fire({
                  title: "ذخیره تم با موفقیت انجام شد",
                  icon: "success",
                  iconHtml: "!",
                  confirmButtonText: "تایید",
                });
                this.modalService.dismissAll();
                this.handleCloseFormModal();
              } else {
                Swal.fire({
                  title: "ذخیره تم با خطا مواجه شد",
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
        this.client.create(this.myForm.value).subscribe(
          (result) => {
            if (result > 0) {
              this.service.getAllTheme();
              Swal.fire({
                title: "ذخیره تم با موفقیت انجام شد",
                icon: "success",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });

              this.modalService.dismissAll();
              this.handleCloseFormModal();
            } else {
              Swal.fire({
                title: "ذخیره تم با خطا مواجه شد",
                icon: "error",
                iconHtml: "!",
                confirmButtonText: "تایید",
              });
              this.modalService.dismissAll();
            }
          },
          (error) => console.error(error)
        );
      }
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

  edit(item: ThemeModel) {
    debugger;
    this.selectedId = item.id;

    this.myForm.controls["id"].setValue(this.selectedId ?? null);
    this.myForm.controls["title"].setValue(item.title ?? null);
    this.myForm.controls["btnPrimaryColor"].setValue(
      item.btnPrimaryColor ?? null
    );
    this.btnPrimaryColor = item.btnPrimaryColor;
    this.myForm.controls["btnPrimaryHoverColor"].setValue(
      item.btnPrimaryHoverColor ?? null
    );
    this.btnPrimaryHoverColor = item.btnPrimaryHoverColor;
    this.myForm.controls["btnSecondaryColor"].setValue(
      item.btnSecondaryColor ?? null
    );
    this.btnSecondaryColor = item.btnSecondaryColor;
    this.myForm.controls["btnSecondaryHoverColor"].setValue(
      item.btnSecondaryHoverColor ?? null
    );
    this.btnSecondaryHoverColor = item.btnSecondaryHoverColor;
    this.myForm.controls["themeColor"].setValue(item.themeColor ?? null);
    this.themeColor = item.themeColor;
    this.myForm.controls["headerAndFooterColor"].setValue(
      item.headerAndFooterColor ?? null
    );
    this.headerAndFooterColor = item.headerAndFooterColor;
    this.myForm.controls["isDefault"].setValue(item.isDefault ?? null);
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
    this.client.deleteRangeTheme([...this.checkedItems]).subscribe(
      (result) => {
        if (result == null) {
          Swal.fire({
            title: "حذف تم با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.service.getAllTheme();
        } else {
          Swal.fire({
            title: "حذف تم با خطا مواجه شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });

          this.modalService.dismissAll();
        }
      },
      (error) => console.error(error)
    );

    this.checkedItems = new Set<number>();  }

  public handleCloseFormModal() {
    this.myForm.reset();
    this.myForm.markAsUntouched();
    this.myForm.setErrors(null);
    this.myForm.markAsPristine();
    this.btnPrimaryColor = '#FFF';
    this.btnPrimaryHoverColor = '#FFF';
    this.btnSecondaryColor = '#FFF';
    this.btnSecondaryHoverColor = '#FFF';
    this.themeColor = '#FFF';
    this.headerAndFooterColor = '#FFF';
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
