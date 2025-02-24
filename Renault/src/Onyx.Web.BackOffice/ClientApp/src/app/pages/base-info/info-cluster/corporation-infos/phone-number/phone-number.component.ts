
import { Component, ElementRef, Input, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { DecimalPipe } from "@angular/common";
import { NgbAlertConfig, NgbModal, NgbModalRef } from "@ng-bootstrap/ng-bootstrap";
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from "@angular/forms";
import { Observable, of } from "rxjs";
import Swal from "sweetalert2";
import { CountryDto, CountriesClient, CorporationInfosClient, AddPhoneNumberCommand, RemovePhoneNumberCommand } from "src/app/web-api-client";
import { PhoneNumberGridService } from "./phone-number-grid.service";
import { PhoneNumberModel } from "./phone-number.model";
import { ImageService } from "src/app/core/services/image.service";
// import { NgbdPhoneNumberSortableHeader, SortEvent } from "./addresses-sortable.directive";

@Component({
  selector: 'app-phone-number',
  templateUrl: './phone-number.component.html',
  styleUrls: ['./phone-number.component.scss'],
  providers: [PhoneNumberGridService, DecimalPipe, NgbAlertConfig],
})
export class PhoneNumberComponent {
  // bread crumb items
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: PhoneNumberModel;
  gridjsList$!: Observable<PhoneNumberModel[]>;
  total$: Observable<number>;
  inProgress = false;
  @ViewChild('confirmationModal') confirmationModal: any;
  confirmationModalRef: NgbModalRef | undefined;
  @ViewChild('formModal') formModal: any = [];
  formModalInsertRef: NgbModalRef | undefined;
  formModalUpdateRef: NgbModalRef | undefined;
  @Input() corporationInfo?: any;
  checkedItems: Set<string> = new Set<string>();
  constructor(
    private fb: FormBuilder,
    private modalService: NgbModal,
    public service: PhoneNumberGridService,
    private formBuilder: UntypedFormBuilder,
    public client: CorporationInfosClient) {
    this.gridjsList$ = service.phoneNumbers$;
    this.total$ = service.total$;
    this.myForm = this.fb.group({
      corporationInfoId: 0,
      phoneNumber: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {
    this.client.getCorporationInfoById(this.corporationInfo.id).subscribe({
      next: (res) => {
        this.service.setPhoneNumbers(res.phoneNumbers ?? []);
      }
    });
    this.myForm.controls['corporationInfoId'].setValue(this.corporationInfo.id ?? null);
  }

  toggleCheckbox(itemId: string) {
    if (this.checkedItems.has(itemId)) {
      this.checkedItems.delete(itemId);
    } else {
      this.checkedItems.add(itemId);
    }
  }

  isChecked(itemId: string): boolean {
    return this.checkedItems.has(itemId);
  }

  deleteMultiple(content: any) {
    if ([...this.checkedItems].length > 0) {
      this.confirmationModalRef = this.modalService.open(content, { centered: true });
    }
    else {
      Swal.fire({
        title: 'حداقل یک مورد را انتخاب کنید!',
        icon: 'question',
        iconHtml: '!',
        confirmButtonText: 'تایید'
      })

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
      this.client.addPhoneNumber(
        this.myForm.value.corporationInfoId,
        this.myForm.value
      ).subscribe(result => {
        this.inProgress = false;
        if (result == null) {
          this.client.getCorporationInfoById(this.corporationInfo.id).subscribe({
            next: (res) => {
              this.service.setPhoneNumbers(res.phoneNumbers ?? []);
            }
          });
          
          Swal.fire({
            title: 'ذخیره شماره تماس با موفقیت انجام شد',
            icon: 'success',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });
          debugger;

          this.formModalInsertRef?.close();
          this.myForm.reset();

        } else {
          Swal.fire({
            title: 'ذخیره شماره تماس با خطا مواجه شد',
            icon: 'error',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });
          this.formModalInsertRef?.close();
        }
      }, error => {
        this.inProgress = false;
       console.error(error)
      });
    }else{
      this.inProgress = false;
    }
}

resetForm(): void {
  Object.keys(this.myForm.controls).forEach(controlName => {
    const control = this.myForm.controls[controlName];
    if (control.enabled) {
      control.markAsPristine();
      control.markAsUntouched();
      control.reset();
    }
  });
}


openInsertModal() {
  this.myForm.controls['corporationInfoId'].setValue(this.corporationInfo.id ?? null);
  this.formModalInsertRef = this.modalService.open(this.formModal);
}


openDeleteConfirmationModal(id: any) {

  this.selectedId = id;
  this.modalService.open(this.confirmationModal);
}


deleteItems() {
  this.inProgress = true;
  let cmd = new RemovePhoneNumberCommand();
  cmd.corporationInfoId = this.corporationInfo.id;
  cmd.phoneNumbers = [...this.checkedItems];
  this.client.removePhoneNumber(this.corporationInfo.id, cmd).subscribe(result => {
    this.inProgress = false;
    if (result == null) {

      Swal.fire({
        title: 'حذف شماره تماس با موفقیت انجام شد',
        icon: 'success',
        iconHtml: '!',
        confirmButtonText: 'تایید'
      })
      this.confirmationModalRef?.close();
      this.client.getCorporationInfoById(this.corporationInfo.id).subscribe({
        next: (res) => {
          this.service.setPhoneNumbers(res.phoneNumbers ?? []);
        }
      });

    } else {

      Swal.fire({
        title: 'حذف شماره تماس با خطا مواجه شد',
        icon: 'success',
        iconHtml: '!',
        confirmButtonText: 'تایید'
      })

      this.confirmationModalRef?.close();

    }
  }, error =>{
    this.inProgress = false;
    console.error(error)
});

  this.checkedItems = new Set<string>();

}

  public handleCloseFormModal(){
  this.myForm.reset();
  this.myForm.markAsUntouched();
  this.myForm.setErrors(null);
  this.myForm.markAsPristine();
  this.selectedId = 0;
}

markAllControlsAsTouched(formGroup: FormGroup): void {
  Object.values(formGroup.controls).forEach(control => {
    if (control instanceof FormGroup) {
      this.markAllControlsAsTouched(control);
    } else {
      control.markAsTouched();
    }
  });
}

}



