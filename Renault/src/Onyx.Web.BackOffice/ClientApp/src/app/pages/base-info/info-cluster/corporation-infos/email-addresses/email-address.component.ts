
import { Component, ElementRef, Input, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { DecimalPipe } from "@angular/common";
import { NgbAlertConfig, NgbModal, NgbModalRef } from "@ng-bootstrap/ng-bootstrap";
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from "@angular/forms";
import { Observable, of } from "rxjs";
import Swal from "sweetalert2";
import { CountryDto, CountriesClient, CorporationInfosClient, AddEmailAddressCommand, RemoveEmailAddressCommand } from "src/app/web-api-client";
import { EmailAddressGridService } from "./email-address-grid.service";
import { EmailAddressModel } from "./email-address.model";
import { ImageService } from "src/app/core/services/image.service";
// import { NgbdEmailAddressSortableHeader, SortEvent } from "./addresses-sortable.directive";

@Component({
  selector: 'app-email-address',
  templateUrl: './email-address.component.html',
  styleUrls: ['./email-address.component.scss'],
  providers: [EmailAddressGridService, DecimalPipe, NgbAlertConfig],
})
export class EmailAddressComponent {
  // bread crumb items
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: EmailAddressModel;
  gridjsList$!: Observable<EmailAddressModel[]>;
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
    public service: EmailAddressGridService,
    private formBuilder: UntypedFormBuilder,
    public client: CorporationInfosClient) {
    this.gridjsList$ = service.emailAddresses$;
    this.total$ = service.total$;
    this.myForm = this.fb.group({
      corporationInfoId: 0,
      emailAddress: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {
    this.client.getCorporationInfoById(this.corporationInfo.id).subscribe({
      next: (res) => {
        this.service.setEmailAddresses(res.emailAddresses ?? []);
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
      this.client.addEmailAddress(
        this.myForm.value.corporationInfoId,
        this.myForm.value
      ).subscribe(result => {
        this.inProgress = false;
        if (result == null) {
          this.client.getCorporationInfoById(this.corporationInfo.id).subscribe({
            next: (res) => {
              this.service.setEmailAddresses(res.emailAddresses ?? []);
            }
          });
          
          Swal.fire({
            title: 'ذخیره آدرس ایمیل با موفقیت انجام شد',
            icon: 'success',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });
          this.formModalInsertRef?.close();
          this.myForm.reset();

        } else {
          Swal.fire({
            title: 'ذخیره آدرس ایمیل با خطا مواجه شد',
            icon: 'error',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });
          this.formModalInsertRef?.close();
        }
      }, error => {
        this.inProgress = false;
        console.error(error)
      } );
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
  let cmd = new RemoveEmailAddressCommand();
  cmd.corporationInfoId = this.corporationInfo.id;
  cmd.emailAddresses = [...this.checkedItems];
  this.client.removeEmailAddress(this.corporationInfo.id, cmd).subscribe(result => {
    this.inProgress = false;
    if (result == null) {

      Swal.fire({
        title: 'حذف آدرس ایمیل با موفقیت انجام شد',
        icon: 'success',
        iconHtml: '!',
        confirmButtonText: 'تایید'
      })
      this.confirmationModalRef?.close();
      this.client.getCorporationInfoById(this.corporationInfo.id).subscribe({
        next: (res) => {
          this.service.setEmailAddresses(res.emailAddresses ?? []);
        }
      });

    } else {

      Swal.fire({
        title: 'حذف آدرس ایمیل با خطا مواجه شد',
        icon: 'success',
        iconHtml: '!',
        confirmButtonText: 'تایید'
      })

      this.confirmationModalRef?.close();

    }
  }, error => {
    this.inProgress = false;
    console.error(error)
  } );

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



