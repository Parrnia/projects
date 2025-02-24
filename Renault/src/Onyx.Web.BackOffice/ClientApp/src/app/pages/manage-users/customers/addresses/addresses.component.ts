
import { Component, ElementRef, Input, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { DecimalPipe } from "@angular/common";
import { NgbAlertConfig, NgbModal, NgbModalRef } from "@ng-bootstrap/ng-bootstrap";
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from "@angular/forms";
import { Observable, of } from "rxjs";
import Swal from "sweetalert2";
import { FileParameter, AddressesClient, ProductTypesClient, CountryDto, CountriesClient } from "src/app/web-api-client";
import { AddressesGridService } from "./addresses-grid.service";
import { AddressesModel } from "./addresses.model";
import { ImageService } from "src/app/core/services/image.service";
import { AddressesValidators } from "./addresses-validators";
// import { NgbdAddressesSortableHeader, SortEvent } from "./addresses-sortable.directive";

@Component({
  selector: 'app-addresses',
  templateUrl: './addresses.component.html',
  styleUrls: ['./addresses.component.scss'],
  providers: [AddressesGridService, DecimalPipe, NgbAlertConfig],
})
export class AddressesComponent {
  // bread crumb items
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: AddressesModel;
  gridjsList$!: Observable<AddressesModel[]>;
  total$: Observable<number>;
  inProgress = false;
  @ViewChild('confirmationModal') confirmationModal: any;
  confirmationModalRef: NgbModalRef | undefined;
  @ViewChild('formModal') formModal: any = [];
  formModalInsertRef: NgbModalRef | undefined;
  formModalUpdateRef: NgbModalRef | undefined;
  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;
  @Input() customer?: any;
  checkedItems: Set<number> = new Set<number>();
  countryList?: CountryDto[];
  countryId?: any;
  constructor(
    alertConfig: NgbAlertConfig, 
    public client: AddressesClient, 
    private fb: FormBuilder, 
    private modalService: NgbModal, 
    public service: AddressesGridService, 
    private formBuilder: UntypedFormBuilder,
    public countryClient: CountriesClient) {
    this.gridjsList$ = service.addresses$;
    this.total$ = service.total$;
    this.myForm = this.fb.group({
      id: 0,
      title: ['', [Validators.required]],
      company: '',
      addressDetails1: ['', [Validators.required]],
      addressDetails2: ['', [Validators.required]],
      city: ['', [Validators.required]],
      state: ['', [Validators.required]],
      postcode: ['', [Validators.required]],
      default: false,
      countryId: ['', [Validators.required]],
      customerId: ['', [Validators.required]],
    });

    this.form.title.addAsyncValidators(AddressesValidators.validAddressesTitle(this.client, this.customer?.id ,0));
    this.form.postcode.addAsyncValidators(AddressesValidators.validAddressesPostcode(this.client, this.customer?.id,0));

    this.form.id.valueChanges.subscribe(id => {
      this.form.title.setAsyncValidators(AddressesValidators.validAddressesTitle(this.client, this.customer?.id, id != null ? id : 0));
      this.form.postcode.setAsyncValidators(AddressesValidators.validAddressesPostcode(this.client, this.customer?.id, id != null ? id : 0));
    });

  }

  ngOnInit(): void {

    this.service.getAlladdresses(this.customer?.id);
    this.getAllCountries();
    this.myForm.controls['customerId'].setValue(this.customer.id ?? null);

    console.log('   this.gridjsList$ ', this.gridjsList$)
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
    this.myForm.controls['default'].setValue(this.myForm.controls['default'].value != null ? this.myForm.controls['default'].value : false);

    this.submit = true;
    if (this.myForm.valid) {
      if (this.selectedId > 0) {
        this.myForm.controls['id'].setValue(this.selectedId);

        this.client.update(
           this.myForm.controls['id'].value,
           this.myForm.value
          ).subscribe(result => {
            this.inProgress = false;
          if (result == null) {

             
            this.service.getAlladdresses(this.customer?.id);
            Swal.fire({
              title: 'ذخیره آدرس با موفقیت انجام شد',
              icon: 'success',
              iconHtml: '!',
              confirmButtonText: 'تایید'
            });
            //this.formModal.nativeElement.click();

            this.formModalUpdateRef?.close();
            this.myForm.reset();

          } else {

            Swal.fire({
              title: 'ذخیره آدرس با خطا مواجه شد',
              icon: 'error',
              iconHtml: '!',
              confirmButtonText: 'تایید'
            });
            this.formModalUpdateRef?.close();
          }
        }, error => {
          this.inProgress = false;
         console.error(error)
        });

      } else {

        this.client.create(
          this.myForm.value
          ).subscribe(result => {
            this.inProgress = false;
          if (result > 0) {
            this.service.getAlladdresses(this.customer?.id);
            Swal.fire({
              title: 'ذخیره آدرس با موفقیت انجام شد',
              icon: 'success',
              iconHtml: '!',
              confirmButtonText: 'تایید'
            });
            debugger;
            //this.formModal.nativeElement.click();

            this.formModalInsertRef?.close();
            this.myForm.reset();

          } else {
            Swal.fire({
              title: 'ذخیره آدرس با خطا مواجه شد',
              icon: 'error',
              iconHtml: '!',
              confirmButtonText: 'تایید'
            });
            this.formModalInsertRef?.close();
          }
        }, error => {
          this.inProgress = false;
         console.error(error)});
      }
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

  edit(item: AddressesModel) {

    debugger;
    this.selectedId = item.id;

    this.myForm.controls['id'].setValue(this.selectedId ?? null);
    this.myForm.controls['title'].setValue(item.title ?? null);
    this.myForm.controls['company'].setValue(item.company ?? null);
    this.myForm.controls['addressDetails1'].setValue(item.addressDetails1 ?? null);
    this.myForm.controls['addressDetails2'].setValue(item.addressDetails2 ?? null);
    this.myForm.controls['city'].setValue(item.city ?? null);
    this.myForm.controls['state'].setValue(item.state ?? null);
    this.myForm.controls['postcode'].setValue(item.postcode ?? null);
    this.myForm.controls['default'].setValue(item.default ?? false);
    this.myForm.controls['countryId'].setValue(item.countryId ?? null);
    this.countryId = item.countryId ?? null;
    this.myForm.controls['customerId'].setValue(this.customer.id ?? null);
    
    this.formModalUpdateRef = this.modalService.open(this.formModal, { size: 'lg', backdrop: 'static' });

  }

  openInsertModal() {
    this.myForm.controls['customerId'].setValue(this.customer.id ?? null);
    this.formModalInsertRef =  this.modalService.open(this.formModal);
  }


  openDeleteConfirmationModal(id: any) {

    this.selectedId = id;
     this.modalService.open(this.confirmationModal);
  }


  deleteItems() {
    this.inProgress = true;
    this.client.deleteRangeAddress([...this.checkedItems]).subscribe(result => {
      this.inProgress = false;
      if (result == null) {

        Swal.fire({
          title: 'حذف آدرس با موفقیت انجام شد',
          icon: 'success',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        })
        this.confirmationModalRef?.close();
        this.service.getAlladdresses(this.customer?.id);

      } else {

        Swal.fire({
          title: 'حذف آدرس با خطا مواجه شد',
          icon: 'success',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        })

        this.confirmationModalRef?.close();

      }
    }, error => {
      this.inProgress = false;
     console.error(error)
    });

    this.checkedItems = new Set<number>();
  }
  public getAllCountries() {
    this.countryClient.getAllCountries().subscribe(result => {
      this.countryList = result;
    }, error => console.error(error));

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



