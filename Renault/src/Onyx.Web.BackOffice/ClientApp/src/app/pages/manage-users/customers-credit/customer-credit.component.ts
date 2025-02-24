import { Component, ElementRef, QueryList, ViewChild, ViewChildren } from '@angular/core';

import { Observable } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { NgbAlertConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CustomersClient, SetCustomerCreditCommand, SetCustomerMaxCreditCommand } from 'src/app/web-api-client';
import { CurrencyPipe, DecimalPipe } from '@angular/common';
import Swal from 'sweetalert2';
import { CustomerCreditGridService } from './customer-credit-grid.service';
import { CustomerCreditModel } from './customer-credit.model';
import { NgbdCustomerCreditSortableHeader, SortEvent } from './customer-credit-sortable.directive';
import { ImageService } from 'src/app/core/services/image.service';
import { AuthenticationService } from 'src/app/core/services/authService/auth.service';

@Component({
  selector: 'app-customer-credit',
  templateUrl: './customer-credit.component.html',
  styleUrls: ['./customer-credit.component.scss'],
  providers: [CustomerCreditGridService, DecimalPipe, NgbAlertConfig]
})
export class CustomerCreditComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  registerForm!: FormGroup;
  updateForm!: FormGroup;
  selectedId: any = null;
  submit!: boolean;
  selectedItem?: CustomerCreditModel;
  gridjsList$!: Observable<CustomerCreditModel[]>;
  total$: Observable<number>;
  inProgress = false;
  personTypesList? = [1, 2];
  selectedCustomer?: any;
  public formattedCredit: string | null = '';
  public formattedMaxCredit: string | null = '';
  @ViewChildren(NgbdCustomerCreditSortableHeader) customerCredits!: QueryList<NgbdCustomerCreditSortableHeader>;
  @ViewChild('confirmationModal') confirmationModal: any;
  @ViewChild('updateFormModal') updateFormModal: any = [];
  @ViewChild('updateMaxFormModal') updateMaxFormModal: any = [];
  @ViewChild('registerFormModal') registerFormModal: any = [];
  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;
  @ViewChild('customerTabModel') customerTabModel: any = [];
  personType?: any;
  formData = new FormData();
  checkedItems: Set<string> = new Set<string>();
  creditValue:  number = 0;
  maxCreditValue:  number = 0;
  constructor(alertConfig: NgbAlertConfig,
    private decimalPipe: DecimalPipe,
    public client: CustomersClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    public service: CustomerCreditGridService,
    public authenticationService: AuthenticationService,
    private imageService: ImageService) {
    this.gridjsList$ = service.customerCredits$;
    this.total$ = service.total$;


    this.updateForm = this.fb.group({
      id: '',
      firstName: '',
      lastName: '',
      userName: '',
      nationalCode: '',
      phoneNumber: '',
      credit: [0, [Validators.required , Validators.maxLength(15)]],
      maxCredit: [0, [Validators.required , Validators.maxLength(15)]],
    });

    this.updateForm.controls['firstName'].disable();
    this.updateForm.controls['lastName'].disable();
    this.updateForm.controls['userName'].disable();
    this.updateForm.controls['nationalCode'].disable();
    this.updateForm.controls['phoneNumber'].disable();

    alertConfig.type = 'success';


  }
  
  onCreditInput(event: any): void {
    let input = event.target.value.replace(/,/g, ''); 

    if (!isNaN(input) && input !== '') {

      const numericValue = parseFloat(input);

      this.updateForm.controls['credit'].setValue(numericValue);
      this.formattedCredit = this.decimalPipe.transform(numericValue, '1.0-0');
      this.creditValue = numericValue;
    } else {
      this.updateForm.controls['credit'].setValue(null);
      this.formattedCredit = '';
      this.creditValue = 0;
    }
  }

  onMaxCreditInput(event: any): void {
    let input = event.target.value.replace(/,/g, ''); 

    if (!isNaN(input) && input !== '') {

      const numericValue = parseFloat(input);

      this.updateForm.controls['maxCredit'].setValue(numericValue);
      this.formattedMaxCredit = this.decimalPipe.transform(numericValue, '1.0-0');
      this.maxCreditValue = numericValue;
    } else {
      this.updateForm.controls['maxCredit'].setValue(null);
      this.formattedMaxCredit = '';
      this.maxCreditValue = 0;
    }
  }



  ngOnInit(): void {
    this.breadCrumbItems = [
      { label: 'خوشه مدیریت کاربران' },
      { label: 'اعتبار مشتری', active: true }
    ];
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
      this.modalService.open(content, { centered: true });
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
  /**
  * Sort table data
  * @param param0 sort the column
  *
  */
  onSort({ column, direction }: SortEvent) {
    // resetting other customerCredits
    this.customerCredits.forEach(customerCredit => {
      if (customerCredit.sortable !== column) {
        customerCredit.direction = '';
      }
    });

    this.service.sortColumn = column;
    this.service.sortDirection = direction;
  }
  get registerFormControls() {
    return this.registerForm.controls;
  }
  get updateFormControls() {
    return this.updateForm.controls;
  }


  updateSubmit(): void {
    this.inProgress = true;
    this.updateForm.get('credit')?.setValidators([Validators.required, Validators.maxLength(15)]);
    this.updateForm.get('credit')?.updateValueAndValidity();
    this.markAllControlsAsTouched(this.updateForm);
    this.submit = true;
    if (this.updateForm.valid) {
      let cmd = new SetCustomerCreditCommand()
      debugger;
      cmd.id = this.updateForm.value.id;
      cmd.credit = parseFloat(this.updateForm.value.credit.toString());

      this.client.setCustomerCredit(cmd).subscribe(result => {
        this.inProgress = false;
        if (result == null) {
          this.service.refreshCustomerCredits();
          Swal.fire({
            title: 'ذخیره اعتبار مشتری با موفقیت انجام شد',
            icon: 'success',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });
          this.service.refreshCustomerCredits();
          this.modalService.dismissAll();
          this.handleCloseUpdateFormModal();
        } else {
          Swal.fire({
            title: 'ذخیره اعتبار مشتری با خطا مواجه شد',
            icon: 'error',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });
          this.modalService.dismissAll();
        }

      }, error => {
        this.inProgress = false;
        console.error(error);
      });
    }
  }

  updateMaxSubmit(): void {
    this.inProgress = true;
    this.updateForm.get('maxCredit')?.setValidators([Validators.required, Validators.maxLength(15)]);
    this.updateForm.get('maxCredit')?.updateValueAndValidity();
    this.markAllControlsAsTouched(this.updateForm);
    this.submit = true;
    if (this.updateForm.valid) {
      let cmd = new SetCustomerMaxCreditCommand()
      debugger;
      cmd.id = this.updateForm.value.id;
      cmd.maxCredit = parseFloat(this.updateForm.value.maxCredit.toString());

      this.client.setMaxCustomerCredit(cmd).subscribe(result => {
        this.inProgress = false;
        if (result == null) {
          this.service.refreshCustomerCredits();
          Swal.fire({
            title: 'ذخیره سقف اعتبار مشتری با موفقیت انجام شد',
            icon: 'success',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });
          this.service.refreshCustomerCredits();
          this.modalService.dismissAll();
          this.handleCloseUpdateFormModal();
        } else {
          Swal.fire({
            title: 'ذخیره سقف اعتبار مشتری با خطا مواجه شد',
            icon: 'error',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });
          this.modalService.dismissAll();
        }

      }, error => {
        this.inProgress = false;
        console.error(error);
      });
    }
  }


  resetUpdateForm(): void {
   Object.keys(this.updateForm.controls).forEach(controlName => {
      const control = this.updateForm.controls[controlName];
      if (control.enabled) {
        control.markAsPristine();
        control.markAsUntouched();
        control.reset();
      }
    });
  }


  edit(item: CustomerCreditModel) {
    this.SetModalData(item);
    this.modalService.open(this.updateFormModal, { size: 'lg', backdrop: 'static' });
  }

  editMax(item: CustomerCreditModel) {
    this.SetModalData(item);
    this.modalService.open(this.updateMaxFormModal, { size: 'lg', backdrop: 'static' });
  }

  SetModalData(item: CustomerCreditModel){
    this.creditValue = parseFloat(item.credit.toString()) || 0;
    this.maxCreditValue = parseFloat(item.maxCredit.toString()) || 0;
    debugger;
    this.selectedId = item.id;

    this.updateForm.controls['id'].setValue(this.selectedId ?? null);
    this.updateForm.controls['userName'].setValue(item.userName ?? null);
    this.updateForm.controls['firstName'].setValue(item.firstName ?? null);
    this.updateForm.controls['lastName'].setValue(item.lastName ?? null);
    this.updateForm.controls['nationalCode'].setValue(item.nationalCode ?? null);
    this.updateForm.controls['phoneNumber'].setValue(item.phoneNumber ?? null);
    // this.updateForm.controls['credit'].setValue(item.credit ?? 0);

    this.formattedCredit = this.decimalPipe.transform(this.creditValue, '1.0-0');
    this.updateForm.controls['credit'].setValue(this.formattedCredit);
    this.formattedMaxCredit = this.decimalPipe.transform(this.maxCreditValue, '1.0-0');
    this.updateForm.controls['maxCredit'].setValue(this.formattedMaxCredit);
    

    debugger;

    this.updateForm.controls['userName'].disable();
    this.updateForm.controls['firstName'].disable();
    this.updateForm.controls['lastName'].disable();
    this.updateForm.controls['nationalCode'].disable();
    this.updateForm.controls['phoneNumber'].disable();
  }

  openInsertModal() {
    this.modalService.open(this.registerFormModal, { size: 'lg', backdrop: 'static' });
  }


  openDeleteConfirmationModal(id: any) {

    this.selectedId = id;
    console.log('    this.selectedId = id;', this.selectedId);
    this.modalService.open(this.confirmationModal);
  }



  public handleCloseUpdateFormModal() {
    this.updateForm.reset();
    this.updateForm.markAsUntouched();
    this.updateForm.setErrors(null);
    this.updateForm.markAsPristine();
    this.updateForm.controls['userName'].enable();
    this.updateForm.controls['firstName'].enable();
    this.updateForm.controls['lastName'].enable();
    this.updateForm.controls['nationalCode'].enable();
    this.updateForm.controls['phoneNumber'].enable();
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


  openMoreDetailCustomer(item: any) {
    debugger;
    this.selectedCustomer = item;
    this.modalService.open(this.customerTabModel, { size: 'lg', backdrop: 'static' });
  }
}

