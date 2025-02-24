import {
  Component,
  ElementRef,
  QueryList,
  ViewChild,
  ViewChildren,
} from "@angular/core";

import { Observable, of } from "rxjs";
import { AbstractControl, FormBuilder, FormGroup, Validators } from "@angular/forms";
import { CustomerValidators } from './customer-validators';

import { NgbAlertConfig, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import {
  CustomersClient,
  FileParameter,
  FileUploadMetadataDto,
} from "src/app/web-api-client";
import { DecimalPipe } from "@angular/common";
import Swal from "sweetalert2";
import { CustomerGridService } from "./customer-grid.service";
import { CustomerModel } from "./customer.model";
import {
  NgbdCustomerSortableHeader,
  SortEvent,
} from "./customer-sortable.directive";
import { ImageService } from "src/app/core/services/image.service";
import { AuthenticationService } from "src/app/core/services/authService/auth.service";
import { AdminRegisterCustomerCommand } from "src/app/core/services/authService/models/commands/AdminRegisterCustomerCommand";

@Component({
  selector: "app-customer",
  templateUrl: "./customer.component.html",
  styleUrls: ["./customer.component.scss"],
  providers: [CustomerGridService, DecimalPipe, NgbAlertConfig],
})
export class CustomerComponent {
  selectedFileUrl: string | undefined = undefined;
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  registerForm!: FormGroup;
  updateForm!: FormGroup;
  selectedId: any = null;
  submit!: boolean;
  selectedItem?: CustomerModel;
  gridjsList$!: Observable<CustomerModel[]>;
  total$: Observable<number>;
  inProgress = false;
  personTypesList? = [1, 2];
  customerTypesList? = [1, 2, 3, 4];
  @ViewChildren(NgbdCustomerSortableHeader)
  customers!: QueryList<NgbdCustomerSortableHeader>;
  @ViewChild("confirmationModal") confirmationModal: any;
  @ViewChild("updateFormModal") updateFormModal: any = [];
  @ViewChild("registerFormModal") registerFormModal: any = [];
  @ViewChild("fileInput") fileInput!: ElementRef<HTMLInputElement>;
  selectedUser?: any;
  @ViewChild("userTabModel") userTabModel: any = [];
  personType?: any;
  customerType?: any;
  formData = new FormData();
  checkedItems: Set<string> = new Set<string>();
  constructor(
    alertConfig: NgbAlertConfig,
    public client: CustomersClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    public service: CustomerGridService,
    public authenticationService: AuthenticationService,
    private imageService: ImageService
  ) {
    this.gridjsList$ = service.customers$;
    this.total$ = service.total$;

    this.registerForm = this.fb.group({
      firstName: ["", [Validators.required]],
      lastName: ["", [Validators.required]],
      userName: ["", [Validators.required , Validators.pattern(/^[A-Za-z0-9_.]+$/)]],
      nationalCode: ["", [Validators.required]],
      phoneNumber: ["", [Validators.required , Validators.maxLength(11),Validators.minLength(11),Validators.pattern(/^(?:\+98|0)?9\d{9}$/)]],
      email: ["", [Validators.required, Validators.email]],
      password: ["", [Validators.required ,  Validators.minLength(8),
        (control: AbstractControl) => {
            const password = control.value;
            if (!password) {
                return null;
            }
    
            if (!/\d/.test(password)) {
                return { missingDigit: true };
            }
    
            if (!/[a-z]/.test(password)) {
                return { missingLowercase: true };
            }
    
            if (!/[A-Z]/.test(password)) {
                return { missingUppercase: true };
            } 
    
            if (new Set(password).size < 3) {
                return { notEnoughUniqueChars: true };
            }
    
            return null;
        }]],
      confirmPassword: ["", [Validators.required]],
      personType: [0, [Validators.required]],
      customerType: [0, [Validators.required]],
      avatar: "",
    } ,  { validators: [CustomerValidators.mustMatchValidator('password', 'confirmPassword')]  });

    this.registerForm.get('userName')?.addAsyncValidators(
      CustomerValidators.validUniqueUserName(this.authenticationService,'')
    );
    this.registerForm.get('nationalCode')?.addAsyncValidators(
      CustomerValidators.validUniqueNationalCode(this.authenticationService,'')
    );

    this.registerForm.get('id')?.valueChanges.subscribe((id) => {
      this.registerForm.get('userName')?.setAsyncValidators(
        CustomerValidators.validUniqueUserName(
          this.authenticationService,
          id != null ? id : 0
        )
      );
      this.registerForm.get('nationalCode')?.setAsyncValidators(
        CustomerValidators.validUniqueNationalCode(
          this.authenticationService,
          id != null ? id : 0
        )
      );
    });


    this.updateForm = this.fb.group({
      id: "",
      firstName: ["", [Validators.required]],
      lastName: ["", [Validators.required]],
      userName: ["", [Validators.required]],
      nationalCode: ["", [Validators.required]],
      phoneNumber: ["", [Validators.required]],
      email: ["", [Validators.required, Validators.email]],
      isActive: [""],
      roles: [""],
      registerDateTime: ["", [Validators.required]],
      lockoutEnabled: false,
      personType: [0, [Validators.required]],
      customerType: [0, [Validators.required]],
      avatar: "",
    });

    this.updateForm.controls["userName"].disable();
    this.updateForm.controls["phoneNumber"].disable();
    this.updateForm.controls["registerDateTime"].disable();
    this.updateForm.controls["isActive"].disable();
    this.updateForm.controls["roles"].disable();

    this.updateForm.get('nationalCode')?.addAsyncValidators(
      CustomerValidators.validUniqueNationalCode(this.authenticationService,'')
    );

    this.updateForm.get('id')?.valueChanges.subscribe((id) => {
      this.updateForm.get('nationalCode')?.setAsyncValidators(
        CustomerValidators.validUniqueNationalCode(
          this.authenticationService,
          id != null ? id : 0
        )
      );
    });

    alertConfig.type = "success";
  }

  ngOnInit(): void {
    this.breadCrumbItems = [
      { label: "خوشه مدیریت کاربران" },
      { label: "مشتری", active: true },
    ];
  }

  onFileSelectedUpdate(event: FileUploadMetadataDto | null): void {
    if (event) {
      this.updateForm.controls["avatar"].setValue(event.fileId);
    } else {
      this.updateForm.controls["avatar"].setValue(null);
    }
  }
  onFileSelectedRegister(event: FileUploadMetadataDto | null): void {
    if (event) {
      this.registerForm.controls["avatar"].setValue(event.fileId);
    } else {
      this.registerForm.controls["avatar"].setValue(null);
    }
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

  changeMultiple(content: any) {
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
   * Password Hide/Show
   */
    toggleFieldTextType(inputField: HTMLInputElement) {
      inputField.type = inputField.type === 'password' ? 'text' : 'password';
    }

  /**
   * Sort table data
   * @param param0 sort the column
   *
   */
  onSort({ column, direction }: SortEvent) {
    // resetting other customers
    this.customers.forEach((customer) => {
      if (customer.sortable !== column) {
        customer.direction = "";
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

  registerSubmit(): void {
    this.inProgress = true;
    this.markAllControlsAsTouched(this.registerForm);
    this.submit = true;
    this.registerForm.value.personType = parseInt(this.personType);
    this.registerForm.value.customerType = parseInt(this.customerType);
    let adminRegisterCustomerCommand = new AdminRegisterCustomerCommand();
    adminRegisterCustomerCommand.firstName = this.registerForm.value.firstName;
    adminRegisterCustomerCommand.lastName = this.registerForm.value.lastName;
    adminRegisterCustomerCommand.userName = this.registerForm.value.userName;
    adminRegisterCustomerCommand.nationalCode =
      this.registerForm.value.nationalCode;
    adminRegisterCustomerCommand.phoneNumber =
      this.registerForm.value.phoneNumber;
    adminRegisterCustomerCommand.email = this.registerForm.value.email;
    adminRegisterCustomerCommand.password = this.registerForm.value.password;
    adminRegisterCustomerCommand.confirmPassword =
      this.registerForm.value.confirmPassword;
    adminRegisterCustomerCommand.personType =
      this.registerForm.value.personType;
    adminRegisterCustomerCommand.customerType =
      this.registerForm.value.customerType;
    debugger;
    if (this.registerForm.valid) {
      this.authenticationService
        .adminRegisterCustomer(
          adminRegisterCustomerCommand,
          this.registerForm.value.avatar
        )
        .subscribe(
          (result) => {
            this.inProgress = false;
            debugger;
            this.service.refreshCustomers();
            Swal.fire({
              title: "افزودن مشتری با موفقیت انجام شد",
              icon: "success",
              iconHtml: "!",
              confirmButtonText: "تایید",
            });
            this.service.refreshCustomers();
            this.modalService.dismissAll();
            this.handleCloseRegisterFormModal();
          },
          (error) => {
            this.inProgress = false;
            Swal.fire({
              title: "افزودن مشتری با خطا مواجه شد",
              icon: "error",
              iconHtml: "!",
              confirmButtonText: "تایید",
            });
            this.modalService.dismissAll();
            console.error(error);
          }
        );
    }else{
      this.inProgress = false;
    }
  }

  updateSubmit(): void {
    this.inProgress = true;
    this.updateForm.controls["lockoutEnabled"].setValue(
      this.updateForm.controls["lockoutEnabled"].value != null
        ? this.updateForm.controls["lockoutEnabled"].value
        : false
    );

    this.markAllControlsAsTouched(this.updateForm);
    this.submit = true;
    this.updateForm.value.personType = parseInt(this.personType);
    this.updateForm.value.customerType = parseInt(this.customerType);
    if (this.updateForm.valid) {
      this.authenticationService
        .updateCustomer(this.updateForm.value, this.updateForm.value.avatar)
        .subscribe(
          (result) => {
            this.inProgress = false;
            this.service.refreshCustomers();
            Swal.fire({
              title: "ذخیره مشتری با موفقیت انجام شد",
              icon: "success",
              iconHtml: "!",
              confirmButtonText: "تایید",
            });
            this.service.refreshCustomers();
            this.modalService.dismissAll();
            this.handleCloseUpdateFormModal();
          },
          (error) => {
            this.inProgress = false;
            console.error(error);
            Swal.fire({
              title: "ذخیره مشتری با خطا مواجه شد",
              icon: "error",
              iconHtml: "!",
              confirmButtonText: "تایید",
            });
            this.modalService.dismissAll();
          }
        );
    }else{
      this.inProgress = false;
    }
  }

  resetUpdateForm(): void {
    Object.keys(this.updateForm.controls).forEach((controlName) => {
      const control = this.updateForm.controls[controlName];
      if (control.enabled) {
        control.markAsPristine();
        control.markAsUntouched();
        control.reset();
      }
    });
    // this.selectedFile = undefined;
    // this.selectedFileUrl = of(undefined);
  }

  resetRegisterForm(): void {
    Object.keys(this.registerForm.controls).forEach((controlName) => {
      const control = this.registerForm.controls[controlName];
      if (control.enabled) {
        control.markAsPristine();
        control.markAsUntouched();
        control.reset();
      }
    });
    // this.selectedFile = undefined;
    // this.selectedFileUrl = of(undefined);
  }

  edit(item: CustomerModel) {
    debugger;
    this.selectedId = item.id;

    this.updateForm.controls["id"].setValue(this.selectedId ?? null);
    this.updateForm.controls["userName"].setValue(item.userName ?? null);
    this.updateForm.controls["firstName"].setValue(item.firstName ?? null);
    this.updateForm.controls["lastName"].setValue(item.lastName ?? null);
    this.updateForm.controls["avatar"].setValue(item.avatar ?? null);
    this.updateForm.controls["nationalCode"].setValue(
      item.nationalCode ?? null
    );
    this.updateForm.controls["phoneNumber"].setValue(item.phoneNumber ?? null);
    this.updateForm.controls["email"].setValue(item.email ?? null);
    this.updateForm.controls["isActive"].setValue(
      item.isActive.toString() == "true" ? "بله" : "خیر"
    );
    this.updateForm.controls["roles"].setValue(item.roles ?? null);
    this.updateForm.controls["registerDateTime"].setValue(
      item.registerDateTime ?? null
    );
    this.updateForm.controls["personType"].setValue(
      this.getCustomerTypeId(item.personType) ?? null
    );
    this.updateForm.controls["customerType"].setValue(
      this.getCustomerTypeId(item.customerType) ?? null
    );
    this.updateForm.controls["lockoutEnabled"].setValue(
      item.lockoutEnabled ?? null
    );

    debugger;
    this.personType = this.getPersonTypeId(item.personType) ?? null;
    this.customerType = this.getCustomerTypeId(item.customerType) ?? null;

    this.client.getCustomerById(item.id).subscribe((customer) => {
      debugger;
      this.selectedFileUrl = item.avatar;
      this.modalService.open(this.updateFormModal, {
        size: "lg",
        backdrop: "static",
      });
    });
  }

  openInsertModal() {
    this.modalService.open(this.registerFormModal, {
      size: "lg",
      backdrop: "static",
    });
  }

  openDeleteConfirmationModal(id: any) {
    this.selectedId = id;
    console.log("    this.selectedId = id;", this.selectedId);
    this.modalService.open(this.confirmationModal);
  }

  deactivateItems() {
    this.inProgress = true;
    this.authenticationService
      .deactivateUsers([...this.checkedItems])
      .subscribe(
        (result) => {
          this.inProgress = false;
          Swal.fire({
            title: "غیرفعال کردن مشتری با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.service.refreshCustomers();
        },
        (error) => {
          this.inProgress = false;
          console.error(error);
          Swal.fire({
            title: "غیرفعال کردن مشتری با خطا مواجه شد",
            icon: "error",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });

          this.modalService.dismissAll();
        }
      );

    this.selectedId = null;
  }

  activateItems() {
    this.inProgress = true;
    this.authenticationService.activateUsers([...this.checkedItems]).subscribe(
      (result) => {
        this.inProgress = false;
        Swal.fire({
          title: "فعال کردن مشتری با موفقیت انجام شد",
          icon: "success",
          iconHtml: "!",
          confirmButtonText: "تایید",
        });
        this.modalService.dismissAll();
        this.service.refreshCustomers();
      },
      (error) => {
        Swal.fire({
          title: "فعال کردن مشتری با خطا مواجه شد",
          icon: "error",
          iconHtml: "!",
          confirmButtonText: "تایید",
        });
        this.modalService.dismissAll();
        this.inProgress = false;
        console.error(error);
      }
    );

    this.selectedId = null;
  }

  public handleCloseUpdateFormModal() {
    this.updateForm.reset();
    this.updateForm.markAsUntouched();
    this.updateForm.setErrors(null);
    this.updateForm.markAsPristine();
    this.selectedId = null;
    this.selectedFileUrl = undefined;
  }

  public handleCloseRegisterFormModal() {
    this.registerForm.reset();
    this.registerForm.markAsUntouched();
    this.registerForm.setErrors(null);
    this.registerForm.markAsPristine();
    this.selectedId = null;
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

  openMoreDetailUser(item: any) {
    debugger;
    this.selectedUser = item;
    this.modalService.open(this.userTabModel, {
      size: "lg",
      backdrop: "static",
    });
  }

  public getCustomerTypeNameByEnglishName(name: string) {
    switch (name) {
      case "Personal":
        return "شخصی";
      case "Store":
        return "فروشگاهی";
      case "Agency":
        return "نمایندگی";
      case "CentralRepairShop":
        return "تعمیرگاه مرکزی";
      default:
        return "";
    }
  }
  public getCustomerTypeName(id: number) {
    switch (id) {
      case 1:
        return "شخصی";
      case 2:
        return "فروشگاهی";
      case 3:
        return "نمایندگی";
      case 4:
        return "تعمیرگاه مرکزی";
      default:
        return "";
    }
  }

  public getCustomerTypeId(name: string) {
    switch (name) {
      case "Personal":
        return "1";
      case "Store":
        return "2";
      case "Agency":
        return "3";
      case "CentralRepairShop":
        return "4";
      default:
        return "";
    }
  }

  public getPersonTypeNameByEnglishName(name: string) {
    switch (name) {
      case "Natural":
        return "حقیقی";
      case "Legal":
        return "حقوقی";
      default:
        return "";
    }
  }
  public getPersonTypeName(id: number) {
    switch (id) {
      case 1:
        return "حقیقی";
      case 2:
        return "حقوقی";
      default:
        return "";
    }
  }

  public getPersonTypeId(name: string) {
    switch (name) {
      case "Natural":
        return "1";
      case "Legal":
        return "2";
      default:
        return "";
    }
  }
}


