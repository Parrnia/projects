import {
  Component,
  ElementRef,
  QueryList,
  ViewChild,
  ViewChildren,
} from "@angular/core";

import { Observable, of } from "rxjs";
import { AbstractControl, FormBuilder, FormGroup, Validators } from "@angular/forms";

import { NgbAlertConfig, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import {
  FileParameter,
  FileUploadMetadataDto,
  UsersClient,
} from "src/app/web-api-client";
import { DecimalPipe } from "@angular/common";
import Swal from "sweetalert2";
import { EmployeeGridService } from "./employee-grid.service";
import { EmployeeModel } from "./employee.model";
import {
  NgbdEmployeeSortableHeader,
  SortEvent,
} from "./employee-sortable.directive";
import { ImageService } from "src/app/core/services/image.service";
import { AuthenticationService } from "src/app/core/services/authService/auth.service";
import { EmployeeValidators } from "./employee-validators";
import { RegisterEmployeeCommand } from "src/app/core/services/authService/models/commands/RegisterEmployeeCommand";
import { UpdateEmployeeCommand } from "src/app/core/services/authService/models/commands/UpdateUserCommandAuth";

@Component({
  selector: "app-employee",
  templateUrl: "./employee.component.html",
  styleUrls: ["./employee.component.scss"],
  providers: [EmployeeGridService, DecimalPipe, NgbAlertConfig],
})
export class EmployeeComponent {
  // selectedFile?: FileParameter;
  // selectedFileName?: any;
  // selectedFileUrl: Observable<string | undefined> = of(undefined);
  selectedFileUrl: string | undefined = undefined;
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  registerForm!: FormGroup;
  updateForm!: FormGroup;
  selectedId: any = null;
  selectedUser?: any;
  submit!: boolean;
  selectedItem?: EmployeeModel;
  gridjsList$!: Observable<EmployeeModel[]>;
  total$: Observable<number>;
  inProgress = false;
  personTypesList? = [1, 2];
  @ViewChildren(NgbdEmployeeSortableHeader)
  employees!: QueryList<NgbdEmployeeSortableHeader>;
  @ViewChild("confirmationModal") confirmationModal: any;
  @ViewChild("updateFormModal") updateFormModal: any = [];
  @ViewChild("registerFormModal") registerFormModal: any = [];
  @ViewChild("fileInput") fileInput!: ElementRef<HTMLInputElement>;
  personType?: any;
  formData = new FormData();
  @ViewChild("userTabModel") userTabModel: any = [];
  checkedItems: Set<string> = new Set<string>();
  constructor(
    alertConfig: NgbAlertConfig,
    public client: UsersClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    public service: EmployeeGridService,
    public authenticationService: AuthenticationService,
    private imageService: ImageService
  ) {
    this.gridjsList$ = service.employees$;
    this.total$ = service.total$;

    this.registerForm = this.fb.group({
      firstName: ["", [Validators.required]],
      lastName: ["", [Validators.required]],
      userName: ["", [Validators.required , Validators.pattern(/^[A-Za-z0-9]+$/)]],
      avatar: "",
      nationalCode: ["",[Validators.required , Validators.pattern(/^[0-9]+$/),Validators.maxLength(10),Validators.minLength(10) ]],
      phoneNumber: ["", [Validators.required , Validators.maxLength(11),Validators.minLength(11),Validators.pattern(/^(?:\+98|0)?9\d{9}$/)]],
      email: ["", [Validators.required, Validators.email]],
      password: ["", [Validators.required , Validators.minLength(8),
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
    } ,  { validators: [EmployeeValidators.mustMatchValidator('password', 'confirmPassword')] });

    this.registerForm.get('userName')?.addAsyncValidators(
      EmployeeValidators.validUniqueUserName(this.authenticationService,'')
    );
    this.registerForm.get('nationalCode')?.addAsyncValidators(
      EmployeeValidators.validUniqueNationalCode(this.authenticationService,'')
    );

    this.registerForm.get('id')?.valueChanges.subscribe((id) => {
      this.registerForm.get('userName')?.setAsyncValidators(
        EmployeeValidators.validUniqueUserName(
          this.authenticationService,
          id != null ? id : 0
        )
      );
      this.registerForm.get('nationalCode')?.setAsyncValidators(
        EmployeeValidators.validUniqueNationalCode(
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
      avatar: "",
      nationalCode: ["", [Validators.required , Validators.pattern(/^[0-9]+$/),Validators.maxLength(10),Validators.minLength(10)]],
      phoneNumber: ["", [Validators.required]],
      email: ["", [Validators.required]],
      isActive: [""],
      roles: [""],
      registerDateTime: ["", [Validators.required]],
      lockoutEnabled: [false, [Validators.required]],
    });
    this.updateForm.controls["userName"].disable();
    this.updateForm.controls["phoneNumber"].disable();
    this.updateForm.controls["registerDateTime"].disable();
    this.updateForm.controls["isActive"].disable();
    this.updateForm.controls["roles"].disable();

    this.updateForm.get('nationalCode')?.addAsyncValidators(
      EmployeeValidators.validUniqueNationalCode(this.authenticationService,'')
    );

    this.updateForm.get('id')?.valueChanges.subscribe((id) => {
      this.updateForm.get('nationalCode')?.setAsyncValidators(
        EmployeeValidators.validUniqueNationalCode(
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
      { label: "کارشناس", active: true },
    ];
  }

  onFileSelectedUpdate(event: FileUploadMetadataDto | null): void {
    debugger;
    if (event) {
      this.updateForm.controls["avatar"].setValue(event.fileId);
    } else {
      debugger;
      this.updateForm.controls["avatar"].setValue(null);
    }
  }
  onFileSelectedRegister(event: FileUploadMetadataDto | null): void {
    debugger;
    if (event) {
      this.registerForm.controls["avatar"].setValue(event.fileId);
    } else {
      debugger;
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
    // resetting other employees
    this.employees.forEach((employee) => {
      if (employee.sortable !== column) {
        employee.direction = "";
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

    let registerEmployeeCommand = new RegisterEmployeeCommand();
    registerEmployeeCommand.firstName = this.registerForm.value.firstName;
    registerEmployeeCommand.lastName = this.registerForm.value.lastName;
    registerEmployeeCommand.userName = this.registerForm.value.userName;
    registerEmployeeCommand.nationalCode = this.registerForm.value.nationalCode;
    registerEmployeeCommand.phoneNumber = this.registerForm.value.phoneNumber;
    registerEmployeeCommand.email = this.registerForm.value.email;
    registerEmployeeCommand.password = this.registerForm.value.password;
    debugger;
    if (this.registerForm.valid) {
      this.authenticationService
        .registerEmployee(
          registerEmployeeCommand,
          this.registerForm.value.avatar
        )
        .subscribe(
          (result) => {
            this.inProgress = false;
            this.service.refreshEmployees();
            Swal.fire({
              title: "افزودن کارشناس با موفقیت انجام شد",
              icon: "success",
              iconHtml: "!",
              confirmButtonText: "تایید",
            });
            this.service.refreshEmployees();
            this.modalService.dismissAll();
            this.handleCloseRegisterFormModal();
          },
          (error) => {
            this.inProgress = false;
            Swal.fire({
              title: "افزودن کارشناس با خطا مواجه شد",
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
    this.markAllControlsAsTouched(this.updateForm);
    this.submit = true;
    if (this.updateForm.valid) {
      let cmd = new UpdateEmployeeCommand();
      cmd.userId = this.updateForm.value.id;
      cmd.firstName = this.updateForm.value.firstName;
      cmd.lastName = this.updateForm.value.lastName;
      cmd.nationalCode = this.updateForm.value.nationalCode;
      cmd.email = this.updateForm.value.email;
      this.authenticationService
        .updateEmployee(cmd, this.updateForm.value.avatar)
        .subscribe(
          (result) => {
            this.inProgress = false;
            this.service.refreshEmployees();
            Swal.fire({
              title: "ذخیره کارشناس با موفقیت انجام شد",
              icon: "success",
              iconHtml: "!",
              confirmButtonText: "تایید",
            });
            this.service.refreshEmployees();
            this.modalService.dismissAll();
            this.handleCloseUpdateFormModal();
          },
          (error) => {
            this.inProgress = false;
            console.error(error);
            Swal.fire({
              title: "ذخیره کارشناس با خطا مواجه شد",
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
  }

  edit(item: EmployeeModel) {
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
      item.isActive.toString() == "true" ? "بله" : "خیر" ?? null
    );
    this.updateForm.controls["roles"].setValue(item.roles ?? null);
    this.updateForm.controls["registerDateTime"].setValue(
      item.registerDateTime ?? null
    );
    this.updateForm.controls["lockoutEnabled"].setValue(
      item.lockoutEnabled ?? false
    );
    this.selectedFileUrl = item.avatarImage;

    debugger;

    this.client.getUserById(item.id).subscribe((employee) => {
      debugger;
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

  openMoreDetailUser(item: any) {
    debugger;
    this.selectedUser = item;
    this.modalService.open(this.userTabModel, {
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
            title: "غیرفعال کردن کارشناس با موفقیت انجام شد",
            icon: "success",
            iconHtml: "!",
            confirmButtonText: "تایید",
          });
          this.modalService.dismissAll();
          this.service.refreshEmployees();
        },
        (error) => {
          this.inProgress = false;
          console.error(error);
          Swal.fire({
            title: "غیرفعال کردن کارشناس با خطا مواجه شد",
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
          title: "فعال کردن کارشناس با موفقیت انجام شد",
          icon: "success",
          iconHtml: "!",
          confirmButtonText: "تایید",
        });
        this.modalService.dismissAll();
        this.service.refreshEmployees();
      },
      (error) => {
        this.inProgress = false;
        Swal.fire({
          title: "فعال کردن کارشناس با خطا مواجه شد",
          icon: "error",
          iconHtml: "!",
          confirmButtonText: "تایید",
        });
        this.modalService.dismissAll();
        console.error(error);
      }
    );

    this.selectedId = null;
  }

  public handleCloseUpdateFormModal() {
    this.selectedFileUrl = undefined;
    this.updateForm.reset();
    this.updateForm.markAsUntouched();
    this.updateForm.setErrors(null);
    this.updateForm.markAsPristine();
    this.selectedId = null;
  }

  public handleCloseRegisterFormModal() {
    this.selectedFileUrl = undefined;
    this.registerForm.reset();
    this.registerForm.markAsUntouched();
    this.registerForm.setErrors(null);
    this.registerForm.markAsPristine();
    this.selectedId = null;
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
